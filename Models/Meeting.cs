using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

public class Meeting
{
    public int MeetingId { get; set; }

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    [Required]
    public string LecturerId { get; set; } = string.Empty;

    [ForeignKey(nameof(LecturerId))]
    public ApplicationUser Lecturer { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    public DateTime ScheduledAtUtc { get; set; }

    [Range(5, 480)]
    public int DurationMinutes { get; set; } = 60;

    [Required]
    [StringLength(500)]
    public string MeetingUrl { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
