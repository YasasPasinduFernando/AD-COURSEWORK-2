using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AD_COURSEWORK_2.ViewModels;

public class MaterialUploadViewModel
{
    public int CourseId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Display(Name = "File")]
    public IFormFile File { get; set; } = null!;
}
