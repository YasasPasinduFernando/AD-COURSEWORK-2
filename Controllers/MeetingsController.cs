using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Infrastructure;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

[Authorize]
public class MeetingsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IAuditLogger _audit;
    private readonly ILogger<MeetingsController> _logger;

    public MeetingsController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        IAuditLogger audit,
        ILogger<MeetingsController> logger)
    {
        _db = db;
        _userManager = userManager;
        _emailService = emailService;
        _audit = audit;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User)!;
        var nowUtc = DateTime.UtcNow.AddMinutes(-30);

        IQueryable<Meeting> baseQuery = _db.Meetings
            .AsNoTracking()
            .Include(m => m.Course)
            .Include(m => m.Lecturer);

        if (User.IsInRole(AppRoles.Lecturer) && !User.IsInRole(AppRoles.Administrator))
        {
            baseQuery = baseQuery.Where(m => m.LecturerId == userId);
        }
        else if (User.IsInRole(AppRoles.Student))
        {
            var courseIds = await _db.Enrollments
                .Where(e => e.StudentId == userId)
                .Select(e => e.CourseId)
                .ToListAsync();
            baseQuery = baseQuery.Where(m => courseIds.Contains(m.CourseId));
        }

        var rows = await baseQuery
            .OrderBy(m => m.ScheduledAtUtc < nowUtc)
            .ThenBy(m => m.ScheduledAtUtc)
            .Select(m => new MeetingRowViewModel
            {
                MeetingId = m.MeetingId,
                Title = m.Title,
                Description = m.Description,
                ScheduledAtUtc = m.ScheduledAtUtc,
                DurationMinutes = m.DurationMinutes,
                MeetingUrl = m.MeetingUrl,
                CourseId = m.CourseId,
                CourseCode = m.Course.Code,
                CourseName = m.Course.Name,
                LecturerName = m.Lecturer.FullName,
                IsMine = m.LecturerId == userId,
                EnrolledCount = m.Course.Enrollments.Count
            })
            .ToListAsync();

        return View(rows);
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Create(int? courseId = null)
    {
        await PopulateCoursesAsync();
        return View(new MeetingInputViewModel
        {
            CourseId = courseId ?? 0,
            MeetingUrl = MeetLinkGenerator.GenerateGoogleMeetUrl()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Create(MeetingInputViewModel model)
    {
        var userId = _userManager.GetUserId(User)!;

        var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseId == model.CourseId);
        if (course == null)
            ModelState.AddModelError(nameof(model.CourseId), "Course not found.");
        else if (course.LecturerId != userId)
            return Forbid();

        if (model.AutoGenerate || string.IsNullOrWhiteSpace(model.MeetingUrl))
        {
            model.MeetingUrl = MeetLinkGenerator.GenerateGoogleMeetUrl();
        }
        else if (!MeetLinkGenerator.IsValidMeetingUrl(model.MeetingUrl))
        {
            ModelState.AddModelError(nameof(model.MeetingUrl),
                "Provide a Google Meet, Zoom, Teams or Webex URL, or tick auto-generate.");
        }

        var scheduledLocal = DateTime.SpecifyKind(model.ScheduledLocal, DateTimeKind.Local);
        if (scheduledLocal < DateTime.Now.AddMinutes(-1))
            ModelState.AddModelError(nameof(model.ScheduledLocal), "Scheduled time must be in the future.");

        if (!ModelState.IsValid)
        {
            await PopulateCoursesAsync();
            return View(model);
        }

        var entity = new Meeting
        {
            CourseId = model.CourseId,
            LecturerId = userId,
            Title = model.Title.Trim(),
            Description = model.Description?.Trim(),
            ScheduledAtUtc = scheduledLocal.ToUniversalTime(),
            DurationMinutes = model.DurationMinutes,
            MeetingUrl = model.MeetingUrl!.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Meetings.Add(entity);
        await _db.SaveChangesAsync();
        await _audit.LogAsync(AuditCategories.Meeting, "Create",
            $"course={course!.Code} title={entity.Title} when={scheduledLocal:s}");

        if (model.NotifyStudents)
        {
            await NotifyStudentsAsync(entity, course, isUpdate: false);
        }

        TempData["Success"] = model.NotifyStudents
            ? $"Meeting scheduled and emails sent to enrolled students of {course!.Code}."
            : "Meeting scheduled.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = _userManager.GetUserId(User);
        var meeting = await _db.Meetings.Include(m => m.Course).FirstOrDefaultAsync(m => m.MeetingId == id);
        if (meeting == null)
            return NotFound();
        if (meeting.LecturerId != userId)
            return Forbid();

        await PopulateCoursesAsync();
        var local = DateTime.SpecifyKind(meeting.ScheduledAtUtc.ToLocalTime(), DateTimeKind.Local);
        var vm = new MeetingInputViewModel
        {
            MeetingId = meeting.MeetingId,
            CourseId = meeting.CourseId,
            Title = meeting.Title,
            Description = meeting.Description,
            ScheduledLocal = local,
            DurationMinutes = meeting.DurationMinutes,
            MeetingUrl = meeting.MeetingUrl,
            AutoGenerate = false,
            NotifyStudents = false
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Edit(int id, MeetingInputViewModel model)
    {
        if (id != model.MeetingId)
            return BadRequest();

        var userId = _userManager.GetUserId(User);
        var meeting = await _db.Meetings.Include(m => m.Course).FirstOrDefaultAsync(m => m.MeetingId == id);
        if (meeting == null)
            return NotFound();
        if (meeting.LecturerId != userId)
            return Forbid();

        var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseId == model.CourseId);
        if (course == null)
            ModelState.AddModelError(nameof(model.CourseId), "Course not found.");
        else if (course.LecturerId != userId)
            return Forbid();

        if (model.AutoGenerate || string.IsNullOrWhiteSpace(model.MeetingUrl))
            model.MeetingUrl = MeetLinkGenerator.GenerateGoogleMeetUrl();
        else if (!MeetLinkGenerator.IsValidMeetingUrl(model.MeetingUrl))
            ModelState.AddModelError(nameof(model.MeetingUrl),
                "Provide a Google Meet, Zoom, Teams or Webex URL, or tick auto-generate.");

        var scheduledLocal = DateTime.SpecifyKind(model.ScheduledLocal, DateTimeKind.Local);

        if (!ModelState.IsValid)
        {
            await PopulateCoursesAsync();
            return View(model);
        }

        meeting.CourseId = model.CourseId;
        meeting.Title = model.Title.Trim();
        meeting.Description = model.Description?.Trim();
        meeting.ScheduledAtUtc = scheduledLocal.ToUniversalTime();
        meeting.DurationMinutes = model.DurationMinutes;
        meeting.MeetingUrl = model.MeetingUrl!.Trim();

        await _db.SaveChangesAsync();
        await _audit.LogAsync(AuditCategories.Meeting, "Update",
            $"course={course!.Code} title={meeting.Title} when={scheduledLocal:s}");

        if (model.NotifyStudents)
            await NotifyStudentsAsync(meeting, course, isUpdate: true);

        TempData["Success"] = model.NotifyStudents
            ? "Meeting updated and students re-notified."
            : "Meeting updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = _userManager.GetUserId(User);
        var meeting = await _db.Meetings.Include(m => m.Course).FirstOrDefaultAsync(m => m.MeetingId == id);
        if (meeting == null)
            return NotFound();
        if (meeting.LecturerId != userId)
            return Forbid();

        var title = meeting.Title;
        var code = meeting.Course.Code;
        _db.Meetings.Remove(meeting);
        await _db.SaveChangesAsync();
        await _audit.LogAsync(AuditCategories.Meeting, "Delete", $"course={code} title={title}");
        TempData["Success"] = "Meeting deleted.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Join(int id)
    {
        var userId = _userManager.GetUserId(User);
        var meeting = await _db.Meetings
            .AsNoTracking()
            .Include(m => m.Course)
            .FirstOrDefaultAsync(m => m.MeetingId == id);
        if (meeting == null)
            return NotFound();

        var allowed = User.IsInRole(AppRoles.Administrator)
            || (User.IsInRole(AppRoles.Lecturer) && meeting.LecturerId == userId)
            || (User.IsInRole(AppRoles.Student) && await _db.Enrollments.AnyAsync(e =>
                    e.CourseId == meeting.CourseId && e.StudentId == userId));
        if (!allowed)
            return Forbid();

        if (string.IsNullOrWhiteSpace(meeting.MeetingUrl) || !MeetLinkGenerator.IsValidMeetingUrl(meeting.MeetingUrl))
        {
            TempData["Error"] = "This meeting link is invalid.";
            return RedirectToAction(nameof(Index));
        }

        await _audit.LogAsync(AuditCategories.Meeting, "Join",
            $"course={meeting.Course.Code} title={meeting.Title}");

        return Redirect(meeting.MeetingUrl);
    }

    /// <summary>
    /// Returns a downloadable .ics file. Anyone in the course can download
    /// (lecturer, admin, enrolled student) and import to any calendar app.
    /// </summary>
    [AllowAnonymous]
    public async Task<IActionResult> Ics(int id, string? token = null)
    {
        var meeting = await _db.Meetings
            .AsNoTracking()
            .Include(m => m.Course)
            .Include(m => m.Lecturer)
            .FirstOrDefaultAsync(m => m.MeetingId == id);
        if (meeting == null)
            return NotFound();

        if (!User.Identity?.IsAuthenticated ?? true)
        {
            var expected = BuildIcsToken(meeting);
            if (string.IsNullOrEmpty(token) || !string.Equals(token, expected, StringComparison.Ordinal))
                return Challenge();
        }
        else
        {
            var userId = _userManager.GetUserId(User);
            var allowed = User.IsInRole(AppRoles.Administrator)
                || (User.IsInRole(AppRoles.Lecturer) && meeting.LecturerId == userId)
                || (User.IsInRole(AppRoles.Student) && await _db.Enrollments.AnyAsync(e =>
                        e.CourseId == meeting.CourseId && e.StudentId == userId));
            if (!allowed)
                return Forbid();
        }

        var ics = BuildIcsContent(meeting);
        var safeName = $"{meeting.Course.Code}-{meeting.MeetingId}.ics";
        return File(System.Text.Encoding.UTF8.GetBytes(ics), "text/calendar; charset=utf-8", safeName);
    }

    private static string BuildIcsToken(Meeting m)
    {
        var raw = $"{m.MeetingId}|{m.CreatedAtUtc:O}|{m.MeetingUrl}";
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes).Substring(0, 16).ToLowerInvariant();
    }

    private string BuildIcsContent(Meeting meeting)
    {
        return CalendarLink.BuildIcs(BuildCalendarEvent(meeting));
    }

    private CalendarLink.CalendarEvent BuildCalendarEvent(Meeting meeting)
    {
        return new CalendarLink.CalendarEvent(
            Uid: $"meeting-{meeting.MeetingId}@unimanage",
            Title: $"{meeting.Course.Code} — {meeting.Title}",
            Description: meeting.Description,
            Location: meeting.MeetingUrl,
            StartUtc: DateTime.SpecifyKind(meeting.ScheduledAtUtc, DateTimeKind.Utc),
            EndUtc: DateTime.SpecifyKind(meeting.ScheduledAtUtc, DateTimeKind.Utc).AddMinutes(meeting.DurationMinutes),
            OrganizerName: meeting.Lecturer?.FullName,
            OrganizerEmail: meeting.Lecturer?.Email,
            Url: meeting.MeetingUrl);
    }

    private async Task PopulateCoursesAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        var courses = await _db.Courses
            .AsNoTracking()
            .Where(c => c.LecturerId == userId)
            .OrderBy(c => c.Code)
            .Select(c => new SelectListItem($"{c.Code} — {c.Name}", c.CourseId.ToString()))
            .ToListAsync();
        ViewBag.Courses = courses;
    }

    private async Task NotifyStudentsAsync(Meeting meeting, Course course, bool isUpdate)
    {
        var students = await _db.Enrollments
            .Where(e => e.CourseId == meeting.CourseId)
            .Select(e => e.Student)
            .Where(s => s.Email != null)
            .ToListAsync();

        var lecturer = await _userManager.GetUserAsync(User);
        meeting.Lecturer ??= lecturer!;
        var scheduledLocal = meeting.ScheduledAtUtc.ToLocalTime();
        var subjectPrefix = isUpdate ? "Updated session" : "New live session";
        var subject = $"{subjectPrefix}: {course.Code} - {meeting.Title}";

        var calEvent = BuildCalendarEvent(meeting);
        var icsBytes = System.Text.Encoding.UTF8.GetBytes(CalendarLink.BuildIcs(calEvent));
        var icsAttachment = new EmailAttachment(
            FileName: $"{course.Code}-{meeting.MeetingId}.ics",
            ContentType: "text/calendar; method=PUBLISH; charset=utf-8",
            Content: icsBytes);

        var googleUrl = CalendarLink.GoogleCalendarUrl(calEvent);
        var outlookUrl = CalendarLink.OutlookCalendarUrl(calEvent);
        var token = BuildIcsToken(meeting);
        var icsDownloadUrl = Url.Action("Ics", "Meetings",
            new { id = meeting.MeetingId, token }, Request.Scheme);

        foreach (var student in students)
        {
            try
            {
                var html = EmailTemplates.BuildMeetingInviteEmail(
                    student.FullName,
                    course.Code,
                    course.Name,
                    meeting.Title,
                    meeting.Description,
                    scheduledLocal,
                    meeting.DurationMinutes,
                    meeting.MeetingUrl,
                    lecturer?.FullName,
                    googleCalendarUrl: googleUrl,
                    outlookCalendarUrl: outlookUrl,
                    icsDownloadUrl: icsDownloadUrl);
                await _emailService.SendAsync(student.Email!, subject, html, new[] { icsAttachment });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Meeting invite email failed for {Email}", student.Email);
            }
        }
    }
}
