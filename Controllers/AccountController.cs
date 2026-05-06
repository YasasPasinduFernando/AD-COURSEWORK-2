using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using AD_COURSEWORK_2.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AD_COURSEWORK_2.Controllers;

public class AccountController : Controller
{
    private static readonly Regex UsernameRegex = new("^[A-Za-z0-9._-]{3,30}$", RegexOptions.Compiled);

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAuditLogger _audit;
    private readonly IWebHostEnvironment _env;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        ILogger<AccountController> logger,
        IConfiguration configuration,
        IAuditLogger audit,
        IWebHostEnvironment env)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _logger = logger;
        _configuration = configuration;
        _audit = audit;
        _env = env;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        model.UserName = model.UserName?.Trim() ?? string.Empty;
        model.Email = model.Email?.Trim() ?? string.Empty;

        if (!RegisterViewModel.AllowedRoles.Contains(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "Invalid role.");
        }

        if (!UsernameRegex.IsMatch(model.UserName))
        {
            ModelState.AddModelError(nameof(model.UserName), "Invalid username format. Use 3-30 letters, numbers, underscore (_), dot (.), or hyphen (-).");
        }

        if (model.UserName.Contains('@'))
        {
            ModelState.AddModelError(nameof(model.UserName), "Username must not be an email address.");
        }

        var existingByUserName = !string.IsNullOrWhiteSpace(model.UserName)
            ? await _userManager.FindByNameAsync(model.UserName)
            : null;
        if (existingByUserName != null)
        {
            ModelState.AddModelError(nameof(model.UserName), "Username already exists.");
        }

        var existingByEmail = !string.IsNullOrWhiteSpace(model.Email)
            ? await _userManager.FindByEmailAsync(model.Email)
            : null;
        if (existingByEmail != null)
        {
            ModelState.AddModelError(nameof(model.Email), "Email already exists.");
        }

        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            EmailConfirmed = true,
            FullName = model.FullName,
            PhoneNumber = model.Phone,
            DateOfBirth = model.DateOfBirth
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            await _audit.LogAsync(AuditCategories.Auth, "Register failed", model.Email, success: false);
            foreach (var e in result.Errors)
            {
                if (e.Code.Contains("DuplicateUserName", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError(nameof(model.UserName), "Username already exists.");
                else if (e.Code.Contains("DuplicateEmail", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError(nameof(model.Email), "Email already exists.");
                else if (e.Code.Contains("InvalidUserName", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError(nameof(model.UserName), "Invalid username format.");
                else
                    ModelState.AddModelError(string.Empty, e.Description);
            }
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, model.Role);
        await _signInManager.SignInAsync(user, isPersistent: false);
        await _audit.LogAsync(AuditCategories.Auth, "Register success",
            $"{user.Email} as {model.Role}", userId: user.Id, userName: user.Email);

        await TrySendEmailAsync(
            user.Email!,
            "Welcome to UniManage",
            EmailTemplates.BuildWelcomeEmail(user.FullName, model.Role));
        TempData["Success"] = "Registration successful. Welcome to UniManage.";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null, string? googleError = null)
    {
        if (!string.IsNullOrWhiteSpace(googleError))
            TempData["Error"] = "Google sign-in was cancelled or failed. Please try again.";

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var identifier = model.LoginIdentifier?.Trim() ?? string.Empty;
        var user = identifier.Contains('@')
            ? await _userManager.FindByEmailAsync(identifier)
            : await _userManager.FindByNameAsync(identifier);

        if (user == null)
        {
            await _audit.LogAsync(AuditCategories.Auth, "Login failed",
                "Unknown email/username", success: false);
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            await _audit.LogAsync(AuditCategories.Auth, "Login locked out",
                "Too many failed attempts",
                success: false, userId: user.Id, userName: user.UserName ?? user.Email);
            ModelState.AddModelError(string.Empty,
                "Account temporarily locked due to repeated failed attempts. Try again in 15 minutes.");
            return View(model);
        }

        if (!result.Succeeded)
        {
            await _audit.LogAsync(AuditCategories.Auth, "Login failed",
                "Invalid credentials",
                success: false, userId: user.Id, userName: user.UserName ?? user.Email);
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        await _audit.LogAsync(AuditCategories.Auth, "Login success",
            "Signed in", userId: user.Id, userName: user.UserName ?? user.Email);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            await SendLoginNotificationAsync(user);
            return Redirect(model.ReturnUrl);
        }

        await SendLoginNotificationAsync(user);
        TempData["Success"] = "Signed in successfully.";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult LoginWithGoogle(string? returnUrl = null)
    {
        var clientId = _configuration["Authentication:Google:ClientId"];
        var clientSecret = _configuration["Authentication:Google:ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            TempData["Error"] = "Google login is not configured yet.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        var redirectUrl = Url.Action(nameof(GoogleCallback), "Account", new { returnUrl });
        const string googleCallbackPath = "/signin-google2";
        var diagnosticCallbackUrl =
            $"{Request.Scheme}://{Request.Host.ToUriComponent()}{Request.PathBase}{googleCallbackPath}";

        if (_env.IsDevelopment())
        {
            _logger.LogInformation(
                "Google OAuth challenge generated. Scheme={Scheme}, Host={Host}, CallbackPath={CallbackPath}, RedirectUri={RedirectUri}",
                Request.Scheme,
                Request.Host.Value,
                googleCallbackPath,
                diagnosticCallbackUrl);
        }
        else
        {
            _logger.LogDebug(
                "Google OAuth challenge generated. Scheme={Scheme}, Host={Host}, CallbackPath={CallbackPath}, RedirectUri={RedirectUri}",
                Request.Scheme,
                Request.Host.Value,
                googleCallbackPath,
                diagnosticCallbackUrl);
        }

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            GoogleDefaults.AuthenticationScheme,
            redirectUrl!);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            TempData["Error"] = "Google login failed. Please try again.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info?.Principal == null)
        {
            TempData["Error"] = "Google login failed. External login info was not received.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email)
            ?? info.Principal.FindFirstValue("email");
        var name = info.Principal.FindFirstValue(ClaimTypes.Name)
            ?? info.Principal.FindFirstValue("name")
            ?? "Google user";
        var googleUserId = info.ProviderKey;

        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["Error"] = "Google account email is missing. Use another account or normal login.";
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        if (string.IsNullOrWhiteSpace(googleUserId))
        {
            TempData["Error"] = "Google user id is missing. Please try again.";
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            var generatedUserName = await GenerateUniqueUserNameFromEmailAsync(email);
            user = new ApplicationUser
            {
                UserName = generatedUserName,
                Email = email,
                EmailConfirmed = true,
                FullName = name
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                await _audit.LogAsync(AuditCategories.Auth, "Google login create user failed", email, success: false);
                TempData["Error"] = "Unable to create account from Google sign-in. Please contact support.";
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return RedirectToAction(nameof(Login), new { returnUrl });
            }

            await _userManager.AddToRoleAsync(user, AppRoles.Student);
            await _audit.LogAsync(AuditCategories.Auth, "Google user created",
                email, userId: user.Id, userName: user.Email);
            _logger.LogInformation("Google user created for {Email}", email);
        }

        await _audit.LogAsync(AuditCategories.Auth, "Google login success",
            email, userId: user.Id, userName: email);
        _logger.LogInformation("Google login success for {Email}, Name: {Name}", email, name);
        await _signInManager.SignInAsync(user, isPersistent: false);
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            await SendLoginNotificationAsync(user);
            return Redirect(returnUrl);
        }

        await SendLoginNotificationAsync(user);
        TempData["Success"] = "Signed in with Google.";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var name = User.Identity?.Name;
        var uid = _userManager.GetUserId(User);
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        await _audit.LogAsync(AuditCategories.Auth, "Logout", name, userId: uid, userName: name);
        TempData["Success"] = "You have been signed out.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(
                nameof(ResetPassword),
                "Account",
                new
                {
                    email = user.Email,
                    token = Uri.EscapeDataString(token)
                },
                protocol: Request.Scheme);

            if (!string.IsNullOrWhiteSpace(callbackUrl))
            {
                await TrySendEmailAsync(
                    user.Email!,
                    "Reset your UniManage password",
                    EmailTemplates.BuildPasswordResetEmail(user.FullName, callbackUrl));
            }

            await _audit.LogAsync(AuditCategories.Auth, "Password reset requested",
                user.Email, userId: user.Id, userName: user.Email);
        }
        else
        {
            await _audit.LogAsync(AuditCategories.Auth, "Password reset for unknown email",
                model.Email, success: false);
        }

        TempData["Success"] = "If an account exists for that email, a password reset link was sent.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string email, string token)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            return RedirectToAction(nameof(Login));

        return View(new ResetPasswordViewModel
        {
            Email = email,
            Token = Uri.UnescapeDataString(token)
        });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["Success"] = "Password has been reset successfully.";
            return RedirectToAction(nameof(Login));
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (!result.Succeeded)
        {
            await _audit.LogAsync(AuditCategories.Auth, "Password reset failed",
                user.Email, success: false, userId: user.Id, userName: user.Email);
            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(model);
        }

        await _userManager.ResetAccessFailedCountAsync(user);
        await _audit.LogAsync(AuditCategories.Auth, "Password reset success",
            user.Email, userId: user.Id, userName: user.Email);

        TempData["Success"] = "Password has been reset successfully.";
        return RedirectToAction(nameof(Login));
    }

    private async Task TrySendEmailAsync(string toEmail, string subject, string bodyHtml)
    {
        try
        {
            await _emailService.SendAsync(toEmail, subject, bodyHtml);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email send failed for {Email} ({Subject})", toEmail, subject);
        }
    }

    private Task SendLoginNotificationAsync(ApplicationUser user)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
            return Task.CompletedTask;

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = Request.Headers.UserAgent.ToString();

        return TrySendEmailAsync(
            user.Email,
            "UniManage login alert",
            EmailTemplates.BuildLoginAlertEmail(user.FullName, DateTime.Now, ip, userAgent));
    }

    private async Task<string> GenerateUniqueUserNameFromEmailAsync(string email)
    {
        var prefix = email.Split('@', 2)[0].Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(prefix))
            prefix = "user";

        prefix = Regex.Replace(prefix, @"[^a-z0-9._-]", "-");
        prefix = Regex.Replace(prefix, @"[-_.]{2,}", "-").Trim('-', '_', '.');
        if (prefix.Length < 3)
            prefix = prefix.PadRight(3, 'x');
        if (prefix.Length > 30)
            prefix = prefix[..30];

        var candidate = prefix;
        var counter = 1;
        while (await _userManager.FindByNameAsync(candidate) != null)
        {
            var suffix = $"-{counter}";
            var maxBaseLength = 30 - suffix.Length;
            var basePart = prefix.Length > maxBaseLength ? prefix[..maxBaseLength] : prefix;
            candidate = basePart + suffix;
            counter++;
        }

        return candidate;
    }
}
