using AD_COURSEWORK_2.ViewModels;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class LoginViewModelValidationTests
{
    [Fact]
    public void Missing_email_is_rejected()
    {
        var model = new LoginViewModel
        {
            Email = string.Empty,
            Password = "AnyPassword1!"
        };

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(LoginViewModel.Email)));
    }

    [Fact]
    public void Missing_password_is_rejected()
    {
        var model = new LoginViewModel
        {
            Email = "user@example.com",
            Password = string.Empty
        };

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(LoginViewModel.Password)));
    }

    [Fact]
    public void Invalid_email_format_is_rejected()
    {
        var model = new LoginViewModel
        {
            Email = "not-an-email",
            Password = "AnyPassword1!"
        };

        var errors = ValidationTestHelper.ValidateAll(model);

        errors.Should().Contain(e => e.MemberNames.Contains(nameof(LoginViewModel.Email)));
    }
}
