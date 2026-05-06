# Unit Test Code Examples (Appendix Snippets)

These are short excerpts from the `UniManage.Tests` project. Full files live under the `UniManage.Tests/` folder on the `unit-test` branch. The snippets below are intended for inclusion in the report appendix as illustrative evidence of test style; they are not full source listings.

## 1. ValidationTestHelper excerpt

**File:** `UniManage.Tests/Helpers/ValidationTestHelper.cs`  
**Purpose:** wrap `Validator.TryValidateObject` so each test can collect every data-annotation error in one call.

```csharp
using System.ComponentModel.DataAnnotations;

namespace UniManage.Tests.Helpers;

public static class ValidationTestHelper
{
    public static IList<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }
}
```

The helper validates **all** properties (not just `[Required]`), which is what the controller experiences during model binding.

## 2. AppRolesTests excerpt

**File:** `UniManage.Tests/AppRolesTests.cs`  
**Purpose:** lock the RBAC role string constants so accidental renames are caught quickly.

```csharp
[Fact]
public void AppRoles_ShouldContainExpectedRoleNames()
{
    AppRoles.Administrator.Should().Be("Administrator");
    AppRoles.Lecturer.Should().Be("Lecturer");
    AppRoles.Student.Should().Be("Student");
}
```

These constants are referenced by `[Authorize(Roles = AppRoles.X)]` attributes throughout the project, so any drift would cascade into broken authorisation.

## 3. LoginViewModel validation test excerpt

**File:** `UniManage.Tests/ViewModelValidationTests.cs`  
**Purpose:** confirm the login form rejects empty credentials before the controller runs.

```csharp
[Fact]
public void LoginViewModel_WithMissingEmailAndPassword_ShouldBeInvalid()
{
    var model = new LoginViewModel();

    var results = ValidationTestHelper.ValidateModel(model);

    results.Should().NotBeEmpty();
    results.Should().Contain(r => r.MemberNames.Contains(nameof(LoginViewModel.Email)));
    results.Should().Contain(r => r.MemberNames.Contains(nameof(LoginViewModel.Password)));
}
```

This protects the `[Required]` and `[EmailAddress]` rules on `LoginViewModel`. A regression that removes either attribute would surface here as soon as `dotnet test` runs.

## 4. RegisterViewModel valid input test excerpt

**File:** `UniManage.Tests/ViewModelValidationTests.cs`  
**Purpose:** prove the registration view model is valid for a realistic happy-path payload (`FullName`, `Email`, matching `Password`/`ConfirmPassword`, allowed `Role`).

```csharp
[Fact]
public void RegisterViewModel_WithValidInput_ShouldBeValid()
{
    var model = new RegisterViewModel
    {
        FullName = "Test Student",
        Email = "student@test.com",
        Password = "Password123!",
        ConfirmPassword = "Password123!",
        Role = AppRoles.Student
    };

    var results = ValidationTestHelper.ValidateModel(model);

    results.Should().BeEmpty();
}
```

This complements the negative tests for missing fields, mismatched confirm-password, and password-too-short, all of which live in the same file.

## 5. Infrastructure utility test excerpt – MeetLinkGenerator

**File:** `UniManage.Tests/InfrastructureUtilityTests.cs`  
**Purpose:** confirm the meeting link generator returns a non-empty Google Meet URL that the project's own validator accepts.

```csharp
[Fact]
public void MeetLinkGenerator_GenerateGoogleMeetUrl_ShouldReturnNonEmptyMeetUrl()
{
    var url = MeetLinkGenerator.GenerateGoogleMeetUrl();

    url.Should().NotBeNullOrWhiteSpace();
    url.Should().StartWith("https://meet.google.com/");
    MeetLinkGenerator.IsValidMeetingUrl(url).Should().BeTrue();
}
```

A second test in the same file exercises `IsValidMeetingUrl` directly with empty input, an unknown host, and known hosts (Zoom, Teams), which keeps the URL allow-list honest.
