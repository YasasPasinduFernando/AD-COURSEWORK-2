using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

public class AssignmentsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AssignmentsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> ForCourse(int id)
    {
        var userId = _userManager.GetUserId(User);
        var course = await _db.Courses
            .AsNoTracking()
            .Include(c => c.Assignments).ThenInclude(a => a.Submissions)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
            return NotFound();

        if (course.LecturerId != userId)
            return Forbid();

        var list = course.Assignments.OrderByDescending(a => a.DueDateUtc).ToList();
        ViewBag.CourseId = course.CourseId;
        ViewBag.CourseCode = course.Code;
        ViewBag.CourseName = course.Name;
        ViewBag.EnrolledCount = course.Enrollments.Count;
        return View(list);
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public IActionResult Create(int courseId)
    {
        return View(new AssignmentInputViewModel { CourseId = courseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Create(AssignmentInputViewModel model)
    {
        var userId = _userManager.GetUserId(User);
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseId == model.CourseId);
        if (course == null)
            return NotFound();

        if (course.LecturerId != userId)
            return Forbid();

        model.DueDateLocal = DateTime.SpecifyKind(model.DueDateLocal, DateTimeKind.Local);

        if (!ModelState.IsValid)
            return View(model);

        _db.Assignments.Add(new Assignment
        {
            CourseId = model.CourseId,
            Title = model.Title.Trim(),
            Description = model.Description?.Trim(),
            DueDateUtc = model.DueDateLocal.ToUniversalTime(),
            MaxPoints = model.MaxPoints,
            CreatedAtUtc = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        TempData["Success"] = "Assignment created.";
        return RedirectToAction(nameof(ForCourse), new { id = model.CourseId });
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = _userManager.GetUserId(User);
        var a = await _db.Assignments.Include(x => x.Course).FirstOrDefaultAsync(x => x.AssignmentId == id);
        if (a == null)
            return NotFound();

        if (a.Course.LecturerId != userId)
            return Forbid();

        var local = DateTime.SpecifyKind(a.DueDateUtc.ToLocalTime(), DateTimeKind.Local);
        var vm = new AssignmentInputViewModel
        {
            AssignmentId = a.AssignmentId,
            CourseId = a.CourseId,
            Title = a.Title,
            Description = a.Description,
            DueDateLocal = local,
            MaxPoints = a.MaxPoints
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Edit(int id, AssignmentInputViewModel model)
    {
        if (id != model.AssignmentId)
            return BadRequest();

        var userId = _userManager.GetUserId(User);
        var a = await _db.Assignments.Include(x => x.Course).FirstOrDefaultAsync(x => x.AssignmentId == id);
        if (a == null)
            return NotFound();

        if (a.Course.LecturerId != userId)
            return Forbid();

        model.DueDateLocal = DateTime.SpecifyKind(model.DueDateLocal, DateTimeKind.Local);

        if (!ModelState.IsValid)
            return View(model);

        a.Title = model.Title.Trim();
        a.Description = model.Description?.Trim();
        a.DueDateUtc = model.DueDateLocal.ToUniversalTime();
        a.MaxPoints = model.MaxPoints;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Assignment updated.";
        return RedirectToAction(nameof(ForCourse), new { id = a.CourseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = _userManager.GetUserId(User);
        var a = await _db.Assignments.Include(x => x.Course).FirstOrDefaultAsync(x => x.AssignmentId == id);
        if (a == null)
            return NotFound();

        if (a.Course.LecturerId != userId)
            return Forbid();

        var courseId = a.CourseId;
        _db.Assignments.Remove(a);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Assignment deleted.";
        return RedirectToAction(nameof(ForCourse), new { id = courseId });
    }

    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Mine()
    {
        var userId = _userManager.GetUserId(User)!;

        var courseIds = await _db.Enrollments
            .Where(e => e.StudentId == userId)
            .Select(e => e.CourseId)
            .ToListAsync();

        var assignments = await _db.Assignments
            .AsNoTracking()
            .Where(a => courseIds.Contains(a.CourseId))
            .Include(a => a.Course)
            .OrderBy(a => a.DueDateUtc)
            .ToListAsync();

        var subLookup = await _db.Submissions
            .Where(s => s.StudentId == userId)
            .ToDictionaryAsync(s => s.AssignmentId, s => s);

        ViewBag.Submissions = subLookup;
        return View(assignments);
    }
}
