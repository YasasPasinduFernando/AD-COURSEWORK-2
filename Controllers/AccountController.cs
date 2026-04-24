using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using AD_COURSEWORK_2.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AD_COURSEWORK_2.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        ILogger<AccountController> logger,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _logger = logger;
        _configuration = configuration;
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
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!RegisterViewModel.AllowedRoles.Contains(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "Invalid role.");
        }

        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            FullName = model.FullName,
            PhoneNumber = model.Phone
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, model.Role);
        await _signInManager.SignInAsync(user, isPersistent: false);
        await TrySendEmailAsync(
            user.Email!,
            "Welcome to UniManage",
            $"""
            <p>Hello {HtmlEncoder.Default.Encode(user.FullName)},</p>
            <p>Your account was created successfully as <strong>{HtmlEncoder.Default.Encode(model.Role)}</strong>.</p>
            <p>You can now access your dashboard and start using UniManage.</p>
            <p>Thanks,<br/>UniManage LMS</p>
            """);
        TempData["Success"] = "Registration successful. Welcome to UniManage.";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

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
            TempData["Error"] = "Registration required. Your Google account is not linked to an LMS user.";
            _logger.LogInformation("Google login rejected for unknown email {Email}", email);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return RedirectToAction(nameof(AccessDenied));
        }

        _logger.LogInformation("Google login success for {Email}, Name: {Name}, GoogleId: {GoogleUserId}", email, name, googleUserId);
        await _signInManager.SignInAsync(user, isPersistent: false);
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        TempData["Success"] = "Signed in with Google.";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
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
                    $"""
                    <p>Hello {HtmlEncoder.Default.Encode(user.FullName)},</p>
                    <p>Click the link below to reset your password:</p>
                    <p><a href="{HtmlEncoder.Default.Encode(callbackUrl)}">Reset password</a></p>
                    <p>If you did not request this, you can ignore this email.</p>
                    """);
            }
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
            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(model);
        }

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
}
