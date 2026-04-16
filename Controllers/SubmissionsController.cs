using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Infrastructure;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

public class SubmissionsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public SubmissionsController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment env)
    {
        _db = db;
        _userManager = userManager;
        _env = env;
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> ForAssignment(int id)
    {
        var userId = _userManager.GetUserId(User);
        var assignment = await _db.Assignments
            .Include(a => a.Course)
            .Include(a => a.Submissions).ThenInclude(s => s.Student)
            .FirstOrDefaultAsync(a => a.AssignmentId == id);

        if (assignment == null)
            return NotFound();

        if (assignment.Course.LecturerId != userId)
            return Forbid();

        return View(assignment);
    }

    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Submit(int id)
    {
        var userId = _userManager.GetUserId(User)!;

        var assignment = await _db.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.AssignmentId == id);

        if (assignment == null)
            return NotFound();

        var enrolled = await _db.Enrollments.AnyAsync(e =>
            e.StudentId == userId && e.CourseId == assignment.CourseId);
        if (!enrolled)
            return Forbid();

        var submission = await _db.Submissions
            .FirstOrDefaultAsync(s => s.AssignmentId == id && s.StudentId == userId);

        if (submission == null)
        {
            submission = new Submission
            {
                AssignmentId = id,
                StudentId = userId,
                Status = SubmissionStatus.NotSubmitted
            };
            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();
        }

        var vm = new SubmissionSubmitViewModel
        {
            SubmissionId = submission.SubmissionId,
            AssignmentId = assignment.AssignmentId,
            CourseCode = assignment.Course.Code,
            AssignmentTitle = assignment.Title,
            DueDateUtc = assignment.DueDateUtc,
            TextContent = submission.TextContent,
            ExistingFileName = submission.StoredFileName,
            Status = submission.Status.ToString(),
            Grade = submission.Grade,
            Feedback = submission.Feedback
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Submit(int id, SubmissionSubmitViewModel model)
    {
        var userId = _userManager.GetUserId(User)!;

        var assignment = await _db.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.AssignmentId == id);

        if (assignment == null)
            return NotFound();

        if (id != model.AssignmentId)
            return BadRequest();

        var enrolled = await _db.Enrollments.AnyAsync(e =>
            e.StudentId == userId && e.CourseId == assignment.CourseId);
        if (!enrolled)
            return Forbid();

        var submission = await _db.Submissions
            .FirstOrDefaultAsync(s => s.AssignmentId == id && s.StudentId == userId);

        if (submission == null)
        {
            submission = new Submission
            {
                AssignmentId = id,
                StudentId = userId,
                Status = SubmissionStatus.NotSubmitted
            };
            _db.Submissions.Add(submission);
        }

        if (submission.Status == SubmissionStatus.Graded && submission.Grade.HasValue)
        {
            TempData["Error"] = "This submission has already been graded.";
            return RedirectToAction("Mine", "Assignments");
        }

        if (model.File == null && string.IsNullOrWhiteSpace(model.TextContent))
        {
            ModelState.AddModelError(string.Empty, "Provide text or attach a file.");
        }

        if (!ModelState.IsValid)
        {
            model.SubmissionId = submission.SubmissionId;
            model.CourseCode = assignment.Course.Code;
            model.AssignmentTitle = assignment.Title;
            model.DueDateUtc = assignment.DueDateUtc;
            model.ExistingFileName = submission.StoredFileName;
            model.Status = submission.Status.ToString();
            model.Grade = submission.Grade;
            model.Feedback = submission.Feedback;
            return View(model);
        }

        submission.TextContent = model.TextContent?.Trim();

        if (model.File != null && model.File.Length > 0)
        {
            UploadedFileStore.TryDelete(_env, "submissions", submission.StoredFileName);
            var saved = await UploadedFileStore.SaveAsync(_env, model.File, "submissions");
            if (saved == null)
            {
                ModelState.AddModelError(nameof(model.File), "Invalid file.");
                model.SubmissionId = submission.SubmissionId;
                model.CourseCode = assignment.Course.Code;
                model.AssignmentTitle = assignment.Title;
                model.DueDateUtc = assignment.DueDateUtc;
                model.ExistingFileName = submission.StoredFileName;
                return View(model);
            }

            submission.StoredFileName = saved.Value.StoredName;
            submission.ContentType = saved.Value.ContentType;
            submission.FileSizeBytes = saved.Value.Size;
        }

        submission.SubmittedAtUtc = DateTime.UtcNow;
        submission.Status = SubmissionStatus.Submitted;
        await _db.SaveChangesAsync();

        TempData["Success"] = "Submission saved.";
        return RedirectToAction("Mine", "Assignments");
    }

    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Grade(int id)
    {
        var userId = _userManager.GetUserId(User);
        var sub = await _db.Submissions
            .Include(s => s.Student)
            .Include(s => s.Assignment).ThenInclude(a => a.Course)
            .FirstOrDefaultAsync(s => s.SubmissionId == id);

        if (sub == null)
            return NotFound();

        if (sub.Assignment.Course.LecturerId != userId)
            return Forbid();

        var vm = new GradeSubmissionViewModel
        {
            SubmissionId = sub.SubmissionId,
            AssignmentId = sub.AssignmentId,
            StudentName = sub.Student.FullName,
            TextContent = sub.TextContent,
            FileName = sub.StoredFileName,
            MaxPoints = sub.Assignment.MaxPoints,
            Grade = sub.Grade ?? 0,
            Feedback = sub.Feedback,
            SubmittedAtUtc = sub.SubmittedAtUtc
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Lecturer)]
    public async Task<IActionResult> Grade(int id, GradeSubmissionViewModel model)
    {
        var userId = _userManager.GetUserId(User);
        if (id != model.SubmissionId)
            return BadRequest();

        var sub = await _db.Submissions
            .Include(s => s.Assignment).ThenInclude(a => a.Course)
            .FirstOrDefaultAsync(s => s.SubmissionId == id);

        if (sub == null)
            return NotFound();

        if (sub.Assignment.Course.LecturerId != userId)
            return Forbid();

        if (model.Grade < 0 || model.Grade > sub.Assignment.MaxPoints)
            ModelState.AddModelError(nameof(model.Grade), $"Grade must be between 0 and {sub.Assignment.MaxPoints}.");

        if (!ModelState.IsValid)
        {
            model.AssignmentId = sub.AssignmentId;
            model.StudentName = (await _userManager.FindByIdAsync(sub.StudentId))?.FullName ?? "";
            model.TextContent = sub.TextContent;
            model.FileName = sub.StoredFileName;
            model.MaxPoints = sub.Assignment.MaxPoints;
            model.SubmittedAtUtc = sub.SubmittedAtUtc;
            return View(model);
        }

        sub.Grade = model.Grade;
        sub.Feedback = model.Feedback?.Trim();
        sub.Status = SubmissionStatus.Graded;
        sub.GradedAtUtc = DateTime.UtcNow;
        sub.GradedById = userId;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Grade saved.";
        return RedirectToAction(nameof(ForAssignment), new { id = sub.AssignmentId });
    }

    [Authorize]
    public async Task<IActionResult> CourseMaterialFile(int id)
    {
        var userId = _userManager.GetUserId(User);
        var material = await _db.CourseMaterials
            .Include(m => m.Course)
            .FirstOrDefaultAsync(m => m.CourseMaterialId == id);

        if (material == null)
            return NotFound();

        if (User.IsInRole(AppRoles.Administrator))
        {
        }
        else if (User.IsInRole(AppRoles.Lecturer) && material.Course.LecturerId == userId)
        {
        }
        else if (User.IsInRole(AppRoles.Student) && await _db.Enrollments.AnyAsync(e =>
                     e.CourseId == material.CourseId && e.StudentId == userId))
        {
        }
        else
            return Forbid();

        var path = UploadedFileStore.GetPhysicalPath(_env, "materials", material.StoredFileName);
        if (path == null)
            return NotFound();

        var stream = System.IO.File.OpenRead(path);
        return File(stream, material.ContentType ?? "application/octet-stream", material.StoredFileName);
    }

    [Authorize]
    public async Task<IActionResult> SubmissionFile(int id)
    {
        var userId = _userManager.GetUserId(User);
        var sub = await _db.Submissions
            .Include(s => s.Assignment).ThenInclude(a => a.Course)
            .FirstOrDefaultAsync(s => s.SubmissionId == id);

        if (sub == null)
            return NotFound();

        if (User.IsInRole(AppRoles.Administrator))
        {
        }
        else if (sub.StudentId == userId)
        {
        }
        else if (User.IsInRole(AppRoles.Lecturer) && sub.Assignment.Course.LecturerId == userId)
        {
        }
        else
            return Forbid();

        if (string.IsNullOrEmpty(sub.StoredFileName))
            return NotFound();

        var path = UploadedFileStore.GetPhysicalPath(_env, "submissions", sub.StoredFileName);
        if (path == null)
            return NotFound();

        var stream = System.IO.File.OpenRead(path);
        return File(stream, sub.ContentType ?? "application/octet-stream", sub.StoredFileName);
    }
}
