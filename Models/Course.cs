using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

// Represents a university course, including its lecturer, enrollment limit,
// prerequisite relationship, assignments, materials, and enrolled students.
public class Course
{
    public int CourseId { get; set; }

    // Short institutional code used to identify the course in dashboards and reports.
    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    // Human-readable course title displayed to students and lecturers.
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [Range(1, 60)]
    public int Credits { get; set; }

    [Range(1, 10000)]
    public int EnrollmentLimit { get; set; }

    // Identity user identifier for the lecturer responsible for this course.
    [Required]
    public string LecturerId { get; set; } = string.Empty;

    [ForeignKey(nameof(LecturerId))]
    public ApplicationUser Lecturer { get; set; } = null!;

    // Optional course that must be enrolled before this course can be joined.
    public int? PrerequisiteId { get; set; }

    [ForeignKey(nameof(PrerequisiteId))]
    public Course? Prerequisite { get; set; }

    public ICollection<Course> DependentCourses { get; set; } = new List<Course>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<CourseMaterial> Materials { get; set; } = new List<CourseMaterial>();
    public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
}
