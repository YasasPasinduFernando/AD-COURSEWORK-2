using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Infrastructure;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

[Authorize(Roles = AppRoles.Student)]
public class EnrollmentsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditLogger _audit;

    public EnrollmentsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IAuditLogger audit)
    {
        _db = db;
        _userManager = userManager;
        _audit = audit;
    }

    public async Task<IActionResult> Browse()
    {
        var userId = _userManager.GetUserId(User)!;

        var enrolledIds = await _db.Enrollments
            .Where(e => e.StudentId == userId)
            .Select(e => e.CourseId)
            .ToListAsync();

        var courses = await _db.Courses
            .AsNoTracking()
            .Include(c => c.Lecturer)
            .Include(c => c.Prerequisite)
            .Include(c => c.Enrollments)
            .OrderBy(c => c.Code)
            .ToListAsync();

        var rows = new List<CourseBrowseRowViewModel>();
        foreach (var c in courses)
        {
            var count = c.Enrollments.Count;
            var isEnrolled = enrolledIds.Contains(c.CourseId);
            string? block = null;
            var canEnroll = false;

            if (isEnrolled)
                block = "Already enrolled.";
            else if (count >= c.EnrollmentLimit)
                block = "Course is full.";
            else if (c.PrerequisiteId.HasValue)
            {
                var hasPrereq = enrolledIds.Contains(c.PrerequisiteId.Value);
                if (!hasPrereq)
                {
                    var prereq = courses.FirstOrDefault(x => x.CourseId == c.PrerequisiteId);
                    block = prereq == null
                        ? "Prerequisite required."
                        : $"Complete enrollment in {prereq.Code} first.";
                }
            }

            if (block == null)
            {
                canEnroll = true;
            }

            rows.Add(new CourseBrowseRowViewModel
            {
                CourseId = c.CourseId,
                Code = c.Code,
                Name = c.Name,
                Credits = c.Credits,
                LecturerName = c.Lecturer.FullName,
                PrerequisiteSummary = c.Prerequisite == null ? null : $"{c.Prerequisite.Code} - {c.Prerequisite.Name}",
                CurrentEnrollmentCount = count,
                EnrollmentLimit = c.EnrollmentLimit,
                IsEnrolled = isEnrolled,
                CanEnroll = canEnroll,
                BlockReason = block
            });
        }

        return View(rows);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int id)
    {
        var userId = _userManager.GetUserId(User)!;

        var course = await _db.Courses
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
        {
            TempData["Error"] = "Course not found.";
            return RedirectToAction(nameof(Browse));
        }

        if (await _db.Enrollments.AnyAsync(e => e.StudentId == userId && e.CourseId == id))
        {
            TempData["Error"] = "You are already enrolled in this course.";
            return RedirectToAction(nameof(Browse));
        }

        if (course.Enrollments.Count >= course.EnrollmentLimit)
        {
            TempData["Error"] = "This course has reached its enrollment limit.";
            return RedirectToAction(nameof(Browse));
        }

        if (course.PrerequisiteId.HasValue)
        {
            var ok = await _db.Enrollments.AnyAsync(e =>
                e.StudentId == userId && e.CourseId == course.PrerequisiteId.Value);
            if (!ok)
            {
                TempData["Error"] = "You must be enrolled in the prerequisite course first.";
                return RedirectToAction(nameof(Browse));
            }
        }

        _db.Enrollments.Add(new Enrollment
        {
            StudentId = userId,
            CourseId = id,
            EnrolledAtUtc = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        await _audit.LogAsync(AuditCategories.Enrollment, "Enroll",
            $"course={course.Code}");
        TempData["Success"] = $"Enrolled in {course.Code}.";
        return RedirectToAction(nameof(Browse));
    }
}
