using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Infrastructure;
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

    // ============================================================
    // Course popularity
    // ============================================================
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> CoursePopularity()
    {
        var rows = await GetCoursePopularityAsync();
        return View(rows);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> CoursePopularityCsv()
    {
        var rows = await GetCoursePopularityAsync();
        var bytes = CsvWriter.Build(
            new[] { "Code", "Name", "EnrollmentCount", "Capacity", "FillRatePercent" },
            rows.Select(r => new object?[] { r.Code, r.Name, r.EnrollmentCount, r.Capacity, Math.Round(r.FillRate * 100, 1) }));
        return File(bytes, "text/csv", $"course-popularity-{DateTime.UtcNow:yyyyMMddHHmm}.csv");
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> CoursePopularityPdf()
    {
        var rows = await GetCoursePopularityAsync();
        var totalEnroll = rows.Sum(r => r.EnrollmentCount);
        var totalCap = rows.Sum(r => r.Capacity);
        var avgFill = rows.Count > 0 ? rows.Average(r => r.FillRate) * 100 : 0;
        var pdf = PdfReport.Build(
            "Course Popularity",
            "Enrollment headcount and fill rate per course.",
            new[]
            {
                new PdfReport.Chip("Courses", rows.Count.ToString()),
                new PdfReport.Chip("Enrollments", totalEnroll.ToString()),
                new PdfReport.Chip("Capacity", totalCap.ToString()),
                new PdfReport.Chip("Avg fill", $"{avgFill:0.#}%")
            },
            new PdfReport.TableSpec
            {
                Headers = new[] { "Code", "Course name", "Enrolled", "Capacity", "Fill rate" },
                RelativeWidths = new[] { 1f, 4f, 1.2f, 1.2f, 1.5f },
                RightAlign = new[] { false, false, true, true, true },
                Rows = rows.Select(r => new object?[]
                {
                    r.Code, r.Name, r.EnrollmentCount, r.Capacity, $"{Math.Round(r.FillRate * 100, 1)}%"
                }).ToList()
            },
            footerNote: "Generated automatically by UniManage Reports.");
        return File(pdf, "application/pdf", $"course-popularity-{DateTime.UtcNow:yyyyMMddHHmm}.pdf");
    }

    // ============================================================
    // Student performance
    // ============================================================
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> StudentPerformance()
    {
        var rows = await GetStudentPerformanceAsync();
        return View(rows);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> StudentPerformanceCsv()
    {
        var rows = await GetStudentPerformanceAsync();
        var bytes = CsvWriter.Build(
            new[] { "Student", "Email", "GradedCount", "AveragePercent" },
            rows.Select(r => new object?[] { r.StudentName, r.Email, r.GradedCount, r.AveragePercent.HasValue ? Math.Round(r.AveragePercent.Value, 1) : (object?)null }));
        return File(bytes, "text/csv", $"student-performance-{DateTime.UtcNow:yyyyMMddHHmm}.csv");
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> StudentPerformancePdf()
    {
        var rows = await GetStudentPerformanceAsync();
        var graded = rows.Count(r => r.AveragePercent.HasValue);
        var avg = rows.Where(r => r.AveragePercent.HasValue).Select(r => r.AveragePercent!.Value).DefaultIfEmpty(0).Average();
        var pdf = PdfReport.Build(
            "Student Performance",
            "Average graded score across all submissions per student.",
            new[]
            {
                new PdfReport.Chip("Students", rows.Count.ToString()),
                new PdfReport.Chip("With grades", graded.ToString()),
                new PdfReport.Chip("Cohort avg", $"{avg:0.#}%")
            },
            new PdfReport.TableSpec
            {
                Headers = new[] { "#", "Student", "Email", "Graded", "Average" },
                RelativeWidths = new[] { 0.5f, 2.5f, 3f, 1f, 1.2f },
                RightAlign = new[] { true, false, false, true, true },
                Rows = rows.Select((r, i) => new object?[]
                {
                    i + 1, r.StudentName, r.Email, r.GradedCount,
                    r.AveragePercent.HasValue ? $"{Math.Round(r.AveragePercent.Value, 1)}%" : "—"
                }).ToList()
            });
        return File(pdf, "application/pdf", $"student-performance-{DateTime.UtcNow:yyyyMMddHHmm}.pdf");
    }

    // ============================================================
    // Lecturer workload
    // ============================================================
    [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Lecturer}")]
    public async Task<IActionResult> LecturerWorkload()
    {
        var rows = await GetLecturerWorkloadAsync();
        return View(rows);
    }

    [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Lecturer}")]
    public async Task<IActionResult> LecturerWorkloadCsv()
    {
        var rows = await GetLecturerWorkloadAsync();
        var bytes = CsvWriter.Build(
            new[] { "Lecturer", "Email", "Courses", "Assignments", "Submissions" },
            rows.Select(r => new object?[] { r.LecturerName, r.Email, r.CourseCount, r.AssignmentCount, r.SubmissionCount }));
        return File(bytes, "text/csv", $"lecturer-workload-{DateTime.UtcNow:yyyyMMddHHmm}.csv");
    }

    [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Lecturer}")]
    public async Task<IActionResult> LecturerWorkloadPdf()
    {
        var rows = await GetLecturerWorkloadAsync();
        var pdf = PdfReport.Build(
            "Lecturer Workload",
            "Courses, assignments and submissions handled per lecturer.",
            new[]
            {
                new PdfReport.Chip("Lecturers", rows.Count.ToString()),
                new PdfReport.Chip("Courses", rows.Sum(r => r.CourseCount).ToString()),
                new PdfReport.Chip("Submissions", rows.Sum(r => r.SubmissionCount).ToString())
            },
            new PdfReport.TableSpec
            {
                Headers = new[] { "Lecturer", "Email", "Courses", "Assignments", "Submissions" },
                RelativeWidths = new[] { 2.5f, 3f, 1f, 1.3f, 1.3f },
                RightAlign = new[] { false, false, true, true, true },
                Rows = rows.Select(r => new object?[]
                {
                    r.LecturerName, r.Email, r.CourseCount, r.AssignmentCount, r.SubmissionCount
                }).ToList()
            });
        return File(pdf, "application/pdf", $"lecturer-workload-{DateTime.UtcNow:yyyyMMddHHmm}.pdf");
    }

    // ============================================================
    // Enrollments timeline
    // ============================================================
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Enrollments()
    {
        var rows = await GetEnrollmentRowsAsync();
        return View(rows);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> EnrollmentsCsv()
    {
        var rows = await GetEnrollmentRowsAsync();
        var bytes = CsvWriter.Build(
            new[] { "DateUtc", "Student", "CourseCode", "CourseName" },
            rows.Select(r => new object?[] { r.DateUtc, r.Student, r.CourseCode, r.CourseName }));
        return File(bytes, "text/csv", $"enrollments-{DateTime.UtcNow:yyyyMMddHHmm}.csv");
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> EnrollmentsPdf()
    {
        var rows = await GetEnrollmentRowsAsync();
        var pdf = PdfReport.Build(
            "Enrollment Timeline",
            "All enrollments ordered by most recent first.",
            new[]
            {
                new PdfReport.Chip("Total", rows.Count.ToString()),
                new PdfReport.Chip("Latest", rows.FirstOrDefault()?.DateUtc.ToLocalTime().ToString("MMM d, yyyy") ?? "—"),
                new PdfReport.Chip("Earliest", rows.LastOrDefault()?.DateUtc.ToLocalTime().ToString("MMM d, yyyy") ?? "—")
            },
            new PdfReport.TableSpec
            {
                Headers = new[] { "Date", "Student", "Code", "Course" },
                RelativeWidths = new[] { 1.6f, 2.5f, 1f, 4f },
                Rows = rows.Select(r => new object?[]
                {
                    r.DateUtc, r.Student, r.CourseCode, r.CourseName
                }).ToList()
            });
        return File(pdf, "application/pdf", $"enrollments-{DateTime.UtcNow:yyyyMMddHHmm}.pdf");
    }

    // ============================================================
    // NEW: Pass / Fail analysis
    // ============================================================
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> PassFail(int passMark = 50)
    {
        passMark = Math.Clamp(passMark, 1, 99);
        var vm = await GetPassFailAsync(passMark);
        return View(vm);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> PassFailCsv(int passMark = 50)
    {
        passMark = Math.Clamp(passMark, 1, 99);
        var vm = await GetPassFailAsync(passMark);
        var bytes = CsvWriter.Build(
            new[] { "Code", "Course", "Graded", "Pass", "Fail", "PassRatePercent", "AveragePercent" },
            vm.Rows.Select(r => new object?[]
            {
                r.CourseCode, r.CourseName, r.GradedCount, r.PassCount, r.FailCount,
                Math.Round(r.PassRate, 1), Math.Round(r.AveragePercent, 1)
            }));
        return File(bytes, "text/csv", $"pass-fail-{passMark}pct-{DateTime.UtcNow:yyyyMMddHHmm}.csv");
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> PassFailPdf(int passMark = 50)
    {
        passMark = Math.Clamp(passMark, 1, 99);
        var vm = await GetPassFailAsync(passMark);
        var pdf = PdfReport.Build(
            "Pass / Fail Analysis",
            $"Performance breakdown using a {passMark}% pass mark across all graded submissions.",
            new[]
            {
                new PdfReport.Chip("Graded", vm.TotalGraded.ToString()),
                new PdfReport.Chip("Passes", vm.TotalPass.ToString()),
                new PdfReport.Chip("Fails", vm.TotalFail.ToString()),
                new PdfReport.Chip("Pass rate", $"{vm.OverallPassRate:0.#}%")
            },
            new PdfReport.TableSpec
            {
                Headers = new[] { "Code", "Course", "Graded", "Pass", "Fail", "Pass %", "Avg %" },
                RelativeWidths = new[] { 1f, 3.5f, 0.9f, 0.9f, 0.9f, 1.1f, 1.1f },
                RightAlign = new[] { false, false, true, true, true, true, true },
                Rows = vm.Rows.Select(r => new object?[]
                {
                    r.CourseCode, r.CourseName, r.GradedCount, r.PassCount, r.FailCount,
                    $"{Math.Round(r.PassRate, 1)}%", $"{Math.Round(r.AveragePercent, 1)}%"
                }).ToList()
            },
            footerNote: $"Pass mark: ≥ {passMark}%. Includes only assignments with graded submissions.");
        return File(pdf, "application/pdf", $"pass-fail-{passMark}pct-{DateTime.UtcNow:yyyyMMddHHmm}.pdf");
    }

    // ============================================================
    // NEW: Assignment attendance (turn-in rate)
    // ============================================================
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> AssignmentAttendance()
    {
        var vm = await GetAssignmentAttendanceAsync();
        return View(vm);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> AssignmentAttendanceCsv()
    {
        var vm = await GetAssignmentAttendanceAsync();
        var bytes = CsvWriter.Build(
            new[] { "Code", "Assignment", "DueDateUtc", "Enrolled", "Submitted", "Missing", "AttendanceRatePercent" },
            vm.Rows.Select(r => new object?[]
            {
                r.CourseCode, r.AssignmentTitle, r.DueDateUtc,
                r.Enrolled, r.Submitted, r.Missing, Math.Round(r.AttendanceRate, 1)
            }));
        return File(bytes, "text/csv", $"assignment-attendance-{DateTime.UtcNow:yyyyMMddHHmm}.csv");
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> AssignmentAttendancePdf()
    {
        var vm = await GetAssignmentAttendanceAsync();
        var pdf = PdfReport.Build(
            "Assignment Attendance",
            "Turn-in rate per assignment: how many enrolled students submitted before/after the due date.",
            new[]
            {
                new PdfReport.Chip("Assignments", vm.Rows.Count.ToString()),
                new PdfReport.Chip("Submissions", vm.TotalSubmittedAcross.ToString()),
                new PdfReport.Chip("Missing", vm.TotalMissingAcross.ToString()),
                new PdfReport.Chip("Avg attendance", $"{vm.OverallAttendance:0.#}%")
            },
            new PdfReport.TableSpec
            {
                Headers = new[] { "Code", "Assignment", "Due", "Enrolled", "Submitted", "Missing", "Rate" },
                RelativeWidths = new[] { 0.9f, 3.5f, 1.6f, 1f, 1f, 1f, 1.1f },
                RightAlign = new[] { false, false, false, true, true, true, true },
                Rows = vm.Rows.Select(r => new object?[]
                {
                    r.CourseCode, r.AssignmentTitle, r.DueDateUtc.ToLocalTime().ToString("yyyy-MM-dd"),
                    r.Enrolled, r.Submitted, r.Missing, $"{Math.Round(r.AttendanceRate, 1)}%"
                }).ToList()
            },
            footerNote: "Attendance = unique students who submitted (any status) ÷ enrolled students for the course.");
        return File(pdf, "application/pdf", $"assignment-attendance-{DateTime.UtcNow:yyyyMMddHHmm}.pdf");
    }

    // ============================================================
    // Data fetchers
    // ============================================================
    private async Task<List<ReportCoursePopularityRow>> GetCoursePopularityAsync()
    {
        return await _db.Courses
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
    }

    private async Task<List<ReportStudentPerformanceRow>> GetStudentPerformanceAsync()
    {
        var students = await _userManager.GetUsersInRoleAsync(AppRoles.Student);
        var studentIds = students.Select(s => s.Id).ToList();

        var graded = await _db.Submissions
            .Where(s => studentIds.Contains(s.StudentId) && s.Status == SubmissionStatus.Graded && s.Grade != null)
            .Join(_db.Assignments, s => s.AssignmentId, a => a.AssignmentId, (s, a) => new { s.StudentId, s.Grade, a.MaxPoints })
            .ToListAsync();

        return studentIds.Select(id =>
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
    }

    private async Task<List<ReportLecturerWorkloadRow>> GetLecturerWorkloadAsync()
    {
        var lecturers = await _userManager.GetUsersInRoleAsync(AppRoles.Lecturer);
        var me = _userManager.GetUserId(User);

        if (User.IsInRole(AppRoles.Lecturer) && !User.IsInRole(AppRoles.Administrator))
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

        return rows.OrderByDescending(r => r.SubmissionCount).ToList();
    }

    private async Task<List<ReportEnrollmentRow>> GetEnrollmentRowsAsync()
    {
        return await (
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
    }

    private async Task<ReportPassFailViewModel> GetPassFailAsync(int passMark)
    {
        var data = await (
            from s in _db.Submissions
            join a in _db.Assignments on s.AssignmentId equals a.AssignmentId
            join c in _db.Courses on a.CourseId equals c.CourseId
            where s.Status == SubmissionStatus.Graded && s.Grade != null && a.MaxPoints > 0
            select new
            {
                c.CourseId,
                c.Code,
                c.Name,
                Percent = (double)(s.Grade!.Value / a.MaxPoints * 100m)
            }).ToListAsync();

        var rows = data
            .GroupBy(x => new { x.CourseId, x.Code, x.Name })
            .Select(g => new ReportPassFailRow
            {
                CourseCode = g.Key.Code,
                CourseName = g.Key.Name,
                GradedCount = g.Count(),
                PassCount = g.Count(x => x.Percent >= passMark),
                FailCount = g.Count(x => x.Percent < passMark),
                AveragePercent = g.Average(x => x.Percent)
            })
            .OrderByDescending(r => r.PassRate)
            .ThenByDescending(r => r.GradedCount)
            .ToList();

        return new ReportPassFailViewModel { Rows = rows, PassMark = passMark };
    }

    private async Task<ReportAttendanceViewModel> GetAssignmentAttendanceAsync()
    {
        var enrollmentByCourse = await _db.Enrollments
            .GroupBy(e => e.CourseId)
            .Select(g => new { CourseId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CourseId, x => x.Count);

        var assignments = await (
            from a in _db.Assignments.AsNoTracking()
            join c in _db.Courses on a.CourseId equals c.CourseId
            select new
            {
                a.AssignmentId,
                a.CourseId,
                CourseCode = c.Code,
                a.Title,
                a.DueDateUtc
            }).ToListAsync();

        var assignmentIds = assignments.Select(a => a.AssignmentId).ToList();
        var submissionsByAssn = await _db.Submissions
            .Where(s => assignmentIds.Contains(s.AssignmentId) && s.SubmittedAtUtc != null)
            .GroupBy(s => s.AssignmentId)
            .Select(g => new { AssignmentId = g.Key, Distinct = g.Select(s => s.StudentId).Distinct().Count() })
            .ToDictionaryAsync(x => x.AssignmentId, x => x.Distinct);

        var rows = assignments
            .Select(a => new ReportAttendanceRow
            {
                AssignmentId = a.AssignmentId,
                CourseCode = a.CourseCode,
                AssignmentTitle = a.Title,
                DueDateUtc = a.DueDateUtc,
                Enrolled = enrollmentByCourse.TryGetValue(a.CourseId, out var ec) ? ec : 0,
                Submitted = submissionsByAssn.TryGetValue(a.AssignmentId, out var sc) ? sc : 0
            })
            .OrderByDescending(r => r.DueDateUtc)
            .ToList();

        return new ReportAttendanceViewModel { Rows = rows };
    }
}
