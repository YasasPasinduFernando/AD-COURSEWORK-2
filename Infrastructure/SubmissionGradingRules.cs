namespace AD_COURSEWORK_2.Infrastructure;

/// <summary>
/// Centralises the inclusive grade bounds used when saving a submission grade.
/// </summary>
public static class SubmissionGradingRules
{
    public static bool IsValidGrade(decimal grade, decimal maxPoints) =>
        grade >= 0 && grade <= maxPoints;
}
