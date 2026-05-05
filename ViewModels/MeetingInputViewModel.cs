using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.ViewModels;

public class MeetingInputViewModel
{
    public int? MeetingId { get; set; }

    [Required]
    [Display(Name = "Course")]
    public int CourseId { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Topic / title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Scheduled date & time")]
    public DateTime ScheduledLocal { get; set; } = DateTime.Now.AddDays(1).Date.AddHours(10);

    [Required]
    [Range(5, 480)]
    [Display(Name = "Duration (minutes)")]
    public int DurationMinutes { get; set; } = 60;

    [Display(Name = "Meeting link (Google Meet, Zoom, Teams)")]
    [StringLength(500)]
    public string? MeetingUrl { get; set; }

    [Display(Name = "Auto-generate Google Meet link")]
    public bool AutoGenerate { get; set; } = true;

    public bool NotifyStudents { get; set; } = true;
}

public class MeetingRowViewModel
{
    public int MeetingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; }
    public string MeetingUrl { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string LecturerName { get; set; } = string.Empty;
    public bool IsMine { get; set; }
    public int EnrolledCount { get; set; }
}
