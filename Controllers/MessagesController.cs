using System.Globalization;
using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Infrastructure;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Controllers;

[Authorize(Roles = $"{AppRoles.Student},{AppRoles.Lecturer}")]
public class MessagesController : Controller
{
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };
    private const long MaxImageBytes = 5 * 1024 * 1024;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditLogger _audit;

    public MessagesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IAuditLogger audit)
    {
        _db = db;
        _userManager = userManager;
        _audit = audit;
    }

    public async Task<IActionResult> Inbox()
    {
        var me = _userManager.GetUserId(User)!;
        var rows = await BuildInboxAsync(me);
        return View(rows);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllRead()
    {
        var me = _userManager.GetUserId(User)!;
        var unread = await _db.Messages
            .Where(m => m.ReceiverId == me && !m.IsRead)
            .ToListAsync();
        foreach (var m in unread) m.IsRead = true;
        if (unread.Count > 0)
        {
            await _db.SaveChangesAsync();
            await _audit.LogAsync(AuditCategories.Profile, "Mark all messages read", $"{unread.Count} messages");
        }

        TempData["Success"] = unread.Count == 0
            ? "Nothing to mark — your inbox is already up to date."
            : $"Marked {unread.Count} message{(unread.Count == 1 ? "" : "s")} as read.";
        return RedirectToAction(nameof(Inbox));
    }

    public async Task<IActionResult> Thread(string id)
    {
        var me = _userManager.GetUserId(User)!;
        if (!await CanCommunicateAsync(me, id))
            return Forbid();

        var other = await _db.Users.FindAsync(id);
        if (other == null)
            return NotFound();

        var list = await _db.Messages
            .Where(m =>
                (m.SenderId == me && m.ReceiverId == id) ||
                (m.SenderId == id && m.ReceiverId == me))
            .OrderBy(m => m.SentAtUtc)
            .ToListAsync();

        foreach (var m in list.Where(m => m.ReceiverId == me && !m.IsRead))
            m.IsRead = true;

        if (list.Any(m => m.ReceiverId == me))
            await _db.SaveChangesAsync();

        var otherRoles = await _userManager.GetRolesAsync(other);

        var vm = new MessageThreadViewModel
        {
            OtherUserId = id,
            OtherUserName = other.FullName,
            OtherUserEmail = other.Email,
            OtherUserRole = otherRoles.FirstOrDefault(),
            Messages = list.Select(m => new MessageThreadViewModel.MessageLine
            {
                IsMine = m.SenderId == me,
                Subject = m.Subject,
                Content = m.Content,
                SentAtUtc = m.SentAtUtc,
                IsRead = m.IsRead
            }).ToList(),
            Conversations = await BuildInboxAsync(me)
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(string id, string subject, string content, IFormFile? photo)
    {
        var me = _userManager.GetUserId(User)!;
        if (!await CanCommunicateAsync(me, id))
            return Forbid();

        if (string.IsNullOrWhiteSpace(content) && photo == null)
        {
            TempData["Error"] = "Message content or photo is required.";
            return RedirectToAction(nameof(Thread), new { id });
        }

        var body = (content ?? string.Empty).Trim();
        if (body.Length > 16000)
            body = body[..16000];

        if (string.IsNullOrWhiteSpace(subject))
            subject = "(no subject)";
        if (subject.Length > 200)
            subject = subject[..200];

        var imageToken = string.Empty;
        if (photo != null)
        {
            var ext = Path.GetExtension(photo.FileName);
            if (!AllowedImageExtensions.Contains(ext))
            {
                TempData["Error"] = "Only JPG, PNG, GIF, and WEBP images are allowed.";
                return RedirectToAction(nameof(Thread), new { id });
            }
            if (photo.Length <= 0 || photo.Length > MaxImageBytes)
            {
                TempData["Error"] = "Image size must be between 1 byte and 5 MB.";
                return RedirectToAction(nameof(Thread), new { id });
            }

            var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "messages");
            Directory.CreateDirectory(uploadsRoot);
            var fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}_{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
            var absolutePath = Path.Combine(uploadsRoot, fileName);

            await using (var stream = System.IO.File.Create(absolutePath))
            {
                await photo.CopyToAsync(stream);
            }

            imageToken = $"[[img:/uploads/messages/{fileName}]]";
        }

        var finalContent = body;
        if (!string.IsNullOrEmpty(imageToken))
            finalContent = string.IsNullOrWhiteSpace(finalContent) ? imageToken : $"{finalContent}\n{imageToken}";

        _db.Messages.Add(new Message
        {
            SenderId = me,
            ReceiverId = id,
            Subject = subject.Trim(),
            Content = finalContent,
            SentAtUtc = DateTime.UtcNow,
            IsRead = false
        });
        await _db.SaveChangesAsync();
        await _audit.LogAsync(AuditCategories.Profile, "Send message", $"to={id} chars={finalContent.Length}");

        TempData["Success"] = "Message sent.";
        return RedirectToAction(nameof(Thread), new { id });
    }

    public async Task<IActionResult> Compose(string? recipientId = null)
    {
        var me = _userManager.GetUserId(User)!;
        var vm = new MessageComposeViewModel { RecipientId = recipientId };
        vm.AllowedRecipients = await GetAllowedRecipientsAsync(me);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Compose(MessageComposeViewModel model)
    {
        var me = _userManager.GetUserId(User)!;
        model.AllowedRecipients = await GetAllowedRecipientsAsync(me);

        if (string.IsNullOrWhiteSpace(model.RecipientId) || !model.AllowedRecipients.ContainsKey(model.RecipientId))
            ModelState.AddModelError(nameof(model.RecipientId), "Select a valid recipient.");

        if (!ModelState.IsValid)
            return View(model);

        _db.Messages.Add(new Message
        {
            SenderId = me,
            ReceiverId = model.RecipientId!,
            Subject = model.Subject.Trim(),
            Content = model.Content.Trim(),
            SentAtUtc = DateTime.UtcNow,
            IsRead = false
        });
        await _db.SaveChangesAsync();
        await _audit.LogAsync(AuditCategories.Profile, "Send message", $"to={model.RecipientId} chars={model.Content.Length}");
        TempData["Success"] = "Message sent.";
        return RedirectToAction(nameof(Thread), new { id = model.RecipientId });
    }

    private async Task<List<MessageInboxRowViewModel>> BuildInboxAsync(string me)
    {
        var messages = await _db.Messages
            .AsNoTracking()
            .Where(m => m.SenderId == me || m.ReceiverId == me)
            .OrderByDescending(m => m.SentAtUtc)
            .ToListAsync();

        var otherIds = messages
            .Select(m => m.SenderId == me ? m.ReceiverId : m.SenderId)
            .Distinct()
            .ToList();

        var users = await _db.Users
            .Where(u => otherIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.FullName);

        return messages
            .GroupBy(m => m.SenderId == me ? m.ReceiverId : m.SenderId)
            .Select(g =>
            {
                var latest = g.OrderByDescending(x => x.SentAtUtc).First();
                return new MessageInboxRowViewModel
                {
                    OtherUserId = g.Key,
                    OtherName = users.GetValueOrDefault(g.Key) ?? "User",
                    LastSubject = latest.Subject,
                    LastPreview = latest.Content.Length > 120 ? latest.Content[..120] + "..." : latest.Content,
                    LastAtUtc = latest.SentAtUtc,
                    Unread = g.Any(x => x.ReceiverId == me && !x.IsRead)
                };
            })
            .OrderByDescending(r => r.LastAtUtc)
            .ToList();
    }

    private async Task<bool> CanCommunicateAsync(string me, string other)
    {
        var allowed = await GetAllowedRecipientsAsync(me);
        return allowed.ContainsKey(other);
    }

    private async Task<Dictionary<string, string>> GetAllowedRecipientsAsync(string me)
    {
        if (User.IsInRole(AppRoles.Student))
        {
            var lecturerIds = await (
                from e in _db.Enrollments
                join c in _db.Courses on e.CourseId equals c.CourseId
                where e.StudentId == me
                select c.LecturerId).Distinct().ToListAsync();

            var lecturers = await _db.Users
                .Where(u => lecturerIds.Contains(u.Id))
                .OrderBy(u => u.FullName)
                .Select(u => new { u.Id, u.FullName, u.Email })
                .ToListAsync();

            return lecturers.ToDictionary(x => x.Id, x => $"{x.FullName} ({x.Email})");
        }

        if (User.IsInRole(AppRoles.Lecturer))
        {
            var studentIds = await (
                from e in _db.Enrollments
                join c in _db.Courses on e.CourseId equals c.CourseId
                where c.LecturerId == me
                select e.StudentId).Distinct().ToListAsync();

            var students = await _db.Users
                .Where(u => studentIds.Contains(u.Id))
                .OrderBy(u => u.FullName)
                .Select(u => new { u.Id, u.FullName, u.Email })
                .ToListAsync();

            return students.ToDictionary(x => x.Id, x => $"{x.FullName} ({x.Email})");
        }

        return new Dictionary<string, string>();
    }
}
