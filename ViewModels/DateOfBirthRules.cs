namespace AD_COURSEWORK_2.ViewModels;

/// <summary>Shared validation for optional date-of-birth fields.</summary>
public static class DateOfBirthRules
{
    public static string? ValidateOptional(DateOnly? dob)
    {
        if (!dob.HasValue)
            return null;

        var d = dob.Value;
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        if (d > today)
            return "Date of birth cannot be in the future.";

        if (d < today.AddYears(-120))
            return "Please enter a realistic date of birth.";

        return null;
    }
}
