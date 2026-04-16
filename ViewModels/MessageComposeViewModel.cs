using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.ViewModels;

public class MessageComposeViewModel
{
    [Required]
    public string? RecipientId { get; set; }

    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [StringLength(16000)]
    public string Content { get; set; } = string.Empty;

    public Dictionary<string, string> AllowedRecipients { get; set; } = new();
}
