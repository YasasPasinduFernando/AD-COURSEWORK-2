using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

// Represents a student's response to an assignment, including written content,
// optional uploaded file metadata, grading status, score, and feedback.
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

    // UTC timestamp indicating when the student last submitted work.
    public DateTime? SubmittedAtUtc { get; set; }

    // Optional written answer submitted directly through the application.
    [StringLength(16000)]
    public string? TextContent { get; set; }

    // Safe generated filename for the uploaded submission attachment.
    [StringLength(500)]
    public string? StoredFileName { get; set; }

    public string? ContentType { get; set; }

    public long? FileSizeBytes { get; set; }

    // Current submission lifecycle state, such as not submitted, submitted, or graded.
    public SubmissionStatus Status { get; set; } = SubmissionStatus.NotSubmitted;

    // Lecturer-assigned score for the submission.
    [Column(TypeName = "decimal(10,2)")]
    public decimal? Grade { get; set; }

    // Lecturer feedback shown to the student after grading.
    [StringLength(8000)]
    public string? Feedback { get; set; }

    public DateTime? GradedAtUtc { get; set; }

    public string? GradedById { get; set; }

    [ForeignKey(nameof(GradedById))]
    public ApplicationUser? GradedBy { get; set; }
}
