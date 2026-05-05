# Project Scan Summary - UniManage UCMS

Prepared for CS6004ES Application Development Coursework 2.

Scan date: 5 May 2026  
Repository path: `C:\Users\HP\source\repos\AD COURSEWORK 2`  
Current branch detected: `main`  
Remote detected: `https://github.com/YasasPasinduFernando/AD-COURSEWORK-2.git`  
Build check: `dotnet build --no-restore` succeeded with 0 warnings and 0 errors.

## 1. Detected Technology Stack

The repository contains a single Visual Studio solution:

- Solution: `AD COURSEWORK 2.sln`
- Project: `AD COURSEWORK 2.csproj`
- Visual Studio solution format: Visual Studio Version 17
- Project SDK: `Microsoft.NET.Sdk.Web`
- Target framework: `net8.0`
- MVC framework: ASP.NET Core MVC on .NET 8
- Language: C# with nullable reference types and implicit usings enabled
- Database access: Entity Framework Core 8
- Database provider: Pomelo Entity Framework Core MySQL
- Database server version configured in code: MySQL 8.0.36
- Authentication: ASP.NET Core Identity with role-based authorisation
- Optional external authentication: Google authentication package
- Reporting export support: CSV writer and QuestPDF PDF report generation
- UI libraries and assets: Bootstrap, jQuery, jQuery validation, Bootstrap Icons, custom CSS, campus image assets
- Build output folders detected: `bin`, `obj`, and `artifacts_build`

Package evidence from `AD COURSEWORK 2.csproj`:

- `Microsoft.AspNetCore.Authentication.Google` 8.0.11
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` 8.0.11
- `Microsoft.EntityFrameworkCore.Design` 8.0.11
- `Microsoft.EntityFrameworkCore.Tools` 8.0.11
- `Pomelo.EntityFrameworkCore.MySql` 8.0.2
- `QuestPDF` 2024.12.3

The application is ASP.NET Core MVC rather than classic ASP.NET MVC 5.

## 2. Main Project Modules

### Controllers

- `AccountController.cs` - registration, login, logout, Google login callback, access denied, forgotten password and reset password flows.
- `DashboardController.cs` - role-based redirection and separate Student, Lecturer, and Administrator dashboards.
- `CoursesController.cs` - administrator course CRUD, lecturer course listing, course details, and lecturer material upload.
- `EnrollmentsController.cs` - student course catalogue browsing and enrolment logic.
- `AssignmentsController.cs` - lecturer assignment management and student assignment list.
- `SubmissionsController.cs` - student submission, lecturer grading, and secure file download for materials/submissions.
- `ReportsController.cs` - analytics views and CSV/PDF exports.
- `MessagesController.cs` - student-lecturer messaging, inbox, conversation thread, replies, and mark all read.
- `MeetingsController.cs` - online meeting scheduling, meeting joins, student notifications, and calendar file generation.
- `ProfileController.cs` - profile update and password change.
- `AuditLogsController.cs` - administrator audit log browsing with filters and pagination.
- `HomeController.cs` - landing page and error action.

### Models and Entities

- `ApplicationUser` - Identity user extension with full name, phone number, and navigation collections.
- `Course` - course code, name, description, credits, enrolment limit, lecturer, prerequisite, materials, assignments and meetings.
- `Enrollment` - student-course enrolment link with timestamp.
- `Assignment` - course assignment with title, description, due date and maximum points.
- `Submission` - student submission with text, file metadata, status, grade and feedback.
- `SubmissionStatus` - submission status enum.
- `Message` - direct message between sender and receiver.
- `CourseMaterial` - uploaded course material metadata.
- `Meeting` - online meeting details with course, lecturer, date/time and meeting URL.
- `AuditLog` - audit entry with category, action, user information, IP address and user agent.
- `AppRoles` - role constants for Student, Lecturer and Administrator.

### Data Layer

- `ApplicationDbContext.cs` - Identity database context and DbSets for courses, enrolments, assignments, submissions, messages, course materials, audit logs and meetings.
- `ApplicationDbContextFactory.cs` - design-time DbContext factory for EF Core tooling.
- `DbInitializer.cs` - database migration and seed logic.
- `Data/Migrations` - EF Core migrations and model snapshot.

Migrations detected:

- `20260408041036_InitialUniManage`
- `20260501122117_AddAuditLogAndSecurity`
- `20260501125135_AddMeetings`
- `ApplicationDbContextModelSnapshot.cs`

Raw SQL script files were not detected. The project uses EF Core migrations instead.

### ViewModels

View models are present for login, registration, profile, course input, assignment input, submissions, grading, messaging, meetings, dashboards and reports.

### Views and Pages

Razor view folders detected:

- `Account`
- `Assignments`
- `AuditLogs`
- `Courses`
- `Dashboard`
- `Enrollments`
- `Home`
- `Meetings`
- `Messages`
- `Profile`
- `Reports`
- `Shared`
- `Submissions`

Shared views include `_Layout.cshtml`, `_Alerts.cshtml`, `_DashboardCalendar.cshtml`, `_ValidationScriptsPartial.cshtml`, and `Error.cshtml`.

### Infrastructure

- `AuditLogger.cs` and `IAuditLogger.cs`
- `SmtpEmailService.cs`, `IEmailService.cs`, `EmailSettings.cs`, and `EmailTemplates.cs`
- `UploadedFileStore.cs`
- `SecurityHeadersMiddleware.cs`
- `CsvWriter.cs`
- `PdfReport.cs`
- `CalendarLink.cs`
- `MeetLinkGenerator.cs`

### Static Assets

- Main stylesheet: `wwwroot/css/site.css`
- JavaScript: `wwwroot/js/site.js`
- Bootstrap, jQuery and validation libraries under `wwwroot/lib`
- Images under `wwwroot/assets/images`
- Google icon under `wwwroot/assets/icons`
- Upload placeholder folder: `wwwroot/uploads/.gitkeep`

## 3. Implemented Coursework Tasks

The following coursework-related functions are evidenced in the repository.

| Coursework area | Evidence found | Status |
|---|---|---|
| ASP.NET MVC web application | `AD COURSEWORK 2.csproj`, `Program.cs`, Controllers, Views | Implemented |
| Visual Studio solution | `AD COURSEWORK 2.sln` and single web project | Implemented |
| Authentication | ASP.NET Core Identity in `Program.cs`, `AccountController.cs`, Identity models | Implemented |
| Role management | `AppRoles.cs`, `DbInitializer.cs`, role-based `[Authorize]` attributes | Implemented |
| Student role | Student dashboard, catalogue, enrolment, assignments, submissions, messages and meetings | Implemented |
| Lecturer role | Lecturer dashboard, own courses, material upload, assignment management, grading, messages and meetings | Implemented |
| Administrator role | Administrator dashboard, course CRUD, reports and audit logs | Implemented |
| Course management | `CoursesController.cs`, Course views, `Course` entity | Implemented |
| Enrolment feature | `EnrollmentsController.cs`, `Enrollment` entity, Browse view | Implemented |
| Assignment feature | `AssignmentsController.cs`, `Assignment` entity, Assignment views | Implemented |
| Grading feature | `SubmissionsController.cs`, `Submission` entity, Grade view | Implemented |
| Reporting and analytics | `ReportsController.cs`, report view models, CSV and PDF helpers | Implemented |
| Communication and messaging | `MessagesController.cs`, `Message` entity, message views | Implemented |
| Meetings / live sessions | `MeetingsController.cs`, `Meeting` entity, calendar helpers | Implemented |
| Course material upload | `CoursesController.UploadMaterial`, `UploadedFileStore.cs`, `CourseMaterial` entity | Implemented |
| Audit logging | `AuditLog` entity, `AuditLogger.cs`, `AuditLogsController.cs` | Implemented |
| Validation | Data annotations and `ModelState` checks across models/view models/controllers | Implemented |
| Exception handling | `UseExceptionHandler`, `UseDeveloperExceptionPage`, error view, controller `try/catch` blocks | Implemented |
| Test/seed data | `DbInitializer.cs` creates roles, demo users, demo courses and one assignment | Implemented |
| README/setup documentation | `README.md` exists and includes setup/configuration notes | Implemented |
| Build verification | `dotnet build --no-restore` succeeded on 5 May 2026 | Implemented |

## 4. Missing or Unclear Coursework Tasks

The following items were missing, unclear, or not evidenced by the current scan:

- Dedicated administrator user-management screens for creating/editing users or assigning roles were not detected. Role creation is seeded, and student/lecturer self-registration exists. [NEEDS CONFIRMATION]
- Automated test projects, unit tests, integration tests, or browser tests were not detected. [NEEDS CONFIRMATION]
- Existing coursework screenshots were not detected in the repository. The `wwwroot/assets/images` folder contains UI theme images, not submission evidence screenshots. [NEEDS CONFIRMATION]
- Raw SQL scripts were not detected. EF Core migrations are present instead. [NEEDS CONFIRMATION if raw SQL scripts are required]
- A database backup/export file was not detected. [NEEDS CONFIRMATION]
- A deployment target, hosting URL, or published web server evidence was not detected. [NEEDS CONFIRMATION]
- Formal user acceptance testing evidence was not detected. [NEEDS CONFIRMATION]
- Exact individual contribution ownership is not evidenced by the repository scan alone. [NEEDS CONFIRMATION]
- Coursework brief/rubric files were not detected in the repository. [NEEDS CONFIRMATION]
- Final Word/PDF submission file was not detected. [NEEDS CONFIRMATION]
- Sensitive configuration values exist in application settings files. The documentation deliberately does not reproduce those values; the final submission should use redacted configuration examples. [NEEDS CONFIRMATION]

## 5. Suggested Screenshots Needed

The screenshots folder has been created at `documentation_application_development/screenshots/`. Suggested screenshots include:

1. Visual Studio solution open with `AD COURSEWORK 2.sln`.
2. Successful build output showing 0 errors.
3. Home page.
4. Register page with role selection.
5. Login page, including optional Google sign-in button if configured.
6. Access denied page.
7. Student dashboard.
8. Student course catalogue / enrolment page.
9. Student course details page showing course materials.
10. Student assignment list.
11. Student submission form.
12. Student grade/feedback view.
13. Lecturer dashboard.
14. Lecturer My Courses page.
15. Lecturer course details and material upload.
16. Lecturer assignment create/edit page.
17. Lecturer submissions list for an assignment.
18. Lecturer grading page.
19. Administrator dashboard.
20. Administrator course list with search/pagination.
21. Administrator create/edit course page.
22. Reports index.
23. Each report page: course popularity, student performance, lecturer workload, enrolments, pass/fail, and assignment attendance.
24. CSV export evidence.
25. PDF export evidence.
26. Messaging inbox.
27. Messaging conversation thread.
28. Meeting index.
29. Meeting create/edit page.
30. Calendar/ICS download evidence if used.
31. Audit log page.
32. Profile and change password pages.
33. Validation error examples, such as invalid login, missing required field, invalid file, or invalid meeting URL.
34. Database table/migration evidence from MySQL or EF Core tooling. [NEEDS CONFIRMATION]

## 6. Suggested Diagrams Needed

The diagrams folder has been created at `documentation_application_development/diagrams/`. Suggested diagrams:

1. MVC architecture diagram showing browser, controllers, views, models, services and MySQL database.
2. Entity relationship diagram for users, roles, courses, enrolments, assignments, submissions, messages, materials, meetings and audit logs.
3. Role-based access matrix for Student, Lecturer and Administrator.
4. Use case diagram for the UCMS actors.
5. Student enrolment workflow.
6. Assignment submission and grading sequence diagram.
7. Reporting data flow from database entities to report views, CSV export and PDF export.
8. Authentication flow for local login and optional Google login.
9. Deployment/setup diagram for Visual Studio, ASP.NET Core application and MySQL database. [NEEDS CONFIRMATION]

## 7. Suggested Appendices

Suggested appendices for the final coursework report:

- Appendix A: GitHub repository link and local solution path.
- Appendix B: Build output evidence.
- Appendix C: Database migration list and entity relationship diagram.
- Appendix D: Seeded user accounts and seeded course data.
- Appendix E: Screenshots by role and feature.
- Appendix F: Validation and security evidence.
- Appendix G: Report export evidence for CSV and PDF.
- Appendix H: Individual contribution evidence. [NEEDS CONFIRMATION]
- Appendix 9: Test plan, test cases and results. [NEEDS CONFIRMATION]
- Appendix 10: Deployment or setup evidence. [NEEDS CONFIRMATION]
- Appendix 11: Redacted configuration sample.

## 8. Questions That Must Be Answered Before Final Submission

1. What is the author's full name, student ID, group number and module cohort?
2. Which features were personally implemented by the author?
3. Which screenshots have been captured and approved for inclusion?
4. Is the final submission expected as Word, PDF, or both?
5. Does the coursework require raw SQL scripts, or are EF Core migrations acceptable?
6. Is a deployed URL required, or is local Visual Studio execution sufficient?
7. Should the optional Google login be included in the final report, or treated as an extension?
8. Should the meeting/calendar feature be presented as part of communication, or as an additional enhancement?
9. Are automated tests required by the marking criteria?
10. Has the appsettings configuration been redacted before submission?
11. Are there any group contribution records that should be cited without blaming other members?
12. Which database instance and sample data should be shown in screenshots?
13. Should the GitHub repository remain public for marking, or should a downloadable ZIP be provided?
14. Are there required page limits, font sizes, citation style, or appendix rules?
