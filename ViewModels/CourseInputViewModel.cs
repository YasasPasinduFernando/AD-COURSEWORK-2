using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.ViewModels;

public class CourseInputViewModel
{
    public int? CourseId { get; set; }

    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [Range(1, 60)]
    public int Credits { get; set; } = 3;

    [Range(1, 10000)]
    [Display(Name = "Enrollment limit")]
    public int EnrollmentLimit { get; set; } = 40;

    [Required]
    [Display(Name = "Lecturer")]
    public string LecturerId { get; set; } = string.Empty;

    [Display(Name = "Prerequisite course")]
    public int? PrerequisiteId { get; set; }
}
