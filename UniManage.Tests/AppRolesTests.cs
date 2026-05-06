using AD_COURSEWORK_2.Models;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class AppRolesTests
{
    [Fact]
    public void AppRoles_ShouldContainExpectedRoleNames()
    {
        AppRoles.Administrator.Should().Be("Administrator");
        AppRoles.Lecturer.Should().Be("Lecturer");
        AppRoles.Student.Should().Be("Student");
    }

    [Fact]
    public void AppRoles_ShouldNotContainEmptyValues()
    {
        AppRoles.Administrator.Should().NotBeNullOrWhiteSpace();
        AppRoles.Lecturer.Should().NotBeNullOrWhiteSpace();
        AppRoles.Student.Should().NotBeNullOrWhiteSpace();
    }
}
