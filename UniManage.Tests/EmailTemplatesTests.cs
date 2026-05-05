using System.Text.Encodings.Web;
using AD_COURSEWORK_2.Infrastructure;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class EmailTemplatesTests
{
    [Fact]
    public void BuildPasswordResetEmail_contains_reset_guidance_and_encoded_link()
    {
        var html = EmailTemplates.BuildPasswordResetEmail(
            "Alex Student",
            "https://lms.example.com/Account/ResetPassword?token=abc");

        html.Should().Contain("Reset your UniManage password");
        html.Should().Contain("Password reset");
        html.Should().Contain("Reset password");
        html.Should().Contain(HtmlEncoder.Default.Encode("https://lms.example.com/Account/ResetPassword?token=abc"));
    }
}
