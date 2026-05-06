namespace AD_COURSEWORK_2.Infrastructure;

public static class GradeBandHelper
{
    public static decimal ToPercentage(decimal grade, decimal maxPoints)
    {
        if (maxPoints <= 0) return 0m;
        return Math.Round((grade / maxPoints) * 100m, 2);
    }

    public static string ToLetter(decimal grade, decimal maxPoints)
    {
        var percentage = ToPercentage(grade, maxPoints);
        if (percentage >= 85m) return "A+";
        if (percentage >= 75m) return "A";
        if (percentage >= 65m) return "B";
        if (percentage >= 55m) return "C";
        if (percentage >= 45m) return "D";
        return "F";
    }
}
