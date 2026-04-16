using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReportsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        if (User.IsInRole(AppRoles.Administrator))
            return View();

        if (User.IsInRole(AppRoles.Lecturer))
            return RedirectToAction(nameof(LecturerWorkload));

        return Forbid();
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> CoursePopularity()
    {
        var rows = await _db.Courses
            .AsNoTracking()
            .Select(c => new ReportCoursePopularityRow
            {
                Code = c.Code,
                Name = c.Name,
                EnrollmentCount = c.Enrollments.Count,
                Capacity = c.EnrollmentLimit,
                FillRate = c.EnrollmentLimit == 0 ? 0 : (double)c.Enrollments.Count / c.EnrollmentLimit
            })
            .OrderByDescending(r => r.EnrollmentCount)
            .ToListAsync();

        return View(rows);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> StudentPerformance()
    {
        var students = await _userManager.GetUsersInRoleAsync(AppRoles.Student);
        var studentIds = students.Select(s => s.Id).ToList();

        var graded = await _db.Submissions
            .Where(s => studentIds.Contains(s.StudentId) && s.Status == SubmissionStatus.Graded && s.Grade != null)
            .Join(_db.Assignments, s => s.AssignmentId, a => a.AssignmentId, (s, a) => new { s.StudentId, s.Grade, a.MaxPoints })
            .ToListAsync();

        var rows = studentIds.Select(id =>
        {
            var mine = graded.Where(x => x.StudentId == id).ToList();
            if (mine.Count == 0)
                return new ReportStudentPerformanceRow
                {
                    StudentName = students.First(u => u.Id == id).FullName,
                    Email = students.First(u => u.Id == id).Email ?? "",
                    GradedCount = 0,
                    AveragePercent = null
                };

            var avg = mine.Average(x => (double)(x.Grade!.Value / x.MaxPoints * 100m));
            return new ReportStudentPerformanceRow
            {
                StudentName = students.First(u => u.Id == id).FullName,
                Email = students.First(u => u.Id == id).Email ?? "",
                GradedCount = mine.Count,
                AveragePercent = avg
            };
        }).OrderByDescending(r => r.AveragePercent).ToList();

        return View(rows);
    }

    [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Lecturer}")]
    public async Task<IActionResult> LecturerWorkload()
    {
        var lecturers = await _userManager.GetUsersInRoleAsync(AppRoles.Lecturer);
        var me = _userManager.GetUserId(User);

        if (User.IsInRole(AppRoles.Lecturer))
            lecturers = lecturers.Where(u => u.Id == me).ToList();

        var rows = new List<ReportLecturerWorkloadRow>();
        foreach (var lec in lecturers)
        {
            var courseIds = await _db.Courses
                .Where(c => c.LecturerId == lec.Id)
                .Select(c => c.CourseId)
                .ToListAsync();

            var assignmentIds = await _db.Assignments
                .Where(a => courseIds.Contains(a.CourseId))
                .Select(a => a.AssignmentId)
                .ToListAsync();

            var submissionCount = await _db.Submissions
                .CountAsync(s => assignmentIds.Contains(s.AssignmentId));

            rows.Add(new ReportLecturerWorkloadRow
            {
                LecturerName = lec.FullName,
                Email = lec.Email ?? "",
                CourseCount = courseIds.Count,
                AssignmentCount = assignmentIds.Count,
                SubmissionCount = submissionCount
            });
        }

        rows = rows.OrderByDescending(r => r.SubmissionCount).ToList();
        return View(rows);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Enrollments()
    {
        var rows = await (
            from e in _db.Enrollments
            join c in _db.Courses on e.CourseId equals c.CourseId
            join s in _db.Users on e.StudentId equals s.Id
            orderby e.EnrolledAtUtc descending
            select new ReportEnrollmentRow
            {
                DateUtc = e.EnrolledAtUtc,
                Student = s.FullName,
                CourseCode = c.Code,
                CourseName = c.Name
            }).ToListAsync();

        return View(rows);
    }
}
