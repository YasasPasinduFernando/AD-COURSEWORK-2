using AD_COURSEWORK_2.Infrastructure;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class SubmissionGradingRulesTests
{
    [Theory]
    [InlineData(0, 100, true)]
    [InlineData(50, 100, true)]
    [InlineData(100, 100, true)]
    [InlineData(-0.01, 100, false)]
    [InlineData(100.01, 100, false)]
    public void IsValidGrade_matches_inclusive_zero_to_max(decimal grade, decimal maxPoints, bool expected)
    {
        SubmissionGradingRules.IsValidGrade(grade, maxPoints).Should().Be(expected);
    }
}
