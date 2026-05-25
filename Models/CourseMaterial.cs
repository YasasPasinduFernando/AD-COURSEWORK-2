using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

// Represents a learning material file uploaded by a lecturer for a course.
public class CourseMaterial
{
    public int CourseMaterialId { get; set; }

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    // Descriptive title shown to students on the course details page.
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    // Safe generated filename used for storage under the uploads directory.
    [Required]
    [StringLength(500)]
    public string StoredFileName { get; set; } = string.Empty;

    public string? ContentType { get; set; }

    public long FileSizeBytes { get; set; }

    // Identity user identifier for the lecturer who uploaded the material.
    [Required]
    public string UploadedById { get; set; } = string.Empty;

    [ForeignKey(nameof(UploadedById))]
    public ApplicationUser UploadedBy { get; set; } = null!;

    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
}
