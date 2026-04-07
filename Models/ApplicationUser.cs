using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AD_COURSEWORK_2.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(30)]
    public override string? PhoneNumber { get; set; }

    public ICollection<Course> TeachingCourses { get; set; } = new List<Course>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    public ICollection<CourseMaterial> UploadedMaterials { get; set; } = new List<CourseMaterial>();
}
