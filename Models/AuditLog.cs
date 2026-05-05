using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.Models;

public class AuditLog
{
    public long AuditLogId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [StringLength(450)]
    public string? UserId { get; set; }

    [StringLength(256)]
    public string? UserName { get; set; }

    [Required]
    [StringLength(80)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Action { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Detail { get; set; }

    [StringLength(64)]
    public string? IpAddress { get; set; }

    [StringLength(512)]
    public string? UserAgent { get; set; }

    public bool Success { get; set; } = true;
}

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
