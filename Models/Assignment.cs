using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

// Represents an assessed task created by a lecturer for a course, including
// due date, maximum points, and related student submissions.
public class Assignment
{
    public int AssignmentId { get; set; }

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    // Title shown to students in assignment lists, calendars, and submission pages.
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(8000)]
    public string? Description { get; set; }

    // UTC deadline used for ordering assignments and identifying overdue work.
    public DateTime DueDateUtc { get; set; }

    // Maximum achievable score used to validate grades and calculate percentages.
    [Range(0, 10000)]
    public decimal MaxPoints { get; set; } = 100;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
