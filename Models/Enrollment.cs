using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

// Represents the relationship between a student and a course, including the
// date and time when the student joined the course.
public class Enrollment
{
    public int EnrollmentId { get; set; }

    // Identity user identifier for the enrolled student.
    [Required]
    public string StudentId { get; set; } = string.Empty;

    [ForeignKey(nameof(StudentId))]
    public ApplicationUser Student { get; set; } = null!;

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    // UTC timestamp used for reports and dashboard enrollment trends.
    public DateTime EnrolledAtUtc { get; set; } = DateTime.UtcNow;
}
