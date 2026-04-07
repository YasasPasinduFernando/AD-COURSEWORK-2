using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

public class Message
{
    public int MessageId { get; set; }

    [Required]
    public string SenderId { get; set; } = string.Empty;

    [ForeignKey(nameof(SenderId))]
    public ApplicationUser Sender { get; set; } = null!;

    [Required]
    public string ReceiverId { get; set; } = string.Empty;

    [ForeignKey(nameof(ReceiverId))]
    public ApplicationUser Receiver { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [StringLength(16000)]
    public string Content { get; set; } = string.Empty;

    public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;

    public bool IsRead { get; set; }
}
