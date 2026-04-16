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
            select new { c.Code, a.Title, s.Grade, a.MaxPoints, s.Feedback }).Take(10).ToListAsync();

        var grades = gradesRaw.Select(x => new StudentDashboardViewModel.GradeRow
        {
            CourseCode = x.Code,
            Assignment = x.Title,
            Grade = x.Grade,
            MaxPoints = x.MaxPoints,
            FeedbackShort = x.Feedback == null
                ? null
                : (x.Feedback.Length > 80 ? x.Feedback.Substring(0, 80) + "..." : x.Feedback)
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

        var calItems = assnInMonth.Select(a => (a.DueDateUtc, a.Title, a.Course.Code));
        var calendar = BuildAssignmentCalendar(calNow.Year, calNow.Month, calItems);

        var vm = new StudentDashboardViewModel
        {
            EnrolledCourses = enrolled,
            UpcomingDeadlines = deadlineRows,
            RecentGrades = grades,
            UnreadMessages = unread,
            AssignmentCalendar = calendar
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

        var assignmentIds = await _db.Assignments
            .Where(a => courseIds.Contains(a.CourseId))
            .Select(a => a.AssignmentId)
            .ToListAsync();

        var subs = await _db.Submissions
            .Where(s => assignmentIds.Contains(s.AssignmentId))
            .ToListAsync();

        var pending = subs.Count(s => s.Status == SubmissionStatus.Submitted);
        var graded = subs.Count(s => s.Status == SubmissionStatus.Graded);

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

        var calItems = assnInMonth.Select(a => (a.DueDateUtc, a.Title, a.Course.Code));
        var calendar = BuildAssignmentCalendar(calNow.Year, calNow.Month, calItems);

        var vm = new LecturerDashboardViewModel
        {
            Courses = courseRows,
            PendingSubmissions = pending,
            GradedSubmissions = graded,
            UnreadMessages = unread,
            AssignmentCalendar = calendar
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

        var vm = new AdminDashboardViewModel
        {
            UserCount = userCount,
            CourseCount = courseCount,
            EnrollmentCount = enrollmentCount,
            AssignmentCount = assignmentCount,
            PopularCourses = popular
        };
        return View(vm);
    }

    private static DashboardCalendarModel BuildAssignmentCalendar(int year, int month, IEnumerable<(DateTime DueDateUtc, string Title, string Detail)> items)
    {
        var dict = new Dictionary<DateOnly, List<DashboardCalendarEvent>>();
        foreach (var it in items)
        {
            var local = it.DueDateUtc.ToLocalTime();
            if (local.Year != year || local.Month != month)
                continue;

            var d = DateOnly.FromDateTime(local);
            if (!dict.TryGetValue(d, out var list))
            {
                list = new List<DashboardCalendarEvent>();
                dict[d] = list;
            }

            list.Add(new DashboardCalendarEvent { Title = it.Title, Detail = it.Detail });
        }

        return new DashboardCalendarModel { Year = year, Month = month, EventsByDay = dict };
    }
}
