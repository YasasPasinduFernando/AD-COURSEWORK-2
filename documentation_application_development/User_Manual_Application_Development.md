# Installation Guide and User Manual

Project: UniManage - University Course Management System (UCMS)  
Module: CS6004ES Application Development  
Assessment: Coursework 2  
Student: Yasas Pasindu Fernando  
Student ID: 25026764  
Centre: ESOFT Metro Campus / London Metropolitan University  
Submission Type: Approved individual submission  
Companion Documents: `Main_Application_Development_Report.md`, `Expanded_Individual_Contribution.md`

---

## 1. Introduction

This document is the installation guide and user manual for UniManage, the University Course Management System developed for CS6004ES Application Development Coursework 2. The manual is intended for the marker, the external moderator, and any reader who needs to install, run, demonstrate, or operate the application. The instructions are based on the actual project files and have been verified locally during the development of the project.

The manual is organised into fourteen sections. Sections 1 to 5 explain the system requirements, the project package contents, the installation steps, and the first-time setup. Sections 6 to 9 explain the login process and the role-by-role user guides for Student, Lecturer, and Administrator. Section 10 lists the validation and error messages the user can expect to see. Section 11 explains the reporting and analytics screens. Section 12 contains security notes for users. Section 13 contains a troubleshooting table. Section 14 contains the viva demonstration script.

Following approval to complete the coursework individually, the author took responsibility for the final implementation, testing, documentation, and submission preparation. The manual is written in UK English, uses a practical step-by-step style, and refers to "the user" or to a specific role rather than to any other student.

## 2. System Requirements

The following items are required to install and run UniManage on a development workstation.

| Item | Requirement | Notes |
|---|---|---|
| Operating system | Windows 10 or later | The development environment is Windows; macOS and Linux can run the application using the .NET 8 SDK and Pomelo MySQL provider, but Visual Studio integration is best on Windows. |
| Development environment | Visual Studio 2022 (Community edition is sufficient) | Older Visual Studio versions are not supported because the project targets .NET 8. |
| .NET runtime | .NET 8 SDK | Required for `dotnet build`, `dotnet run`, and the Visual Studio project. |
| Database engine | MySQL Server 8.x | The project uses the Pomelo Entity Framework Core MySQL provider. |
| Database tool | Optional MySQL Workbench, HeidiSQL, or DBeaver | Used to inspect tables and capture database evidence. |
| Browser | Microsoft Edge, Google Chrome, or Mozilla Firefox (latest stable) | Used to access the running application. |
| Internet connection | Required for the first run | Used to restore NuGet packages, optional Google login, optional SMTP email, and the small number of CDN-hosted assets. |
| Disk space | About 1 GB free | Project source, NuGet cache, and database storage. |

If the marker uses SQL Server / LocalDB instead of MySQL, the connection string and the EF Core provider must be replaced. The current implementation uses MySQL through Pomelo and is therefore the supported configuration. SQL Server / LocalDB compatibility is [VERIFY].

## 3. Project Package Contents

The project package contains the following items.

| Item | Path | Purpose |
|---|---|---|
| Solution file | `AD COURSEWORK 2.sln` | Visual Studio solution file. |
| Project file | `AD COURSEWORK 2.csproj` | Web project file with package references. |
| Application entry point | `Program.cs` | Service registration, middleware pipeline, and seeding. |
| Controllers | `Controllers/` | Twelve controllers handling the request flow. |
| Domain models | `Models/` | Twelve model classes for academic entities. |
| View models | `ViewModels/` | Sixteen view models for forms and dashboards. |
| Razor views | `Views/` | Forty-six `.cshtml` views including shared layouts and partials. |
| Data access | `Data/` | `ApplicationDbContext`, factory, seeder, and EF Core migrations. |
| Infrastructure | `Infrastructure/` | Twelve infrastructure files: audit logger, email service, file store, security headers middleware, CSV writer, PDF report builder, calendar link helper, meeting link helper, and supporting interfaces. |
| Static assets | `wwwroot/` | Bootstrap, custom CSS, custom JavaScript, library folder, and images. |
| Configuration | `appsettings.json`, `appsettings.Development.json` | Connection string, email settings, optional Google credentials. |
| Launch settings | `Properties/launchSettings.json` | HTTP and HTTPS launch profiles for development. |
| Documentation | `documentation_application_development/` | Main report, individual contribution, this manual, evidence checklist, source and build links, diagrams, screenshots README, and final QA artefacts. |
| Project README | `README.md` | Top-level project description. |

## 4. Installation Guide

The installation guide assumes that the marker has Windows, Visual Studio 2022, the .NET 8 SDK, and a running MySQL 8 server. Each step has been kept short and practical so that the marker can complete the setup without unnecessary detours.

### 4.1 Extracting the Project

If the project has been delivered as a ZIP archive, extract the archive into a short local path such as `C:\src\unimanage` or `C:\workspace\AD COURSEWORK 2`. Avoid placing the project under a deep path with spaces and special characters because some EF Core tooling commands behave better with shorter paths.

If the project has been delivered as a Git repository, clone the repository into a similar local path:

```text
git clone https://github.com/YasasPasinduFernando/AD-COURSEWORK-2.git
```

After extraction or cloning, the local folder should contain `AD COURSEWORK 2.sln`, `AD COURSEWORK 2.csproj`, `Program.cs`, the source folders listed in Section 3, and the `documentation_application_development` folder.

Figure UM1: [INSERT SCREENSHOT HERE]

This screenshot shows the solution loaded in Visual Studio with the source folders visible in Solution Explorer.

### 4.2 Opening the Solution in Visual Studio

Double-click `AD COURSEWORK 2.sln` to open the solution in Visual Studio 2022. Confirm that the project loads without missing project errors. The status bar should report that the solution is ready and the Solution Explorer should display the `AD COURSEWORK 2` web project together with the `documentation_application_development` folder if it is included in the solution.

If Visual Studio prompts to install missing components, accept the prompts so that the .NET 8 SDK and the ASP.NET Core workload are present. If the prompt suggests SQL Server LocalDB, that component is not required because the project uses MySQL.

### 4.3 Restoring NuGet Packages

NuGet packages are restored automatically the first time Visual Studio opens the solution. To restore manually from the command line, open a PowerShell window in the project folder and run:

```text
dotnet restore
```

The main packages used by the project include:

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.Design`
- `Pomelo.EntityFrameworkCore.MySql`
- `Microsoft.AspNetCore.Authentication.Google`
- `QuestPDF`

If a restore failure occurs, confirm that NuGet.org is reachable and that the workstation has internet access. The exact pinned versions are recorded in `AD COURSEWORK 2.csproj`.

### 4.4 Checking appsettings.json

Open `appsettings.json` in the project root. Confirm the following sections.

- `ConnectionStrings:DefaultConnection` contains the MySQL server, port, database name, user, and password to use during development.
- `Email` contains the SMTP host, port, sender name, sender email, username, and password used by the password reset flow. If email is not required for the marker's environment, the application still runs because email is only sent during password reset.
- `Authentication:Google` contains the optional Google client identifier and client secret. If both values are present, Google login is enabled in `Program.cs`. If either value is empty, Google login is disabled and the standard Identity login is used.

Figure UM2: [INSERT SCREENSHOT HERE]

This screenshot shows the configuration file with the connection string visible. Personal credentials must be redacted before the screenshot is included in the report.

The marker should not paste sensitive credentials such as the database password, the SMTP password, or the Google client secret into the report or into shared environments. The recommended practice is to redact these values in screenshots and to keep the actual configuration on the local machine only.

### 4.5 Configuring the Database Connection

The default connection string targets a local MySQL server on a non-standard port. The format is:

```text
Server=localhost;Port=3307;Database=unimanage;User=root;Password=YOUR_PASSWORD;TreatTinyAsBoolean=true;CharSet=utf8mb4;
```

To adapt the connection string to the marker's machine, edit the `Server`, `Port`, `Database`, `User`, and `Password` parts as appropriate. If the local MySQL server uses the default port, change `3307` to `3306`. If a different database name is preferred, change `unimanage` to the chosen name; the application will create the schema during the first run.

The marker can also place the connection string into `appsettings.Development.json` so that the value is only used during development. Sensitive credentials must remain outside of source control and outside of the report.

### 4.6 Applying Migrations or Creating the Database

The project applies pending migrations automatically at startup through `Data/DbInitializer.cs`. As soon as the application runs, the schema is created and the demonstration roles, users, and sample data are seeded.

If the marker prefers to apply migrations manually before the first run, open a PowerShell window in the project folder and run:

```text
dotnet ef database update
```

The current migrations are:

- `20260408041036_InitialUniManage`
- `20260501122117_AddAuditLogAndSecurity`
- `20260501125135_AddMeetings`

To list the migrations from the command line, run:

```text
dotnet ef migrations list
```

The output is the evidence captured for Figure 17b in the main report. Raw SQL backup scripts and SQL Server / LocalDB scripts are not included in the project; the EF Core migrations are the supported approach.

### 4.7 Building the Project

To build the project from the command line, run the following from the project folder:

```text
dotnet build --no-restore
```

The expected output is:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

Figure UM3: [INSERT SCREENSHOT HERE]

This screenshot shows the console output from `dotnet build --no-restore` with zero warnings and zero errors.

To build inside Visual Studio, select Build, then Build Solution from the top menu. The Output window displays the same zero warning and zero error result.

### 4.8 Running the Application

To run the application from the command line, use:

```text
dotnet run
```

To run inside Visual Studio, press F5 or click the Start button on the main toolbar. The Visual Studio launch profile uses the HTTPS profile by default.

The detected launch URLs are:

- `https://localhost:7212`
- `http://localhost:5103`

The first run applies the migrations, seeds the demonstration roles and users, and starts listening on the launch URLs. Open a browser and navigate to the HTTPS URL. The home page is displayed. From there, the user can log in or register.

### 4.9 Troubleshooting Build Errors

| Error | Likely cause | Suggested fix |
|---|---|---|
| The .NET 8 SDK is missing | Visual Studio components are out of date | Open Visual Studio Installer, modify the workload, and add the .NET 8 SDK. |
| NuGet restore fails | No internet access or NuGet feed not reachable | Confirm internet access, run `dotnet nuget list source`, and add `https://api.nuget.org/v3/index.json` if missing. |
| `EF Core tools` not found | Global tool not installed | Run `dotnet tool install --global dotnet-ef`. |
| Database connection refused | MySQL not running or wrong port | Start the MySQL service and confirm the port matches the connection string. |
| Access denied for the database user | Wrong user or password | Recreate the user, grant rights to the `unimanage` database, and update the connection string. |
| Migration fails | Database user does not have CREATE privilege | Grant the user the necessary privileges or use a higher-privilege user for the first run. |
| `dotnet build` fails | Stale obj/bin folders or wrong .NET SDK | Run `dotnet clean` and `dotnet build --no-restore` again. |
| Port already in use | Another process is using 5103 or 7212 | Stop the conflicting process or change the port in `Properties/launchSettings.json`. |
| Static files not served | `wwwroot` missing or build skipped | Confirm that the `wwwroot` folder is present and that the build completed. |
| HTTPS certificate trust prompt | Development certificate not trusted | Run `dotnet dev-certs https --trust` once on the workstation. |

## 5. First-Time Setup

The first time the application runs, `DbInitializer` creates three roles (Administrator, Lecturer, Student) and three demonstration users. Sample courses, enrolments, and assignments may also be created where the seeder has been configured to do so. The marker can sign in straight away using the demonstration credentials listed in Section 6.

If the seeded users are not visible after the first run, restart the application or check the logs for an error during seeding. The application logs the seeding result through the standard logger.

Figure UM4: [INSERT SCREENSHOT HERE]

This screenshot shows the login page after the first run.

Figure UM5: [INSERT SCREENSHOT HERE]

This screenshot shows the registration page with the role selector visible.

## 6. Login and User Roles

The seeded demonstration accounts are created by `Data/DbInitializer.cs`. The credentials are intended for the viva and for marking, not for production use.

| Role | Email | Password |
|---|---|---|
| Administrator | `admin@unimanage.local` | `Admin123!` |
| Lecturer | `lecturer@unimanage.local` | `Lecturer123!` |
| Student | `student@unimanage.local` | `Student123!` |

To log in, open the application, navigate to the login page, enter one of the credentials above, and click Sign in. The system applies the password rules, the lockout policy (five failed attempts, fifteen minute lockout), and the rate limiter on the authentication endpoint (ten requests per minute per IP).

After a successful login, the user is redirected to the dashboard appropriate to their role. The Administrator goes to the Administrator dashboard, the Lecturer goes to the Lecturer dashboard, and the Student goes to the Student dashboard.

To register a new account, click the Register link on the login page. The user is asked for a full name, an email address, a phone number, a password, and a role (Student or Lecturer). The Administrator role is not available through self-registration. After a successful registration, the new user is signed in automatically and redirected to the role dashboard.

## 7. Student User Guide

After logging in, the Student is redirected to the Student dashboard. The dashboard shows enrolled courses, upcoming deadlines, recent grades, recent messages, the calendar, and meetings.

The Student can perform the following actions.

1. **View enrolled courses.** The dashboard lists the Student's current enrolments. Each course title links to a detailed view.
2. **Browse the course catalogue.** Use the navigation menu to open the course browse page. The page lists eligible courses with their code, name, credits, lecturer, prerequisite, and current enrolment count.
3. **Enrol on a course.** Click the Enrol button next to an open course. The system checks that the course is not full, that the Student has not already enrolled, and that any prerequisite has been satisfied. If the checks pass, the enrolment is saved and a confirmation message is displayed. If a check fails, an error message explains the reason.
4. **View assignments.** Open the Assignments link in the navigation menu. The page lists assignments for the Student's enrolled courses with the due date and status.
5. **Submit an assignment.** Open an assignment, enter the submission text, optionally attach a file, and click Submit. The system validates the file extension, the file size, and the Student's enrolment, then stores the submission.
6. **View grades and feedback.** Open the assignment after grading. The Student can read the grade and the lecturer's feedback. After grading, the submission is locked and cannot be edited.
7. **Send a message to a lecturer.** Open the Messages page, click Compose, choose a Lecturer who teaches a course on which the Student is enrolled, enter a subject and content, and click Send. The system enforces the allowed-recipient rule.
8. **View meetings.** Open the Meetings page. Meetings linked to the Student's enrolled courses are listed with the scheduled time and the join link.
9. **Update profile.** Open the Profile page to view and update the full name, email, and phone number.
10. **Change password.** Open the Change Password page from the Profile menu, enter the current and new passwords, and click Save.

Figure UM6: [INSERT SCREENSHOT HERE]

This screenshot shows the Student dashboard after a successful login.

Figure UM10: [INSERT SCREENSHOT HERE]

This screenshot shows the course browse page with the Enrol button visible.

Figure UM12: [INSERT SCREENSHOT HERE]

This screenshot shows the assignment submission form with text and file fields.

## 8. Lecturer User Guide

After logging in, the Lecturer is redirected to the Lecturer dashboard. The dashboard shows the Lecturer's teaching courses, enrolment counts, assignment counts, pending submissions, recently graded submissions, recent messages, recent meetings, and recent activity.

The Lecturer can perform the following actions.

1. **View taught courses.** Open the My Courses page. The page lists only the courses owned by the signed-in Lecturer.
2. **Open course details.** Click a course to open its details, including the description, credits, enrolment limit, lecturer, prerequisite, enrolled students, assignments, materials, and meetings.
3. **Upload course materials.** From the course details page, click Upload Material, enter a title, select a file, and click Save. The system validates the file extension and size and stores the metadata in the database. The actual file is stored on disk through the upload service.
4. **Create an assignment.** Open the Assignments page for an owned course, click Create, and complete the form. The form requires a title, description, due date, and maximum points. After submission, the assignment is saved and listed.
5. **Edit or delete an assignment.** From the assignments list, click Edit or Delete next to an assignment. The system checks that the assignment belongs to a course owned by the Lecturer.
6. **View submissions.** Click an assignment to view its submissions. Each submission shows the Student, the submitted time, the file name where applicable, and the status.
7. **Grade a submission.** Click Grade next to a submission, enter a grade and feedback, and click Save. The system validates the grade range and the feedback length. After grading, the submission is locked and the Student can see the grade and feedback.
8. **Send a message to a Student.** Open the Messages page, click Compose, choose a Student enrolled on a course owned by the Lecturer, and send the message.
9. **Schedule a meeting.** Open the Meetings page, click Create, select a course owned by the Lecturer, enter a title, scheduled time, duration, and meeting URL, then click Save. The system validates the URL format and the schedule.
10. **Edit or delete a meeting.** From the meeting list, click Edit or Delete next to a meeting. The Lecturer can only manage meetings for owned courses.
11. **Update profile and change password.** Use the Profile menu in the same way as the Student.

Figure UM7: [INSERT SCREENSHOT HERE]

This screenshot shows the Lecturer dashboard with teaching summary and pending submissions.

Figure UM11: [INSERT SCREENSHOT HERE]

This screenshot shows the assignment create form.

Figure UM13: [INSERT SCREENSHOT HERE]

This screenshot shows the grading form with grade and feedback fields.

Figure UM15: [INSERT SCREENSHOT HERE]

This screenshot shows the inbox or compose page.

Figure UM16: [INSERT SCREENSHOT HERE]

This screenshot shows the meeting create form.

## 9. Administrator User Guide

After logging in, the Administrator is redirected to the Administrator dashboard. The dashboard shows system-wide counts (users, courses, enrolments, assignments, submissions, messages), popular courses by enrolment count, and recent audit log activity.

The Administrator can perform the following actions.

1. **View the dashboard.** The dashboard provides a quick overview of the system.
2. **Manage courses.** Open the Courses page to list, search, page, view, create, edit, or delete courses. The course form requires the code (unique), name, description, credits, enrolment limit, lecturer, and optional prerequisite.
3. **View reports.** Open the Reports page to access the six reports: course popularity, student performance, lecturer workload, enrolments, pass and fail, and assignment attendance. Each report can be exported to CSV. Selected reports can be exported to PDF through QuestPDF.
4. **Review the audit log.** Open the Audit Logs page to inspect recent events. The log records the category, action, detail, user identifier (where available), success flag, and timestamp.
5. **Manage meetings (where authorised).** Administrators can review meetings as part of the academic oversight responsibility.
6. **Update profile and change password.** Use the Profile menu in the same way as the other roles.

A dedicated user-management screen is referenced in the brief but has not been confirmed in the source code. This is recorded as [VERIFY] in this manual and as [NEEDS CONFIRMATION] in the main report.

Figure UM8: [INSERT SCREENSHOT HERE]

This screenshot shows the Administrator dashboard with counts and popular courses.

Figure UM9: [INSERT SCREENSHOT HERE]

This screenshot shows the course list with search and paging visible.

## 10. Validation and Error Messages

The application uses data annotations on view models and additional checks in controller actions. The following validation behaviour is expected during normal operation.

| Scenario | Expected Validation Message |
|---|---|
| Required field missing | The field is required. |
| Invalid email format | The Email field is not a valid email address. |
| Weak password | The password must be at least 8 characters and include an uppercase letter, a lowercase letter, and a digit. |
| Confirm password does not match | The new password and confirmation password do not match. |
| Course code already exists | A course with this code already exists. |
| Enrolment full | This course has reached its enrolment limit. |
| Already enrolled | You are already enrolled on this course. |
| Prerequisite not satisfied | You must complete the prerequisite course before you can enrol. |
| Wrong file extension | The file type is not allowed. |
| File too large | The file is larger than the allowed size. |
| Disallowed message recipient | You are not allowed to message this user. |
| Invalid grade | The grade must be within the allowed range. |
| Anti-forgery token missing | The form has expired. Reload the page and try again. |
| Rate limit hit | Too many requests. Please wait and try again. |
| Access denied | You do not have permission to view this page. |

Figure UM17: [INSERT SCREENSHOT HERE]

This screenshot shows a form with one or more validation messages displayed.

The friendly error page `Views/Shared/Error.cshtml` is shown in production for any unhandled exception. The page does not reveal stack traces or internal server information. In development, the developer exception page provides detail to help locate the cause.

## 11. Reporting and Analytics

The Reports area is available to the Administrator. The following reports are implemented.

| Report | Description | Source-Code Evidence |
|---|---|---|
| Course Popularity | Lists courses ordered by enrolment count. | `ReportsController.CoursePopularity`, `Views/Reports/CoursePopularity.cshtml` |
| Student Performance | Aggregates grades per student. | `ReportsController.StudentPerformance`, `Views/Reports/StudentPerformance.cshtml` |
| Lecturer Workload | Aggregates assignments and pending submissions per lecturer. | `ReportsController.LecturerWorkload`, `Views/Reports/LecturerWorkload.cshtml` |
| Enrolments | Lists enrolments with optional filters. | `ReportsController.Enrollments`, `Views/Reports/Enrollments.cshtml` |
| Pass and Fail | Groups submissions into pass and fail buckets using a configurable threshold. | `ReportsController.PassFail`, `Views/Reports/PassFail.cshtml` |
| Assignment Attendance | Shows the proportion of enrolled students who submitted each assignment. | `ReportsController.AssignmentAttendance`, `Views/Reports/AssignmentAttendance.cshtml` |

To export a report, open the report and click the Export CSV or Export PDF button where present. The CSV file is generated through `Infrastructure/CsvWriter.cs` and the PDF file is generated through `Infrastructure/PdfReport.cs` using QuestPDF.

Figure UM14: [INSERT SCREENSHOT HERE]

This screenshot shows the reports landing page or a representative report output.

## 12. Security Notes for Users

The following security notes apply to all users of UniManage.

1. The application uses HTTPS by default during local development. If the browser shows a certificate warning, run `dotnet dev-certs https --trust` once to install the development certificate.
2. The application uses anti-forgery tokens on every form. Submitted forms must include the token; otherwise the request is rejected.
3. The application uses rate limiting on authentication and upload endpoints. Submitting too many requests in a short period of time produces a 429 Too Many Requests response.
4. The application uses account lockout. After five failed sign-in attempts, the account is locked for fifteen minutes.
5. Passwords are not stored in plain text. The Identity framework hashes passwords using PBKDF2 with a per-user salt.
6. Audit log entries are recorded for security-sensitive actions. The Administrator can review these entries through the Audit Logs page.
7. Sensitive configuration values (database password, SMTP password, Google client secret) must remain outside the report and outside any shared screenshots. The screenshots in this manual must be redacted.
8. Personal data shown in screenshots must also be redacted before inclusion in the final submission.

Figure UM18: [INSERT SCREENSHOT HERE]

This screenshot shows a database table such as `Courses`, `Enrollments`, or `Submissions` after a few actions, with personal data redacted.

## 13. Troubleshooting

| Problem | Suggested fix |
|---|---|
| The application does not start | Confirm that the .NET 8 SDK is installed and that the connection string is valid. Inspect the console output for an exception. |
| The login page rejects valid credentials | Confirm that the seeded users have been created. Check the application logs and re-run the application. |
| The login page shows a rate-limit error | Wait one minute and try again. The auth policy permits ten requests per minute per IP. |
| The account is locked | Wait fifteen minutes for the lockout window to expire, or reset the password. |
| A POST form is rejected | Reload the page to obtain a new anti-forgery token, then submit again. |
| File upload fails | Confirm that the file extension is allowed and that the file is under the maximum size. |
| The dashboard is empty | Confirm that the seeded data has been created. The Administrator dashboard counts use the seeded users and any data added during testing. |
| The friendly error page is shown | Inspect the application logs for the exception detail; the page does not reveal stack traces. |
| Email is not delivered | Check the `Email` section in `appsettings.json` and confirm that the SMTP server is reachable. |
| Google login is not available | Confirm that the `Authentication:Google:ClientId` and `Authentication:Google:ClientSecret` values are populated. If either is empty, Google login is disabled by design. |
| Migrations fail | Confirm database privileges. Run `dotnet ef migrations list` to see the current migrations and `dotnet ef database update` to apply them. |
| The reports page is empty | Confirm that data has been seeded or added through the application. Empty reports indicate no underlying data, not an error. |

## 14. Notes for Viva Demonstration

The viva demonstration follows the script in Appendix J of the main report. The notes below summarise the order in which the application is presented.

1. **Open the solution.** Open Visual Studio and load `AD COURSEWORK 2.sln`. Highlight the controller, model, view model, view, data, and infrastructure folders to demonstrate the MVC structure.
2. **Build verification.** Run `dotnet build --no-restore` from a terminal and show the zero warning, zero error result.
3. **Start the application.** Press F5 in Visual Studio or run `dotnet run`. Show the seeded URL `https://localhost:7212`.
4. **Administrator demonstration.** Sign in as `admin@unimanage.local` / `Admin123!`. Show the Administrator dashboard, the course list, the reports landing page, one report (for example course popularity) with CSV export, and the audit log review.
5. **Lecturer demonstration.** Sign out and sign in as `lecturer@unimanage.local` / `Lecturer123!`. Show the Lecturer dashboard, My Courses, an assignment, the grading screen, the materials upload, and a meeting create form.
6. **Student demonstration.** Sign out and sign in as `student@unimanage.local` / `Student123!`. Show the Student dashboard, the course browse page, an enrolment, an assignment submission, and an inbox conversation.
7. **Validation demonstration.** Trigger a validation error such as an empty required field or a duplicate course code, and show the friendly inline message.
8. **Security demonstration.** Show the audit log entry for a recent action. Optionally show the access denied page by attempting an unauthorised action as a Student.
9. **Code walkthrough.** Briefly walk through `Program.cs` to highlight Identity, EF Core retry on failure, the rate limiter (`auth` and `upload` policies), anti-forgery configuration, the security headers middleware, and the routing pattern.
10. **Honest reporting.** Briefly mention the items that remain [VERIFY] or [NEEDS CONFIRMATION], such as the dedicated user-management screen, the automated test project, the hosted deployment, and the formal accessibility audit. The aim is to keep the viva grounded in evidence.

End of installation guide and user manual.
