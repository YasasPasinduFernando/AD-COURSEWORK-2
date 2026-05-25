namespace AD_COURSEWORK_2.ViewModels;

// Provides the data required to display a course row in the student course
// browsing page, including enrollment state and blocking reasons.
public class CourseBrowseRowViewModel
{
    public int CourseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string LecturerName { get; set; } = string.Empty;
    public string? PrerequisiteSummary { get; set; }
    public int CurrentEnrollmentCount { get; set; }
    public int EnrollmentLimit { get; set; }
    public bool IsEnrolled { get; set; }
    public bool CanEnroll { get; set; }
    public string? BlockReason { get; set; }
}
