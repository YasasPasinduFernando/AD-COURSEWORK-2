using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AD_COURSEWORK_2.ViewModels;

public class SubmissionSubmitViewModel
{
    public int? SubmissionId { get; set; }

    public int AssignmentId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string AssignmentTitle { get; set; } = string.Empty;

    public DateTime DueDateUtc { get; set; }

    [StringLength(16000)]
    public string? TextContent { get; set; }

    [Display(Name = "Attachment")]
    public IFormFile? File { get; set; }

    public string? ExistingFileName { get; set; }
    public string? Status { get; set; }
    public decimal? Grade { get; set; }
    public string? Feedback { get; set; }
}
