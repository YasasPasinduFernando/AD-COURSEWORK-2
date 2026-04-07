using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

public class Assignment
{
    public int AssignmentId { get; set; }

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(8000)]
    public string? Description { get; set; }

    public DateTime DueDateUtc { get; set; }

    [Range(0, 10000)]
    public decimal MaxPoints { get; set; } = 100;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
