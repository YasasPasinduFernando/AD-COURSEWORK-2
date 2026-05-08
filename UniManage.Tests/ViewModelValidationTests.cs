using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using FluentAssertions;
using UniManage.Tests.Helpers;
using Xunit;

namespace UniManage.Tests;

public class ViewModelValidationTests
{
    [Fact]
    public void LoginViewModel_WithMissingIdentifierAndPassword_ShouldBeInvalid()
    {
        var model = new LoginViewModel();

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(LoginViewModel.LoginIdentifier)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(LoginViewModel.Password)));
    }

    [Fact]
    public void LoginViewModel_WithIdentifierAndPassword_ShouldBeValid()
    {
        var model = new LoginViewModel
        {
            LoginIdentifier = "student01",
            Password = "AnyPassword1!"
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().BeEmpty();
    }

    [Fact]
    public void RegisterViewModel_WithValidInput_ShouldBeValid()
    {
        var model = new RegisterViewModel
        {
            FullName = "Test Student",
            UserName = "student01",
            Email = "student@test.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            Role = AppRoles.Student
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().BeEmpty();
    }

    [Fact]
    public void RegisterViewModel_WithMissingRequiredFields_ShouldBeInvalid()
    {
        var model = new RegisterViewModel
        {
            FullName = string.Empty,
            UserName = string.Empty,
            Email = string.Empty,
            Password = string.Empty,
            ConfirmPassword = string.Empty,
            Role = string.Empty
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.FullName)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.UserName)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.Email)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.Password)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.Role)));
    }

    [Fact]
    public void RegisterViewModel_WithMismatchedConfirmPassword_ShouldBeInvalid()
    {
        var model = new RegisterViewModel
        {
            FullName = "Test Student",
            UserName = "student01",
            Email = "student@test.com",
            Password = "Password123!",
            ConfirmPassword = "DifferentPassword!",
            Role = AppRoles.Student
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.ConfirmPassword)));
    }

    [Fact]
    public void RegisterViewModel_WithPasswordBelowMinimumLength_ShouldBeInvalid()
    {
        var model = new RegisterViewModel
        {
            FullName = "Test Student",
            UserName = "student01",
            Email = "student@test.com",
            Password = "Short1!",
            ConfirmPassword = "Short1!",
            Role = AppRoles.Student
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.Password)));
    }

    [Fact]
    public void RegisterViewModel_AllowedRoles_ShouldContainOnlyStudentAndLecturer()
    {
        RegisterViewModel.AllowedRoles
            .Should().BeEquivalentTo(new[] { AppRoles.Student, AppRoles.Lecturer });
    }

    [Fact]
    public void CourseInputViewModel_WithMissingCodeAndName_ShouldBeInvalid()
    {
        var model = new CourseInputViewModel
        {
            Code = string.Empty,
            Name = string.Empty,
            Credits = 3,
            EnrollmentLimit = 40,
            LecturerId = "lecturer-id-1"
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(CourseInputViewModel.Code)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(CourseInputViewModel.Name)));
    }

    [Fact]
    public void CourseInputViewModel_WithEnrolmentLimitOutOfRange_ShouldBeInvalid()
    {
        var model = new CourseInputViewModel
        {
            Code = "CS6004",
            Name = "Application Development",
            Credits = 3,
            EnrollmentLimit = 0,
            LecturerId = "lecturer-id-1"
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(CourseInputViewModel.EnrollmentLimit)));
    }

    [Fact]
    public void CourseInputViewModel_WithMissingLecturerId_ShouldBeInvalid()
    {
        var model = new CourseInputViewModel
        {
            Code = "CS6004",
            Name = "Application Development",
            Credits = 3,
            EnrollmentLimit = 40,
            LecturerId = string.Empty
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(CourseInputViewModel.LecturerId)));
    }

    [Fact]
    public void AssignmentInputViewModel_WithMissingTitle_ShouldBeInvalid()
    {
        var model = new AssignmentInputViewModel
        {
            CourseId = 1,
            Title = string.Empty,
            DueDateLocal = DateTime.UtcNow.AddDays(7),
            MaxPoints = 100
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(AssignmentInputViewModel.Title)));
    }

    [Fact]
    public void AssignmentInputViewModel_WithInvalidMaxPoints_ShouldBeInvalid()
    {
        var model = new AssignmentInputViewModel
        {
            CourseId = 1,
            Title = "Coursework 1",
            DueDateLocal = DateTime.UtcNow.AddDays(7),
            MaxPoints = 0
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(AssignmentInputViewModel.MaxPoints)));
    }

    [Fact]
    public void MessageComposeViewModel_WithMissingRecipientSubjectAndContent_ShouldBeInvalid()
    {
        var model = new MessageComposeViewModel();

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(MessageComposeViewModel.RecipientId)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(MessageComposeViewModel.Subject)));
        results.Should().Contain(r => r.MemberNames.Contains(nameof(MessageComposeViewModel.Content)));
    }

    [Fact]
    public void MessageComposeViewModel_WithAllRequiredFields_ShouldBeValid()
    {
        var model = new MessageComposeViewModel
        {
            RecipientId = "lecturer-id-1",
            Subject = "Question about assignment",
            Content = "Could we extend the deadline by one day?"
        };

        var results = ValidationTestHelper.ValidateModel(model);

        results.Should().BeEmpty();
    }
}
