using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.Models;

// Represents an audit trail entry for important authentication, academic,
// administrative, and security-related actions.
public class AuditLog
{
    public long AuditLogId { get; set; }

    // UTC timestamp used for audit ordering, filtering, and reporting.
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [StringLength(450)]
    public string? UserId { get; set; }

    [StringLength(256)]
    public string? UserName { get; set; }

    // High-level area of the action, such as Auth, Course, Submission, or Security.
    [Required]
    [StringLength(80)]
    public string Category { get; set; } = string.Empty;

    // Specific action name recorded for later administrative review.
    [Required]
    [StringLength(120)]
    public string Action { get; set; } = string.Empty;

    // Additional non-sensitive context about the audited action.
    [StringLength(2000)]
    public string? Detail { get; set; }

    [StringLength(64)]
    public string? IpAddress { get; set; }

    [StringLength(512)]
    public string? UserAgent { get; set; }

    // Indicates whether the audited operation completed successfully.
    public bool Success { get; set; } = true;
}

// Defines stable category names used when writing audit log records.
public static class AuditCategories
{
    public const string Auth = "Auth";
    public const string Course = "Course";
    public const string Assignment = "Assignment";
    public const string Submission = "Submission";
    public const string Enrollment = "Enrollment";
    public const string Material = "Material";
    public const string Profile = "Profile";
    public const string Security = "Security";
    public const string Meeting = "Meeting";
    public const string Message = "Message";
}
