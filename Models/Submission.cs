using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

public class Submission
{
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }

    [ForeignKey(nameof(AssignmentId))]
    public Assignment Assignment { get; set; } = null!;

    [Required]
    public string StudentId { get; set; } = string.Empty;

    [ForeignKey(nameof(StudentId))]
    public ApplicationUser Student { get; set; } = null!;

    public DateTime? SubmittedAtUtc { get; set; }

    [StringLength(16000)]
    public string? TextContent { get; set; }

    [StringLength(500)]
    public string? StoredFileName { get; set; }

    public string? ContentType { get; set; }

    public long? FileSizeBytes { get; set; }

    public SubmissionStatus Status { get; set; } = SubmissionStatus.NotSubmitted;

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Grade { get; set; }

    [StringLength(8000)]
    public string? Feedback { get; set; }

    public DateTime? GradedAtUtc { get; set; }

    public string? GradedById { get; set; }

    [ForeignKey(nameof(GradedById))]
    public ApplicationUser? GradedBy { get; set; }
}
