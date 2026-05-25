using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.Models;

// Represents an authenticated UniManage user and extends ASP.NET Identity with
// academic profile information and navigation links to teaching and learning records.
public class ApplicationUser : IdentityUser
{
    // Stores the display name used across dashboards, messages, reports, and emails.
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(30)]
    public override string? PhoneNumber { get; set; }

    // Optional date of birth (date only, no time).
    public DateOnly? DateOfBirth { get; set; }

    // Courses assigned to the user when the user acts as a lecturer.
    public ICollection<Course> TeachingCourses { get; set; } = new List<Course>();

    // Enrollment records associated with the user when the user acts as a student.
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    // Assignment submissions created by the student.
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();

    // Course material files uploaded by the user as a lecturer.
    public ICollection<CourseMaterial> UploadedMaterials { get; set; } = new List<CourseMaterial>();
}
