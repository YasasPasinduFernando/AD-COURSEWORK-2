# Unit Test Evidence (Supporting Coursework)

**Project:** UniManage – University Course Management System  
**Branch name:** `unit-test` (branch from `main`; merge into `main` only after review)  
**Test project:** `UniManage.Tests`  
**Framework:** xUnit 2.9 with FluentAssertions 6.12  
**Test type:** selected unit tests (no integration / no end-to-end)  
**Application target:** .NET 8 (`AD COURSEWORK 2.csproj`)

## Why tests were added

The coursework is assessed primarily through the running application and Visual Studio. The `unit-test` branch adds a small xUnit project so that Chapter 15 of the main report can cite **selected automated checks** alongside the existing manual test plan. The tests provide quick, deterministic confirmation that important data-annotation rules and small infrastructure utilities behave as documented. They do not claim full behavioural or regression coverage.

Final coursework documentation and publish-ready Word and PDF outputs remain on the `main` branch (with report Markdown maintained on the `documentation` branch). The `unit-test` branch is **not** the final submission branch.

## Test classes created

| File | Purpose |
|---|---|
| `UniManage.Tests/Helpers/ValidationTestHelper.cs` | Shared `ValidateModel` helper that runs `Validator.TryValidateObject` with `validateAllProperties: true`. |
| `UniManage.Tests/AppRolesTests.cs` | Confirms the `AppRoles` constants used for RBAC. |
| `UniManage.Tests/ViewModelValidationTests.cs` | Data-annotation validation across `LoginViewModel`, `RegisterViewModel`, `CourseInputViewModel`, `AssignmentInputViewModel`, and `MessageComposeViewModel`. |
| `UniManage.Tests/InfrastructureUtilityTests.cs` | Selected behaviour checks for `CsvWriter`, `CalendarLink`, `MeetLinkGenerator`, `EmailTemplates.BuildPasswordResetEmail`, `SecurityHeadersMiddleware`, and `SubmissionGradingRules`. |

## Test cases

| Test ID | Test class | Test method | Purpose | Expected result | Status |
|---|---|---|---|---|---|
| UT1 | `AppRolesTests` | `AppRoles_ShouldContainExpectedRoleNames` | Stable role string constants | Values match `Administrator`, `Lecturer`, `Student` | Pass |
| UT2 | `AppRolesTests` | `AppRoles_ShouldNotContainEmptyValues` | No accidental empty role | Each constant is non-empty | Pass |
| UT3 | `ViewModelValidationTests` | `LoginViewModel_WithMissingEmailAndPassword_ShouldBeInvalid` | Required `Email` and `Password` | Validation errors on both | Pass |
| UT4 | `ViewModelValidationTests` | `LoginViewModel_WithInvalidEmailFormat_ShouldBeInvalid` | `[EmailAddress]` on `Email` | Validation error on `Email` | Pass |
| UT5 | `ViewModelValidationTests` | `RegisterViewModel_WithValidInput_ShouldBeValid` | Happy path for registration | No validation errors | Pass |
| UT6 | `ViewModelValidationTests` | `RegisterViewModel_WithMissingRequiredFields_ShouldBeInvalid` | Required attributes | Errors on `FullName`, `Email`, `Password`, `Role` | Pass |
| UT7 | `ViewModelValidationTests` | `RegisterViewModel_WithMismatchedConfirmPassword_ShouldBeInvalid` | `[Compare]` on `ConfirmPassword` | Error on `ConfirmPassword` | Pass |
| UT8 | `ViewModelValidationTests` | `RegisterViewModel_WithPasswordBelowMinimumLength_ShouldBeInvalid` | `[StringLength]` minimum | Error on `Password` | Pass |
| UT9 | `ViewModelValidationTests` | `RegisterViewModel_AllowedRoles_ShouldContainOnlyStudentAndLecturer` | Self-service role list | Equals `{Student, Lecturer}` | Pass |
| UT10 | `ViewModelValidationTests` | `CourseInputViewModel_WithMissingCodeAndName_ShouldBeInvalid` | Required `Code` and `Name` | Errors on both | Pass |
| UT11 | `ViewModelValidationTests` | `CourseInputViewModel_WithEnrolmentLimitOutOfRange_ShouldBeInvalid` | `[Range]` on `EnrollmentLimit` | Error on `EnrollmentLimit` | Pass |
| UT12 | `ViewModelValidationTests` | `CourseInputViewModel_WithMissingLecturerId_ShouldBeInvalid` | Required `LecturerId` | Error on `LecturerId` | Pass |
| UT13 | `ViewModelValidationTests` | `AssignmentInputViewModel_WithMissingTitle_ShouldBeInvalid` | Required `Title` | Error on `Title` | Pass |
| UT14 | `ViewModelValidationTests` | `AssignmentInputViewModel_WithInvalidMaxPoints_ShouldBeInvalid` | `[Range]` on `MaxPoints` | Error on `MaxPoints` | Pass |
| UT15 | `ViewModelValidationTests` | `MessageComposeViewModel_WithMissingRecipientSubjectAndContent_ShouldBeInvalid` | Required messaging fields | Errors on `RecipientId`, `Subject`, `Content` | Pass |
| UT16 | `ViewModelValidationTests` | `MessageComposeViewModel_WithAllRequiredFields_ShouldBeValid` | Happy path for messaging | No validation errors | Pass |
| UT17 | `InfrastructureUtilityTests` | `CsvWriter_Build_ShouldIncludeHeadersAndUtf8Bom` | Report CSV helper | UTF-8 BOM, header row, data row | Pass |
| UT18 | `InfrastructureUtilityTests` | `CalendarLink_BuildIcs_ShouldContainTitleStartAndUrl` | Meeting `.ics` content | Contains VCALENDAR, SUMMARY, URL | Pass |
| UT19 | `InfrastructureUtilityTests` | `CalendarLink_GoogleCalendarUrl_ShouldContainEncodedTitleAndCalendarHost` | Google Calendar deep link | URL host and encoded title | Pass |
| UT20 | `InfrastructureUtilityTests` | `MeetLinkGenerator_GenerateGoogleMeetUrl_ShouldReturnNonEmptyMeetUrl` | Generated meeting link | Starts with `https://meet.google.com/`, accepted by `IsValidMeetingUrl` | Pass |
| UT21 | `InfrastructureUtilityTests` | `MeetLinkGenerator_IsValidMeetingUrl_ShouldRejectUnknownHostsAndEmptyValues` | URL validator | Rejects empty / unknown host; accepts Zoom and Teams hosts | Pass |
| UT22 | `InfrastructureUtilityTests` | `EmailTemplates_BuildPasswordResetEmail_ShouldContainResetGuidanceAndEncodedLink` | Password reset HTML | Expected copy and encoded reset URL | Pass |
| UT23 | `InfrastructureUtilityTests` | `SecurityHeadersMiddleware_ShouldAddExpectedResponseHeaders` | Security response headers | `X-Frame-Options`, `X-Content-Type-Options`, CSP, etc. | Pass |
| UT24–UT28 | `InfrastructureUtilityTests` | `SubmissionGradingRules_IsValidGrade_ShouldRespectInclusiveZeroToMax` (theory) | Inclusive bounds for grading | Five `[InlineData]` rows match expected | Pass |

*(UT24–UT28 are the five `[InlineData]` rows of one `[Theory]` method; xUnit reports them as five separate outcomes.)*

## Command

From the repository root, with `unit-test` checked out:

```text
dotnet test "AD COURSEWORK 2.sln"
```

## Result summary

```text
Passed!  - Failed: 0, Passed: 28, Skipped: 0, Total: 28
Duration: ~800 ms - UniManage.Tests.dll (net8.0)
```

Re-run before submission so Figure 21 (the `dotnet test` console screenshot for Chapter 15.10) reflects the latest result.

## Limitations

- Selected unit tests only; not a full behavioural or regression suite.
- No live MySQL or `DbContext` integration tests.
- No SMTP, no real email delivery.
- No Google OAuth.
- No browser automation, no Razor view rendering tests.
- No Google Cloud or Docker dependency in the test process.

## Note

Final documentation and publish-ready Word and PDF outputs remain on the `main` branch. Report Markdown is authored on the `documentation` branch. The `unit-test` branch contributes the test project and this evidence file only; merge into `main` after diff review.
