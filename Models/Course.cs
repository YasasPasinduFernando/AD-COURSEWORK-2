using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

public class Course
{
    public int CourseId { get; set; }

    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [Range(1, 60)]
    public int Credits { get; set; }

    [Range(1, 10000)]
    public int EnrollmentLimit { get; set; }

    [Required]
    public string LecturerId { get; set; } = string.Empty;

    [ForeignKey(nameof(LecturerId))]
    public ApplicationUser Lecturer { get; set; } = null!;

    public int? PrerequisiteId { get; set; }

    [ForeignKey(nameof(PrerequisiteId))]
    public Course? Prerequisite { get; set; }

    public ICollection<Course> DependentCourses { get; set; } = new List<Course>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<CourseMaterial> Materials { get; set; } = new List<CourseMaterial>();
}
