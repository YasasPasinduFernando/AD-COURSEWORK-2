using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

// Represents a direct communication record between an enrolled student and a lecturer.
public class Message
{
    public int MessageId { get; set; }

    // Identity user identifier for the sender of the message.
    [Required]
    public string SenderId { get; set; } = string.Empty;

    [ForeignKey(nameof(SenderId))]
    public ApplicationUser Sender { get; set; } = null!;

    [Required]
    public string ReceiverId { get; set; } = string.Empty;

    [ForeignKey(nameof(ReceiverId))]
    public ApplicationUser Receiver { get; set; } = null!;

    // Short subject displayed in inbox and conversation previews.
    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    // Message body stored for the conversation thread.
    [Required]
    [StringLength(16000)]
    public string Content { get; set; } = string.Empty;

    // UTC timestamp used to order conversations and inbox rows.
    public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;

    // Indicates whether the receiver has opened or acknowledged the message.
    public bool IsRead { get; set; }
}
