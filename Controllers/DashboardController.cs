using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        if (User.IsInRole(AppRoles.Administrator))
            return RedirectToAction(nameof(Administrator));
        if (User.IsInRole(AppRoles.Lecturer))
            return RedirectToAction(nameof(Lecturer));
        if (User.IsInRole(AppRoles.Student))
            return RedirectToAction(nameof(Student));
        return RedirectToAction("Login", "Account");
    }

    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Student()
    {
        var userId = _userManager.GetUserId(User)!;

        var enrolled = await (
            from e in _db.Enrollments
            join c in _db.Courses on e.CourseId equals c.CourseId
            join lec in _db.Users on c.LecturerId equals lec.Id
            where e.StudentId == userId
            select new StudentDashboardViewModel.EnrolledCourseRow
            {
                CourseId = c.CourseId,
                Code = c.Code,
                Name = c.Name,
                Lecturer = lec.FullName
            }).ToListAsync();

        var courseIds = enrolled.Select(e => e.CourseId).ToList();

        var assn = await _db.Assignments
            .AsNoTracking()
            .Include(a => a.Course)
            .Where(a => courseIds.Contains(a.CourseId))
            .OrderBy(a => a.DueDateUtc)
            .Take(25)
            .ToListAsync();

        var assnIds = assn.Select(a => a.AssignmentId).ToList();
        var subsByAssn = await _db.Submissions
            .Where(s => s.StudentId == userId && assnIds.Contains(s.AssignmentId))
            .ToDictionaryAsync(s => s.AssignmentId);

        var deadlineRows = assn.Select(a => subsByAssn.TryGetValue(a.AssignmentId, out var s)
            ? new StudentDashboardViewModel.DeadlineRow
            {
                AssignmentId = a.AssignmentId,
                Title = a.Title,
                CourseCode = a.Course.Code,
                DueDateUtc = a.DueDateUtc,
                Status = s.Status.ToString()
            }
            : new StudentDashboardViewModel.DeadlineRow
            {
                AssignmentId = a.AssignmentId,
                Title = a.Title,
                CourseCode = a.Course.Code,
                DueDateUtc = a.DueDateUtc,
                Status = "Not started"
            }).ToList();

        var gradesRaw = await (
            from s in _db.Submissions
            join a in _db.Assignments on s.AssignmentId equals a.AssignmentId
            join c in _db.Courses on a.CourseId equals c.CourseId
            where s.StudentId == userId && s.Status == SubmissionStatus.Graded && s.Grade != null
            orderby s.GradedAtUtc descending
            select new { c.Code, a.Title, s.Grade, a.MaxPoints, s.Feedback, s.GradedAtUtc }).Take(10).ToListAsync();

        var grades = gradesRaw.Select(x => new StudentDashboardViewModel.GradeRow
        {
            CourseCode = x.Code,
            Assignment = x.Title,
            Grade = x.Grade,
            MaxPoints = x.MaxPoints,
            FeedbackShort = x.Feedback == null
                ? null
                : (x.Feedback.Length > 80 ? x.Feedback.Substring(0, 80) + "..." : x.Feedback),
            GradedAtUtc = x.GradedAtUtc
        }).ToList();

        var unread = await _db.Messages.CountAsync(m => m.ReceiverId == userId && !m.IsRead);

        var calNow = DateTime.Now;
        var rangeStart = new DateTime(calNow.Year, calNow.Month, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
        var rangeEnd = rangeStart.AddMonths(1);

        var assnInMonth = await _db.Assignments
            .AsNoTracking()
            .Include(a => a.Course)
            .Where(a => courseIds.Contains(a.CourseId) && a.DueDateUtc >= rangeStart && a.DueDateUtc < rangeEnd)
            .OrderBy(a => a.DueDateUtc)
            .ToListAsync();

        var matsInMonth = await _db.CourseMaterials
            .AsNoTracking()
            .Include(m => m.Course)
            .Where(m => courseIds.Contains(m.CourseId) && m.UploadedAtUtc >= rangeStart && m.UploadedAtUtc < rangeEnd)
            .OrderBy(m => m.UploadedAtUtc)
            .ToListAsync();

        var subsInMonth = await (
            from s in _db.Submissions.AsNoTracking()
            join a in _db.Assignments on s.AssignmentId equals a.AssignmentId
            join c in _db.Courses on a.CourseId equals c.CourseId
            where s.StudentId == userId
                  && s.SubmittedAtUtc != null
                  && s.SubmittedAtUtc >= rangeStart && s.SubmittedAtUtc < rangeEnd
            orderby s.SubmittedAtUtc
            select new { Date = s.SubmittedAtUtc!.Value, a.Title, c.Code }).ToListAsync();

        var meetingsInMonth = await _db.Meetings
            .AsNoTracking()
            .Include(m => m.Course)
            .Include(m => m.Lecturer)
            .Where(m => courseIds.Contains(m.CourseId)
                        && m.ScheduledAtUtc >= rangeStart && m.ScheduledAtUtc < rangeEnd)
            .OrderBy(m => m.ScheduledAtUtc)
            .ToListAsync();

        var nowUtcEarly = DateTime.UtcNow;
        var upcomingMeetings = await _db.Meetings
            .AsNoTracking()
            .Include(m => m.Course)
            .Include(m => m.Lecturer)
            .Where(m => courseIds.Contains(m.CourseId) && m.ScheduledAtUtc >= nowUtcEarly.AddHours(-1))
            .OrderBy(m => m.ScheduledAtUtc)
            .Take(5)
            .Select(m => new UpcomingMeetingRow
            {
                MeetingId = m.MeetingId,
                Title = m.Title,
                CourseCode = m.Course.Code,
                LecturerName = m.Lecturer.FullName,
                ScheduledAtUtc = m.ScheduledAtUtc,
                DurationMinutes = m.DurationMinutes,
                MeetingUrl = m.MeetingUrl,
                IsMine = false
            })
            .ToListAsync();

        var calItems = new List<CalendarItem>();
        calItems.AddRange(assnInMonth.Select(a => new CalendarItem(a.DueDateUtc, $"Due: {a.Title}", a.Course.Code, DashboardCalendarEventKind.Deadline)));
        calItems.AddRange(matsInMonth.Select(m => new CalendarItem(m.UploadedAtUtc, $"New material: {m.Title}", m.Course.Code, DashboardCalendarEventKind.Material)));
        calItems.AddRange(subsInMonth.Select(s => new CalendarItem(s.Date, $"Submitted: {s.Title}", s.Code, DashboardCalendarEventKind.Submission)));
        calItems.AddRange(meetingsInMonth.Select(m => new CalendarItem(m.ScheduledAtUtc, $"Meeting: {m.Title}", m.Course.Code, DashboardCalendarEventKind.Meeting)));

        var calendar = BuildCalendar(calNow.Year, calNow.Month, calItems);

        var nowUtc = DateTime.UtcNow;
        var countNotStarted = deadlineRows.Count(d => d.Status == "Not started" || d.Status == "NotSubmitted");
        var countSubmitted = deadlineRows.Count(d => d.Status == "Submitted");
        var countGraded = deadlineRows.Count(d => d.Status == "Graded");
        var countOverdue = deadlineRows.Count(d => (d.Status == "Not started" || d.Status == "NotSubmitted") && d.DueDateUtc < nowUtc);

        var gradeTrend = grades
            .Where(g => g.Grade.HasValue && g.MaxPoints > 0 && g.GradedAtUtc.HasValue)
            .OrderBy(g => g.GradedAtUtc)
            .Take(10)
            .Select(g => new TrendPoint
            {
                Label = g.Assignment.Length > 20 ? g.Assignment[..20] + "…" : g.Assignment,
                Value = (double)(g.Grade!.Value / g.MaxPoints) * 100
            }).ToList();

        var activity = new List<ActivityFeedItem>();
        foreach (var g in grades.Take(3))
        {
            activity.Add(new ActivityFeedItem
            {
                Icon = "bi-award",
                Tone = "success",
                Title = $"Graded: {g.Assignment}",
                Detail = $"{g.CourseCode} · {g.Grade}/{g.MaxPoints}",
                WhenUtc = g.GradedAtUtc ?? DateTime.UtcNow
            });
        }
        foreach (var s in subsInMonth.OrderByDescending(s => s.Date).Take(3))
        {
            activity.Add(new ActivityFeedItem
            {
                Icon = "bi-file-earmark-arrow-up",
                Tone = "info",
                Title = $"Submitted: {s.Title}",
                Detail = s.Code,
                WhenUtc = s.Date
            });
        }
        foreach (var m in matsInMonth.OrderByDescending(m => m.UploadedAtUtc).Take(3))
        {
            activity.Add(new ActivityFeedItem
            {
                Icon = "bi-upload",
                Tone = "warn",
                Title = $"New material: {m.Title}",
                Detail = m.Course.Code,
                WhenUtc = m.UploadedAtUtc
            });
        }
        activity = activity.OrderByDescending(a => a.WhenUtc).Take(8).ToList();

        var vm = new StudentDashboardViewModel
        {
            EnrolledCourses = enrolled,
            UpcomingDeadlines = deadlineRows,
            RecentGrades = grades,
            UnreadMessages = unread,
            AssignmentCalendar = calendar,
            GradeTrend = gradeTrend,
            CountNotStarted = countNotStarted,
            CountSubmitted = countSubmitted,
            CountGraded = countGraded,
            CountOverdue = countOverdue,
            Activity = activity,
            UpcomingMeetings = upcomingMeetings
        };

        return View(vm);
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Lecturer()
    {
        var userId = _userManager.GetUserId(User)!;

        var courses = await _db.Courses
            .AsNoTracking()
            .Where(c => c.LecturerId == userId)
            .Include(c => c.Enrollments)
            .Include(c => c.Assignments)
            .OrderBy(c => c.Code)
            .ToListAsync();

        var courseRows = courses.Select(c => new LecturerDashboardViewModel.CourseSummary
        {
            CourseId = c.CourseId,
            Code = c.Code,
            Name = c.Name,
            EnrollmentCount = c.Enrollments.Count,
            AssignmentCount = c.Assignments.Count
        }).ToList();

        var courseIds = courses.Select(c => c.CourseId).ToList();

        var assignmentToCourse = await _db.Assignments
            .Where(a => courseIds.Contains(a.CourseId))
            .Select(a => new { a.AssignmentId, a.CourseId })
            .ToListAsync();
        var assignmentIds = assignmentToCourse.Select(a => a.AssignmentId).ToList();

        var subs = await _db.Submissions
            .Where(s => assignmentIds.Contains(s.AssignmentId))
            .Select(s => new { s.SubmissionId, s.AssignmentId, s.Status })
            .ToListAsync();

        var pending = subs.Count(s => s.Status == SubmissionStatus.Submitted);
        var graded = subs.Count(s => s.Status == SubmissionStatus.Graded);

        var assnCourseLookup = assignmentToCourse.ToDictionary(x => x.AssignmentId, x => x.CourseId);
        var courseCodeLookup = courses.ToDictionary(c => c.CourseId, c => c.Code);

        var subsByCourse = subs
            .Where(s => assnCourseLookup.ContainsKey(s.AssignmentId))
            .GroupBy(s => assnCourseLookup[s.AssignmentId])
            .ToDictionary(g => g.Key, g => g.ToList());

        var submissionsPerCourse = courses.Select(c =>
            new TrendPoint { Label = c.Code, Value = subsByCourse.TryGetValue(c.CourseId, out var l) ? l.Count : 0 }).ToList();
        var gradedPerCourse = courses.Select(c =>
            new TrendPoint { Label = c.Code, Value = subsByCourse.TryGetValue(c.CourseId, out var l) ? l.Count(s => s.Status == SubmissionStatus.Graded) : 0 }).ToList();

        var recentSubs = await (
            from s in _db.Submissions.AsNoTracking()
            join a in _db.Assignments on s.AssignmentId equals a.AssignmentId
            join c in _db.Courses on a.CourseId equals c.CourseId
            join u in _db.Users on s.StudentId equals u.Id
            where courseIds.Contains(a.CourseId) && s.SubmittedAtUtc != null
            orderby s.SubmittedAtUtc descending
            select new LecturerDashboardViewModel.RecentSubmissionRow
            {
                SubmissionId = s.SubmissionId,
                AssignmentId = a.AssignmentId,
                AssignmentTitle = a.Title,
                CourseCode = c.Code,
                StudentName = u.FullName,
                Status = s.Status.ToString(),
                SubmittedAtUtc = s.SubmittedAtUtc
            }).Take(8).ToListAsync();

        var unread = await _db.Messages.CountAsync(m => m.ReceiverId == userId && !m.IsRead);

        var calNow = DateTime.Now;
        var rangeStart = new DateTime(calNow.Year, calNow.Month, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
        var rangeEnd = rangeStart.AddMonths(1);

        var assnInMonth = await _db.Assignments
            .AsNoTracking()
            .Include(a => a.Course)
            .Where(a => courseIds.Contains(a.CourseId) && a.DueDateUtc >= rangeStart && a.DueDateUtc < rangeEnd)
            .OrderBy(a => a.DueDateUtc)
            .ToListAsync();

        var matsInMonth = await _db.CourseMaterials
            .AsNoTracking()
            .Include(m => m.Course)
            .Where(m => courseIds.Contains(m.CourseId)
                        && m.UploadedById == userId
                        && m.UploadedAtUtc >= rangeStart && m.UploadedAtUtc < rangeEnd)
            .OrderBy(m => m.UploadedAtUtc)
            .ToListAsync();

        var subsInMonth = await (
            from s in _db.Submissions.AsNoTracking()
            join a in _db.Assignments on s.AssignmentId equals a.AssignmentId
            join c in _db.Courses on a.CourseId equals c.CourseId
            join u in _db.Users on s.StudentId equals u.Id
            where courseIds.Contains(a.CourseId)
                  && s.SubmittedAtUtc != null
                  && s.SubmittedAtUtc >= rangeStart && s.SubmittedAtUtc < rangeEnd
            orderby s.SubmittedAtUtc
            select new { Date = s.SubmittedAtUtc!.Value, a.Title, c.Code, Student = u.FullName }).ToListAsync();

        var meetingsInMonth = await _db.Meetings
            .AsNoTracking()
            .Include(m => m.Course)
            .Where(m => m.LecturerId == userId
                        && m.ScheduledAtUtc >= rangeStart && m.ScheduledAtUtc < rangeEnd)
            .OrderBy(m => m.ScheduledAtUtc)
            .ToListAsync();

        var nowUtcLec = DateTime.UtcNow;
        var upcomingMeetings = await _db.Meetings
            .AsNoTracking()
            .Include(m => m.Course)
            .Where(m => m.LecturerId == userId && m.ScheduledAtUtc >= nowUtcLec.AddHours(-1))
            .OrderBy(m => m.ScheduledAtUtc)
            .Take(5)
            .Select(m => new UpcomingMeetingRow
            {
                MeetingId = m.MeetingId,
                Title = m.Title,
                CourseCode = m.Course.Code,
                LecturerName = m.Lecturer.FullName,
                ScheduledAtUtc = m.ScheduledAtUtc,
                DurationMinutes = m.DurationMinutes,
                MeetingUrl = m.MeetingUrl,
                IsMine = true
            })
            .ToListAsync();

        var calItems = new List<CalendarItem>();
        calItems.AddRange(assnInMonth.Select(a => new CalendarItem(a.DueDateUtc, $"Due: {a.Title}", a.Course.Code, DashboardCalendarEventKind.Deadline)));
        calItems.AddRange(matsInMonth.Select(m => new CalendarItem(m.UploadedAtUtc, $"Uploaded: {m.Title}", m.Course.Code, DashboardCalendarEventKind.Material)));
        calItems.AddRange(subsInMonth.Select(s => new CalendarItem(s.Date, $"{s.Student} submitted: {s.Title}", s.Code, DashboardCalendarEventKind.Submission)));
        calItems.AddRange(meetingsInMonth.Select(m => new CalendarItem(m.ScheduledAtUtc, $"Meeting: {m.Title}", m.Course.Code, DashboardCalendarEventKind.Meeting)));

        var calendar = BuildCalendar(calNow.Year, calNow.Month, calItems);

        var activity = new List<ActivityFeedItem>();
        foreach (var s in recentSubs.Take(5))
        {
            activity.Add(new ActivityFeedItem
            {
                Icon = s.Status == "Graded" ? "bi-patch-check-fill" : "bi-file-earmark-arrow-up",
                Tone = s.Status == "Graded" ? "success" : "info",
                Title = $"{s.StudentName} — {s.AssignmentTitle}",
                Detail = $"{s.CourseCode} · {s.Status}",
                WhenUtc = s.SubmittedAtUtc ?? DateTime.UtcNow
            });
        }
        foreach (var m in matsInMonth.OrderByDescending(m => m.UploadedAtUtc).Take(3))
        {
            activity.Add(new ActivityFeedItem
            {
                Icon = "bi-upload",
                Tone = "warn",
                Title = $"You uploaded: {m.Title}",
                Detail = m.Course.Code,
                WhenUtc = m.UploadedAtUtc
            });
        }
        activity = activity.OrderByDescending(a => a.WhenUtc).Take(8).ToList();

        var vm = new LecturerDashboardViewModel
        {
            Courses = courseRows,
            PendingSubmissions = pending,
            GradedSubmissions = graded,
            UnreadMessages = unread,
            AssignmentCalendar = calendar,
            SubmissionsPerCourse = submissionsPerCourse,
            GradedPerCourse = gradedPerCourse,
            RecentSubmissions = recentSubs,
            Activity = activity,
            UpcomingMeetings = upcomingMeetings
        };
        return View(vm);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Administrator()
    {
        var userCount = await _db.Users.CountAsync();
        var courseCount = await _db.Courses.CountAsync();
        var enrollmentCount = await _db.Enrollments.CountAsync();
        var assignmentCount = await _db.Assignments.CountAsync();
        var submissionCount = await _db.Submissions.CountAsync();
        var messageCount = await _db.Messages.CountAsync();

        var students = await _userManager.GetUsersInRoleAsync(AppRoles.Student);
        var lecturers = await _userManager.GetUsersInRoleAsync(AppRoles.Lecturer);
        var admins = await _userManager.GetUsersInRoleAsync(AppRoles.Administrator);

        var popular = await _db.Courses
            .AsNoTracking()
            .Select(c => new AdminDashboardViewModel.PopularCourseRow
            {
                Code = c.Code,
                Name = c.Name,
                Enrollments = c.Enrollments.Count
            })
            .OrderByDescending(p => p.Enrollments)
            .Take(6)
            .ToListAsync();

        var sinceUtc = DateTime.UtcNow.AddDays(-29);
        var enrollDates = await _db.Enrollments
            .Where(e => e.EnrolledAtUtc >= sinceUtc)
            .Select(e => e.EnrolledAtUtc)
            .ToListAsync();

        var byDay = enrollDates
            .GroupBy(d => DateOnly.FromDateTime(d.ToLocalTime()))
            .ToDictionary(g => g.Key, g => g.Count());

        var trend = new List<TrendPoint>();
        var startDay = DateOnly.FromDateTime(DateTime.Now.AddDays(-29));
        for (var d = 0; d < 30; d++)
        {
            var day = startDay.AddDays(d);
            trend.Add(new TrendPoint
            {
                Label = day.ToString("MMM d"),
                Value = byDay.TryGetValue(day, out var cnt) ? cnt : 0
            });
        }

        var recentAudit = await _db.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAtUtc)
            .Take(8)
            .ToListAsync();

        var activity = recentAudit.Select(a => new ActivityFeedItem
        {
            Icon = a.Category switch
            {
                "Auth" => "bi-shield-lock",
                "Course" => "bi-collection",
                "Material" => "bi-upload",
                "Submission" => "bi-file-earmark-text",
                "Enrollment" => "bi-person-plus",
                "Profile" => "bi-person-gear",
                _ => "bi-clock-history"
            },
            Tone = a.Success ? "info" : "danger",
            Title = $"{a.Action}",
            Detail = $"{a.UserName ?? "system"} · {a.Detail}",
            WhenUtc = a.CreatedAtUtc
        }).ToList();

        var vm = new AdminDashboardViewModel
        {
            UserCount = userCount,
            CourseCount = courseCount,
            EnrollmentCount = enrollmentCount,
            AssignmentCount = assignmentCount,
            SubmissionCount = submissionCount,
            MessageCount = messageCount,
            StudentCount = students.Count,
            LecturerCount = lecturers.Count,
            AdminCount = admins.Count,
            PopularCourses = popular,
            EnrollmentTrend = trend,
            RecentActivity = activity
        };
        return View(vm);
    }

    private sealed record CalendarItem(DateTime WhenUtc, string Title, string Detail, DashboardCalendarEventKind Kind);

    private static DashboardCalendarModel BuildCalendar(int year, int month, IEnumerable<CalendarItem> items)
    {
        var dict = new Dictionary<DateOnly, List<DashboardCalendarEvent>>();
        foreach (var it in items.OrderBy(x => x.WhenUtc))
        {
            var local = it.WhenUtc.ToLocalTime();
            if (local.Year != year || local.Month != month)
                continue;

            var d = DateOnly.FromDateTime(local);
            if (!dict.TryGetValue(d, out var list))
            {
                list = new List<DashboardCalendarEvent>();
                dict[d] = list;
            }

            list.Add(new DashboardCalendarEvent
            {
                Title = it.Title,
                Detail = it.Detail,
                Kind = it.Kind
            });
        }

        return new DashboardCalendarModel { Year = year, Month = month, EventsByDay = dict };
    }
}
