using AD_COURSEWORK_2.Data;
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
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public MessagesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Inbox()
    {
        var me = _userManager.GetUserId(User)!;
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

        var rows = messages
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

        return View(rows);
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

        var vm = new MessageThreadViewModel
        {
            OtherUserId = id,
            OtherUserName = other.FullName,
            Messages = list.Select(m => new MessageThreadViewModel.MessageLine
            {
                IsMine = m.SenderId == me,
                Subject = m.Subject,
                Content = m.Content,
                SentAtUtc = m.SentAtUtc,
                IsRead = m.IsRead
            }).ToList()
        };

        return View(vm);
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
        TempData["Success"] = "Message sent.";
        return RedirectToAction(nameof(Thread), new { id = model.RecipientId });
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
