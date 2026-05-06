using System.ComponentModel.DataAnnotations;
using AD_COURSEWORK_2.Models;

namespace AD_COURSEWORK_2.ViewModels;

public class RegisterViewModel : IValidatableObject
{
    [Required]
    [StringLength(200)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

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

    public static IEnumerable<string> AllowedRoles { get; } =
        new[] { AppRoles.Student, AppRoles.Lecturer };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var err = DateOfBirthRules.ValidateOptional(DateOfBirth);
        if (err != null)
            yield return new ValidationResult(err, new[] { nameof(DateOfBirth) });
    }
}
