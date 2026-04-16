using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.ViewModels;

public class AssignmentInputViewModel
{
    public int? AssignmentId { get; set; }

    public int CourseId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(8000)]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Due date")]
    public DateTime DueDateLocal { get; set; } = DateTime.Now.AddDays(7);

    [Range(1, 10000)]
    [Display(Name = "Max points")]
    public decimal MaxPoints { get; set; } = 100;
}
