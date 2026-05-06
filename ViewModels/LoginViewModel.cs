using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Email or username")]
    public string LoginIdentifier { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
