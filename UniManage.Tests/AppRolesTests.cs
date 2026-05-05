using AD_COURSEWORK_2.Models;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class AppRolesTests
{
    [Fact]
    public void Administrator_is_non_empty_stable_string()
    {
        AppRoles.Administrator.Should().Be("Administrator");
        AppRoles.Administrator.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Lecturer_is_non_empty_stable_string()
    {
        AppRoles.Lecturer.Should().Be("Lecturer");
        AppRoles.Lecturer.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Student_is_non_empty_stable_string()
    {
        AppRoles.Student.Should().Be("Student");
        AppRoles.Student.Should().NotBeNullOrWhiteSpace();
    }
}
