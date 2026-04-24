using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Infrastructure;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

public class CoursesController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailService _emailService;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment env,
        IEmailService emailService,
        ILogger<CoursesController> logger)
    {
        _db = db;
        _userManager = userManager;
        _env = env;
        _emailService = emailService;
        _logger = logger;
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Index()
    {
        var list = await _db.Courses
            .AsNoTracking()
            .Include(c => c.Lecturer)
            .Include(c => c.Prerequisite)
            .OrderBy(c => c.Code)
            .ToListAsync();
        return View(list);
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> MyCourses()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return Unauthorized();

        var list = await _db.Courses
            .AsNoTracking()
            .Where(c => c.LecturerId == userId)
            .Include(c => c.Enrollments)
            .OrderBy(c => c.Code)
            .ToListAsync();
        return View(list);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);
        var course = await _db.Courses
            .Include(c => c.Lecturer)
            .Include(c => c.Prerequisite)
            .Include(c => c.Enrollments)
            .Include(c => c.Materials).ThenInclude(m => m.UploadedBy)
            .Include(c => c.Assignments)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
            return NotFound();

        if (User.IsInRole(AppRoles.Lecturer) && course.LecturerId != userId)
            return Forbid();

        if (User.IsInRole(AppRoles.Student))
        {
            var isEnrolled = await _db.Enrollments.AnyAsync(e => e.CourseId == id && e.StudentId == userId);
            if (!isEnrolled)
                return Forbid();

            var mySubs = await (
                from s in _db.Submissions.AsNoTracking()
                join a in _db.Assignments on s.AssignmentId equals a.AssignmentId
                where s.StudentId == userId && a.CourseId == id
                orderby a.DueDateUtc
                select new CourseStudentSubmissionRow
                {
                    AssignmentId = a.AssignmentId,
                    Title = a.Title,
                    DueDateUtc = a.DueDateUtc,
                    Status = s.Status.ToString(),
                    Grade = s.Grade,
                    MaxPoints = a.MaxPoints
                }).ToListAsync();

            ViewBag.MySubmissions = mySubs;
        }

        ViewBag.CanUpload = User.IsInRole(AppRoles.Lecturer) && course.LecturerId == userId;
        return View(course);
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Create()
    {
        await PopulateLecturersAndPrereqsAsync();
        return View(new CourseInputViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Create(CourseInputViewModel model)
    {
        if (!await _db.Users.AnyAsync(u => u.Id == model.LecturerId))
            ModelState.AddModelError(nameof(model.LecturerId), "Lecturer not found.");

        if (model.PrerequisiteId.HasValue && !await _db.Courses.AnyAsync(c => c.CourseId == model.PrerequisiteId))
            ModelState.AddModelError(nameof(model.PrerequisiteId), "Invalid prerequisite.");

        if (!ModelState.IsValid)
        {
            await PopulateLecturersAndPrereqsAsync();
            return View(model);
        }

        if (await _db.Courses.AnyAsync(c => c.Code == model.Code))
        {
            ModelState.AddModelError(nameof(model.Code), "Course code already exists.");
            await PopulateLecturersAndPrereqsAsync();
            return View(model);
        }

        var entity = new Course
        {
            Code = model.Code.Trim(),
            Name = model.Name.Trim(),
            Description = model.Description?.Trim(),
            Credits = model.Credits,
            EnrollmentLimit = model.EnrollmentLimit,
            LecturerId = model.LecturerId,
            PrerequisiteId = model.PrerequisiteId
        };

        _db.Courses.Add(entity);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Course created.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Edit(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null)
            return NotFound();

        await PopulateLecturersAndPrereqsAsync(id);
        var vm = new CourseInputViewModel
        {
            CourseId = course.CourseId,
            Code = course.Code,
            Name = course.Name,
            Description = course.Description,
            Credits = course.Credits,
            EnrollmentLimit = course.EnrollmentLimit,
            LecturerId = course.LecturerId,
            PrerequisiteId = course.PrerequisiteId
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Edit(int id, CourseInputViewModel model)
    {
        if (id != model.CourseId)
            return BadRequest();

        var course = await _db.Courses.FindAsync(id);
        if (course == null)
            return NotFound();

        if (!await _db.Users.AnyAsync(u => u.Id == model.LecturerId))
            ModelState.AddModelError(nameof(model.LecturerId), "Lecturer not found.");

        if (model.PrerequisiteId == id)
            ModelState.AddModelError(nameof(model.PrerequisiteId), "A course cannot be its own prerequisite.");

        if (model.PrerequisiteId.HasValue && !await _db.Courses.AnyAsync(c => c.CourseId == model.PrerequisiteId))
            ModelState.AddModelError(nameof(model.PrerequisiteId), "Invalid prerequisite.");

        if (!ModelState.IsValid)
        {
            await PopulateLecturersAndPrereqsAsync(id);
            return View(model);
        }

        var codeTaken = await _db.Courses.AnyAsync(c => c.Code == model.Code && c.CourseId != id);
        if (codeTaken)
        {
            ModelState.AddModelError(nameof(model.Code), "Course code already exists.");
            await PopulateLecturersAndPrereqsAsync(id);
            return View(model);
        }

        course.Code = model.Code.Trim();
        course.Name = model.Name.Trim();
        course.Description = model.Description?.Trim();
        course.Credits = model.Credits;
        course.EnrollmentLimit = model.EnrollmentLimit;
        course.LecturerId = model.LecturerId;
        course.PrerequisiteId = model.PrerequisiteId;

        await _db.SaveChangesAsync();
        TempData["Success"] = "Course updated.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var course = await _db.Courses
            .AsNoTracking()
            .Include(c => c.Lecturer)
            .FirstOrDefaultAsync(c => c.CourseId == id);
        if (course == null)
            return NotFound();
        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Administrator)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null)
            return NotFound();

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Course deleted.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> UploadMaterial(MaterialUploadViewModel model)
    {
        var userId = _userManager.GetUserId(User);
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseId == model.CourseId);
        if (course == null)
            return NotFound();

        if (course.LecturerId != userId)
            return Forbid();

        if (model.File == null || model.File.Length == 0)
        {
            ModelState.AddModelError(nameof(model.File), "A file is required.");
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Upload failed. Check the file and title.";
            return RedirectToAction(nameof(Details), new { id = model.CourseId });
        }

            var saved = await UploadedFileStore.SaveAsync(_env, model.File!, "materials");
        if (saved == null)
        {
            TempData["Error"] = "File could not be saved (size or type).";
            return RedirectToAction(nameof(Details), new { id = model.CourseId });
        }

        _db.CourseMaterials.Add(new CourseMaterial
        {
            CourseId = model.CourseId,
            Title = model.Title.Trim(),
            StoredFileName = saved.Value.StoredName,
            ContentType = saved.Value.ContentType,
            FileSizeBytes = saved.Value.Size,
            UploadedById = userId!
        });
        await _db.SaveChangesAsync();

        var students = await _db.Enrollments
            .Where(e => e.CourseId == model.CourseId)
            .Select(e => e.Student)
            .Where(s => s.Email != null)
            .ToListAsync();

        var lecturer = await _userManager.GetUserAsync(User);
        var courseUrl = Url.Action(nameof(Details), "Courses", new { id = model.CourseId }, protocol: Request.Scheme);

        foreach (var student in students)
        {
            try
            {
                await _emailService.SendAsync(
                    student.Email!,
                    $"New lesson uploaded - {course.Code}",
                    EmailTemplates.BuildMaterialUploadEmail(
                        student.FullName,
                        course.Code,
                        course.Name,
                        model.Title,
                        lecturer?.FullName,
                        courseUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Material upload email failed for {Email}", student.Email);
            }
        }

        TempData["Success"] = "Material uploaded.";
        return RedirectToAction(nameof(Details), new { id = model.CourseId });
    }

    private async Task PopulateLecturersAndPrereqsAsync(int? editingCourseId = null)
    {
        var lecturers = await _userManager.GetUsersInRoleAsync(AppRoles.Lecturer);
        ViewBag.Lecturers = lecturers.OrderBy(u => u.FullName).Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(
            $"{u.FullName} ({u.Email})", u.Id)).ToList();

        var prereqQuery = _db.Courses.AsNoTracking().AsQueryable();
        if (editingCourseId.HasValue)
            prereqQuery = prereqQuery.Where(c => c.CourseId != editingCourseId.Value);

        ViewBag.Prerequisites = await prereqQuery
            .OrderBy(c => c.Code)
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem($"{c.Code} - {c.Name}", c.CourseId.ToString()))
            .ToListAsync();
    }
}
