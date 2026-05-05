using AD_COURSEWORK_2.ViewModels;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class CourseInputViewModelValidationTests
{
    [Fact]
    public void Missing_course_code_is_rejected()
    {
        var model = ValidCourse();
        model.Code = string.Empty;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(CourseInputViewModel.Code)));
    }

    [Fact]
    public void Missing_course_name_is_rejected()
    {
        var model = ValidCourse();
        model.Name = string.Empty;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(CourseInputViewModel.Name)));
    }

    [Fact]
    public void Enrolment_limit_below_minimum_is_rejected()
    {
        var model = ValidCourse();
        model.EnrollmentLimit = 0;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(CourseInputViewModel.EnrollmentLimit)));
    }

    [Fact]
    public void Missing_lecturer_id_is_rejected()
    {
        var model = ValidCourse();
        model.LecturerId = string.Empty;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(CourseInputViewModel.LecturerId)));
    }

    private static CourseInputViewModel ValidCourse() => new()
    {
        Code = "CS6004",
        Name = "Application Development",
        Credits = 3,
        EnrollmentLimit = 40,
        LecturerId = "lecturer-id-1",
        Description = null,
        PrerequisiteId = null
    };
}
