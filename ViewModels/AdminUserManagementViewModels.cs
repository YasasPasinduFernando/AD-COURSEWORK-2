using System.ComponentModel.DataAnnotations;
using AD_COURSEWORK_2.Models;

namespace AD_COURSEWORK_2.ViewModels;

public class AdminUserRowViewModel
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsLocked { get; set; }
    public bool EmailConfirmed { get; set; }
}

public class AdminUserListViewModel
{
    public List<AdminUserRowViewModel> Users { get; set; } = new();
    public string? Query { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public int AdminCount { get; set; }
    public bool CanEditAdmin { get; set; }
    public bool CanLockAdmin { get; set; }
}

public class AdminUserCreateViewModel : IValidatableObject
{
    [Required]
    [StringLength(200)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be 3 to 30 characters.")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    public string? Phone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of birth (optional)")]
    public DateOnly? DateOfBirth { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Role")]
    public string Role { get; set; } = AppRoles.Student;

    public static IReadOnlyList<string> AllowedRoles { get; } =
        new[] { AppRoles.Student, AppRoles.Lecturer, AppRoles.Administrator };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var err = DateOfBirthRules.ValidateOptional(DateOfBirth);
        if (err != null)
            yield return new ValidationResult(err, new[] { nameof(DateOfBirth) });

        var value = UserName?.Trim() ?? string.Empty;
        if (value.Contains('@'))
        {
            yield return new ValidationResult(
                "Username must not be an email address.",
                new[] { nameof(UserName) });
        }
    }
}

public class AdminUserEditViewModel : IValidatableObject
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be 3 to 30 characters.")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    public string? Phone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of birth (optional)")]
    public DateOnly? DateOfBirth { get; set; }

    [Required]
    [Display(Name = "Role")]
    public string Role { get; set; } = AppRoles.Student;

    public bool IsLocked { get; set; }
    public bool EmailConfirmed { get; set; }

    public static IReadOnlyList<string> AllowedRoles { get; } =
        new[] { AppRoles.Student, AppRoles.Lecturer, AppRoles.Administrator };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var err = DateOfBirthRules.ValidateOptional(DateOfBirth);
        if (err != null)
            yield return new ValidationResult(err, new[] { nameof(DateOfBirth) });

        var value = UserName?.Trim() ?? string.Empty;
        if (value.Contains('@'))
        {
            yield return new ValidationResult(
                "Username must not be an email address.",
                new[] { nameof(UserName) });
        }
    }
}
