using AD_COURSEWORK_2.ViewModels;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class AssignmentInputViewModelValidationTests
{
    [Fact]
    public void Missing_title_is_rejected()
    {
        var model = ValidAssignment();
        model.Title = string.Empty;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(AssignmentInputViewModel.Title)));
    }

    [Fact]
    public void Max_points_below_minimum_is_rejected()
    {
        var model = ValidAssignment();
        model.MaxPoints = 0;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(AssignmentInputViewModel.MaxPoints)));
    }

    private static AssignmentInputViewModel ValidAssignment() => new()
    {
        CourseId = 1,
        Title = "Coursework 1",
        Description = null,
        DueDateLocal = DateTime.UtcNow.AddDays(7),
        MaxPoints = 100
    };
}
