using AD_COURSEWORK_2.Infrastructure;
using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AD_COURSEWORK_2.Controllers;

[Authorize(Roles = AppRoles.Administrator)]
public class AdminUsersController : Controller
{
    private static readonly Regex UsernameRegex = new("^[A-Za-z0-9._-]{3,30}$", RegexOptions.Compiled);

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuditLogger _audit;

    public AdminUsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IAuditLogger audit)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _audit = audit;
    }

    public async Task<IActionResult> Index(string? q = null, int page = 1, int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 10) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _userManager.Users.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var qq = q.Trim();
            query = query.Where(u =>
                (u.UserName != null && u.UserName.Contains(qq)) ||
                (u.Email != null && u.Email.Contains(qq)) ||
                u.FullName.Contains(qq));
        }

        var total = await query.CountAsync();
        var totalPages = total == 0 ? 1 : (int)Math.Ceiling(total / (double)pageSize);
        if (page > totalPages) page = totalPages;

        var rows = await query
            .OrderBy(u => u.UserName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var adminCount = (await _userManager.GetUsersInRoleAsync(AppRoles.Administrator)).Count;
        var vm = new AdminUserListViewModel
        {
            Query = q,
            Page = page,
            PageSize = pageSize,
            Total = total,
            TotalPages = totalPages,
            AdminCount = adminCount,
            CanEditAdmin = adminCount > 1,
            CanLockAdmin = adminCount >= 2,
            Users = new List<AdminUserRowViewModel>()
        };

        foreach (var u in rows)
        {
            var roles = await _userManager.GetRolesAsync(u);
            vm.Users.Add(new AdminUserRowViewModel
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                FullName = u.FullName,
                Email = u.Email ?? string.Empty,
                Phone = u.PhoneNumber,
                Role = roles.FirstOrDefault() ?? "—",
                IsAdmin = roles.Contains(AppRoles.Administrator),
                IsLocked = u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTimeOffset.UtcNow,
                EmailConfirmed = u.EmailConfirmed
            });
        }

        return View(vm);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AdminUserCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminUserCreateViewModel model)
    {
        model.UserName = model.UserName?.Trim() ?? string.Empty;
        model.Email = model.Email?.Trim() ?? string.Empty;

        if (!AdminUserCreateViewModel.AllowedRoles.Contains(model.Role))
            ModelState.AddModelError(nameof(model.Role), "Invalid role.");

        if (!UsernameRegex.IsMatch(model.UserName))
            ModelState.AddModelError(nameof(model.UserName), "Invalid username format.");

        if (!ModelState.IsValid)
            return View(model);

        if (await _userManager.FindByNameAsync(model.UserName) != null)
            ModelState.AddModelError(nameof(model.UserName), "Username already exists.");

        if (await _userManager.FindByEmailAsync(model.Email) != null)
            ModelState.AddModelError(nameof(model.Email), "Email already exists.");

        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            FullName = model.FullName.Trim(),
            UserName = model.UserName,
            Email = model.Email,
            EmailConfirmed = true,
            PhoneNumber = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim(),
            DateOfBirth = model.DateOfBirth
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(model);
        }

        if (!await _roleManager.RoleExistsAsync(model.Role))
            await _roleManager.CreateAsync(new IdentityRole(model.Role));

        await _userManager.AddToRoleAsync(user, model.Role);
        await _audit.LogAsync(AuditCategories.Profile, "Admin create user",
            $"{user.Email} as {model.Role}", userId: user.Id, userName: user.UserName);

        TempData["Success"] = "User account created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var isTargetAdmin = roles.Contains(AppRoles.Administrator);
        var adminCount = (await _userManager.GetUsersInRoleAsync(AppRoles.Administrator)).Count;
        if (isTargetAdmin && adminCount <= 1)
        {
            TempData["Error"] = "At least one administrator must remain unchanged. Editing the only admin is blocked.";
            return RedirectToAction(nameof(Index));
        }

        var vm = new AdminUserEditViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Phone = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Role = roles.FirstOrDefault() ?? AppRoles.Student,
            IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow,
            EmailConfirmed = user.EmailConfirmed
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AdminUserEditViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
            return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);
        var isTargetAdmin = currentRoles.Contains(AppRoles.Administrator);
        var adminCount = (await _userManager.GetUsersInRoleAsync(AppRoles.Administrator)).Count;
        if (isTargetAdmin && adminCount <= 1)
        {
            TempData["Error"] = "At least one administrator must remain unchanged. Editing the only admin is blocked.";
            return RedirectToAction(nameof(Index));
        }

        model.UserName = model.UserName?.Trim() ?? string.Empty;
        model.Email = model.Email?.Trim() ?? string.Empty;

        if (!AdminUserEditViewModel.AllowedRoles.Contains(model.Role))
            ModelState.AddModelError(nameof(model.Role), "Invalid role.");

        if (!UsernameRegex.IsMatch(model.UserName))
            ModelState.AddModelError(nameof(model.UserName), "Invalid username format.");

        if (!ModelState.IsValid)
            return View(model);

        var byName = await _userManager.FindByNameAsync(model.UserName);
        if (byName != null && byName.Id != user.Id)
            ModelState.AddModelError(nameof(model.UserName), "Username already exists.");

        var byEmail = await _userManager.FindByEmailAsync(model.Email);
        if (byEmail != null && byEmail.Id != user.Id)
            ModelState.AddModelError(nameof(model.Email), "Email already exists.");

        if (!ModelState.IsValid)
            return View(model);

        user.FullName = model.FullName.Trim();
        user.UserName = model.UserName;
        user.NormalizedUserName = _userManager.NormalizeName(model.UserName);
        user.Email = model.Email;
        user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
        user.EmailConfirmed = model.EmailConfirmed;
        user.PhoneNumber = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim();
        user.DateOfBirth = model.DateOfBirth;

        var update = await _userManager.UpdateAsync(user);
        if (!update.Succeeded)
        {
            foreach (var e in update.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(model);
        }

        var existingRoles = await _userManager.GetRolesAsync(user);
        if (!existingRoles.Contains(model.Role))
        {
            if (existingRoles.Contains(AppRoles.Administrator)
                && adminCount <= 1
                && !string.Equals(model.Role, AppRoles.Administrator, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.Role), "Cannot remove the last administrator role.");
                return View(model);
            }

            if (existingRoles.Count > 0)
                await _userManager.RemoveFromRolesAsync(user, existingRoles);
            await _userManager.AddToRoleAsync(user, model.Role);
        }

        await _audit.LogAsync(AuditCategories.Profile, "Admin update user",
            $"{user.Email} role={model.Role}", userId: user.Id, userName: user.UserName);

        TempData["Success"] = "User account updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var me = _userManager.GetUserId(User);
        if (user.Id == me)
        {
            TempData["Error"] = "You cannot lock your own account.";
            return RedirectToAction(nameof(Index));
        }

        var isLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;
        var roles = await _userManager.GetRolesAsync(user);
        var isTargetAdmin = roles.Contains(AppRoles.Administrator);
        if (!isLocked && isTargetAdmin)
        {
            var adminCount = (await _userManager.GetUsersInRoleAsync(AppRoles.Administrator)).Count;
            if (adminCount < 2)
            {
                TempData["Error"] = "At least two administrators are required before locking an administrator.";
                return RedirectToAction(nameof(Index));
            }
        }

        if (isLocked)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
            await _audit.LogAsync(AuditCategories.Security, "Admin unlock user", user.Email, userId: user.Id, userName: user.UserName);
            TempData["Success"] = "User unlocked.";
        }
        else
        {
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(50));
            await _audit.LogAsync(AuditCategories.Security, "Admin lock user", user.Email, userId: user.Id, userName: user.UserName);
            TempData["Success"] = "User locked.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var me = _userManager.GetUserId(User);
        if (user.Id == me)
        {
            TempData["Error"] = "You cannot delete your own account.";
            return RedirectToAction(nameof(Index));
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains(AppRoles.Administrator))
        {
            TempData["Error"] = "Administrator accounts cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            TempData["Error"] = "Unable to delete user. This account may be referenced by course or submission records.";
            return RedirectToAction(nameof(Index));
        }

        await _audit.LogAsync(AuditCategories.Security, "Admin delete user", user.Email, userId: user.Id, userName: user.UserName);
        TempData["Success"] = "User deleted.";
        return RedirectToAction(nameof(Index));
    }
}
