# Evidence Checklist

Coursework: CS6004ES Application Development Coursework 2  
Application: UniManage - University Course Management System

## Status Key

- Completed - evidence is present or has been verified.
- Pending - evidence should be captured before final submission.
- [NEEDS CONFIRMATION] - the repository scan cannot verify this item.

## Repository and Build Evidence

| Evidence item | Source/path | Status | Notes |
|---|---|---|---|
| Visual Studio solution | `AD COURSEWORK 2.sln` | Completed | Single project solution detected. |
| ASP.NET Core project file | `AD COURSEWORK 2.csproj` | Completed | Targets `net8.0`. |
| Build output | `dotnet build --no-restore` | Completed | Succeeded with 0 warnings and 0 errors on 5 May 2026. |
| Git remote | `origin` | Completed | `https://github.com/YasasPasinduFernando/AD-COURSEWORK-2.git` |
| Current branch | `main` | Completed | Detected by repository scan. |
| README/setup file | `README.md` | Completed | Includes features, setup and seeded accounts. |
| Workflow files | `.github/workflows` | [NEEDS CONFIRMATION] | Directory exists but no workflow file was detected. |
| Deployment artefacts | `artifacts_build/` | Completed | Build artefacts detected; deployment target still unclear. |
| Public deployment URL | Not detected | [NEEDS CONFIRMATION] | Add if required by the coursework. |

## Code Evidence

| Area | Evidence files | Status |
|---|---|---|
| Application startup | `Program.cs` | Completed |
| MVC controllers | `Controllers/` | Completed |
| Razor views | `Views/` | Completed |
| Entity models | `Models/` | Completed |
| View models | `ViewModels/` | Completed |
| Database context | `Data/ApplicationDbContext.cs` | Completed |
| EF migrations | `Data/Migrations/` | Completed |
| Seed data | `Data/DbInitializer.cs` | Completed |
| Authentication | `AccountController.cs`, `Program.cs`, `ApplicationUser.cs` | Completed |
| Role constants | `Models/AppRoles.cs` | Completed |
| Security middleware | `Infrastructure/SecurityHeadersMiddleware.cs` | Completed |
| Audit logging | `Infrastructure/AuditLogger.cs`, `AuditLogsController.cs` | Completed |
| Email service | `Infrastructure/SmtpEmailService.cs`, `EmailTemplates.cs` | Completed |
| Upload service | `Infrastructure/UploadedFileStore.cs` | Completed |
| CSV export | `Infrastructure/CsvWriter.cs` | Completed |
| PDF export | `Infrastructure/PdfReport.cs` | Completed |
| SQL scripts | Not detected | [NEEDS CONFIRMATION] |
| Automated tests | Not detected | [NEEDS CONFIRMATION] |

## Screenshot Checklist

Capture screenshots into `documentation_application_development/screenshots/`.

- [ ] Visual Studio solution explorer.
- [ ] Successful build output.
- [ ] Home page.
- [ ] Register page.
- [ ] Login page.
- [ ] Student dashboard.
- [ ] Student catalogue/enrolment page.
- [ ] Student assignment list.
- [ ] Student submission form.
- [ ] Student grade/feedback view.
- [ ] Lecturer dashboard.
- [ ] Lecturer course details.
- [ ] Lecturer material upload.
- [ ] Lecturer assignment create/edit.
- [ ] Lecturer submissions list.
- [ ] Lecturer grading page.
- [ ] Administrator dashboard.
- [ ] Administrator course list.
- [ ] Administrator course create/edit.
- [ ] Reports index.
- [ ] Course popularity report.
- [ ] Student performance report.
- [ ] Lecturer workload report.
- [ ] Enrolments report.
- [ ] Pass/fail report.
- [ ] Assignment attendance report.
- [ ] CSV export file opened or downloaded.
- [ ] PDF export file opened or downloaded.
- [ ] Messaging inbox.
- [ ] Messaging thread.
- [ ] Meeting list.
- [ ] Meeting create/edit page.
- [ ] Audit log page.
- [ ] Profile page.
- [ ] Change password page.
- [ ] Validation error example.
- [ ] Access denied page.
- [ ] Database tables or EF migration evidence. [NEEDS CONFIRMATION]

## Diagram Checklist

- [ ] MVC architecture diagram.
- [ ] Entity relationship diagram.
- [ ] Role access matrix.
- [ ] Use case diagram.
- [ ] Student enrolment workflow.
- [ ] Assignment submission and grading sequence.
- [ ] Reporting/export data flow.
- [ ] Authentication flow.
- [ ] Deployment/setup diagram. [NEEDS CONFIRMATION]

## Testing Checklist

| Test area | Suggested evidence | Status |
|---|---|---|
| Build test | Build output screenshot/log | Completed |
| Login test | Screenshot of successful login | Pending |
| Invalid login test | Screenshot of validation/error message | Pending |
| Student enrolment test | Before/after screenshot | Pending |
| Prerequisite block test | Screenshot or test note | [NEEDS CONFIRMATION] |
| Course full block test | Screenshot or test note | [NEEDS CONFIRMATION] |
| Assignment submission test | Screenshot and database evidence | Pending |
| File upload validation test | Screenshot or test note | Pending |
| Lecturer grading test | Screenshot and student grade evidence | Pending |
| Report export test | CSV/PDF evidence | Pending |
| Messaging test | Two-role conversation evidence | Pending |
| Meeting scheduling test | Meeting page and email/calendar evidence | Pending |
| Access control test | Access denied screenshot | Pending |

## Configuration Evidence

Configuration files are present, but sensitive values should be redacted in any submitted document:

- `appsettings.json`
- `appsettings.Development.json`
- `Properties/launchSettings.json`

Do not include database passwords, SMTP passwords, Google client secrets or other private configuration values in screenshots or appendices.

## Individual Contribution Evidence

The following must be confirmed before final submission:

- [ ] Author name and student ID.
- [ ] Feature ownership.
- [ ] Commit evidence or task evidence.
- [ ] Screenshots linked to the author's contribution.
- [ ] Short reflective contribution statement.
- [ ] Any group work evidence phrased neutrally.

