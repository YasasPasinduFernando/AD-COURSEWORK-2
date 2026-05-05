using AD_COURSEWORK_2.Models;
using AD_COURSEWORK_2.ViewModels;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class RegisterViewModelValidationTests
{
    [Fact]
    public void Missing_full_name_is_rejected()
    {
        var model = ValidRegister();
        model.FullName = string.Empty;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(RegisterViewModel.FullName)));
    }

    [Fact]
    public void Missing_email_is_rejected()
    {
        var model = ValidRegister();
        model.Email = string.Empty;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(RegisterViewModel.Email)));
    }

    [Fact]
    public void Missing_password_is_rejected()
    {
        var model = ValidRegister();
        model.Password = string.Empty;
        model.ConfirmPassword = string.Empty;

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(RegisterViewModel.Password)));
    }

    [Fact]
    public void Password_and_confirm_password_mismatch_is_rejected()
    {
        var model = ValidRegister();
        model.Password = "Password1!";
        model.ConfirmPassword = "Password2!";

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(RegisterViewModel.ConfirmPassword)));
    }

    [Fact]
    public void Password_shorter_than_minimum_length_is_rejected()
    {
        var model = ValidRegister();
        model.Password = "Short1!";
        model.ConfirmPassword = "Short1!";

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(RegisterViewModel.Password)));
    }

    [Fact]
    public void Allowed_roles_list_contains_only_student_and_lecturer()
    {
        RegisterViewModel.AllowedRoles.Should().BeEquivalentTo(new[] { AppRoles.Student, AppRoles.Lecturer });
    }

    private static RegisterViewModel ValidRegister() => new()
    {
        FullName = "Test User",
        Email = "test@example.com",
        Phone = null,
        Password = "Password1!",
        ConfirmPassword = "Password1!",
        Role = AppRoles.Student
    };
}
