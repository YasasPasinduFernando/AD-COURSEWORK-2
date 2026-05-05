# User Manual - UniManage Application Development Coursework

## 1. Introduction

This user manual explains how to install, run, and demonstrate UniManage, a University Course Management System developed for CS6004ES Application Development Coursework 2. The guide is based on the scanned ASP.NET Core MVC project and is prepared for an approved individual submission.

Following approval to complete the coursework individually, the author completed the implementation, testing, documentation, and final preparation as an individual submission.

## 2. System Requirements

- Visual Studio 2017 or higher. Visual Studio 2022 is recommended for .NET 8 projects.
- .NET 8 SDK and ASP.NET Core MVC runtime.
- SQL Server / LocalDB: [VERIFY]. The scanned project uses Entity Framework Core with MySQL through Pomelo.
- MySQL 8.x for the current scanned configuration.
- Modern browser such as Microsoft Edge, Chrome, or Firefox.
- Internet connection if NuGet packages, Google login, SMTP email, or external fonts/CDN resources are required.

## 3. Project Package Contents

The project package contains:

- `AD COURSEWORK 2.sln`
- `AD COURSEWORK 2.csproj`
- `Program.cs`
- `Controllers/`
- `Models/`
- `ViewModels/`
- `Views/`
- `Data/`
- `Infrastructure/`
- `wwwroot/`
- `appsettings.json`
- `appsettings.Development.json`
- `README.md`
- `documentation_application_development/`

## 4. Installation Guide

### 4.1 Extracting the Project

Extract the packaged project ZIP into a suitable local development folder. Keep the folder path short and avoid moving files out of the project structure.

Figure UM1: This screenshot shows the project opened in Visual Studio.

### 4.2 Opening the Solution in Visual Studio

Open `AD COURSEWORK 2.sln` in Visual Studio. Confirm that the web project is loaded and that `AD COURSEWORK 2.csproj` is visible in Solution Explorer.

### 4.3 Restoring NuGet Packages

Restore NuGet packages through Visual Studio or with:

```text
dotnet restore
```

The scanned project uses packages for ASP.NET Core Identity, EF Core, Pomelo MySQL, Google authentication, and QuestPDF.

### 4.4 Configuring the Database Connection

The scanned project configures the database connection through `appsettings.json` and `appsettings.Development.json`. Sensitive values must be redacted in screenshots and the final report.

The source code uses EF Core with MySQL. SQL Server or LocalDB setup is [VERIFY].

### 4.5 Applying Migrations or Running SQL Script

EF Core migrations are stored in `Data/Migrations/`. The application also calls `DbInitializer.SeedAsync` at startup, which applies migrations and seeds roles and demo data.

Useful command:

```text
dotnet ef database update
```

Raw SQL scripts were not detected. If the lecturer requires a SQL script, this remains [NEEDS CONFIRMATION].

### 4.6 Building the Project

Build from Visual Studio or command line:

```text
dotnet build --no-restore
```

The build was verified successfully with 0 warnings and 0 errors.

### 4.7 Running the Application

Run through Visual Studio or command line:

```text
dotnet run
```

Detected launch URLs:

- `https://localhost:7212`
- `http://localhost:5103`

## 5. Login and User Roles

Seeded accounts are created by `DbInitializer.cs`:

| Role | Email | Password |
|---|---|---|
| Administrator | `admin@unimanage.local` | `Admin123!` |
| Lecturer | `lecturer@unimanage.local` | `Lecturer123!` |
| Student | `student@unimanage.local` | `Student123!` |

Figure UM2: This screenshot shows the login page.

Figure UM3: This screenshot shows the registration page.

## 6. Student User Guide

After login, the student is redirected to the Student dashboard. The student can:

- View enrolled courses.
- Browse course catalogue.
- Enrol on eligible courses.
- View assignments.
- Submit assignment text or file.
- View grades and feedback.
- Send and receive messages with lecturers linked to enrolled courses.
- View and join course meetings.

Figure UM4: This screenshot shows the Student dashboard.

Figure UM8: This screenshot shows the enrollment screen.

Figure UM9: This screenshot shows the assignment submission screen.

## 7. Lecturer User Guide

After login, the lecturer is redirected to the Lecturer dashboard. The lecturer can:

- View taught courses.
- Open course details.
- Upload course materials.
- Create, edit, and delete assignments for own courses.
- View submissions.
- Record grades and feedback.
- Send and receive messages with enrolled students.
- Schedule meetings for own courses.
- View workload reporting.

Figure UM5: This screenshot shows the Lecturer dashboard.

Figure UM10: This screenshot shows the grading screen.

Figure UM12: This screenshot shows the messaging screen.

## 8. Administrator User Guide

After login, the administrator is redirected to the Administrator dashboard. The administrator can:

- View overall user, course, enrolment, assignment, submission, and message counts.
- Create, edit, delete, search, and page through courses.
- View reports and analytics.
- Review audit logs.
- View meeting records where authorised.

A dedicated user-management screen was not detected. This remains [NEEDS CONFIRMATION].

Figure UM6: This screenshot shows the Administrator dashboard.

Figure UM7: This screenshot shows the course management screen.

## 9. Validation and Error Messages

The application uses data annotations, `ModelState`, anti-forgery tokens, ownership checks, file validation, and role restrictions. Examples include invalid login, missing required fields, invalid course prerequisite, duplicate enrolment, full course capacity, invalid file upload, invalid meeting URL, and unauthorised access.

Figure UM13: This screenshot shows a validation error example.

## 10. Reporting and Analytics

The Reports area includes course popularity, student performance, lecturer workload, enrolments, pass/fail analysis, and assignment attendance. Reports can be exported as CSV or PDF where implemented.

Figure UM11: This screenshot shows the reports screen.

## 11. Troubleshooting

| Problem | Suggested fix |
|---|---|
| Project does not load | Confirm Visual Studio version and .NET 8 SDK installation |
| NuGet restore fails | Check internet access and package sources |
| Database connection fails | Check MySQL server, port, database name, and credentials |
| Migration fails | Confirm database permissions and EF Core tools |
| Login fails | Confirm seeded account details and database seeding |
| Access denied | Confirm the signed-in user's role |
| Upload fails | Check file extension, size, and content type |
| Email does not send | Confirm SMTP settings, or continue without email for local testing |

## 12. Notes for Viva Demonstration

For the viva, the author should be ready to explain:

- Why ASP.NET Core MVC was used.
- How controllers, models, views, and view models are separated.
- How Identity and roles work.
- How EF Core stores courses, enrolments, assignments, submissions, and messages.
- How validation and security are implemented.
- How reports are generated.
- Which features are confirmed by source code.
- Which evidence items remain [NEEDS CONFIRMATION].
