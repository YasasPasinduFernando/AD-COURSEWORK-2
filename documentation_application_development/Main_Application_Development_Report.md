# Main Application Development Report

Coursework: CS6004ES Application Development Coursework 2  
Case study: UniManage - University Course Management System (UCMS)  
Application type: ASP.NET Core MVC web application using C# in Visual Studio  
Author details: [NEEDS CONFIRMATION]  
Submission date: [NEEDS CONFIRMATION]

## Abstract

This report documents the design and implementation of UniManage, a university course management system developed as an ASP.NET Core MVC web application. The application supports role-based access for students, lecturers and administrators. Repository evidence shows implemented features for authentication, course management, enrolment, assignment submission, grading, reporting, messaging, online meetings, audit logging and profile management. The system uses Entity Framework Core with a MySQL database and ASP.NET Core Identity for user and role management. This report records the implemented technical design, evidence sources and remaining confirmation points required before final submission.

## 1. Introduction

The UniManage case study requires a web-based system for managing university courses and related academic workflows. The current project is implemented as a Visual Studio solution named `AD COURSEWORK 2.sln`, containing a single ASP.NET Core MVC web project. The solution follows a conventional MVC structure with controllers, models, view models, Razor views, infrastructure services and a data access layer.

The aim of the application is to provide separate functionality for students, lecturers and administrators. Students can browse and enrol on courses, view assignments, submit work and communicate with lecturers. Lecturers can manage their own course materials, create assignments, grade submissions, arrange online meetings and communicate with enrolled students. Administrators can manage courses, review analytics and inspect audit logs.

## 2. Technology Stack

The repository scan detected the following stack:

- ASP.NET Core MVC on .NET 8.
- C# with nullable reference types enabled.
- Visual Studio solution format version 17.
- Entity Framework Core 8.
- Pomelo EF Core provider for MySQL.
- ASP.NET Core Identity for authentication and roles.
- Optional Google authentication package.
- QuestPDF for PDF report output.
- Bootstrap, jQuery, jQuery validation and custom CSS for the front end.

The application is therefore documented as an ASP.NET Core MVC 8.0 application, not as a classic ASP.NET MVC 5 application.

## 3. Solution Structure

The main folders and files are:

- `Controllers/` - MVC controller actions for each feature area.
- `Models/` - database entities and role constants.
- `ViewModels/` - form and display models used by Razor views.
- `Views/` - Razor views grouped by controller.
- `Data/` - EF Core DbContext, design-time factory, seed logic and migrations.
- `Infrastructure/` - email, audit logging, uploads, reporting and security helpers.
- `wwwroot/` - static CSS, JavaScript, libraries, image assets and upload placeholder.
- `Program.cs` - application startup, service registration and HTTP pipeline configuration.
- `README.md` - project setup and feature notes.

## 4. Requirements Mapping

| Requirement area | Implementation evidence | Confirmation status |
|---|---|---|
| User authentication | `AccountController`, `ApplicationUser`, ASP.NET Core Identity configuration | Evidenced |
| Role-based access | `AppRoles`, `[Authorize(Roles = ...)]`, seeded roles | Evidenced |
| Student features | Student dashboard, enrolment, submissions, messages and meetings | Evidenced |
| Lecturer features | Lecturer dashboard, course materials, assignments, grading, messages and meetings | Evidenced |
| Administrator features | Administrator dashboard, course CRUD, reports and audit logs | Evidenced |
| Course management | `CoursesController`, Course views and `Course` model | Evidenced |
| Enrolment | `EnrollmentsController`, `Enrollment` model | Evidenced |
| Assignment and grading | `AssignmentsController`, `SubmissionsController`, `Assignment` and `Submission` models | Evidenced |
| Reporting and analytics | `ReportsController`, CSV/PDF helpers and report views | Evidenced |
| Communication | `MessagesController`, `Message` model, meeting scheduling | Evidenced |
| Automated testing | No test project detected | [NEEDS CONFIRMATION] |
| Deployment evidence | No deployed URL detected | [NEEDS CONFIRMATION] |

## 5. Architecture

The application uses the Model-View-Controller pattern. Browser requests are routed to controllers through the default route configured in `Program.cs`. Controllers validate the request, check the user's role, access the database through `ApplicationDbContext`, and return Razor views or downloadable files.

The data layer uses Entity Framework Core. The context inherits from `IdentityDbContext<ApplicationUser>`, which allows the system to store custom application entities and Identity tables in the same MySQL database. Infrastructure services provide cross-cutting behaviour such as email notification, audit logging, upload validation, CSV generation, PDF generation and security headers.

## 6. Database Design

The main entities are:

- `ApplicationUser`
- `Course`
- `Enrollment`
- `Assignment`
- `Submission`
- `Message`
- `CourseMaterial`
- `Meeting`
- `AuditLog`

Important relationships include:

- A lecturer can teach many courses.
- A course can have many enrolments, assignments, materials and meetings.
- A student can have many enrolments and submissions.
- An assignment belongs to one course and can have many submissions.
- A message links a sender and receiver.
- A course may have a prerequisite course.

The database configuration is implemented in `ApplicationDbContext.cs`. The scan detected three EF Core migrations plus a model snapshot. Raw SQL scripts were not detected, so the report should state that database schema management is handled by EF Core migrations unless raw scripts are later added.

## 7. Authentication and Authorisation

Authentication is configured in `Program.cs` through ASP.NET Core Identity. Password settings require a minimum length of eight characters, digits, upper-case letters and lower-case letters. Lockout is enabled after repeated failed attempts. The application uses cookie authentication and defines login, logout and access denied paths.

Roles are defined in `AppRoles.cs` as Student, Lecturer and Administrator. `DbInitializer.cs` seeds these roles and creates default users. Role restrictions are applied with `[Authorize]` attributes in controllers.

The application also includes optional Google authentication. The controller logic requires a Google account email to match an existing user. This should be described as an optional external login extension. Sensitive Google and email configuration values must be redacted from any submitted evidence.

## 8. Main Feature Implementation

### 8.1 Student Features

The student dashboard shows enrolled courses, upcoming deadlines, recent grades, unread messages, activity and upcoming meetings. Students can browse available courses through the enrolment catalogue. The enrolment logic prevents duplicate enrolment, checks course capacity and enforces prerequisite enrolment where a prerequisite is configured.

Students can view assignments for their enrolled courses and submit work using text content and/or a file attachment. The submission workflow prevents access to assignments from courses where the student is not enrolled. Once graded, a submission cannot be resubmitted through the standard workflow.

### 8.2 Lecturer Features

Lecturers can view their own courses and manage assignments for those courses. The assignment controller restricts create, edit and delete operations to the lecturer assigned to the course. Lecturers can view submissions for their assignments and enter grades and feedback.

Lecturers can upload course materials. File uploads are checked by extension, content type and size through `UploadedFileStore.cs`. The repository also includes email notification logic for material uploads, assignment submissions and grade release.

### 8.3 Administrator Features

Administrators can access a dashboard with total users, courses, enrolments, assignments, submissions and messages. They can manage courses through create, edit and delete views. Administrators can also access reports and audit logs.

A dedicated administrator interface for managing users and roles was not detected. This should remain `[NEEDS CONFIRMATION]` unless further evidence is added.

### 8.4 Course Management

Course management is implemented in `CoursesController.cs`. Administrators can create, edit and delete courses. Course fields include code, name, description, credits, enrolment limit, lecturer and optional prerequisite. The controller checks duplicate course codes and validates lecturer and prerequisite selections.

### 8.5 Enrolment

Enrolment is implemented in `EnrollmentsController.cs`. Students can browse courses and enrol when the course is not full, the student is not already enrolled, and any prerequisite enrolment requirement is satisfied.

### 8.6 Assignment Submission and Grading

Assignment management is implemented in `AssignmentsController.cs`. Submission and grading are implemented in `SubmissionsController.cs`. Students submit work through a controlled route, while lecturers can grade only submissions for their own courses. Grading stores a score, feedback, grading timestamp and the lecturer who graded the work.

### 8.7 Reporting and Analytics

`ReportsController.cs` provides:

- Course popularity.
- Student performance.
- Lecturer workload.
- Enrolment timeline.
- Pass/fail analysis.
- Assignment attendance.

The reports can be viewed through Razor pages and exported as CSV or PDF. CSV generation is handled by `CsvWriter.cs`, while PDF generation is handled by `PdfReport.cs`.

### 8.8 Communication and Meetings

Student-lecturer messaging is implemented in `MessagesController.cs`. Students can message lecturers linked to their enrolled courses. Lecturers can message students enrolled in their courses. Messages support inbox listing, conversation threads, replies and read status.

The meetings module supports lecturer-created course meetings. Meeting URLs can be generated or validated for supported providers such as Google Meet, Zoom, Teams and Webex. Students can view and join meetings for enrolled courses. Calendar file generation and calendar deep links are supported through the calendar infrastructure.

## 9. Validation, Exception Handling and Security

Validation is implemented through data annotations in models and view models, as well as controller-level `ModelState` checks. Examples include required fields, string length limits, range checks, email validation, password confirmation and file validation.

Exception handling includes:

- Production exception route through `/Home/Error`.
- Developer exception page in development.
- Controller-level `try/catch` blocks around email delivery and audit logging.
- Status responses such as `NotFound`, `BadRequest`, `Forbid` and `Challenge`.

Security-related implementation includes:

- Role-based authorisation.
- Anti-forgery tokens on POST actions.
- Rate limiting for authentication and upload endpoints.
- Password lockout.
- Secure file access checks.
- Security headers middleware, including content security policy and frame protection.
- Audit logging for security-sensitive actions.

## 10. Build and Test Evidence

The project was built with:

`dotnet build --no-restore`

The build succeeded with 0 warnings and 0 errors on 5 May 2026.

Automated test projects were not detected in the repository. Manual test evidence, user acceptance test notes and screenshot evidence should be added before final submission. [NEEDS CONFIRMATION]

## 11. Setup and Deployment

The project can be opened in Visual Studio using `AD COURSEWORK 2.sln`. The application uses a MySQL connection string and EF Core migrations. `DbInitializer.cs` applies migrations at startup and seeds roles, demo users, demo courses and one demo assignment.

Local launch profiles are defined in `Properties/launchSettings.json`. The detected local URLs are:

- HTTP: `http://localhost:5103`
- HTTPS: `https://localhost:7212`
- IIS Express: configured with SSL port `44349`

A public deployment URL was not detected. [NEEDS CONFIRMATION]

## 12. Evaluation and Limitations

The application covers the main UCMS workflows required for a role-based university course management system. It demonstrates a structured MVC implementation, database persistence, role-based security, file upload handling, reporting exports and evidence of successful compilation.

Current limitations or confirmation items are:

- No automated test project was detected.
- No final screenshot pack was detected.
- No deployment evidence was detected.
- No raw SQL scripts were detected, although EF Core migrations are present.
- No dedicated administrator user-management screens were detected.
- Individual contribution evidence must be confirmed by the author.

## 13. Conclusion

The repository provides an ASP.NET Core MVC implementation of UniManage with clear separation between controllers, entities, view models, views and infrastructure services. The implemented features align with the core case study areas of authentication, course management, enrolment, assignment submission, grading, reporting and communication. Before final submission, the author should complete the evidence pack, confirm personal contribution details, add screenshots and remove or redact sensitive configuration values from any submitted material.

## References and Evidence Sources

- `AD COURSEWORK 2.sln`
- `AD COURSEWORK 2.csproj`
- `Program.cs`
- `Data/ApplicationDbContext.cs`
- `Data/DbInitializer.cs`
- `Controllers/`
- `Models/`
- `ViewModels/`
- `Views/`
- `Infrastructure/`
- `README.md`
- `Properties/launchSettings.json`

