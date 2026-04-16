using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.ViewModels;

public class GradeSubmissionViewModel
{
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string? TextContent { get; set; }

    public string? FileName { get; set; }

    public decimal MaxPoints { get; set; }

    [Display(Name = "Grade")]
    public decimal Grade { get; set; }

    [StringLength(8000)]
    public string? Feedback { get; set; }

    public DateTime? SubmittedAtUtc { get; set; }
}
