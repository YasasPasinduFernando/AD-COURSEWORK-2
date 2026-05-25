using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

// Represents an online teaching session scheduled for a course, including
// lecturer ownership, meeting link, time, and duration.
public class Meeting
{
    public int MeetingId { get; set; }

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    // Identity user identifier for the lecturer who owns the meeting.
    [Required]
    public string LecturerId { get; set; } = string.Empty;

    [ForeignKey(nameof(LecturerId))]
    public ApplicationUser Lecturer { get; set; } = null!;

    // Topic or title displayed in meeting lists, calendars, and invitation emails.
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    // UTC start time used for calendar exports and role-based meeting listings.
    public DateTime ScheduledAtUtc { get; set; }

    [Range(5, 480)]
    public int DurationMinutes { get; set; } = 60;

    // Join link for the scheduled online session.
    [Required]
    [StringLength(500)]
    public string MeetingUrl { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
