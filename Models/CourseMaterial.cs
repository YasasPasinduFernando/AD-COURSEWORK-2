using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_COURSEWORK_2.Models;

public class CourseMaterial
{
    public int CourseMaterialId { get; set; }

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string StoredFileName { get; set; } = string.Empty;

    public string? ContentType { get; set; }

    public long FileSizeBytes { get; set; }

    [Required]
    public string UploadedById { get; set; } = string.Empty;

    [ForeignKey(nameof(UploadedById))]
    public ApplicationUser UploadedBy { get; set; } = null!;

    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
}
