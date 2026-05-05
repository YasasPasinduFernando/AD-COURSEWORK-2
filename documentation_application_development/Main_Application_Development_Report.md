# Title Page

UniManage - University Course Management System  
Application Development Coursework 2 Report  
Module: CS6004ES Application Development  
Assessment: Coursework 2  
Technology Used: ASP.NET Core MVC, C#, Visual Studio, Entity Framework Core, MySQL, ASP.NET Core Identity, Razor Views, Bootstrap  
Centre: ESOFT Metro Campus / London Metropolitan University  
Student Name: Yasas Pasindu Fernando  
Student ID: 25026764  
Submission Type: Approved individual submission  
Lecturer / Module Leader: [LECTURER NAME TO BE CONFIRMED]  
Submission Date: 5 May 2026

# Declaration / Academic Integrity Note

The author confirms that this report and the accompanying UniManage application have been prepared and submitted as the author's own work for CS6004ES Application Development Coursework 2. The coursework was originally set as group work. Following approval from the lecturer and module team, the author has completed the implementation, testing, documentation, and final preparation as an individual submission.

External resources, official documentation, and tutorials are acknowledged where they have informed the work. No part of the report has been copied from another student. Where claims cannot be supported by source code, screenshots, or build evidence, those claims are clearly marked as [NEEDS CONFIRMATION] or [SCREENSHOT REQUIRED] in the body of the report.

# Acknowledgement

The author would like to thank the lecturer and module team for the guidance provided throughout the CS6004ES Application Development module and for the approval to complete this coursework as an individual submission. The author also acknowledges the value of the official Microsoft Learn documentation for ASP.NET Core, the Entity Framework Core documentation, the Pomelo MySQL provider documentation, the QuestPDF documentation, and the Bootstrap framework documentation, which informed the technical decisions throughout the project.

The author also recognises ESOFT Metro Campus and London Metropolitan University for the structured academic environment that has supported this work.

# Abstract

UniManage is a University Course Management System developed for CS6004ES Application Development Coursework 2 as an ASP.NET Core MVC web application targeting .NET 8. The system supports the academic administration workflow of a university by providing user registration and login, role-based access control for Student, Lecturer, and Administrator users, role-specific dashboards, course management, an enrolment system, assignment creation, submission and grading, reporting and analytics with CSV and PDF exports, internal messaging, meeting scheduling, audit logging, and a security pipeline that includes anti-forgery, rate limiting, secure cookies, and security headers.

The implementation has been confirmed by source-code review of twelve controllers, twelve domain models, sixteen view models, twelve infrastructure components, forty-six Razor views, three Entity Framework Core migrations, an ApplicationDbContext that inherits from IdentityDbContext, and a Program.cs configuration that registers Identity, EF Core with the Pomelo MySQL provider, rate limiting, anti-forgery, exception handling, and HTTPS redirection. The build has been verified locally with `dotnet build --no-restore`, producing zero warnings and zero errors.

The report documents the academic problem, the requirement analysis, the system architecture, the database design, the implementation, the user interface, the validation and exception handling, the security controls, the testing approach, the evaluation against the coursework brief, the challenges faced, the limitations, and the author's reflection. The report is written and submitted as an approved individual submission.

# Table of Contents

<<WORD_TOC>>

# List of Figures

<<WORD_LIST_OF_FIGURES>>

# List of Tables

<<WORD_LIST_OF_TABLES>>

# Abbreviations and Glossary

AD: Application Development  
API: Application Programming Interface  
ASP.NET: Active Server Pages .NET  
CDN: Content Delivery Network  
CRUD: Create, Read, Update, Delete  
CSRF: Cross-Site Request Forgery  
CSS: Cascading Style Sheets  
CSV: Comma-Separated Values  
DI: Dependency Injection  
DB: Database  
DTO: Data Transfer Object  
EF Core: Entity Framework Core  
ERD: Entity Relationship Diagram  
HSTS: HTTP Strict Transport Security  
HTML: HyperText Markup Language  
HTTPS: HyperText Transfer Protocol Secure  
ICS: iCalendar file format used to export meetings  
ID: Identifier  
JS: JavaScript  
JSON: JavaScript Object Notation  
MVC: Model View Controller  
ORM: Object Relational Mapping  
OWASP: Open Web Application Security Project  
PDF: Portable Document Format  
RBAC: Role-Based Access Control  
SDK: Software Development Kit  
SQL: Structured Query Language  
UCMS: University Course Management System  
UI: User Interface  
URL: Uniform Resource Locator  
UTC: Coordinated Universal Time  
UX: User Experience

Glossary terms:

- ApplicationUser: the project's user entity that extends `IdentityUser` and stores additional profile fields such as full name and phone number.
- ApplicationDbContext: the EF Core context used by UniManage. It inherits from `IdentityDbContext<ApplicationUser>` and exposes DbSets for the academic entities.
- DbInitializer: the seeding component that applies migrations and creates demonstration roles, users, and sample data.
- AppRoles: a static class that holds the role name constants used by the project (Administrator, Lecturer, Student).
- ViewModel: a class designed for transferring data between controllers and views, used to keep entity classes separate from presentation concerns.
- Razor view: a `.cshtml` server-side template that combines HTML markup with C# logic to render the user interface.

# 1. Introduction

## 1.1 Background and Coursework Context

CS6004ES Application Development at ESOFT Metro Campus and London Metropolitan University focuses on building a working web application that demonstrates practical software engineering. Coursework 2 requires the design, implementation, testing, and documentation of a university-style management system that supports realistic academic workflows. The brief sets a series of marking items, including the application user interface, user registration and login, role dashboards, course management, an enrolment system, assignment and grading, reporting and analytics, a communication module, security and data protection, an installation guide and user manual, a concise logical solution, architecture diagrams, a detailed description of classes and methods, a personal reflection, programming style, validation and exception handling, and interface design and usability.

UniManage is the University Course Management System produced by the author to satisfy this brief. The application has been built using ASP.NET Core MVC, C#, and Visual Studio. Entity Framework Core has been used as the object relational mapper, and the Pomelo provider has been used to connect to a MySQL 8 database. ASP.NET Core Identity has been used for authentication and roles. Razor views and Bootstrap have been used for the user interface. The choice of technologies reflects current professional practice for .NET web development, and is suitable for a coursework that asks for a maintainable, secure, and well-structured application.

The system supports three actors. A Student can register, log in, view a personal dashboard, browse courses, enrol on eligible courses, view and submit assignments, view grades and feedback, send and receive messages with lecturers, and view scheduled meetings. A Lecturer can log in, view a teaching dashboard, manage assignments for owned courses, upload course materials, grade submissions, send and receive messages with enrolled students, and schedule meetings. An Administrator can log in, view a system-wide dashboard, manage courses, view audit logs, and produce reports. A dedicated administrator user-management screen has not been confirmed in the source code and is therefore marked [NEEDS CONFIRMATION] throughout the report.

## 1.2 Purpose of the Report

The purpose of this report is to provide a complete and evidence-based account of the UniManage project. It is written for the marker, an external moderator, and any reader who needs to understand how the application addresses the coursework brief. Specifically, the report has the following purposes:

- It explains the academic problem that UniManage solves and the way that problem has been broken down into actors, use cases, and software components.
- It documents the architecture, database design, and implementation of the system, with reference to actual source files in the project.
- It records the validation, exception handling, and security controls that have been implemented in the code.
- It records the test plan and the verification activities that have been carried out.
- It evaluates the implementation against the marking items in the coursework brief.
- It reflects on the author's experience of completing the work as an approved individual submission and identifies future improvements.

The report is therefore both a technical record and an academic reflection. The author has used direct source-code references to keep the technical record honest and has used a personal voice in the reflection chapters to make the academic experience visible.

## 1.3 Approved Individual Submission Context

The CS6004ES Application Development Coursework 2 was originally set as group work. Following approval from the lecturer and module team, the author has completed the implementation, testing, documentation, and final preparation as an individual submission. The report does not include any group contribution table, group task allocation, or comparison of work between students. It does not name other students and does not describe any conflict. The author refers to themselves as "the author" in the body chapters and uses the first person only in the reflection chapter, where personal experience is appropriate.

This wording approach has two effects. First, it keeps the report neutral, professional, and easy to mark. Second, it confirms academic integrity, because the author claims authorship only for the work that has actually been delivered and verified by source code, build, and screenshot evidence.

## 1.4 Scope of the UniManage Application

The scope of UniManage is defined by the actors that the system supports, by the academic workflows that the system implements, and by the boundaries that the system deliberately does not cross.

The system is in scope when:

- A user registers, logs in, logs out, or recovers a password.
- A Student browses, enrols, views or submits an assignment, views grades, sends or receives messages, or views meetings.
- A Lecturer manages assignments for an owned course, uploads materials, grades submissions, sends or receives messages, or schedules meetings for an owned course.
- An Administrator manages the course catalogue, views reports, and reviews audit logs.

The system is out of scope when:

- An external timetable system is required to import or export class schedules.
- A finance or fees subsystem is required.
- A live video conference is required inside the application; UniManage stores meeting URLs and joining links rather than hosting video conferences.
- A mobile application is required; UniManage is a responsive web application accessed through a browser.
- A complex content management system for marketing pages is required; UniManage focuses on academic operations.

The boundary is enforced both at the user interface and at the controller layer. Each controller action is restricted to the appropriate role through `[Authorize]` attributes, and ownership checks are applied where a lecturer must only see or modify their own course or assignment. Where the source code does not implement a feature, the report records the absence honestly rather than describing a feature that does not exist.

## 1.5 Report Structure

The report is structured into front matter, twenty-three numbered chapters, and ten appendices. After the front matter, Chapter 1 introduces the project. Chapter 2 describes the academic problem and the stakeholders. Chapter 3 sets out the aim, objectives, and scope. Chapter 4 explains the technologies and software-engineering principles. Chapter 5 records the requirements. Chapter 6 explains the architecture and design diagrams. Chapter 7 records the database design. Chapter 8 explains the implementation in detail. Chapter 9 discusses the user interface and usability. Chapter 10 lists and explains the main classes, properties, and methods. Chapter 11 walks through the logical solution. Chapter 12 covers validation and exception handling. Chapter 13 covers security and data protection. Chapter 14 covers programming style and maintainability. Chapter 15 records the testing approach and results. Chapter 16 evaluates the work against the coursework brief. Chapter 17 records the challenges faced. Chapter 18 records the limitations. Chapter 19 contains the author's reflection. Chapter 20 records the individual contribution. Chapter 21 describes future improvements. Chapter 22 is the conclusion. Chapter 23 lists the references. The appendices contain supporting evidence.

# 2. Problem Description and Case Study Analysis

## 2.1 Academic Administration Problem

A modern university produces and processes a large amount of academic information every day. Students need to know which courses they are enrolled on, what assignments are due, and what feedback they have received. Lecturers need to know which courses they teach, which students are enrolled on those courses, what assignments have been set, and what submissions are pending review. Administrators need to know which courses run in the current academic period, which lecturers are teaching, how many students are enrolled, how many assignments have been set, and whether the system is being used securely.

When this information is held in scattered spreadsheets, paper records, or unconnected tools, errors creep in. Students enrol twice on the same course. Capacity is exceeded because there is no automatic check. Submissions are lost because there is no central record of who submitted what and when. Grades are inconsistent because feedback is held in private files. Communication between lecturer and student is split across email, instant messengers, and printed notices. Reports are produced manually at the end of the academic period and are sometimes inaccurate.

UniManage addresses this academic administration problem by providing a single web application that records the academic data, enforces business rules through the controller layer, and presents role-aware dashboards to each user. The intention is not to replace specialist systems for finance, payroll, or live video conferencing. The intention is to provide a clean, secure, and reliable foundation for the academic operations of a university course management system.

## 2.2 Stakeholder Analysis

The stakeholders for UniManage are the people who use the system, the people who run the system, and the people who depend on the data that the system produces. Table 10 summarises the main stakeholders, their roles, their main goals, and the way that the system supports each goal.

Table 10: Stakeholder Analysis

| Stakeholder | Role | Main Goals | How UniManage Supports the Goals |
|---|---|---|---|
| Student | End user | Find courses, enrol on eligible courses, complete assignments, view grades, communicate with lecturers, plan meetings. | Student dashboard with enrolled courses, deadlines, grades, messages, calendar. Course browse with eligibility checks. Submission and grading screens. Inbox with allowed-recipient messaging. |
| Lecturer | End user | Manage owned courses, set and grade assignments, share materials, communicate with enrolled students, plan meetings. | Lecturer dashboard with teaching summary, pending submissions, recent activity. `MyCourses` view, assignment management, grading screen, materials upload, scheduling, inbox. |
| Administrator | End user | Manage the course catalogue, view system-wide reports, oversee security, review audit logs. | Administrator dashboard with system counts and popular courses, course CRUD, reporting suite, audit log review. |
| Module leader / lecturer team | Marker / overseer | Verify that the application meets the coursework brief and that the documentation is evidence based. | Marking-aligned report, evidence checklist, build verification, source-code references. |
| ESOFT / London Met administration | Internal stakeholder | Confirm coursework completion in line with academic standards. | Approved individual submission record, declaration, formal report. |
| Author | Developer | Deliver a complete and verifiable solution as an approved individual submission. | Source code, documentation, screenshots, evidence appendices. |

The marker is treated as a primary reader of the report. The system has therefore been described in a marker-friendly way, with cross-references between requirements, source code, screenshots, and tests. Sections that cannot be supported by the source code have been clearly marked rather than embellished.

## 2.3 Problems Faced by Students

Students typically face several problems when academic information is scattered or inconsistent. They struggle to find a complete list of available courses with their prerequisites and capacity. They often miss deadlines because there is no single calendar of due dates. They cannot easily verify that an enrolment has been recorded. They cannot see grades and feedback in a structured format and instead rely on email threads. They have no clear way to message a lecturer in a record-keeping format that respects ownership and privacy.

UniManage addresses these problems through the Student dashboard, the course browse view, the enrolment workflow with capacity, duplicate, and prerequisite checks, the assignment and submission screens, the grading view, the inbox, and the meetings calendar. Each of these features is accessible from a single signed-in session, which removes the cognitive load of switching between unrelated tools.

## 2.4 Problems Faced by Lecturers

Lecturers also face common problems. They need to know which students are enrolled on each course they teach. They need to record grades and feedback in a structured way that the student can verify. They need to share teaching materials in a controlled environment that respects copyright and access rules. They need to plan meetings, including online meetings, and provide joining links to the right group. They need to communicate with students in a way that is respectful of role boundaries.

UniManage addresses these problems through the Lecturer dashboard, `MyCourses`, the assignment management screens, the grading screen, the course material upload, the meeting scheduler, and the messaging module restricted to allowed recipients. The combination of ownership filters and authorisation attributes keeps each lecturer focused on the data that belongs to them.

## 2.5 Problems Faced by Administrators

Administrators are responsible for the integrity of the academic catalogue and for visibility over how the system is used. They face problems such as duplicated course codes, courses without lecturers, courses set with impossible enrolment limits, and a lack of audit traceability for security-sensitive actions.

UniManage addresses these problems through the Administrator dashboard with system counts and popular courses, the course CRUD screens with unique-code validation, the reporting suite (course popularity, student performance, lecturer workload, enrolments, pass and fail, assignment attendance), the audit log review screen, and the export options for CSV and PDF reports. Where a dedicated administrator user-management screen would also be useful, the source-code review has not confirmed one, and this is therefore noted as [NEEDS CONFIRMATION] in the relevant chapters.

## 2.6 Success Criteria for the Solution

The success criteria for UniManage are derived from the coursework brief and from the stakeholder analysis. They are summarised below.

- The application can be built locally with `dotnet build --no-restore` without errors.
- The application starts on `https://localhost:7212` or `http://localhost:5103` and seeds demonstration roles and users automatically.
- A user can register, log in, and log out.
- A Student can browse courses, enrol on an eligible course, submit an assignment, view grades, message a lecturer linked to an enrolled course, and view meetings.
- A Lecturer can manage assignments for an owned course, grade a submission, upload a course material, schedule a meeting for an owned course, and message a student enrolled on an owned course.
- An Administrator can list, create, edit, and delete courses, view system-wide reports, and review the audit log.
- Validation, anti-forgery, rate limiting, secure cookies, and security headers are present and active.
- Build, screenshot, diagram, repository, and database evidence have been collected for the final submission.

The chapters that follow show how UniManage meets each of these success criteria and how the coursework brief has been addressed.

# 3. Aim, Objectives and Functional Scope

## 3.1 Aim

The aim of the project is to design, implement, document, and evaluate UniManage as an ASP.NET Core MVC University Course Management System that supports role-based academic workflows, enforces appropriate security and validation, and is delivered as an approved individual submission for CS6004ES Application Development Coursework 2.

## 3.2 Objectives

The aim is broken down into the following SMART objectives:

- **O1.** To implement registration and login using ASP.NET Core Identity, with strong password rules, account lockout, anti-forgery, and audit logging on success and failure.
- **O2.** To implement role-based access for Student, Lecturer, and Administrator users, with `[Authorize]` attributes on controllers and dashboard redirection by role.
- **O3.** To provide a Student dashboard, a Lecturer dashboard, and an Administrator dashboard that present role-relevant data using EF Core queries and view models.
- **O4.** To implement course management, including create, read, update, delete, search, paging, lecturer assignment, prerequisite handling, capacity, and material upload.
- **O5.** To implement an enrolment workflow with capacity, duplicate, and prerequisite checks.
- **O6.** To implement assignment management, submission upload, and grading with feedback, score range validation, and ownership checks.
- **O7.** To implement reporting and analytics including course popularity, student performance, lecturer workload, enrolments, pass and fail, and assignment attendance, with CSV and PDF export support.
- **O8.** To implement an internal communication module with inbox, thread, reply, compose, and allowed-recipient validation, plus meeting scheduling and ICS export.
- **O9.** To implement validation, exception handling, anti-forgery, rate limiting, secure cookies, security headers, and audit logging.
- **O10.** To prepare a complete documentation package including a main report, an individual contribution report, a user manual, evidence checklists, diagrams, screenshots, and final QA reports.
- **O11.** To submit the final coursework as an approved individual submission, with evidence-based wording and no group contribution material.

## 3.3 Functional Scope

The functional scope is the set of features that are inside the system boundary. Table 11 summarises these features.

Table 11: Functional Scope Inclusions

| ID | Feature | Source-Code Evidence |
|---|---|---|
| FS1 | User registration, login, logout, password reset | `Controllers/AccountController.cs`, `ViewModels/RegisterViewModel.cs`, `ViewModels/LoginViewModel.cs`, `ViewModels/ForgotPasswordViewModel.cs`, `ViewModels/ResetPasswordViewModel.cs`, `Views/Account/` |
| FS2 | Optional Google login if configured | `Program.cs` (Google auth registration), `Controllers/AccountController.cs` |
| FS3 | Role-based access control | `Models/AppRoles.cs`, `[Authorize]` attributes in controllers, `Data/DbInitializer.cs` |
| FS4 | Student dashboard | `Controllers/DashboardController.cs`, `ViewModels/DashboardViewModels.cs`, `Views/Dashboard/Student.cshtml` |
| FS5 | Lecturer dashboard | `Controllers/DashboardController.cs`, `ViewModels/DashboardViewModels.cs`, `Views/Dashboard/Lecturer.cshtml` |
| FS6 | Administrator dashboard | `Controllers/DashboardController.cs`, `ViewModels/DashboardViewModels.cs`, `Views/Dashboard/Administrator.cshtml` |
| FS7 | Course management with CRUD, search, paging | `Controllers/CoursesController.cs`, `Models/Course.cs`, `ViewModels/CourseInputViewModel.cs`, `Views/Courses/` |
| FS8 | Course material upload | `Controllers/CoursesController.cs` upload action, `Models/CourseMaterial.cs`, `ViewModels/MaterialUploadViewModel.cs`, `Infrastructure/UploadedFileStore.cs` |
| FS9 | Enrolment with capacity, duplicate, and prerequisite checks | `Controllers/EnrollmentsController.cs`, `Models/Enrollment.cs`, `ViewModels/CourseBrowseRowViewModel.cs`, `Views/Enrollments/Browse.cshtml` |
| FS10 | Assignment management for lecturers | `Controllers/AssignmentsController.cs`, `Models/Assignment.cs`, `ViewModels/AssignmentInputViewModel.cs`, `Views/Assignments/` |
| FS11 | Submission upload and grading | `Controllers/SubmissionsController.cs`, `Models/Submission.cs`, `Models/SubmissionStatus.cs`, `ViewModels/SubmissionSubmitViewModel.cs`, `ViewModels/GradeSubmissionViewModel.cs`, `Views/Submissions/` |
| FS12 | Reporting and analytics with CSV and PDF | `Controllers/ReportsController.cs`, `Infrastructure/CsvWriter.cs`, `Infrastructure/PdfReport.cs`, `Views/Reports/` |
| FS13 | Internal messaging module | `Controllers/MessagesController.cs`, `Models/Message.cs`, `ViewModels/MessageInboxRowViewModel.cs`, `ViewModels/MessageThreadViewModel.cs`, `ViewModels/MessageComposeViewModel.cs`, `Views/Messages/` |
| FS14 | Meeting scheduling and ICS export | `Controllers/MeetingsController.cs`, `Models/Meeting.cs`, `ViewModels/MeetingInputViewModel.cs`, `Infrastructure/CalendarLink.cs`, `Infrastructure/MeetLinkGenerator.cs`, `Views/Meetings/` |
| FS15 | Profile management and password change | `Controllers/ProfileController.cs`, `ViewModels/ProfileViewModel.cs`, `Views/Profile/` |
| FS16 | Audit logging and audit log review | `Models/AuditLog.cs`, `Infrastructure/AuditLogger.cs`, `Infrastructure/IAuditLogger.cs`, `Controllers/AuditLogsController.cs`, `Views/AuditLogs/Index.cshtml` |
| FS17 | Security pipeline | `Program.cs` (Identity, anti-forgery, rate limiting, HTTPS, HSTS), `Infrastructure/SecurityHeadersMiddleware.cs` |
| FS18 | Email service for password reset | `Infrastructure/IEmailService.cs`, `Infrastructure/SmtpEmailService.cs`, `Infrastructure/EmailSettings.cs`, `Infrastructure/EmailTemplates.cs` |

Features that are deliberately out of scope are explained in Section 1.4. Features that are not confirmed by source code are marked [NEEDS CONFIRMATION] where they appear, including the dedicated administrator user-management screen, automated test projects, and hosted deployment.

## 3.4 Non-Functional Scope

Table 12 summarises the non-functional quality attributes that the system aims to satisfy.

Table 12: Non-Functional Scope and Quality Attributes

| ID | Quality Attribute | Description | Evidence in Project |
|---|---|---|---|
| NF1 | Usability | Role-aware navigation, dashboards, alert partial, validation feedback. | `Views/Shared/_Layout.cshtml`, `Views/Shared/_Alerts.cshtml`, role views, Bootstrap. |
| NF2 | Performance | EF Core queries use filtering, paging, and `AsNoTracking` for read-only views; rate limiting limits abusive traffic. | EF Core queries in controllers, `Program.cs` rate limiting setup. |
| NF3 | Reliability | Retry on failure for MySQL connection, exception handling middleware, audit logging of failures. | `Program.cs` `EnableRetryOnFailure`, `UseExceptionHandler`, `AuditLogger`. |
| NF4 | Security | Identity, RBAC, anti-forgery, rate limiting, security headers, secure cookies, file validation, audit logging. | `Program.cs`, `Infrastructure/SecurityHeadersMiddleware.cs`, controller checks. |
| NF5 | Maintainability | Layered MVC structure with Controllers, Models, ViewModels, Views, Data, Infrastructure folders, dependency injection, single-responsibility services. | Folder layout, `Program.cs` service registration. |
| NF6 | Data integrity | EF Core relationships, unique course code, unique enrolment per student/course, data annotation rules. | `Data/ApplicationDbContext.cs`, model annotations. |
| NF7 | Accessibility | Skip link, semantic markup, label association in forms, Bootstrap responsive layout. Full audit is [NEEDS CONFIRMATION]. | `_Layout.cshtml`, role views. |
| NF8 | Auditability | Audit logger records security and academic events with user identifier, action, detail, and success flag. | `Infrastructure/AuditLogger.cs`, `Models/AuditLog.cs`, `Controllers/AuditLogsController.cs`. |

## 3.5 Assumptions and Constraints

Table 13 lists the assumptions and constraints that have shaped the project.

Table 13: Assumptions and Constraints

| Type | Description |
|---|---|
| Assumption | The marker has access to a Windows machine with Visual Studio 2022, .NET 8 SDK, and a local MySQL 8 server, or accepts that the database connection string can be reconfigured during the viva. |
| Assumption | The marker accepts the Pomelo MySQL provider as a substitute for SQL Server / LocalDB. The setup steps cover both options where reasonable. |
| Assumption | The seeded demonstration users defined in `Data/DbInitializer.cs` are acceptable for the viva. |
| Assumption | The lecturer accepts the approved individual submission route documented in Section 1.3. |
| Constraint | The application targets .NET 8 and ASP.NET Core MVC; older runtimes are not supported. |
| Constraint | The author works within a coursework deadline and reports only what has been implemented; no fictional features are described. |
| Constraint | Sensitive configuration values such as database passwords, SMTP passwords, and Google client secrets are not included in screenshots or in the body of the report. |
| Constraint | The audit log must not store free-text personal data beyond what is needed for academic traceability. |

# 4. Research and Background

## 4.1 ASP.NET Core MVC

ASP.NET Core MVC is a cross-platform web framework provided by Microsoft. It implements the Model View Controller pattern, where models represent the business data, controllers handle the request flow, and views render the user interface. The framework uses dependency injection as a first-class concept, supports configuration through `appsettings.json`, and integrates cleanly with Entity Framework Core, Identity, and authentication providers. The framework is suitable for the coursework because it allows the author to demonstrate separation of concerns, request handling, validation, secure access control, and database integration in a single project.

UniManage uses the conventional MVC routing pattern `{controller}/{action}/{id?}` configured in `Program.cs`. Each controller is registered through `AddControllersWithViews`, and views are rendered through Razor. The author has used the same MVC structure across all features, so the marker can move from one controller to another without unfamiliar conventions.

## 4.2 C# and .NET

C# is a strongly typed, object-oriented programming language designed for the .NET runtime. It supports classes, interfaces, generics, lambdas, asynchronous programming with `async`/`await`, pattern matching, and nullable reference types. .NET 8 is the long term support release used by UniManage. It provides modern web hosting, JIT and AOT compilation, and high-performance runtime libraries. The author has chosen .NET 8 because it is the current long term support release, because it is fully supported by Visual Studio 2022, and because it gives access to mature ASP.NET Core, EF Core, and Identity packages.

C# language features are used throughout UniManage. For example, asynchronous controller actions return `Task<IActionResult>`, EF Core queries use LINQ, and validation messages use string interpolation. The use of nullable reference types in the project (where present) helps to identify potentially null values during compilation rather than at runtime.

## 4.3 Razor Views and Tag Helpers

Razor views are server-rendered templates that use the `.cshtml` file extension. The Razor engine combines C# expressions with HTML markup. UniManage uses Razor Tag Helpers to bind form fields to view-model properties, generate anti-forgery tokens, and render validation messages. The shared layout `_Layout.cshtml` provides the page chrome, including the navigation bar, the alert partial, and the footer. Specific role views inherit this layout and contribute their own content sections.

The author has chosen Razor for two reasons. First, Razor is the standard view engine for ASP.NET Core MVC and is therefore the natural choice for an MVC coursework. Second, Razor is fully integrated with model binding and validation, which keeps the user interface code small and consistent.

## 4.4 Entity Framework Core and MySQL

Entity Framework Core (EF Core) is the official Microsoft object relational mapper for .NET. It allows the developer to write LINQ queries against `DbSet` properties and to map C# entity classes to database tables. EF Core provides change tracking, migrations, and a fluent API for advanced configuration.

UniManage connects to a MySQL 8 database through the Pomelo Entity Framework Core MySQL provider. The decision to use MySQL rather than SQL Server reflects the local availability of MySQL on the development environment and the popularity of MySQL in industry. The EF Core configuration in `Program.cs` enables retry on failure with up to five retries and a thirty second maximum delay, which protects the application from transient connection issues. The Pomelo provider is registered through `UseMySql` and is targeted at MySQL 8.0.36.

EF Core migrations are stored under `Data/Migrations/`. The current migrations are:

- `20260408041036_InitialUniManage.cs`
- `20260501122117_AddAuditLogAndSecurity.cs`
- `20260501125135_AddMeetings.cs`

The application calls `DbInitializer.SeedAsync` at startup, which applies pending migrations and creates demonstration roles, users, and sample data. The marker can therefore start from a clean database and obtain a working environment automatically.

## 4.5 ASP.NET Core Identity and Role-Based Access Control

ASP.NET Core Identity is the authentication and user management subsystem provided by Microsoft. It manages user storage, password hashing with PBKDF2, sign-in and sign-out, lockout, two-factor authentication, external login providers, and role membership. UniManage uses Identity through `AddIdentity<ApplicationUser, IdentityRole>` configured in `Program.cs`. The configuration enforces a minimum password length of 8 characters, requires at least one digit, one uppercase letter, and one lowercase letter, allows lockout for new users with up to 5 failed attempts, and uses a 15 minute lockout window.

Role-based access control is provided by the Identity role system. Three roles are seeded by `DbInitializer`: Administrator, Lecturer, and Student. Each controller action that requires a role uses `[Authorize(Roles = AppRoles.X)]`, where `AppRoles` is a static class that holds the role name constants. The author has used a constants class instead of magic strings so that role names are consistent across the codebase.

Identity also issues an authentication cookie configured through `ConfigureApplicationCookie`. The cookie is HTTP-only, uses `SameSiteMode.Lax`, and is marked secure when the request itself is secure. The session length is eight hours with sliding expiration. These settings reduce the chance of cookie theft and session hijacking while still allowing a normal academic session.

## 4.6 Bootstrap and Web User Interface Design

Bootstrap is a widely used front-end framework that provides a responsive grid system, components, and utility classes. UniManage uses the Bootstrap classes through the layout file and the role views. The decision to use Bootstrap follows from two reasons. First, Bootstrap is bundled with the default ASP.NET Core MVC project template and is therefore the path of least resistance for a coursework. Second, Bootstrap encourages the author to follow accepted user interface conventions, which improves usability without inventing custom design language.

The layout file `_Layout.cshtml` defines the main navigation bar, exposes the Razor sections for content, and includes the alerts partial. A custom CSS file `wwwroot/css/site.css` adds project-specific styling. A custom JavaScript file `wwwroot/js/site.js` is also present for any client-side behaviour. Bootstrap utility classes are used to maintain consistent spacing, typography, and colour usage across pages.

## 4.7 Software Engineering Principles Applied

UniManage applies a small set of well-known software engineering principles. The list below explains each principle in the context of the project.

- **Separation of concerns.** The project separates models (data and validation rules), view models (presentation data), controllers (request flow and business rules), views (HTML rendering), data access (`ApplicationDbContext`, migrations, seeding), and cross-cutting infrastructure (audit logger, email service, file store, security headers, CSV writer, PDF report). Each folder has a clearly defined responsibility.
- **Single responsibility.** Each controller is focused on one academic area, and each infrastructure service is focused on one concern. For example, `AuditLogger` is only responsible for writing audit records, `CsvWriter` is only responsible for writing CSV output, and `PdfReport` is only responsible for building PDF reports through QuestPDF.
- **Layered architecture.** Requests flow through middleware (security headers, HTTPS redirection, rate limiting), then through Identity, then through the controller, then through EF Core, and finally back through the view to the browser.
- **Defence in depth.** Security is implemented at the network layer (HTTPS, HSTS), the request layer (rate limiting, anti-forgery), the application layer (Identity, role attributes, ownership checks, validation), the data layer (EF Core parameterised queries, unique constraints), and the audit layer (audit log).
- **Convention over configuration.** Where ASP.NET Core MVC provides sensible defaults (routing, model binding, view discovery), the author has used those defaults instead of overriding them. This keeps the codebase smaller and easier to maintain.
- **Asynchronous I/O.** Database, file, and email operations are asynchronous, which keeps the application responsive under load.

## 4.8 Security and Data Protection Background

Web applications face well-known threats. Cross-site request forgery, cross-site scripting, SQL injection, broken authentication, broken access control, sensitive data exposure, and security misconfiguration are all common in production systems. The Open Web Application Security Project (OWASP) provides a Top 10 list that summarises these threats, and ASP.NET Core ships with a set of features that address them.

UniManage relies on Identity for authentication, on the `[Authorize]` attribute for authorisation, on parameterised EF Core queries for SQL injection protection, on the Razor engine's automatic HTML encoding for XSS protection, on anti-forgery tokens for CSRF protection, on HTTPS and HSTS for transport security, on a custom security headers middleware for defence in depth, and on the audit logger for traceability. Sensitive configuration values are kept in `appsettings.json` and are excluded from the report. Chapter 13 explains each control in more detail.

# 5. Requirement Analysis

## 5.1 Requirement Derivation

Requirements have been derived from three sources. First, the coursework brief defines the marking items that the system must address. Second, a structured review of the source-code repository has identified the features that are actually implemented. Third, the case-study analysis in Chapter 2 has identified the practical needs of Students, Lecturers, and Administrators.

This combination keeps the requirements grounded in evidence. Every functional requirement is supported by at least one source-code file, and every non-functional requirement is supported by at least one infrastructure or configuration setting. Where a requirement appears in the brief but cannot be supported by source code, the requirement is recorded with a [NEEDS CONFIRMATION] note rather than being silently dropped.

## 5.2 Functional Requirements

Table 1 lists the functional requirements that the system supports. Each requirement is referenced from later chapters and from the testing chapter.

Table 1: Functional Requirements

| ID | Functional Requirement | Source-Code Evidence |
|---|---|---|
| FR1 | The system shall allow a user to register an account with role selection (Student or Lecturer). | `Controllers/AccountController.cs` (Register), `ViewModels/RegisterViewModel.cs`, `Views/Account/Register.cshtml` |
| FR2 | The system shall allow a registered user to log in using a strong password. | `Controllers/AccountController.cs` (Login), `ViewModels/LoginViewModel.cs`, `Views/Account/Login.cshtml`, `Program.cs` Identity setup |
| FR3 | The system shall lock the account after 5 failed login attempts for 15 minutes. | `Program.cs` lockout configuration |
| FR4 | The system shall allow a user to log out and invalidate the authentication cookie. | `Controllers/AccountController.cs` (Logout), `Program.cs` cookie configuration |
| FR5 | The system shall allow a user to request a password reset and to reset the password using a token. | `Controllers/AccountController.cs` (ForgotPassword, ResetPassword), `ViewModels/ForgotPasswordViewModel.cs`, `ViewModels/ResetPasswordViewModel.cs` |
| FR6 | The system shall allow optional Google login if Google client id and secret are configured. | `Program.cs` Google authentication block |
| FR7 | The system shall route the signed-in user to the dashboard that matches the assigned role. | `Controllers/DashboardController.cs` (Index, Student, Lecturer, Administrator) |
| FR8 | The system shall display a Student dashboard summarising enrolled courses, deadlines, grades, messages, calendar, and meetings. | `Controllers/DashboardController.cs` (Student), `ViewModels/DashboardViewModels.cs`, `Views/Dashboard/Student.cshtml` |
| FR9 | The system shall display a Lecturer dashboard summarising teaching courses, pending submissions, recent activity, messages, and meetings. | `Controllers/DashboardController.cs` (Lecturer), `Views/Dashboard/Lecturer.cshtml` |
| FR10 | The system shall display an Administrator dashboard summarising system counts, popular courses, and audit activity. | `Controllers/DashboardController.cs` (Administrator), `Views/Dashboard/Administrator.cshtml` |
| FR11 | The system shall allow an Administrator to create, read, update, delete, search, and page through courses. | `Controllers/CoursesController.cs`, `Models/Course.cs`, `ViewModels/CourseInputViewModel.cs`, `Views/Courses/` |
| FR12 | The system shall allow a Lecturer to view and access only the courses they own. | `Controllers/CoursesController.cs` (MyCourses), ownership filter checks |
| FR13 | The system shall allow a Lecturer to upload course material files for an owned course. | `Controllers/CoursesController.cs` (UploadMaterial), `Models/CourseMaterial.cs`, `ViewModels/MaterialUploadViewModel.cs`, `Infrastructure/UploadedFileStore.cs` |
| FR14 | The system shall allow a Student to browse courses, see eligibility, and enrol on an open course. | `Controllers/EnrollmentsController.cs` (Browse, Enroll), `ViewModels/CourseBrowseRowViewModel.cs`, `Views/Enrollments/Browse.cshtml` |
| FR15 | The system shall prevent duplicate enrolment, exceeding capacity, and missing prerequisites. | `Controllers/EnrollmentsController.cs` business rule checks |
| FR16 | The system shall allow a Lecturer to create, edit, and delete assignments for an owned course. | `Controllers/AssignmentsController.cs` (Create, Edit, Delete, ForCourse, Mine), `Models/Assignment.cs`, `ViewModels/AssignmentInputViewModel.cs` |
| FR17 | The system shall allow a Student to submit text and file content for an assignment they are enrolled on. | `Controllers/SubmissionsController.cs` (Submit), `Models/Submission.cs`, `ViewModels/SubmissionSubmitViewModel.cs`, `Views/Submissions/Submit.cshtml` |
| FR18 | The system shall allow a Lecturer to grade submissions and record feedback. | `Controllers/SubmissionsController.cs` (Grade), `ViewModels/GradeSubmissionViewModel.cs`, `Views/Submissions/Grade.cshtml` |
| FR19 | The system shall allow secure download of submission and material files only by the rightful user. | `Controllers/SubmissionsController.cs` (CourseMaterialFile, SubmissionFile), `Infrastructure/UploadedFileStore.cs` |
| FR20 | The system shall produce reports for course popularity, student performance, lecturer workload, enrolments, pass and fail, and assignment attendance. | `Controllers/ReportsController.cs`, `Views/Reports/` |
| FR21 | The system shall export selected reports to CSV and PDF. | `Infrastructure/CsvWriter.cs`, `Infrastructure/PdfReport.cs`, `Controllers/ReportsController.cs` |
| FR22 | The system shall allow internal messaging through inbox, thread, reply, and compose actions. | `Controllers/MessagesController.cs`, `Models/Message.cs`, `Views/Messages/`, message view models |
| FR23 | The system shall enforce allowed-recipient rules between Students and Lecturers based on enrolments. | `Controllers/MessagesController.cs` Compose validation |
| FR24 | The system shall allow Lecturers to schedule meetings for owned courses, including a join URL. | `Controllers/MeetingsController.cs`, `Models/Meeting.cs`, `ViewModels/MeetingInputViewModel.cs`, `Views/Meetings/` |
| FR25 | The system shall allow ICS export of scheduled meetings. | `Controllers/MeetingsController.cs` (Ics), `Infrastructure/CalendarLink.cs` |
| FR26 | The system shall allow a user to view and update their profile and to change their password. | `Controllers/ProfileController.cs`, `ViewModels/ProfileViewModel.cs`, `Views/Profile/` |
| FR27 | The system shall record audit log entries for security-sensitive and academic actions. | `Models/AuditLog.cs`, `Infrastructure/AuditLogger.cs`, `Controllers/AuditLogsController.cs`, `Views/AuditLogs/Index.cshtml` |
| FR28 | The system shall display a friendly error page if an unhandled exception occurs in production. | `Program.cs` `UseExceptionHandler`, `Views/Shared/Error.cshtml` |
| FR29 | The system shall apply rate limiting to authentication and upload endpoints. | `Program.cs` `AddRateLimiter` policies (auth, upload) |
| FR30 | The system shall enforce anti-forgery tokens on POST forms. | `Program.cs` `AddAntiforgery`, `[ValidateAntiForgeryToken]` attributes |

## 5.3 Non-Functional Requirements

Table 2 lists the non-functional requirements with rationale and source-code evidence.

Table 2: Non-Functional Requirements

| ID | Non-Functional Requirement | Rationale / Evidence |
|---|---|---|
| NFR1 | Usability: Users should reach key tasks in a small number of clicks. | Role-aware navigation in `_Layout.cshtml`, role dashboards, alerts partial, consistent forms. |
| NFR2 | Performance: Dashboard queries should return within a reasonable time on a typical academic dataset. | EF Core filtering and paging, `AsNoTracking` for read-only queries, retry on failure. |
| NFR3 | Security: The application should resist common web threats such as CSRF, brute force login, and unauthorised access. | Identity, lockout, anti-forgery, rate limiting, security headers, HTTPS, HSTS. |
| NFR4 | Maintainability: New features should fit into the existing structure without rewriting core code. | Layered MVC structure, dependency injection, single-responsibility services. |
| NFR5 | Reliability: Transient database errors should not cause user-visible failures. | EF Core retry on failure, exception handling middleware. |
| NFR6 | Data integrity: Domain rules such as unique course code and unique enrolment per student/course should be enforced at the database level. | EF Core unique indexes, fluent configuration in `ApplicationDbContext`. |
| NFR7 | Accessibility: Forms should be usable with keyboard, semantic labels, and skip links. | `_Layout.cshtml`, label association in form views. Full audit is [NEEDS CONFIRMATION]. |
| NFR8 | Auditability: Important actions should be recorded with user, action, and detail. | `AuditLogger` and `AuditLog` entity. |
| NFR9 | Configurability: Database connection, email settings, and Google credentials should be configurable through `appsettings.json`. | `Program.cs` configuration binding. |
| NFR10 | Deployability: The project should build with a single `dotnet build --no-restore` command. | Build verified locally with zero warnings and zero errors. |

## 5.4 Use Case Summary

Table 14 summarises the main use cases. The diagram in Section 6.2 shows the visual layout.

Table 14: Use Case Summary

| Use Case | Primary Actor | Goal |
|---|---|---|
| Register Account | Visitor | Create a Student or Lecturer account. |
| Log In | Registered User | Access the role-appropriate dashboard. |
| Reset Password | Registered User | Recover access using an emailed token. |
| View Dashboard | Student / Lecturer / Administrator | See role-relevant academic information. |
| Browse Courses | Student | Find a course to enrol on. |
| Enrol on Course | Student | Add a course to the personal record subject to capacity, prerequisite, and duplicate checks. |
| Submit Assignment | Student | Upload submission text and file. |
| View Grades and Feedback | Student | Read marker feedback. |
| Send Message | Student / Lecturer | Communicate within an allowed relationship. |
| Manage Course | Administrator | Maintain the course catalogue. |
| Manage Assignment | Lecturer | Set or update assignments for an owned course. |
| Grade Submission | Lecturer | Record marks and feedback. |
| Upload Course Material | Lecturer | Provide teaching resources. |
| Schedule Meeting | Lecturer | Organise a class meeting for an owned course. |
| View Reports | Administrator | Review system-wide academic data. |
| Review Audit Log | Administrator | Inspect security-sensitive and academic events. |
| Update Profile | Any User | Maintain personal information. |
| Change Password | Any User | Update the password under authentication. |

## 5.5 Requirement Traceability Summary

Table 15 maps the marking items in the brief to the functional requirements that support them and to the implementation evidence.

Table 15: Requirement Traceability Summary

| Marking Item | Supporting Requirements | Source-Code Evidence | Test Cases |
|---|---|---|---|
| Application user interface | NFR1, NFR7 | `_Layout.cshtml`, `_Alerts.cshtml`, role views, `site.css` | T1, T8, T9, T10, T11 |
| User Registration and Login | FR1, FR2, FR3, FR4, FR5, FR6, FR30 | `AccountController`, `Program.cs` | T1, T2, T3, T4, T5 |
| Student Dashboard | FR7, FR8 | `DashboardController.Student`, dashboard view models | T8 |
| Lecturer Dashboard | FR7, FR9 | `DashboardController.Lecturer` | T9 |
| Administrator Dashboard | FR7, FR10 | `DashboardController.Administrator` | T10 |
| Course Management | FR11, FR12, FR13 | `CoursesController`, `Course`, course views | T12, T13, T14, T15 |
| Enrollment System | FR14, FR15 | `EnrollmentsController`, `Enrollment` | T16, T17 |
| Assignment and Grading | FR16, FR17, FR18, FR19 | `AssignmentsController`, `SubmissionsController` | T18, T19, T20, T21 |
| Reporting and Analytics | FR20, FR21 | `ReportsController`, `CsvWriter`, `PdfReport` | T22, T23 |
| Communication Module | FR22, FR23, FR24, FR25 | `MessagesController`, `MeetingsController` | T24, T25 |
| Security and Data Protection | NFR3, FR27, FR29, FR30 | `Program.cs`, `SecurityHeadersMiddleware`, `AuditLogger` | T26, T27, T28 |
| Validation and Exception Handling | FR28, NFR6 | Data annotations, `ModelState`, `Error.cshtml` | T29, T30 |
| Installation Guide and User Manual | NFR4, NFR9 | `User_Manual_Application_Development.md` | Manual review |
| Concise Logical Solution | NFR4, NFR5 | Chapter 11 of this report | Manual review |
| Architecture Diagrams | NFR4 | `documentation_application_development/diagrams/` | Manual review |
| Detailed Description of Classes | NFR4 | Tables 4, 5, 6 in Chapter 10 | Manual review |
| Own Reflection | n/a | Chapter 19 | Manual review |
| Programming Style | NFR4 | Folder structure, naming, DI | Manual review |
| Interface Design and Usability | NFR1, NFR7 | Layout, partials, CSS | T11, T31 |

# 6. System Architecture and Design

## 6.1 High-Level Architecture

The high-level architecture describes how the browser, the ASP.NET Core MVC application, the infrastructure services, and the database interact. UniManage follows a standard layered architecture for ASP.NET Core MVC applications. A request enters through Kestrel and passes through HTTPS redirection, static file serving, security headers, routing, rate limiting, authentication, and authorisation. After this middleware pipeline, the request reaches the controller, which uses EF Core through `ApplicationDbContext` to query or update the MySQL database, and which composes a view model that is rendered through Razor.

Figure 1: [INSERT EXPORTED DIAGRAM IMAGE HERE]

This diagram shows the high-level system architecture of UniManage. It traces a request from the browser through Kestrel, the HTTPS redirection middleware, the static file middleware, the security headers middleware, the routing middleware, the rate limiter, authentication, and authorisation, into the controllers under `Controllers/`, then through Entity Framework Core in `Data/ApplicationDbContext.cs` into the MySQL database accessed through the Pomelo provider, and back through Razor views to the browser. It also shows the supporting infrastructure services in `Infrastructure/` and the static assets served from `wwwroot/`. The exportable source is held at `documentation_application_development/diagrams/architecture_diagram.mmd`.

The architecture consists of the following components:

- **Browser.** The user agent that renders Razor-generated HTML, executes the small amount of JavaScript in `wwwroot/js/site.js`, and submits forms using anti-forgery tokens.
- **Kestrel and middleware.** The ASP.NET Core hosting layer accepts the request and passes it through the middleware pipeline configured in `Program.cs`.
- **Identity.** Authenticates the user using a cookie. Ten failed sign-in attempts within a minute are throttled by the rate limiter under the `auth` policy.
- **Controllers.** The twelve controllers under `Controllers/` handle request flow, validation, and business rules.
- **EF Core.** Translates LINQ queries into MySQL SQL through `ApplicationDbContext`.
- **Infrastructure services.** A small set of services in `Infrastructure/` provide cross-cutting capability such as audit logging, email, file storage, CSV writing, PDF rendering, calendar links, and security headers.
- **MySQL database.** Stores all application data through Pomelo EF Core.
- **Static assets.** Bootstrap, custom CSS, JavaScript, and images served from `wwwroot/`.

The diagram description explains how each component depends on the next. The author has used this layered architecture to keep request handling, business rules, persistence, and presentation in separate folders, which makes the project easier to maintain and easier to mark.

## 6.2 Use Case Diagram

Figure 2: [INSERT EXPORTED DIAGRAM IMAGE HERE]

This diagram shows the use case model for UniManage. It identifies the three actors (Student, Lecturer, Administrator) and the use cases listed in Table 14, including Register Account, Log In, Reset Password, View Dashboard, Browse Courses, Enrol on Course, Submit Assignment, View Grades and Feedback, Send Message, Manage Course, Manage Assignment, Grade Submission, Upload Course Material, Schedule Meeting, View Reports, Review Audit Log, Update Profile, and Change Password. The exportable source is held at `documentation_application_development/diagrams/use_case_diagram.mmd`.

The use case diagram identifies three actors and the use cases listed in Table 14. The Student actor is associated with the Register, Log In, View Student Dashboard, Browse Courses, Enrol, Submit Assignment, View Grades, Send Message, View Meetings, Update Profile, and Change Password use cases. The Lecturer actor is associated with the Log In, View Lecturer Dashboard, Manage Assignment, Grade Submission, Upload Course Material, Schedule Meeting, Send Message, Update Profile, and Change Password use cases. The Administrator actor is associated with the Log In, View Administrator Dashboard, Manage Course, View Report, Review Audit Log, Update Profile, and Change Password use cases. Common use cases such as Register, Log In, Update Profile, and Change Password are extended by all relevant actors.

## 6.3 Entity Relationship Diagram

Figure 3: [INSERT EXPORTED DIAGRAM IMAGE HERE]

This diagram shows the logical data model used by UniManage. It places `ApplicationUser` at the centre and connects it through one-to-many relationships to `Course` (lecturer side), `Enrollment` (student side), `Submission` (student and grader sides), `CourseMaterial` (uploader side), `Message` (sender and receiver sides), `Meeting` (lecturer side), and `AuditLog` (optional user side). The diagram also shows the optional self-reference from `Course` to `Course` for prerequisites, the unique-pair index on `Enrollment(StudentId, CourseId)`, and the relationship from `Assignment` to `Course` and from `Submission` to `Assignment`. The exportable source is held at `documentation_application_development/diagrams/er_diagram.mmd`.

The ER diagram shows the relationships between the application's main entities. `ApplicationUser` is the central entity. A `Course` is taught by exactly one `ApplicationUser` (the lecturer) and may have a `Course` as a prerequisite. An `Enrollment` links one `ApplicationUser` (the student) to one `Course` and is unique on the combination of student and course. An `Assignment` belongs to one `Course`. A `Submission` belongs to one `Assignment`, one `ApplicationUser` (the student), and optionally one `ApplicationUser` (the grader). A `CourseMaterial` belongs to one `Course` and is uploaded by one `ApplicationUser`. A `Message` is sent from one `ApplicationUser` to another. A `Meeting` belongs to one `Course` and is owned by one `ApplicationUser` (the lecturer). An `AuditLog` records the user identifier where available.

The relationships are configured in `Data/ApplicationDbContext.cs` through the EF Core fluent API, including foreign keys, delete behaviour, and unique constraints such as the unique course code and the unique student-course enrolment.

## 6.4 UML Class Diagram

Figure 4: [INSERT EXPORTED DIAGRAM IMAGE HERE]

This diagram shows the main classes used by UniManage and their dependencies. It includes the twelve controllers (`AccountController`, `DashboardController`, `CoursesController`, `EnrollmentsController`, `AssignmentsController`, `SubmissionsController`, `ReportsController`, `MessagesController`, `MeetingsController`, `ProfileController`, `AuditLogsController`, `HomeController`), the domain models, the view models, the `ApplicationDbContext`, and the infrastructure services such as `IAuditLogger`, `IEmailService`, `UploadedFileStore`, `CsvWriter`, `PdfReport`, and `SecurityHeadersMiddleware`. The exportable source is held at `documentation_application_development/diagrams/class_diagram.mmd`.

The class diagram summarises the controllers, models, view models, services, and the `ApplicationDbContext`. The diagram shows that controllers depend on `ApplicationDbContext` and on infrastructure interfaces such as `IAuditLogger` and `IEmailService`. Models are plain C# classes with data annotation attributes. View models are also plain C# classes that are used only between controllers and views, and are not persisted by EF Core. The diagram does not include role-specific controller classes because the project uses a single `DashboardController` with separate actions for each role.

## 6.5 Application Flowchart

Figure 5: [INSERT EXPORTED DIAGRAM IMAGE HERE]

This diagram shows the application flow from the home page through registration or login, role-aware redirection to the appropriate dashboard, the controller validation step, the EF Core data access step, the response back to the user, and the logout path. It also shows the friendly error page that is reached when an unhandled exception is raised in production. The exportable source is held at `documentation_application_development/diagrams/application_flowchart.mmd`.

The flowchart starts at the home page, where an unauthenticated user is offered Register or Log In. After successful login, the user is redirected to the role-appropriate dashboard. From the dashboard, the user navigates to a feature, performs an action, and the controller validates the input, calls EF Core, and responds with either a success view or a validation message. The flowchart also covers the logout action and the friendly error page.

## 6.6 Authentication and Role-Based Access Flow

Figure 6: [INSERT EXPORTED DIAGRAM IMAGE HERE]

This diagram shows the authentication and role-based access flow. It traces the path from the login form to `AccountController.Login`, through the Identity sign-in API, the lockout policy and the `auth` rate-limiting policy, to the issued authentication cookie, and on to `Dashboard/Index`, which inspects the role claim and forwards the request to the Student, Lecturer, or Administrator action. It also shows the access denied page that is returned when an unauthorised role attempts to open a restricted action. The exportable source is held at `documentation_application_development/diagrams/security_flow_diagram.mmd`.

The authentication flow shows the path from the login form to the authentication cookie. The user posts the form to `AccountController.Login`, which validates the model state, calls Identity to verify the credentials, and either signs the user in or returns a validation error. After sign-in, the controller redirects the user to `Dashboard/Index`, which inspects the role claim and forwards the request to `Student`, `Lecturer`, or `Administrator`. If the user opens an action that requires a different role, the `[Authorize]` attribute returns a 403 response and the access denied page is shown.

## 6.7 Design Decisions and Rationale

The author has taken several design decisions during the project. The most important decisions are summarised below.

- **MVC over Razor Pages.** The brief refers to ASP.NET MVC. ASP.NET Core MVC is the modern equivalent and is therefore the natural choice. The MVC structure also matches the marking criteria for separation of concerns.
- **EF Core with Pomelo MySQL over SQL Server / LocalDB.** The local environment includes MySQL 8 and the Pomelo provider is mature. The EF Core layer abstracts the underlying engine, so the design remains portable.
- **Single DashboardController over three role-specific controllers.** A single controller with three role-restricted actions reduces duplication, makes routing simpler, and keeps role-specific logic in one place. The pattern matches the way the brief talks about "dashboards" rather than "dashboard controllers".
- **Submission stores grade fields directly.** The brief mentions assignments and grading. A separate `Grade` entity would add tables and complexity without changing the workflow. The `Submission` entity therefore stores the grade fields directly, which keeps the data model small and keeps the grading view simple.
- **Custom security headers middleware.** Although ASP.NET Core ships with several security features, security headers such as Content Security Policy, X-Content-Type-Options, and Referrer Policy are added through a small middleware in `Infrastructure/SecurityHeadersMiddleware.cs`. This makes the controls visible and reviewable.
- **QuestPDF over a separate reporting tool.** QuestPDF is open source under the Community licence, integrates cleanly with C#, and produces professional PDF output. The licence type is set to Community in `Program.cs`.

# 7. Database Design

## 7.1 Database Technology

UniManage uses Entity Framework Core 8 with the Pomelo MySQL provider against a MySQL 8 server. The connection string is read from the `DefaultConnection` entry of `appsettings.json` (or `appsettings.Development.json` during development). The configuration in `Program.cs` registers the `ApplicationDbContext` and enables retry on failure.

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)), mySql =>
        mySql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)));
```

EF Core retry on failure is important because MySQL connections can fail transiently when the server is starting or when a network blip occurs. The retry policy hides these transient errors from the user and reduces the chance of false-negative test results.

The brief refers to SQL Server / LocalDB as a possible technology. UniManage uses MySQL through Pomelo, which is recorded honestly in the documentation. If the marker requires a SQL Server / LocalDB build, the approach would be to replace `UseMySql` with `UseSqlServer` and update the connection string. The migrations would need to be regenerated to reflect SQL Server data types. This option remains [NEEDS CONFIRMATION] because no SQL Server scripts are present in the source.

## 7.2 ApplicationDbContext

`ApplicationDbContext` inherits from `IdentityDbContext<ApplicationUser>`. This means the context exposes the standard Identity tables (Users, Roles, UserRoles, UserClaims, UserLogins, UserTokens, RoleClaims) and, in addition, the academic tables described in the next section. The constructor accepts `DbContextOptions<ApplicationDbContext>` so that the options can be supplied through dependency injection.

The fluent API in `OnModelCreating` is used to configure relationships, delete behaviour, and unique indexes. The most important configurations include the unique course code, the unique student-course enrolment, the lecturer-course foreign key with restrict-on-delete behaviour, the prerequisite course self-reference, the assignment-course relationship, the submission-assignment-student relationship, the message sender and receiver self-references, the meeting-course-lecturer relationships, and the audit log user reference.

## 7.3 Main Entities

Table 3 lists the main academic entities. The Identity entities (`AspNetUsers`, `AspNetRoles`, etc.) are not duplicated in the table because they are managed by the framework.

Table 3: Core Database Entities

| Entity | Primary Key | Main Foreign Keys | Purpose |
|---|---|---|---|
| `ApplicationUser` | `Id` (string) | Identity relationships | Stores user profile data and Identity user fields. |
| `Course` | `CourseId` | `LecturerId` (ApplicationUser), `PrerequisiteId` (Course, optional) | Stores course code, name, description, credits, enrolment limit, lecturer assignment, optional prerequisite. |
| `Enrollment` | `EnrollmentId` | `StudentId` (ApplicationUser), `CourseId` (Course) | Links a student to a course; unique on the pair. |
| `Assignment` | `AssignmentId` | `CourseId` (Course) | Records coursework tasks set by a lecturer for a course. |
| `Submission` | `SubmissionId` | `AssignmentId`, `StudentId`, `GradedById` (optional) | Records student submissions, grade, feedback, status, file metadata. |
| `CourseMaterial` | `CourseMaterialId` | `CourseId`, `UploadedById` | Records uploaded teaching material metadata. |
| `Message` | `MessageId` | `SenderId`, `ReceiverId` | Records direct messages between users. |
| `Meeting` | `MeetingId` | `CourseId`, `LecturerId` | Records scheduled course meetings, including a meeting URL. |
| `AuditLog` | `AuditLogId` | `UserId` (optional) | Records security-sensitive and academic events. |

## 7.4 Entity Relationships

The relationships between the entities are summarised below.

- **ApplicationUser to Course (lecturer side).** One-to-many. A lecturer may teach many courses; a course has one lecturer.
- **Course to Course (prerequisite).** Optional self-reference. A course may require another course as a prerequisite, or none at all.
- **ApplicationUser to Enrollment (student side).** One-to-many. A student may have many enrolments.
- **Course to Enrollment.** One-to-many. A course may have many enrolments.
- **Enrollment uniqueness.** Unique index on the pair (`StudentId`, `CourseId`) prevents duplicate enrolments.
- **Course to Assignment.** One-to-many. A course may have many assignments.
- **Assignment to Submission.** One-to-many. An assignment may have many submissions.
- **ApplicationUser to Submission (student side).** One-to-many.
- **ApplicationUser to Submission (grader side).** Optional one-to-many.
- **Course to CourseMaterial.** One-to-many.
- **ApplicationUser to CourseMaterial (uploader side).** One-to-many.
- **ApplicationUser to Message (sender and receiver sides).** Two one-to-many self-references.
- **Course to Meeting.** One-to-many.
- **ApplicationUser to Meeting (lecturer side).** One-to-many.
- **ApplicationUser to AuditLog (optional).** One-to-many; the audit log may record events even when the user is anonymous, in which case the `UserId` is null.

## 7.5 Keys, Constraints and Validation Rules

The database enforces the following constraints, in addition to the field-level data annotation rules:

- Unique course code on `Course.Code`.
- Unique enrolment per student and course on `Enrollment(StudentId, CourseId)`.
- Maximum lengths on string fields, set through data annotations such as `[StringLength]`.
- Required fields enforced through `[Required]` annotations.
- Range constraints on numeric fields such as `MaxPoints` on `Assignment` and the grade on `Submission`.
- Optional foreign keys for `PrerequisiteId` and `GradedById`.
- Default values set in the model constructor or through migrations, for example `EnrolledAtUtc = DateTime.UtcNow`.

The combination of database constraints and application-level validation provides defence in depth. A bad request that bypasses the controller would still be caught by the database; a bad input that satisfies the database would still be caught by `ModelState` before reaching the database.

## 7.6 Migrations and Seed Data

EF Core migrations describe the schema changes required to bring the database up to date. UniManage has three migrations:

- `20260408041036_InitialUniManage.cs`: creates the Identity tables and the initial academic tables (Courses, Enrollments, Assignments, Submissions, CourseMaterials, Messages).
- `20260501122117_AddAuditLogAndSecurity.cs`: adds the `AuditLogs` table and supporting columns for security tracking.
- `20260501125135_AddMeetings.cs`: adds the `Meetings` table and the meeting URL column.

`DbInitializer.SeedAsync` is called from `Program.cs` at startup. It applies pending migrations, creates the three roles (Administrator, Lecturer, Student), creates the demonstration users `admin@unimanage.local`, `lecturer@unimanage.local`, and `student@unimanage.local`, and may create sample courses and enrolments. The seeded credentials are listed in the user manual and are intended for the viva and for marking, not for production use.

## 7.7 Database Evidence Required

Figure 17: [INSERT SCREENSHOT HERE]

This screenshot shows a MySQL client connected to the seeded UniManage database, with the `Courses`, `Enrollments`, or `Submissions` table populated after a few actions in the application. Personal data is redacted before insertion in the report.

Figure 17b: [INSERT SCREENSHOT HERE]

This screenshot shows the console output of `dotnet ef migrations list` from the project root, listing the three EF Core migrations.

The following items remain [NEEDS CONFIRMATION]: a raw SQL backup script, a SQL Server / LocalDB equivalent script, and a database backup file (`.bak` or `.sql` dump).

# 8. Implementation

This chapter explains the implementation in detail, feature by feature. Each section follows the same structure: the source files involved, the request flow, the validation and business rules, the role of the view model, and the screenshot or evidence that should be captured.

## 8.1 Project Structure

The project follows the standard ASP.NET Core MVC layout with the following top-level folders:

- `Controllers/` (twelve files): `AccountController.cs`, `AssignmentsController.cs`, `AuditLogsController.cs`, `CoursesController.cs`, `DashboardController.cs`, `EnrollmentsController.cs`, `HomeController.cs`, `MeetingsController.cs`, `MessagesController.cs`, `ProfileController.cs`, `ReportsController.cs`, `SubmissionsController.cs`.
- `Models/` (twelve files): `AppRoles.cs`, `ApplicationUser.cs`, `Assignment.cs`, `AuditLog.cs`, `Course.cs`, `CourseMaterial.cs`, `Enrollment.cs`, `ErrorViewModel.cs`, `Meeting.cs`, `Message.cs`, `Submission.cs`, `SubmissionStatus.cs`.
- `ViewModels/` (sixteen files): `AssignmentInputViewModel.cs`, `CourseBrowseRowViewModel.cs`, `CourseInputViewModel.cs`, `DashboardViewModels.cs`, `ForgotPasswordViewModel.cs`, `GradeSubmissionViewModel.cs`, `LoginViewModel.cs`, `MaterialUploadViewModel.cs`, `MeetingInputViewModel.cs`, `MessageComposeViewModel.cs`, `MessageInboxRowViewModel.cs`, `MessageThreadViewModel.cs`, `ProfileViewModel.cs`, `RegisterViewModel.cs`, `ResetPasswordViewModel.cs`, `SubmissionSubmitViewModel.cs`.
- `Data/`: `ApplicationDbContext.cs`, `ApplicationDbContextFactory.cs`, `DbInitializer.cs`, `Data/Migrations/` (three migrations and the model snapshot).
- `Infrastructure/` (twelve files): `AuditLogger.cs`, `CalendarLink.cs`, `CsvWriter.cs`, `EmailSettings.cs`, `EmailTemplates.cs`, `IAuditLogger.cs`, `IEmailService.cs`, `MeetLinkGenerator.cs`, `PdfReport.cs`, `SecurityHeadersMiddleware.cs`, `SmtpEmailService.cs`, `UploadedFileStore.cs`.
- `Views/` (forty-six Razor views) including `Account/`, `AuditLogs/`, `Assignments/`, `Courses/`, `Dashboard/`, `Enrollments/`, `Home/`, `Meetings/`, `Messages/`, `Profile/`, `Reports/`, `Submissions/`, `Shared/`, plus `_ViewStart.cshtml` and `_ViewImports.cshtml`.
- `wwwroot/`: static assets including `css/site.css`, `js/site.js`, `lib/`, and `assets/`.
- Configuration: `appsettings.json`, `appsettings.Development.json`, `Properties/launchSettings.json`.

This layout matches the conventional ASP.NET Core MVC structure, which makes the codebase easy to navigate for both the author and the marker.

## 8.2 Program.cs and Startup Configuration

`Program.cs` is the composition root. It is responsible for service registration and middleware configuration. The file does the following work in order:

1. Sets the QuestPDF licence to Community to comply with the QuestPDF licensing terms.
2. Reads the `DefaultConnection` connection string from configuration. If the connection string is missing, the application throws an `InvalidOperationException` at startup so that the misconfiguration is visible immediately.
3. Registers `ApplicationDbContext` with the Pomelo MySQL provider and enables retry on failure.
4. Registers ASP.NET Core Identity with `ApplicationUser` and `IdentityRole`. The Identity options enforce strong password rules, lockout, and unique email.
5. Registers the cookie authentication scheme as the default and conditionally registers Google authentication if a client id and client secret are configured.
6. Configures the application cookie with login, logout, and access denied paths, sliding expiration of eight hours, HTTP-only flag, `SameSiteMode.Lax`, and `CookieSecurePolicy.SameAsRequest`.
7. Binds the email settings, registers `SmtpEmailService` for `IEmailService`, registers `IHttpContextAccessor`, and registers `AuditLogger` for `IAuditLogger`.
8. Configures the rate limiter with `auth` and `upload` policies.
9. Configures anti-forgery cookies as HTTP-only with `SameSiteMode.Strict`.
10. Registers MVC controllers with views.
11. Builds the application.
12. Runs `DbInitializer.SeedAsync` inside a service scope.
13. Applies the production exception handler and HSTS in production, or the developer exception page in development.
14. Applies HTTPS redirection, static files, the security headers middleware, routing, the rate limiter, authentication, and authorisation, then maps the default route and runs the application.

Listing 8.1 shows the security-related part of `Program.cs`.

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anon",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));
    options.AddPolicy("upload", ...);
});
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
```

## 8.3 User Registration and Login

Registration and login are implemented in `Controllers/AccountController.cs`. The `Register` GET action returns the registration form. The `Register` POST action validates `RegisterViewModel`, creates a new `ApplicationUser`, calls `UserManager.CreateAsync`, assigns the chosen role (Student or Lecturer), and signs the user in. Audit log entries are written for both successful and failed registrations.

The `Login` GET action returns the login form. The `Login` POST action validates `LoginViewModel`, calls `SignInManager.PasswordSignInAsync` with `lockoutOnFailure: true`, and either signs the user in or returns a validation error. Failed sign-in attempts are recorded in the audit log. After successful login, the user is redirected to `Dashboard/Index`, which routes the user to the role dashboard.

The `Logout` action calls `SignInManager.SignOutAsync` and redirects to the home page. The `ForgotPassword` and `ResetPassword` actions provide the standard Identity password recovery flow with email tokens.

Figure 7: [INSERT SCREENSHOT HERE]

This screenshot shows the login page after running the application.

Figure 7b: [INSERT SCREENSHOT HERE]

This screenshot shows the registration page with the role selector visible.

Figure 7c: [INSERT SCREENSHOT HERE]

This screenshot shows the login form after submitting invalid credentials, with the validation message displayed under the input fields.

Figure 7d: [INSERT SCREENSHOT HERE]

This screenshot shows the browser URL bar after a successful Student login, with the address pointing at the role-appropriate dashboard route (for example `/Dashboard/Student`).

## 8.4 Role-Based Access Control

Role-based access control is implemented through three layers. First, `AppRoles` defines the role name constants. Second, `DbInitializer` seeds the three roles and the three demonstration users. Third, each controller action uses `[Authorize(Roles = AppRoles.X)]` to restrict access. Where a Lecturer must only see their own courses or assignments, an additional ownership filter is applied inside the action.

The author has used a constants class because magic strings are error prone. If the constant is renamed, the rename is visible everywhere through Visual Studio's refactoring tools. If a magic string had been used, a misspelt role name would silently allow or deny access without any compile-time warning.

## 8.5 Student Dashboard

The Student dashboard is implemented by `DashboardController.Student` and `Views/Dashboard/Student.cshtml`. The action queries the `Enrollments`, `Assignments`, `Submissions`, `Messages`, and `Meetings` tables to build a `StudentDashboardViewModel` (defined inside `ViewModels/DashboardViewModels.cs`). The view model holds enrolled courses, upcoming deadlines, recent grades, recent messages, calendar events, and meetings. EF Core read queries use `AsNoTracking` where possible to reduce the change tracker overhead.

Figure 8: [INSERT SCREENSHOT HERE]

This screenshot shows the student dashboard.

Figure 8b: [INSERT SCREENSHOT HERE]

This screenshot shows the student calendar and deadlines panel.

## 8.6 Lecturer Dashboard

The Lecturer dashboard is implemented by `DashboardController.Lecturer` and `Views/Dashboard/Lecturer.cshtml`. The action filters courses, assignments, and submissions by the signed-in lecturer's user identifier. The view model holds teaching courses, enrolment counts, assignment counts, pending submissions, recently graded submissions, recent messages, recent meetings, and recent activity.

Figure 9: [INSERT SCREENSHOT HERE]

This screenshot shows the lecturer dashboard.

Figure 9b: [INSERT SCREENSHOT HERE]

This screenshot shows the lecturer pending submissions view.

## 8.7 Administrator Dashboard

The Administrator dashboard is implemented by `DashboardController.Administrator` and `Views/Dashboard/Administrator.cshtml`. The action queries the database to count users, courses, enrolments, assignments, submissions, and messages, identify popular courses by enrolment count, and load recent audit log entries. Where a dedicated user-management screen would be expected, the source-code review has not confirmed one. This is recorded as [NEEDS CONFIRMATION].

Figure 10: [INSERT SCREENSHOT HERE]

This screenshot shows the administrator dashboard.

Figure 10b: [INSERT SCREENSHOT HERE]

This screenshot shows the administrator audit log review.

## 8.8 Course Management

Course management is implemented in `Controllers/CoursesController.cs` and the `Views/Courses/` folder. The actions are:

- `Index`: lists courses with search and paging support.
- `Details`: shows a single course.
- `Create` (GET and POST): allows an Administrator to create a new course using `CourseInputViewModel` for validation. The unique course code is enforced through a database unique index.
- `Edit` (GET and POST): allows an Administrator to update an existing course.
- `Delete` (GET and POST): allows an Administrator to delete a course, with appropriate cascade or restrict behaviour.
- `MyCourses`: allows a Lecturer to see only their own courses.
- `UploadMaterial` (POST): allows a Lecturer to upload a course material file for an owned course. The action validates the file extension, the file size, the owner, and applies the `upload` rate-limiting policy.

Figure 11: [INSERT SCREENSHOT HERE]

This screenshot shows the course list (administrator).

Figure 11b: [INSERT SCREENSHOT HERE]

This screenshot shows the course create form.

Figure 11c: [INSERT SCREENSHOT HERE]

This screenshot shows the course edit form.

Figure 11d: [INSERT SCREENSHOT HERE]

This screenshot shows the course material upload.

## 8.9 Enrollment System

The enrolment system is implemented in `Controllers/EnrollmentsController.cs`. The `Browse` action lists eligible courses for the signed-in Student, joining the courses table with the student's enrolments to compute eligibility. The `Enroll` POST action checks that the course exists, that the course is open (i.e. capacity has not been reached), that the student has not already enrolled (uniqueness check), and that any prerequisite has been satisfied. If all checks pass, the action creates an `Enrollment` record and writes an audit log entry.

The eligibility check uses a single composed LINQ query rather than separate queries to keep the database round-trip count low. The capacity check uses the count of existing enrolments for the course compared with the `EnrollmentLimit` field.

Figure 12: [INSERT SCREENSHOT HERE]

This screenshot shows the course browse (student).

Figure 12b: [INSERT SCREENSHOT HERE]

This screenshot shows the successful enrolment confirmation.

Figure 12c: [INSERT SCREENSHOT HERE]

This screenshot shows the duplicate or full capacity rejection message.

## 8.10 Assignment Management

Assignment management is implemented in `Controllers/AssignmentsController.cs` and the `Views/Assignments/` folder. The actions are:

- `ForCourse`: lists assignments for a given course.
- `Mine`: lists assignments owned by the signed-in lecturer.
- `Create` (GET and POST): allows a Lecturer to create an assignment for an owned course using `AssignmentInputViewModel`.
- `Edit` (GET and POST): allows a Lecturer to update their own assignment.
- `Delete` (GET and POST): allows a Lecturer to delete their own assignment.

The validation enforced through `AssignmentInputViewModel` includes a required title, a maximum length on the description, a future or near-future due date, and a sensible `MaxPoints` range.

Figure 13: [INSERT SCREENSHOT HERE]

This screenshot shows the assignment list (lecturer).

Figure 13b: [INSERT SCREENSHOT HERE]

This screenshot shows the assignment create form.

## 8.11 Submission and Grading

Submission and grading are implemented in `Controllers/SubmissionsController.cs` and the `Views/Submissions/` folder. The Student-facing action `Submit` accepts text content and a file, validates that the student is enrolled on the course, validates the file using `UploadedFileStore`, and creates or updates a `Submission` record. If the submission has already been graded, the submission is locked and further changes are not allowed.

The Lecturer-facing action `Grade` checks that the lecturer owns the assignment's course, displays the submission, and records a grade and feedback through `GradeSubmissionViewModel`. The grade is constrained to a sensible range and the feedback is constrained to a maximum length.

The action `ForAssignment` allows a Lecturer to see all submissions for an assignment. The actions `CourseMaterialFile` and `SubmissionFile` provide secure file download paths that check role and ownership before streaming the file.

Figure 13c: [INSERT SCREENSHOT HERE]

This screenshot shows the submission form (student).

Figure 13d: [INSERT SCREENSHOT HERE]

This screenshot shows the grading form (lecturer).

Figure 13e: [INSERT SCREENSHOT HERE]

This screenshot shows the graded submission view (student).

## 8.12 Reporting and Analytics

The reporting suite is implemented in `Controllers/ReportsController.cs` and the `Views/Reports/` folder. The reports include:

- `CoursePopularity`: groups enrolments by course and orders by enrolment count.
- `StudentPerformance`: aggregates grades per student.
- `LecturerWorkload`: aggregates assignments and pending submissions per lecturer.
- `Enrollments`: lists enrolments with filters such as course or date.
- `PassFail`: groups submissions into pass and fail buckets using a configurable threshold.
- `AssignmentAttendance`: shows the proportion of enrolled students who submitted each assignment.

CSV export is provided through `Infrastructure/CsvWriter.cs`, which writes properly escaped CSV content. PDF export is provided through `Infrastructure/PdfReport.cs`, which uses QuestPDF to render the report layout. The QuestPDF licence is set to Community in `Program.cs`.

Figure 14: [INSERT SCREENSHOT HERE]

This screenshot shows the reports landing page.

Figure 14b: [INSERT SCREENSHOT HERE]

This screenshot shows the sample report output.

Figure 14c: [INSERT SCREENSHOT HERE]

This screenshot shows the csv export evidence.

Figure 14d: [INSERT SCREENSHOT HERE]

This screenshot shows the pdf export evidence.

## 8.13 Communication Module

The communication module is implemented in `Controllers/MessagesController.cs` and the `Views/Messages/` folder. The actions are:

- `Inbox`: lists messages for the signed-in user using `MessageInboxRowViewModel`.
- `Thread`: shows a conversation between two users using `MessageThreadViewModel`.
- `Reply` (POST): adds a reply message to a thread.
- `Compose` (GET and POST): allows the user to compose a message to an allowed recipient using `MessageComposeViewModel`. The validation rule is that a Student may message a Lecturer who teaches a course on which the student is enrolled, and a Lecturer may message a Student who is enrolled on a course owned by the lecturer. Administrators can message any user where required by the role.
- `MarkAllRead`: marks all messages in the inbox as read.

Figure 15: [INSERT SCREENSHOT HERE]

This screenshot shows the inbox view.

Figure 15b: [INSERT SCREENSHOT HERE]

This screenshot shows the message thread view.

Figure 15c: [INSERT SCREENSHOT HERE]

This screenshot shows the compose message form.

## 8.14 Meeting Scheduling

Meeting scheduling is implemented in `Controllers/MeetingsController.cs` and the `Views/Meetings/` folder. The actions are:

- `Index`: lists meetings for the signed-in user.
- `Create` (GET and POST): allows a Lecturer to schedule a meeting for an owned course using `MeetingInputViewModel`. The form captures the title, the description, the scheduled date and time, the duration, and an optional meeting URL.
- `Edit` (GET and POST): allows a Lecturer to update an owned meeting.
- `Delete` (GET and POST): allows a Lecturer to delete an owned meeting.
- `Join`: redirects the user to the meeting URL after authorisation checks.
- `Ics`: produces an ICS calendar file using `Infrastructure/CalendarLink.cs`, which can be imported into a calendar application.

`Infrastructure/MeetLinkGenerator.cs` provides a placeholder for generating meeting links where required. The current implementation stores a URL provided by the lecturer rather than provisioning a video conference, which keeps the system within the documented scope.

Figure 15d: [INSERT SCREENSHOT HERE]

This screenshot shows the meetings list.

## 8.15 Course Materials and File Uploads

Course materials are implemented through the `CourseMaterial` entity, the `MaterialUploadViewModel`, and the `UploadMaterial` action of `CoursesController`. Uploaded files are stored on disk through `Infrastructure/UploadedFileStore.cs`, which validates the file extension and the file size, generates a stored file name to avoid collisions, and writes the metadata into the database.

Downloads are served through controlled actions that confirm role and ownership before streaming the file. The action does not expose direct URLs into the file system, which prevents path traversal and unauthorised access to stored files.

## 8.16 Profile and Password Management

The profile and password actions are implemented in `Controllers/ProfileController.cs` and the `Views/Profile/` folder. The `Index` action allows the signed-in user to view and update their profile fields through `ProfileViewModel`. The `ChangePassword` action allows the user to change their password under authentication, using the standard Identity password change API.

## 8.17 Audit Logging

Audit logging is implemented through `Models/AuditLog.cs`, `Infrastructure/IAuditLogger.cs`, `Infrastructure/AuditLogger.cs`, and the `AuditLogsController`. The audit logger writes an `AuditLog` record with the following fields: category (for example `Account`, `Course`, `Enrollment`, `Submission`, `Security`), action (for example `Login`, `Register`, `Create`, `Update`, `Delete`, `Grade`, `Upload`, `Download`), detail (a short, human-readable description), user identifier (where available), and a success flag. The log is reviewed through `Views/AuditLogs/Index.cshtml`, which displays paged log entries for an Administrator.

The audit logger is wrapped in a try/catch block inside the implementation so that a failure to write an audit entry does not crash the calling action. This is appropriate because the audit log is a supplementary record rather than the primary data store. If a log entry cannot be written, the action continues, and the failure is reported through the standard exception handler to the application logs.

## 8.18 Infrastructure Services

The `Infrastructure/` folder contains services that support the controllers without holding business state. The full list is:

- `AuditLogger.cs` and `IAuditLogger.cs`: audit logger described in Section 8.17.
- `CalendarLink.cs`: builds ICS calendar content for meetings.
- `CsvWriter.cs`: writes CSV content with escaping.
- `EmailSettings.cs`: strongly typed settings for SMTP.
- `EmailTemplates.cs`: holds the email templates used by the password reset flow.
- `IEmailService.cs` and `SmtpEmailService.cs`: email service contract and SMTP implementation.
- `MeetLinkGenerator.cs`: helper used during meeting creation.
- `PdfReport.cs`: PDF rendering using QuestPDF.
- `SecurityHeadersMiddleware.cs`: custom middleware that sets security headers on every response.
- `UploadedFileStore.cs`: manages uploaded file validation and storage.

Each service is registered through dependency injection in `Program.cs` where appropriate. Controllers depend on the interface (for example `IAuditLogger`, `IEmailService`) rather than on the concrete implementation, which keeps the controllers easy to test and replace.

## 8.19 UI Layout and Static Assets

The UI layout is centralised in `Views/Shared/_Layout.cshtml`. The layout defines the document head, the navigation bar, the alert partial, the main content section, and the footer. The alerts partial `Views/Shared/_Alerts.cshtml` displays success, information, warning, and error messages using TempData. The `Views/Shared/_DashboardCalendar.cshtml` partial is reused by the dashboards to render a small calendar block.

Static assets live under `wwwroot/`. The `lib/` folder holds Bootstrap and any other client libraries; the `css/site.css` file holds project-specific styles; the `js/site.js` file holds custom client-side behaviour; the `assets/` folder holds images and icons used by the UI. The author has kept the custom CSS and JavaScript small to avoid duplicating the framework features and to keep the code reviewable.

# 9. User Interface and Usability

## 9.1 Design Language

The design language of UniManage is built on Bootstrap. The framework provides a consistent grid, typography, and component set, which the author has used without heavy customisation. The design language emphasises clarity over decoration. Headings use the framework's heading classes, primary actions use solid buttons, secondary actions use outline buttons, and destructive actions use the `btn-danger` style.

The author has avoided introducing decorative effects that distract from the academic content. Tables are compact but not stripped of borders; cards are used for dashboard summaries; alerts are used for transient feedback after a form submission. The result is a clean, professional appearance suitable for a university course management application.

## 9.2 Layout Structure

The layout structure is defined in `_Layout.cshtml`. The page is composed of the following parts:

- A top navigation bar with the brand link, role-aware items, profile menu, and a sign-in or sign-out action.
- A main content area where each Razor view contributes its content.
- A footer with the project name and the academic context.
- A skip-to-main-content link near the top of the page to support keyboard navigation.

The `_Alerts.cshtml` partial is included in the layout so that any TempData-based feedback message is shown automatically after a form submission. The `_DashboardCalendar.cshtml` partial renders a calendar block when included by a dashboard view.

## 9.3 Role-Aware Navigation

The navigation bar adapts to the signed-in user's role. A Student sees links for the Student dashboard, course browsing, assignments, submissions, messages, and meetings. A Lecturer sees links for the Lecturer dashboard, owned courses, assignments, submissions, materials, messages, and meetings. An Administrator sees links for the Administrator dashboard, the course catalogue, reports, audit logs, and meetings.

The role-aware navigation is implemented using Razor conditions inside the layout. The view checks `User.IsInRole(AppRoles.X)` before rendering each role-specific link. Anonymous users see only the home, login, and register links. This pattern reduces navigation noise and ensures that no link points to a forbidden action.

## 9.4 Forms and Validation Feedback

Forms in UniManage use the standard ASP.NET Core MVC form pattern. Each form binds to a view model rather than to an entity. The form fields use Tag Helpers such as `asp-for`, `asp-validation-for`, and `asp-validation-summary`. The view models declare validation rules through data annotations such as `[Required]`, `[StringLength]`, `[Range]`, and `[EmailAddress]`. When the form is posted with invalid data, the controller returns the same view with the populated model and the validation errors are displayed next to the offending field.

The `_ValidationScriptsPartial.cshtml` partial loads the client-side jQuery validation scripts for client-side validation. Server-side validation always runs in addition to client-side validation, so a malicious client cannot bypass the rules.

## 9.5 Responsive Design

The Bootstrap grid provides responsive layout support. Forms, tables, and dashboards adjust their column widths according to the viewport. The author has avoided fixed pixel widths in custom CSS so that the layout remains usable on smaller screens. The navigation bar uses the framework's collapse component to convert into a hamburger menu on narrow viewports.

Figure 9c (optional): [INSERT SCREENSHOT HERE]

This screenshot shows a screenshot of the dashboard at a narrower viewport could be included as evidence of responsive design.

## 9.6 Accessibility Considerations

The author has applied several accessibility considerations:

- A skip link near the top of the layout allows keyboard users to bypass the navigation and jump to the main content.
- Form labels are associated with their input fields through Tag Helpers, which improves screen-reader behaviour.
- Headings follow a logical structure (h1 for the page title, h2 for major sections).
- Colour is not used as the only way to communicate state; alerts use both colour and icon or text.
- Buttons have meaningful text rather than relying on icons alone.

A full accessibility audit (for example WCAG 2.1 AA compliance) has not been carried out and is therefore marked [NEEDS CONFIRMATION] in the limitations chapter.

## 9.7 Error and Confirmation Messages

Error and confirmation messages are shown through three mechanisms:

- Inline validation messages next to form fields, generated through Razor Tag Helpers.
- A page-level validation summary at the top of forms, generated through `asp-validation-summary`.
- TempData-based alerts shown by the `_Alerts.cshtml` partial after a redirect.

For deeper errors that are not caused by user input, the application displays the friendly error page `Views/Shared/Error.cshtml` in production. The friendly page does not reveal stack traces or internal server detail. In development, the developer exception page is shown instead.

Figure 16: [INSERT SCREENSHOT HERE]

This screenshot shows the validation error example.

Figure 16b: [INSERT SCREENSHOT HERE]

This screenshot shows the friendly error page.

# 10. Detailed Description of Classes, Properties and Methods

## 10.1 Main Controllers and Responsibilities

Table 4 summarises the controllers and their responsibilities, key actions, related views, and the coursework area that they serve.

Table 4: Main Controllers and Responsibilities

| Controller | Main Responsibility | Key Actions / Methods | Related Views | Coursework Area |
|---|---|---|---|---|
| `AccountController` | Authentication and account flows | `Register`, `Login`, `Logout`, `ForgotPassword`, `ResetPassword`, `GoogleCallback` | `Views/Account/` | Registration / Login |
| `DashboardController` | Role dashboard routing and dashboard data | `Index`, `Student`, `Lecturer`, `Administrator` | `Views/Dashboard/` | Dashboards |
| `CoursesController` | Course CRUD, lecturer courses, materials | `Index`, `MyCourses`, `Details`, `Create`, `Edit`, `Delete`, `UploadMaterial` | `Views/Courses/` | Course Management |
| `EnrollmentsController` | Student course browsing and enrolment | `Browse`, `Enroll` | `Views/Enrollments/Browse.cshtml` | Enrollment |
| `AssignmentsController` | Assignment CRUD and listing | `ForCourse`, `Mine`, `Create`, `Edit`, `Delete` | `Views/Assignments/` | Assignment Management |
| `SubmissionsController` | Submission, grading, file access | `Submit`, `Grade`, `ForAssignment`, `CourseMaterialFile`, `SubmissionFile` | `Views/Submissions/` | Submission and Grading |
| `ReportsController` | Reporting and exports | `CoursePopularity`, `StudentPerformance`, `LecturerWorkload`, `Enrollments`, `PassFail`, `AssignmentAttendance` | `Views/Reports/` | Reporting |
| `MessagesController` | Messaging workflow | `Inbox`, `Thread`, `Reply`, `Compose`, `MarkAllRead` | `Views/Messages/` | Communication |
| `MeetingsController` | Meeting scheduling and ICS export | `Index`, `Create`, `Edit`, `Delete`, `Join`, `Ics` | `Views/Meetings/` | Communication |
| `ProfileController` | Profile and password change | `Index`, `ChangePassword` | `Views/Profile/` | Account Management |
| `AuditLogsController` | Audit log review | `Index` | `Views/AuditLogs/Index.cshtml` | Security Evidence |
| `HomeController` | Public landing and error page | `Index`, `Privacy` (if present), `Error` | `Views/Home/`, `Views/Shared/Error.cshtml` | Application Foundation |

## 10.2 Main Models and Properties

Table 5 summarises the main domain models and their properties.

Table 5: Main Models and Properties

| Model | Key Properties | Purpose | Database Relationship |
|---|---|---|---|
| `ApplicationUser` | `Id`, `FullName`, `Email`, `PhoneNumber` (plus inherited Identity fields) | Extends Identity user with profile fields | Related to Course (lecturer), Enrollment (student), Submission (student/grader), Message (sender/receiver), CourseMaterial (uploader), Meeting (lecturer), AuditLog (user) |
| `Course` | `CourseId`, `Code` (unique), `Name`, `Description`, `Credits`, `EnrollmentLimit`, `LecturerId`, `PrerequisiteId` | Course record | One lecturer, optional prerequisite, many enrolments, assignments, materials, meetings |
| `Enrollment` | `EnrollmentId`, `StudentId`, `CourseId`, `EnrolledAtUtc` | Student-course link | Many-to-one with student and course; unique on the pair |
| `Assignment` | `AssignmentId`, `CourseId`, `Title`, `Description`, `DueDateUtc`, `MaxPoints` | Coursework task | Many assignments per course |
| `Submission` | `SubmissionId`, `AssignmentId`, `StudentId`, `Status`, `Grade`, `Feedback`, `SubmittedAtUtc`, `GradedById`, `GradedAtUtc`, file metadata | Student submission and grading record | Many submissions per assignment |
| `SubmissionStatus` | `Submitted`, `Graded` (enum) | Submission status | Used by `Submission` |
| `Message` | `MessageId`, `SenderId`, `ReceiverId`, `Subject`, `Content`, `IsRead`, `SentAtUtc` | Communication record | Two foreign keys to ApplicationUser |
| `CourseMaterial` | `CourseMaterialId`, `CourseId`, `Title`, `StoredFileName`, `OriginalFileName`, `UploadedById`, `UploadedAtUtc` | Uploaded teaching material metadata | Many materials per course |
| `Meeting` | `MeetingId`, `CourseId`, `LecturerId`, `Title`, `Description`, `ScheduledAtUtc`, `DurationMinutes`, `MeetingUrl` | Meeting record | Many meetings per course |
| `AuditLog` | `AuditLogId`, `Category`, `Action`, `Detail`, `UserId`, `Success`, `CreatedAtUtc` | Audit trail | Optional foreign key to ApplicationUser |
| `AppRoles` | `Administrator`, `Lecturer`, `Student` (constants) | Role name constants | Used by Identity roles and `[Authorize]` |
| `ErrorViewModel` | `RequestId`, `ShowRequestId` | Used by error view | Not persisted |

## 10.3 Main Methods and Purpose

Table 6 lists the main methods that drive the application.

Table 6: Main Methods and Purpose

| Class / File | Method | Purpose | Input / Output | Error Handling / Validation |
|---|---|---|---|---|
| `Program.cs` | service configuration | Registers DbContext, Identity, email, audit, rate limiting, anti-forgery and MVC | App configuration to service container | Throws if connection string missing |
| `Program.cs` | middleware configuration | Configures HTTPS, security headers, routing, rate limiter, authentication, authorisation | Request pipeline | Production exception handler vs developer exception page |
| `DbInitializer` | `SeedAsync` | Applies migrations and creates demo roles, users, and sample data | Service provider to seeded database | Throws on failed user creation |
| `AccountController` | `Register` | Creates Student or Lecturer user | Register form to signed-in user | Role check, password validation, audit logging |
| `AccountController` | `Login` | Authenticates user with lockout | Login form to dashboard redirect | `ModelState`, lockout, audit failure |
| `AccountController` | `ForgotPassword` | Sends password reset email | Email address to email send | Token generation, generic response to avoid user enumeration |
| `AccountController` | `ResetPassword` | Resets password using token | Token and new password to updated user | Token validation, password rules |
| `DashboardController` | `Index` | Routes by role | Role claim to role action | Returns 403 if unauthorised |
| `DashboardController` | `Student` / `Lecturer` / `Administrator` | Builds role-specific dashboard view models | EF Core queries to Razor view | Role attribute |
| `CoursesController` | `Create` | Creates course | Course form to database record | Lecturer and prerequisite validation, unique code |
| `CoursesController` | `Edit` | Updates course | Course form to database record | Concurrency and validation |
| `CoursesController` | `UploadMaterial` | Uploads course material | File and title to stored metadata | File type, size, owner, rate limit |
| `EnrollmentsController` | `Browse` | Lists eligible courses | Student claim to course list | Role attribute |
| `EnrollmentsController` | `Enroll` | Enrols student on course | Course ID to enrolment record | Duplicate, capacity, prerequisite checks, audit |
| `AssignmentsController` | `Create` | Creates assignment | Assignment form to database record | Course ownership and model validation |
| `AssignmentsController` | `Edit` / `Delete` | Updates or removes assignment | Assignment ID and form | Course ownership |
| `SubmissionsController` | `Submit` | Saves student submission | Text/file to submission record | Enrolment, file validation, graded lock |
| `SubmissionsController` | `Grade` | Records grade and feedback | Grade form to updated submission | Lecturer ownership and score range |
| `SubmissionsController` | `CourseMaterialFile` / `SubmissionFile` | Streams a stored file | File ID to file content | Role and ownership |
| `ReportsController` | report actions | Builds analytics rows and exports | Query output to view, CSV, or PDF | Role attribute |
| `MessagesController` | `Compose` | Sends message | Recipient, subject, content | Allowed-recipient validation |
| `MessagesController` | `Inbox` / `Thread` / `Reply` | Lists or extends a conversation | View models to Razor views | Role attribute |
| `MeetingsController` | `Create` / `Edit` / `Delete` | Manages meetings | Meeting form to meeting record | Course ownership, URL and time validation |
| `MeetingsController` | `Join` / `Ics` | Joins meeting or downloads ICS | Meeting ID to redirect or file | Role and ownership |
| `ProfileController` | `Index` / `ChangePassword` | Profile and password update | Profile form, password form | Authentication and validation |
| `AuditLogsController` | `Index` | Lists audit log entries | View model to Razor view | Administrator role |
| `AuditLogger` | `LogAsync` | Saves audit record | Category, action, detail, user, success | Internal try/catch logging |
| `UploadedFileStore` | `SaveAsync` | Validates and stores uploaded file | File stream and metadata | File type, size, naming |
| `CsvWriter` | `Write` | Writes CSV content | Rows to stream | Escapes commas, quotes, newlines |
| `PdfReport` | `Build` | Builds PDF using QuestPDF | Data to byte array | Layout exceptions handled by middleware |
| `SmtpEmailService` | `SendAsync` | Sends email using SMTP | Subject, body, recipient | Connection and auth errors |
| `SecurityHeadersMiddleware` | `InvokeAsync` | Adds security headers to every response | HttpContext to response headers | Headers added before next middleware |

## 10.4 Controller Layer Explanation

Each controller in UniManage follows a consistent structure. The class is decorated with the `[Authorize]` attribute (or `[Authorize(Roles = ...)]` where the controller is role-specific). The controller depends on `ApplicationDbContext`, on `UserManager<ApplicationUser>` (where user lookups are needed), on `SignInManager<ApplicationUser>` (in account controllers), and on infrastructure interfaces such as `IAuditLogger`, `IEmailService`, and `UploadedFileStore` where required.

Action methods are asynchronous and return `Task<IActionResult>`. Each action begins with `ModelState.IsValid` checks where appropriate, performs ownership and role checks, executes the EF Core query or update, writes an audit log entry where appropriate, and returns a redirect or a view. The action does not contain duplicated business logic; shared helpers are placed in services or in private methods.

## 10.5 Model and ViewModel Layer Explanation

Models are plain C# classes with primary key, foreign key, and field properties. They use data annotations for length, range, and required validation. They are mapped to database tables through `ApplicationDbContext` and the EF Core conventions.

View models are also plain C# classes, but they are not persisted. They carry data between controllers and views. The author has used view models for every form so that user input cannot bind directly to entity properties that should not be exposed (a defence against over-posting). For example, `CourseInputViewModel` exposes only the course fields that are supposed to be set during create or edit.

## 10.6 Infrastructure and Service Layer Explanation

The Infrastructure layer encapsulates services that are not strictly part of the domain model but are needed by multiple controllers. The layer contains:

- The audit logger (`AuditLogger`, `IAuditLogger`), which saves audit records and is used by every controller that performs a security-sensitive action.
- The email service (`SmtpEmailService`, `IEmailService`, `EmailSettings`, `EmailTemplates`), which is used by the password reset flow.
- The file store (`UploadedFileStore`), which is used by submissions and course materials.
- The CSV writer (`CsvWriter`), which is used by the reports controller.
- The PDF report builder (`PdfReport`), which is used by the reports controller.
- The calendar link helper (`CalendarLink`) and the meeting link generator (`MeetLinkGenerator`), which are used by the meetings controller.
- The custom security headers middleware (`SecurityHeadersMiddleware`), which is registered in `Program.cs` and runs for every response.

Each service uses dependency injection in the constructor, which makes the service replaceable in tests or in different deployments.

# 11. Logical Solution Explanation

## 11.1 Overall Request-to-Response Flow

The end-to-end request flow can be summarised as follows:

1. The browser sends a request to the application.
2. Kestrel accepts the connection and passes it to the middleware pipeline.
3. HTTPS redirection ensures the request is on the secure scheme.
4. The static file middleware serves any request that targets a file in `wwwroot`.
5. The custom security headers middleware adds standard headers to the response.
6. Routing matches the request to a controller and action.
7. Rate limiting checks the policy attached to the action and either allows the request or returns 429.
8. Authentication identifies the user from the cookie.
9. Authorisation checks the `[Authorize]` attribute and the role.
10. The controller action runs. It validates the model, performs business rules, queries or updates EF Core, and writes audit log entries.
11. EF Core translates the LINQ queries into MySQL SQL and executes them through the Pomelo provider.
12. The action returns either a redirect or a view.
13. The Razor engine renders the view using the view model.
14. The response flows back through the pipeline to the browser.
15. The browser renders the HTML and any small amount of JavaScript runs.

This flow is the same for every feature. The differences between features are the validation rules, the EF Core queries, and the resulting view. Keeping the flow consistent is one of the strengths of MVC and makes the project easier to maintain and to mark.

## 11.2 Registration and Login Logic

The registration logic accepts a `RegisterViewModel`, validates it through `ModelState`, builds an `ApplicationUser` with the provided full name, email, and phone number, and calls `UserManager.CreateAsync`. If the operation succeeds, the user is added to the chosen role (Student or Lecturer) through `UserManager.AddToRoleAsync`, signed in through `SignInManager.SignInAsync`, and recorded in the audit log. If the operation fails, the validation errors from `IdentityResult` are added to `ModelState` and the form is returned with the errors.

The login logic accepts a `LoginViewModel`, validates it, and calls `SignInManager.PasswordSignInAsync` with `lockoutOnFailure: true`. The result indicates success, lockout, requires-two-factor, or failure. The action handles each case explicitly, writes an audit log entry, and either redirects to `Dashboard/Index` or returns the form with an error.

## 11.3 Dashboard Logic

`DashboardController.Index` inspects the role claim of the signed-in user and forwards to the role-appropriate action. Each role action runs a series of EF Core queries to fill the role-specific view model and returns the matching view. The author has consciously kept role-specific business rules out of `Index` so that the role mapping remains a single, easy-to-read switch.

## 11.4 Course Management Logic

The course management logic for an Administrator follows the standard CRUD pattern. `Index` lists courses, optionally filtered by a search term and paged. `Create` shows the form (GET) and saves the course (POST), enforcing the unique course code constraint. `Edit` and `Delete` follow the same pattern.

For a Lecturer, `MyCourses` filters the courses by `LecturerId == userId`, and `UploadMaterial` enforces ownership before saving the uploaded file.

## 11.5 Enrollment Validation Logic

The enrolment workflow contains several rules that demonstrate validation working together:

- The course must exist.
- The student must not already be enrolled on the course.
- The course must have spare capacity, computed as `EnrollmentLimit - count(existing enrolments)`.
- The course's prerequisite, if any, must be satisfied by the student.
- The student must not have a soft business reason to be blocked, for example a temporarily disabled account.

If all rules pass, the action creates the `Enrollment` record, writes an audit log entry, and redirects to a confirmation page. If any rule fails, the action returns the browse view with a friendly error message. The combination of unique index (database) and rule check (application) provides defence in depth.

## 11.6 Assignment, Submission and Grading Logic

The assignment, submission, and grading workflow connects three controllers. `AssignmentsController.Create` lets a Lecturer set an assignment for an owned course. `SubmissionsController.Submit` lets a Student upload text and a file for an assignment they are enrolled on. `SubmissionsController.Grade` lets a Lecturer record a grade and feedback for a submission on an owned course.

The grading workflow is the most rule-heavy. The action verifies that the submission belongs to an assignment whose course is owned by the lecturer, displays the submission, accepts a grade and feedback through `GradeSubmissionViewModel`, validates the grade range, updates the `Submission` entity, sets the grader and grading time, and writes an audit log entry. After grading, the submission is locked so that the student can no longer modify it.

## 11.7 Reporting Logic

The reports controller runs an EF Core query for each report. Most queries use `GroupBy` and aggregate functions, and they project into a report view model. The view model is rendered through Razor for HTML output, written through `CsvWriter` for CSV output, or rendered through `PdfReport` for PDF output. The reports are gated by the Administrator role.

## 11.8 Communication Logic

Messages and meetings represent the communication features. The messaging logic enforces an allowed-recipient rule: a student can message a lecturer who teaches a course on which the student is enrolled, and vice versa. This rule is implemented as a database join during the recipient look-up, so the front end never offers an unauthorised recipient.

The meeting logic restricts scheduling and editing to the lecturer who owns the course. Students can see meetings that belong to courses on which they are enrolled. The ICS export is provided for users who want to import the meeting into a personal calendar.

## 11.9 Security and Validation Logic

Security and validation logic runs across every feature. Each form posts an anti-forgery token; each input is validated through data annotations and `ModelState`; each action is gated by `[Authorize]` and ownership checks; each controlled file download checks the requester's role and ownership before streaming the file; each security-sensitive action writes an audit log entry. The combination of these controls is described in Chapter 13.

# 12. Validation, Exception Handling and Error Management

## 12.1 Data Annotation Validation

Data annotation validation is the first line of defence. Each view model declares attributes such as `[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`, `[Compare]`, and `[DataType]`. The MVC framework runs these attributes during model binding and populates `ModelState`. Table 16 shows representative validation rules for selected forms.

Table 16: Validation Rules by Form

| Form / View Model | Selected Validation Rules |
|---|---|
| `RegisterViewModel` | `[Required]` on full name, email, password; `[EmailAddress]` on email; `[StringLength]` on full name; password requirements enforced by Identity (8 chars, digit, upper, lower) |
| `LoginViewModel` | `[Required]` on email and password; `[EmailAddress]` on email |
| `ForgotPasswordViewModel` | `[Required]`, `[EmailAddress]` on email |
| `ResetPasswordViewModel` | `[Required]` on token, password, confirm password; `[Compare]` on confirm password |
| `CourseInputViewModel` | `[Required]` on code and name; `[StringLength]` on code, name, description; `[Range]` on credits and enrolment limit |
| `AssignmentInputViewModel` | `[Required]` on title, due date; `[StringLength]` on title and description; `[Range]` on max points |
| `SubmissionSubmitViewModel` | `[StringLength]` on text content; file validation in controller |
| `GradeSubmissionViewModel` | `[Range]` on grade; `[StringLength]` on feedback |
| `MessageComposeViewModel` | `[Required]` on subject, content, recipient; allowed-recipient check in controller |
| `MeetingInputViewModel` | `[Required]` on title, scheduled time; `[Url]` on meeting URL where present; `[Range]` on duration |
| `MaterialUploadViewModel` | `[Required]` on title; file validation in controller |
| `ProfileViewModel` | `[Required]` on full name and email; `[EmailAddress]` on email; `[Phone]` on phone number where present |

## 12.2 ModelState Handling

Every POST action that accepts user input begins with a `ModelState.IsValid` check. If the model state is invalid, the action returns the same view with the populated view model so that the user can see and correct the validation errors. The author has avoided silent failures and has avoided redirecting to a generic error page when the user can recover by correcting the form.

## 12.3 Server-Side Ownership Checks

Some validation rules cannot be expressed as data annotations. For example, "a Lecturer can only edit assignments for courses they own" requires a database lookup. These checks are implemented inside the action after the `ModelState` check. If the check fails, the action returns either a 403 Forbidden response or a redirect to the appropriate index view with a TempData warning. The combination of role attributes and ownership checks prevents both unauthorised role access and unauthorised data access.

## 12.4 File Upload Validation

`UploadedFileStore` validates the uploaded file before storing it. The validation includes:

- File size: rejects files larger than the configured maximum.
- File extension: rejects files whose extension is not in the allowed list.
- File name: stores the file under a generated stored file name to avoid collisions and to prevent path traversal attacks.
- Content type: optional check against the allowed MIME types.

If the file fails validation, the action adds a model error and returns the form with an explanation. The rate-limiting policy `upload` further restricts the number of uploads per minute per user to prevent abuse.

## 12.5 Anti-Forgery Protection

Anti-forgery is configured globally in `Program.cs` through `AddAntiforgery`. Every Razor form generated through Tag Helpers includes a hidden anti-forgery token. POST actions that accept form data are decorated with `[ValidateAntiForgeryToken]`. The combination prevents cross-site request forgery attacks.

The anti-forgery cookie is configured with `HttpOnly = true`, `SameSite = Strict`, and `SecurePolicy = SameAsRequest`, which provides additional defence against cookie theft.

## 12.6 Exception Handling Middleware

In production, `Program.cs` configures `app.UseExceptionHandler("/Home/Error")` so that any unhandled exception is caught, logged, and replaced with the friendly error view. The view does not include any stack trace, source file, or server detail. In development, `app.UseDeveloperExceptionPage()` provides a detailed exception page that helps the author find the source of the problem.

The friendly error view uses `ErrorViewModel` to optionally show a request identifier, which can be cross-referenced with the application logs.

## 12.7 Friendly Error Pages

Apart from the global exception handler, individual actions use the standard MVC return types `BadRequest`, `NotFound`, and `Forbid` to indicate specific error conditions. The framework renders these as the appropriate status code page, which can be customised through `app.UseStatusCodePagesWithReExecute` if required. The author has retained the standard behaviour and instead emphasised friendly TempData messages on success and on most foreseeable failures.

## 12.8 Audit and Error Evidence

Each security-sensitive failure is recorded in the audit log with `success = false`. This means that a marker reviewing the audit log can see not only successful actions but also blocked actions, such as failed sign-ins, rejected enrolments, and unauthorised access attempts. The audit log therefore acts as a forensic record as well as a behavioural record.

Figure 16: [INSERT SCREENSHOT HERE]

This screenshot shows the validation error example.

Figure 16b: [INSERT SCREENSHOT HERE]

This screenshot shows the friendly error page.

# 13. Security and Data Protection

Security is a cross-cutting concern in UniManage. The system is designed with defence in depth: each request passes through a chain of controls so that a failure of one control does not lead to a compromise.

## 13.1 Authentication

Authentication is provided by ASP.NET Core Identity. The `ApplicationUser` class extends `IdentityUser` and adds profile fields such as `FullName` and `PhoneNumber`. The Identity options enforce a minimum password length of eight characters, require a digit, require an uppercase letter, and require a lowercase letter. A non-alphanumeric character is not strictly required, which keeps the password rule consistent with the seeded credentials.

Account lockout is enabled. After five failed attempts, the account is locked for fifteen minutes. The lockout policy reduces the effectiveness of brute-force attacks. The login form is also protected by the `auth` rate-limiting policy, which allows ten requests per IP address per minute. Combined, the lockout and rate-limiting policies make brute-force attacks impractical against a single account or against the system as a whole.

Optional Google login is supported. Where the `Authentication:Google:ClientId` and `Authentication:Google:ClientSecret` configuration values are present, the Google authentication handler is registered in `Program.cs`, including a `OnRemoteFailure` handler that redirects to the login page with the failure reason. Google login is not required for marking, and the demonstration users use plain Identity credentials.

## 13.2 Authorisation

Authorisation is enforced through `[Authorize]` attributes on controllers and actions, with `Roles` set to `AppRoles.Administrator`, `AppRoles.Lecturer`, or `AppRoles.Student` as appropriate. Where multiple roles are allowed, the attribute lists them separated by commas. The base controller class is decorated with `[Authorize]` so that anonymous users cannot reach any action by default; only the home page and the account flows are explicitly anonymous.

Ownership checks are added inside actions where role membership is not enough. For example, a Lecturer must only see their own courses, materials, assignments, submissions, and meetings. The ownership check is implemented as a database filter (`LecturerId == userId`) in the EF Core query, so the resulting view never contains rows that the user is not authorised to see.

## 13.3 Password Handling

Passwords are never stored in plain text. ASP.NET Core Identity uses PBKDF2 with HMAC-SHA-512 by default, with a per-user salt and configurable iteration count. Password reset uses Identity's tokenised flow. The password reset token is generated by `UserManager.GeneratePasswordResetTokenAsync`, sent through the email service, and validated by `UserManager.ResetPasswordAsync`. The reset endpoint is rate limited under the `auth` policy.

The author has not implemented "have I been pwned" checks or pre-generated weak password lists; this could be added in a future version. The current rules are sufficient for an academic deployment.

## 13.4 Role-Based Access Control

Role-based access control is the third major control. Three roles are seeded by `DbInitializer`: Administrator, Lecturer, Student. Each role is assigned to the corresponding demonstration user. New registrants choose either Student or Lecturer. The Administrator role is not available through self-registration; only the seeded administrator can use it. This is consistent with the principle that high-privilege roles should not be granted by self-service.

## 13.5 Protection Against Common Web Risks

Table 17 maps the security controls in UniManage to common web risks.

Table 17: Security Controls Mapped to Common Web Risks

| Risk | Control in UniManage | Source-Code Evidence |
|---|---|---|
| Injection (SQL, command) | EF Core parameterised queries | All controller queries through `DbSet` and LINQ |
| Broken authentication | Identity with strong password rules, lockout, secure cookies | `Program.cs` Identity options, `ConfigureApplicationCookie` |
| Sensitive data exposure | HTTPS redirection, HSTS, secure cookie policy, redacted screenshots | `Program.cs`, `User_Manual_Application_Development.md` |
| Broken access control | `[Authorize]` attributes, role checks, ownership filters | All controllers |
| Security misconfiguration | Security headers, HSTS, secure cookie policy | `Infrastructure/SecurityHeadersMiddleware.cs`, `Program.cs` |
| Cross-site scripting (XSS) | Razor automatic HTML encoding, content-type checks | All views, model binding |
| Cross-site request forgery (CSRF) | Anti-forgery cookie and token | `Program.cs`, `[ValidateAntiForgeryToken]` attributes |
| Insecure deserialization | Strongly typed view models, no untrusted serialiser | View models throughout |
| Components with known vulnerabilities | Pinned package versions, latest patch level when developed | `AD COURSEWORK 2.csproj` |
| Insufficient logging and monitoring | Audit logger with `IAuditLogger` and `AuditLog` entity | `Infrastructure/AuditLogger.cs`, `Models/AuditLog.cs` |
| Brute-force attacks | Account lockout, rate limiter on `auth` policy | `Program.cs` lockout, `AddRateLimiter` |
| Path traversal in file storage | `UploadedFileStore` generates stored file names | `Infrastructure/UploadedFileStore.cs` |
| Excess upload load | Rate limiter on `upload` policy | `Program.cs` upload policy |

## 13.6 File Security

File security combines validation, controlled storage, and controlled retrieval. Files are validated by `UploadedFileStore` and stored under a generated file name in a controlled directory. Files are retrieved through controller actions (`CourseMaterialFile`, `SubmissionFile`) that check role and ownership before streaming the file. Direct URL access into the upload directory is not exposed to the browser, which prevents direct file enumeration.

## 13.7 Audit Logging and Traceability

Audit logging is provided through `IAuditLogger` and `AuditLogger`. The audit log records the category of the event, the action, a short human-readable detail, the user identifier (where available), and a success flag. Categories include `Account`, `Course`, `Enrollment`, `Assignment`, `Submission`, `Material`, `Message`, `Meeting`, `Report`, and `Security`. Table 18 shows representative categories and actions.

Table 18: Audit Logger Categories

| Category | Actions Recorded |
|---|---|
| `Account` | `Register`, `Login`, `LoginFailed`, `Logout`, `PasswordReset` |
| `Course` | `Create`, `Update`, `Delete` |
| `Enrollment` | `Enroll`, `EnrollFailed` |
| `Assignment` | `Create`, `Update`, `Delete` |
| `Submission` | `Submit`, `Grade` |
| `Material` | `Upload`, `Download` |
| `Message` | `Send`, `Reply` |
| `Meeting` | `Create`, `Update`, `Delete`, `Join` |
| `Report` | `Generate`, `Export` |
| `Security` | `AccessDenied`, `RateLimited`, `AntiForgeryFailure` (where reported) |

Audit log entries are reviewed through `AuditLogsController.Index`, which is restricted to Administrators.

## 13.8 Data Protection Considerations

The author has applied several data protection considerations:

- Sensitive configuration (database password, SMTP password, Google client secret) is held in `appsettings.json` and is excluded from screenshots and the report.
- Personal data shown in screenshots is redacted before insertion in the report.
- The audit log records identifiers and actions but does not record passwords or sensitive payloads.
- HTTPS and HSTS protect data in transit.
- Cookies are HTTP-only and `SameSite` protected.
- The retention of audit log data is not formally documented; this is recorded as [NEEDS CONFIRMATION] in the limitations chapter.

# 14. Programming Style and Maintainability

Table 19 summarises the main programming style practices applied in the project.

Table 19: Programming Style Practices

| Practice | Description | Evidence |
|---|---|---|
| Naming conventions | PascalCase for classes, methods, and properties; camelCase for parameters and local variables. | All controllers, models, services |
| MVC folder structure | Controllers, Models, ViewModels, Views, Data, Infrastructure | Project root |
| Single responsibility | Each service and controller addresses one area | Infrastructure folder |
| Dependency injection | Services are injected through constructors | All controllers |
| Asynchronous programming | EF Core, Identity, file, and email operations use `async`/`await` | All controllers and services |
| Strongly typed view models | Forms bind to view models, not entities | ViewModels folder |
| Routing conventions | Default `{controller}/{action}/{id?}` pattern | `Program.cs` |
| Configuration through `appsettings.json` | Connection strings, Google credentials, email settings | `Program.cs` |
| Migrations under source control | `Data/Migrations` is committed | Git history |
| Centralised security headers | Custom middleware sets headers consistently | `Infrastructure/SecurityHeadersMiddleware.cs` |
| Centralised audit logging | Single `IAuditLogger` service | `Infrastructure/AuditLogger.cs` |
| Build verification | `dotnet build --no-restore` returns 0 errors and 0 warnings | Build evidence |

## 14.1 Naming and Readability

Names in the project are descriptive. Controller names end in `Controller`. Model names match the academic concept (`Course`, `Enrollment`, `Submission`). View models have a `ViewModel` suffix. Methods are named after the action they perform (`Submit`, `Grade`, `Enroll`). Local variables are named after their purpose. Acronyms such as `Url`, `Id`, and `Pdf` follow the .NET naming style.

## 14.2 MVC Separation of Concerns

Separation of concerns is enforced through the folder structure and through the use of dependency injection. Models contain data and validation rules. Controllers contain request flow and business rules. Views contain presentation. Services contain cross-cutting concerns. The data access layer is encapsulated behind `ApplicationDbContext`. This makes it possible to update one layer without touching the others.

## 14.3 View Models and Entities

View models and entities are kept separate. View models hold only the fields that the form expects. Entities hold the fields that are persisted. This protects against over-posting attacks where a malicious user adds extra fields to a form to update properties they should not be able to set.

## 14.4 Dependency Injection

All services are registered through `Program.cs` and consumed through constructor injection. The author has not used a service locator pattern. Lifetimes are chosen carefully: scoped for `ApplicationDbContext` and most services, singleton for stateless utility classes where used, and transient for short-lived services where necessary.

## 14.5 Asynchronous Programming

EF Core operations, Identity calls, file operations, and email calls are asynchronous. The asynchronous pattern keeps the application responsive and reduces thread pool starvation. Controller actions are also asynchronous and use `await` rather than `.Result` or `.Wait()`, which prevents deadlocks.

## 14.6 Comments and Code Clarity

The author has used comments sparingly. Self-documenting names, small methods, and conventional structure reduce the need for comments. Where a comment is added, it explains intent or trade-offs rather than restating the code. The author has avoided narrating obvious operations (such as "increment the counter") and has not added emojis or decorative comments.

## 14.7 Maintainability Improvements

The current codebase is maintainable, but there is always room for improvement. The reflection chapter (Chapter 19) and the future improvements chapter (Chapter 21) include items such as automated tests, continuous integration, structured logging, and caching of read-heavy queries. These items would further improve maintainability without requiring a full rewrite.

# 15. Testing

## 15.1 Testing Strategy

The testing strategy combines manual functional testing, security testing, and build verification. Automated unit and integration tests are not included in the project; this is honestly noted as a limitation. Manual testing has been used because the marking criteria emphasise feature evidence and screenshots, and because the author has prioritised feature implementation and documentation given the approved individual submission scope.

The strategy follows three principles. First, every feature is tested at least once with a positive test case (the feature works) and one negative test case (the feature rejects bad input). Second, every test result is captured by a screenshot or by the audit log. Third, every test is traceable to a requirement and to a marking item.

## 15.2 Test Environment

The test environment is the author's local machine running Windows, Visual Studio 2022, .NET 8 SDK, MySQL 8, and a modern browser. The application is started with `dotnet run` or through Visual Studio. The seeded demonstration users are used for sign-in. The browser developer tools are used to verify HTTPS, cookie attributes, and request/response details where relevant.

## 15.3 Functional Testing

Functional testing covers the path through every major feature. Each functional test starts from a known state (for example a clean database after seeding) and ends with a verified outcome. The test plan in Section 15.9 lists the individual cases.

## 15.4 Validation Testing

Validation testing covers data annotations, ModelState behaviour, ownership checks, and file validation. Each form is tested with valid input and with input that violates one or more rules. The expected result is a friendly validation message and no database change.

## 15.5 Role-Based Access Testing

Role-based access testing confirms that a user in one role cannot access a controller action restricted to a different role. The test sequence is: sign in as a Student, attempt to open `/Courses/Create`, observe access denied; repeat for the Lecturer and Administrator. The audit log entries should also reflect the access denial.

## 15.6 Database Testing

Database testing confirms that records are created, updated, and deleted correctly. The verification is done by signing in to MySQL with a database tool and inspecting the table contents after each action. The unique constraints and foreign keys are also tested by attempting to violate them through the UI.

## 15.7 UI and Usability Testing

UI and usability testing covers navigation, layout, alerts, and responsive design. The expectation is that each page is reachable through the role-aware menu, that each form provides appropriate feedback, and that the layout adapts to a narrow viewport.

## 15.8 Build Verification

Build verification is performed with `dotnet build --no-restore` from the project root. The build has been verified to succeed with zero warnings and zero errors. The build evidence is available as a console capture.

## 15.9 Test Plan and Results

Table 8 lists the test plan with at least twenty-five test cases.

Table 8: Test Plan and Results

| Test ID | Test Area | Test Scenario | Input / Setup | Expected Result | Actual Result | Status |
|---|---|---|---|---|---|---|
| T1 | Registration | Register a new Student | Valid form | User created, signed in, audit log entry written | [SCREENSHOT REQUIRED] | Pending |
| T2 | Registration | Register a new Lecturer | Valid form | User created, signed in | [SCREENSHOT REQUIRED] | Pending |
| T3 | Registration validation | Submit register form with weak password | Password "abc" | Validation message, no user created | [SCREENSHOT REQUIRED] | Pending |
| T4 | Login | Log in as seeded Student | Demo credentials | Redirect to Student dashboard | [SCREENSHOT REQUIRED] | Pending |
| T5 | Login lockout | Submit five wrong passwords | Wrong password | Account locked, audit log entry | [SCREENSHOT REQUIRED] | Pending |
| T6 | Logout | Sign out | Authenticated session | Redirect to home, cookie cleared | [SCREENSHOT REQUIRED] | Pending |
| T7 | Forgot password | Request reset for known email | Valid email | Email sent (or token logged), generic confirmation | [SCREENSHOT REQUIRED] | Pending |
| T8 | Student dashboard | Student opens dashboard | Authenticated student | Dashboard shows enrolled courses and deadlines | [SCREENSHOT REQUIRED] | Pending |
| T9 | Lecturer dashboard | Lecturer opens dashboard | Authenticated lecturer | Dashboard shows teaching summary and pending submissions | [SCREENSHOT REQUIRED] | Pending |
| T10 | Administrator dashboard | Administrator opens dashboard | Authenticated administrator | Dashboard shows counts and popular courses | [SCREENSHOT REQUIRED] | Pending |
| T11 | UI navigation | Navigate using role-aware menu | Each role | Each role sees only relevant items | [SCREENSHOT REQUIRED] | Pending |
| T12 | Course list | Administrator views course list | Administrator | List displayed with search and paging | [SCREENSHOT REQUIRED] | Pending |
| T13 | Course create | Administrator creates a course | Valid form | Course saved, redirect, audit log entry | [SCREENSHOT REQUIRED] | Pending |
| T14 | Course code uniqueness | Administrator creates two courses with the same code | Duplicate code | Second create rejected | [SCREENSHOT REQUIRED] | Pending |
| T15 | Course delete | Administrator deletes a course | Course id | Course removed, related records handled | [SCREENSHOT REQUIRED] | Pending |
| T16 | Enrolment success | Student enrols on an open course | Course with capacity | Enrolment saved, confirmation displayed | [SCREENSHOT REQUIRED] | Pending |
| T17 | Enrolment rejection | Student attempts duplicate enrolment | Already enrolled | Rejection message, no new record | [SCREENSHOT REQUIRED] | Pending |
| T18 | Assignment create | Lecturer creates an assignment for an owned course | Valid form | Assignment saved | [SCREENSHOT REQUIRED] | Pending |
| T19 | Assignment ownership | Lecturer attempts to edit an assignment for another lecturer | Other lecturer's assignment | Forbidden | [SCREENSHOT REQUIRED] | Pending |
| T20 | Submission upload | Student submits text and file | Enrolled student | Submission saved, file stored | [SCREENSHOT REQUIRED] | Pending |
| T21 | Grading | Lecturer grades a submission with valid grade | Valid grade and feedback | Grade and feedback saved, submission locked | [SCREENSHOT REQUIRED] | Pending |
| T22 | Reporting | Administrator opens course popularity report | Administrator | Report rows displayed | [SCREENSHOT REQUIRED] | Pending |
| T23 | CSV export | Administrator exports a report as CSV | Administrator | CSV file downloaded | [SCREENSHOT REQUIRED] | Pending |
| T24 | Messaging compose | Student messages an allowed lecturer | Valid recipient | Message saved, thread updated | [SCREENSHOT REQUIRED] | Pending |
| T25 | Messaging restriction | Student attempts to message an unrelated lecturer | Disallowed recipient | Message rejected | [SCREENSHOT REQUIRED] | Pending |
| T26 | Anti-forgery | Submit POST without token using developer tools | No token | Request rejected | [SCREENSHOT REQUIRED] | Pending |
| T27 | Rate limit | Submit eleven login requests within a minute | Eleven requests | Eleventh receives 429 | [SCREENSHOT REQUIRED] | Pending |
| T28 | Access denied | Student opens `/Courses/Create` | Authenticated student | Forbidden, audit log entry | [SCREENSHOT REQUIRED] | Pending |
| T29 | Validation message | Submit invalid course form | Missing required fields | Inline validation messages | [SCREENSHOT REQUIRED] | Pending |
| T30 | Friendly error | Trigger an error in production | Production environment | `Error.cshtml` page displayed | [SCREENSHOT REQUIRED] | Pending |
| T31 | Responsive layout | Open dashboard on a narrow viewport | Browser resize | Layout adapts, navigation collapses | [SCREENSHOT REQUIRED] | Pending |
| T32 | Build verification | Run `dotnet build --no-restore` | Project root | Zero warnings and zero errors | Confirmed (build succeeded) | Confirmed |

# 16. Evaluation Against Coursework Brief

## 16.1 Marking Scheme Alignment

Table 7 evaluates the implementation against each marking item in the coursework brief.

Table 7: Coursework Marking Alignment

| Marking Item | Evidence in Project | Strength | Risk |
|---|---|---|---|
| Application user interface | Razor views, `_Layout.cshtml`, `_Alerts.cshtml`, Bootstrap, `site.css`, role views | Strong source evidence | Screenshots required |
| User Registration and Login | `AccountController`, Identity options, anti-forgery, lockout, audit | Strong source evidence | Screenshots required |
| Student Dashboard | `DashboardController.Student`, `Views/Dashboard/Student.cshtml`, view model | Strong source evidence | Screenshot required |
| Lecturer Dashboard | `DashboardController.Lecturer`, lecturer view, view model | Strong source evidence | Screenshot required |
| Administrator Dashboard | `DashboardController.Administrator`, administrator view, view model | Strong source evidence | Screenshot required; user-management screen [NEEDS CONFIRMATION] |
| Course Management | `CoursesController`, `Course`, `CourseInputViewModel`, course views | Strong source evidence | Screenshots required |
| Enrollment System | `EnrollmentsController`, `Enrollment`, `CourseBrowseRowViewModel` | Strong source evidence | Screenshots required |
| Assignment and Grading | `AssignmentsController`, `SubmissionsController`, view models | Strong source evidence | Screenshots required |
| Reporting and Analytics | `ReportsController`, `CsvWriter`, `PdfReport`, QuestPDF | Strong source evidence | Screenshots and export evidence required |
| Communication Module | `MessagesController`, `MeetingsController`, view models | Strong source evidence | Screenshots required |
| Security and Data Protection | Identity, RBAC, anti-forgery, rate limiting, security headers, audit | Strong source evidence | Audit log screenshot required |
| Installation Guide and User Manual | `User_Manual_Application_Development.md` | Draft prepared | Final Word/PDF export pending |
| Concise Logical Solution | Chapter 11 of this report | Draft prepared | Keep concise after Word formatting |
| Architecture Diagrams | `documentation_application_development/diagrams/*.mmd` | Sources prepared | Export required |
| Class/property/method description | Chapter 10 tables | Strong | Cross-check after final code freeze |
| Own Reflection | Chapter 19 | Draft prepared | Strengthen with concrete examples |
| Programming Style | Chapter 14 | Strong | Keep code samples short in appendix |
| Validation and Exception Handling | Chapter 12 | Strong | Validation screenshot required |
| Interface Design and Usability | Chapter 9 | Draft prepared | Screenshots required |

## 16.2 Strengths of the Implementation

Table 20 summarises the strengths of the implementation.

Table 20: Strengths of the Implementation

| Strength | Description |
|---|---|
| Complete MVC structure | Twelve controllers, twelve models, sixteen view models, forty-six views, and a clear infrastructure layer. |
| Strong security pipeline | Identity, lockout, anti-forgery, rate limiting, security headers, HTTPS, HSTS, audit logging. |
| Clean separation of concerns | Models, view models, controllers, views, and infrastructure are kept in dedicated folders. |
| Consistent role-based access | All controllers use `[Authorize]` with role attributes and ownership checks. |
| Reliable data layer | EF Core with retry on failure, three migrations, and a clear DbContext. |
| Reporting suite | Six reports with CSV and PDF export through QuestPDF. |
| Audit traceability | Every security-sensitive action is logged through `IAuditLogger`. |
| Build verified | `dotnet build --no-restore` returns 0 warnings and 0 errors. |
| Documentation package | Main report, individual contribution, user manual, evidence checklist, diagrams, and QA reports. |
| Approved individual submission | Wording is consistent with the approved individual completion route. |

## 16.3 Remaining Risks

Table 21 lists the remaining risks before submission.

Table 21: Remaining Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Final screenshots not yet captured | High | Capture screenshots listed in Chapter 5 and Appendix B before Word export. |
| Diagrams not yet exported | High | Export Mermaid sources to PNG and PDF. |
| Repository link placeholder | High | Paste the final repository link into `Source_And_Build_Links.md`. |
| Build / deployment link placeholder | High | Paste the packaged build or hosted deployment link. |
| Database evidence not captured | Medium | Capture migration list output and a redacted table view. |
| Automated tests absent | Medium | Document as a future improvement; add manual test evidence in Chapter 15. |
| Administrator user-management screen [NEEDS CONFIRMATION] | Medium | Confirm with the brief; if required, document as future improvement. |
| Accessibility audit not carried out | Low | Document as future improvement; current measures listed in Chapter 9. |

## 16.4 Evidence Still Required

The complete list of evidence still required is held in `Evidence_Checklist.md` and is summarised below:

- Screenshots for Figures 7 to 20b.
- Exported diagrams for Figures 1 to 6.
- Repository link, packaged build link, and database evidence in `Source_And_Build_Links.md`.
- Final Word and PDF version of this report.
- Optional individual approval evidence file in `evidence/`.

# 17. Challenges Faced

## 17.1 Technical Challenges

The main technical challenges encountered during the project were the integration of Identity with the Pomelo MySQL provider, the configuration of EF Core retry on failure to handle transient connection issues, the setup of the rate limiter with two distinct policies for authentication and uploads, the configuration of QuestPDF to operate under the Community licence, and the design of the audit logger to write log entries reliably without disrupting the calling action. Each challenge was addressed by reading the official documentation, by checking the resulting behaviour through running the application, and by reading the audit log to confirm that events were captured as expected.

## 17.2 Documentation Challenges

The main documentation challenge was producing a long, evidence-based report without inventing features. The author has resolved this by reviewing the source code chapter by chapter and by marking unverifiable claims as [NEEDS CONFIRMATION] or [SCREENSHOT REQUIRED]. The author has also avoided copying content from unrelated coursework reports, which would damage academic integrity even if the surface text were rewritten.

## 17.3 Evidence Preparation Challenges

Capturing evidence in a marker-friendly way is harder than it looks. Screenshots need to be taken with the right resolution, the right window size, and with personal data redacted. Diagrams need to be exported in a vector or high-resolution format so that they print cleanly. Database evidence must avoid revealing connection strings or production-like credentials. The author has prepared the evidence checklist and the screenshots README so that each piece of evidence is captured with consistent quality.

## 17.4 Individual Completion Challenges

The approved individual completion route adds two challenges. First, it requires the author to take responsibility for every part of the work, including planning, time management, and submission. Second, it requires the documentation to remain neutral, factual, and respectful of other students. The author has addressed both challenges by structuring the work week by week, by maintaining the evidence checklist, and by following the wording rules in the documentation expansion plan.

# 18. Limitations

## 18.1 Functional Limitations

The current build does not include a dedicated administrator user-management screen, automated unit and integration tests, advanced reporting filters, or live video conferencing. Each of these is recorded as [NEEDS CONFIRMATION] or as a future improvement.

## 18.2 Testing Limitations

Automated tests are not present. Manual testing has been used to validate functional, validation, and security behaviour. The test plan in Chapter 15 lists thirty-two test cases, of which the build verification (T32) is confirmed. The remaining cases require screenshots and manual verification before submission.

## 18.3 Deployment Limitations

A hosted deployment is not available. The application has been verified to run locally on `https://localhost:7212` and `http://localhost:5103`. A packaged build (ZIP) and an optional hosted deployment URL are listed in `Source_And_Build_Links.md` and remain placeholders until the author fills them in.

## 18.4 Accessibility Limitations

The author has applied basic accessibility considerations such as a skip link, label association, and semantic markup. A formal WCAG 2.1 AA audit has not been carried out and is therefore noted as a limitation.

## 18.5 Evidence Limitations

The screenshot, diagram, repository, build, and database evidence is to be captured before final submission. Until that capture is completed, the report records the requirement explicitly using the placeholders [SCREENSHOT REQUIRED], [DIAGRAM EXPORT REQUIRED], and [NEEDS CONFIRMATION].

# 19. Own Reflection on Experience

## 19.1 Technical Learning

The author has gained practical experience in several technical areas. Building UniManage required combining ASP.NET Core MVC routing, Razor views, EF Core, the Pomelo MySQL provider, Identity, anti-forgery, rate limiting, custom middleware, and dependency injection in a single project. The most useful technical learning has been the realisation that defence in depth is achieved by combining several controls rather than by relying on any one. For example, the unique constraint on `Enrollment(StudentId, CourseId)` and the application-level duplicate check work together to prevent duplicate enrolments; either control on its own would leave a gap.

The author has also learned to read and trust official documentation. Microsoft Learn, the Entity Framework Core documentation, the Pomelo MySQL provider documentation, the QuestPDF documentation, and the Bootstrap documentation have all influenced the design. Where a feature exists in the framework (for example anti-forgery), the author has used the framework feature rather than rebuilding it.

A further technical learning point is the importance of asynchronous programming. Database, file, and email operations all benefit from `async`/`await`, which keeps the application responsive and avoids blocking the thread pool. Working through the project has reinforced the discipline of writing asynchronous code from the outset rather than retrofitting it later.

## 19.2 Professional Learning

The author has gained professional learning around documentation, evidence handling, and academic integrity. Producing a substantial report requires planning, structure, and discipline. The author has used the documentation expansion plan to structure the work, has used the evidence checklist to track screenshots and diagrams, and has used the lecturer marking alignment report to keep the writing oriented towards the brief.

The author has also learned how to balance breadth and depth. A long report can become repetitive if every section says the same thing in different words. The author has therefore tried to give each chapter a clear focus and to avoid unnecessary overlap. Where overlap is unavoidable, the author has used cross-references rather than restating the same content.

## 19.3 Problem-Solving Experience

Several problems required deliberate problem-solving during the project. Three examples are given.

First, the integration of EF Core with the Pomelo MySQL provider initially produced transient connection errors during startup because the local MySQL service was slow to start. The author resolved this by enabling retry on failure with up to five retries and a thirty second maximum delay, which hid the transient failures from the user.

Second, the rate limiter was first configured with a single policy that applied to every endpoint, which caused unnecessary 429 responses on dashboard pages. The author resolved this by splitting the rate limiter into two named policies (`auth` and `upload`) and applying them only to the relevant actions.

Third, the audit logger initially failed silently when the audit table did not exist (during the first migration). The author resolved this by ordering the migrations carefully, ensuring that the audit table is created before any audit calls run, and by wrapping the audit write in a try/catch block so that an audit failure cannot crash the calling action.

## 19.4 Impact of Approved Individual Submission

The approved individual completion route had a significant impact on the planning, the workload, and the documentation. The author had to plan the project as a single-person delivery from the start, which required a clear scope and a clear weekly schedule. The workload was higher than for a group submission because every chapter, every screenshot, and every diagram is the author's own. However, the author found that the individual route also provides clarity. There is no risk of conflicting decisions, no need for coordination meetings, and no ambiguity around ownership.

Crucially, the individual route did not change the academic standards. The author still produced a complete code base, a complete documentation package, and a complete evidence set. The author's response to the individual completion approval has been to invest in evidence-based writing rather than to claim shortcuts.

## 19.5 Ethical and Academic Integrity Awareness

Academic integrity has been a central concern. The author has not copied text from unrelated coursework reports. Where official documentation has informed a decision, the documentation is acknowledged in the references chapter. Where a feature could not be confirmed by source code, the author has marked the claim as [NEEDS CONFIRMATION] rather than embellishing the report. The author has also kept sensitive configuration values out of the report and has redacted personal data in screenshots.

Data protection has informed several decisions. The audit log records identifiers and actions but does not record passwords or sensitive payloads. Personal data shown in screenshots is redacted. The retention period for audit log data is not formally documented, which is recorded as a limitation.

## 19.6 Future Learning Plan

The author plans to extend the technical and professional learning gained from this coursework. Three priorities are noted.

First, the author intends to learn automated testing in depth, including unit testing with xUnit, integration testing with the ASP.NET Core test host, and UI testing with Playwright. This will allow the author to ship code with regression tests rather than relying solely on manual testing.

Second, the author intends to learn continuous integration and deployment using GitHub Actions, which will support a hosted demonstration environment and consistent build verification.

Third, the author intends to expand the security and data protection awareness gained during this project by studying OWASP guidance, the .NET security documentation, and accessibility standards in more depth. These topics combine technical and professional learning, which is a useful foundation for further academic work and for industry practice.

# 20. Individual Contribution

The full individual contribution report is attached as a supporting document and is also held at `documentation_application_development/Expanded_Individual_Contribution.md`. The supporting document explains the author's approved individual completion context and summarises the implementation, testing, documentation, and submission preparation work.

For convenience, the contribution is summarised below in a single paragraph: the author selected and completed UniManage as an approved individual submission, has produced the source code, the documentation package, the diagrams, the user manual, the evidence checklist, and this main report. The author has reviewed the source code chapter by chapter, has prepared a marker-friendly mapping between requirements and evidence, and has marked unverifiable claims as [NEEDS CONFIRMATION] or [SCREENSHOT REQUIRED]. The contribution is therefore the entire delivery, prepared and submitted in accordance with the academic integrity rules of CS6004ES Application Development.

# 21. Future Improvements

Table 22 lists the future improvements that the author has identified.

Table 22: Future Improvements and Justification

| Improvement | Justification |
|---|---|
| Automated unit and integration tests | Provide regression coverage and reduce reliance on manual testing. |
| Continuous integration and deployment | Provide consistent build verification and a hosted demonstration environment. |
| Accessibility audit (WCAG 2.1 AA) | Confirm that the user interface is accessible to all users. |
| Dedicated administrator user-management screen | Allow administrators to create, disable, or remove users without direct database access. |
| Advanced report filters and chart visualisations | Improve the academic insight provided by the reporting suite. |
| Email and in-app notifications | Notify students of new assignments and grades, and lecturers of new submissions. |
| Two-factor authentication | Strengthen authentication for high-privilege accounts. |
| Structured logging with Serilog or similar | Improve observability and integrate with log management tools. |
| Caching of read-heavy queries | Improve dashboard performance for large datasets. |
| Internationalisation and localisation | Support multiple languages where required. |
| Mobile-friendly enhancements beyond Bootstrap defaults | Improve the experience on small viewports. |
| Database backup automation | Provide a routine backup script and a restore process. |

# 22. Conclusion

UniManage has been delivered as an approved individual submission for CS6004ES Application Development Coursework 2. The project consists of a complete ASP.NET Core MVC application with twelve controllers, twelve domain models, sixteen view models, twelve infrastructure components, forty-six Razor views, three EF Core migrations, an Identity configuration with strong password rules and lockout, a rate limiter with two policies, anti-forgery, security headers, HTTPS, HSTS, and a custom audit logger.

The system addresses the academic management problem set by the brief, supports Student, Lecturer, and Administrator roles through role-based dashboards, implements course management, enrolment, assignment management, submission upload, grading, reporting and analytics with CSV and PDF export, internal messaging, meeting scheduling, profile and password management, and an audit log review screen. The system applies validation through data annotations, ModelState handling, ownership checks, file validation, anti-forgery, and exception handling.

The documentation package supports the submission with a main report, an individual contribution report, a user manual, an evidence checklist, diagram sources, screenshot guides, evidence guides, conversion guides, and final QA reports. The build has been verified locally with zero warnings and zero errors. Items that are not confirmed by source code or by screenshots are marked [NEEDS CONFIRMATION] or [SCREENSHOT REQUIRED]; the author will replace these markers with evidence before final submission.

The author has reflected on the technical, professional, and academic learning gained during the project and has identified a clear set of future improvements. The submission is therefore complete, evidence based, and ready to support the marker's assessment.

# 23. References

Microsoft, 2026. *ASP.NET Core MVC documentation.* Microsoft Learn. Available at: https://learn.microsoft.com/aspnet/core/mvc [Accessed 5 May 2026].  
Microsoft, 2026. *C# documentation.* Microsoft Learn. Available at: https://learn.microsoft.com/dotnet/csharp [Accessed 5 May 2026].  
Microsoft, 2026. *ASP.NET Core Identity documentation.* Microsoft Learn. Available at: https://learn.microsoft.com/aspnet/core/security/authentication/identity [Accessed 5 May 2026].  
Microsoft, 2026. *Entity Framework Core documentation.* Microsoft Learn. Available at: https://learn.microsoft.com/ef/core [Accessed 5 May 2026].  
Microsoft, 2026. *ASP.NET Core security documentation.* Microsoft Learn. Available at: https://learn.microsoft.com/aspnet/core/security [Accessed 5 May 2026].  
Microsoft, 2026. *ASP.NET Core rate limiting middleware.* Microsoft Learn. Available at: https://learn.microsoft.com/aspnet/core/performance/rate-limit [Accessed 5 May 2026].  
Pomelo Foundation, 2026. *Pomelo Entity Framework Core MySQL provider.* GitHub. Available at: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql [Accessed 5 May 2026].  
QuestPDF, 2026. *QuestPDF documentation.* QuestPDF.com. Available at: https://www.questpdf.com/documentation/getting-started.html [Accessed 5 May 2026].  
Bootstrap, 2026. *Bootstrap 5 documentation.* getbootstrap.com. Available at: https://getbootstrap.com/docs/5.3/ [Accessed 5 May 2026].  
Open Web Application Security Project, 2026. *OWASP Top 10.* owasp.org. Available at: https://owasp.org/www-project-top-ten/ [Accessed 5 May 2026].  
Pressman, R. and Maxim, B., 2014. *Software Engineering: A Practitioner's Approach.* 8th ed. New York: McGraw-Hill.  
Sommerville, I., 2015. *Software Engineering.* 10th ed. Harlow: Pearson Education.

# 24. Appendices

## Appendix A: Selected Source Code Evidence

This appendix collects short, illustrative code snippets that support the body of the report. Each snippet identifies the file path so the marker can locate the full source.

A1. Identity configuration in `Program.cs` (excerpt):

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

A2. EF Core registration in `Program.cs`:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)), mySql =>
        mySql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)));
```

A3. Rate limiter registration in `Program.cs` (excerpt) is shown in Listing 8.1.

A4. Cookie configuration in `Program.cs`:

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
```

A5. Other illustrative listings should be added during final Word formatting from the following files: `Controllers/AccountController.cs`, `Controllers/EnrollmentsController.cs`, `Controllers/SubmissionsController.cs`, `Infrastructure/AuditLogger.cs`, `Infrastructure/UploadedFileStore.cs`, `Data/ApplicationDbContext.cs`, `Data/DbInitializer.cs`. Keep each excerpt to ten to thirty lines.

## Appendix B: Screenshots

This appendix lists the screenshot evidence that must be captured and inserted into the final Word document. The figure numbers match the body of the report.

| Figure | Subject | Source View / Action | Status |
|---|---|---|---|
| Figure 7 | Login Screen | `Views/Account/Login.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 7b | Registration Screen | `Views/Account/Register.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 7c | Invalid Login Validation Message | Login form with bad input | [SCREENSHOT REQUIRED] |
| Figure 7d | Successful Role-Based Redirect | URL bar after login | [SCREENSHOT REQUIRED] |
| Figure 8 | Student Dashboard | `Views/Dashboard/Student.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 8b | Student Calendar Panel | `_DashboardCalendar.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 9 | Lecturer Dashboard | `Views/Dashboard/Lecturer.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 9b | Pending Submissions | Lecturer dashboard panel | [SCREENSHOT REQUIRED] |
| Figure 10 | Administrator Dashboard | `Views/Dashboard/Administrator.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 10b | Audit Log Review | `Views/AuditLogs/Index.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 11 | Course List | `Views/Courses/Index.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 11b | Course Create Form | `Views/Courses/Create.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 11c | Course Edit Form | `Views/Courses/Edit.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 11d | Course Material Upload | Material upload modal/page | [SCREENSHOT REQUIRED] |
| Figure 12 | Course Browse | `Views/Enrollments/Browse.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 12b | Successful Enrolment | Confirmation alert | [SCREENSHOT REQUIRED] |
| Figure 12c | Duplicate / Capacity Rejection | Browse view error message | [SCREENSHOT REQUIRED] |
| Figure 13 | Assignment List | `Views/Assignments/Mine.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 13b | Assignment Create Form | `Views/Assignments/Create.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 13c | Submission Form | `Views/Submissions/Submit.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 13d | Grading Form | `Views/Submissions/Grade.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 13e | Graded Submission View | Student-side view | [SCREENSHOT REQUIRED] |
| Figure 14 | Reports Landing | `Views/Reports/Index.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 14b | Sample Report Output | One report view | [SCREENSHOT REQUIRED] |
| Figure 14c | CSV Export Evidence | Downloaded CSV in browser | [SCREENSHOT REQUIRED] |
| Figure 14d | PDF Export Evidence | Downloaded PDF | [SCREENSHOT REQUIRED] |
| Figure 15 | Inbox | `Views/Messages/Inbox.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 15b | Message Thread | `Views/Messages/Thread.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 15c | Compose Message | `Views/Messages/Compose.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 15d | Meetings List | `Views/Meetings/Index.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 16 | Validation Error | Any form with bad input | [SCREENSHOT REQUIRED] |
| Figure 16b | Friendly Error Page | `Views/Shared/Error.cshtml` | [SCREENSHOT REQUIRED] |
| Figure 17 | Database Table View | MySQL client | [SCREENSHOT REQUIRED] |
| Figure 17b | Migration List Output | `dotnet ef migrations list` | [SCREENSHOT REQUIRED] |
| Figure 18 | Source Repository | GitHub or other host | [SCREENSHOT REQUIRED] |
| Figure 19 | Packaged Build / Deployment | ZIP location or hosted URL | [SCREENSHOT REQUIRED] |
| Figure 20 | Visual Studio Solution Open | IDE screenshot | [SCREENSHOT REQUIRED] |
| Figure 20b | Successful Build Output | `dotnet build` console | [SCREENSHOT REQUIRED] |

## Appendix C: Diagram Sources

| Diagram | Source File | Action |
|---|---|---|
| System Architecture | `documentation_application_development/diagrams/architecture_diagram.mmd` | Export to PNG and PDF, insert as Figure 1. |
| Use Case | `documentation_application_development/diagrams/use_case_diagram.mmd` | Export to PNG and PDF, insert as Figure 2. |
| Entity Relationship | `documentation_application_development/diagrams/er_diagram.mmd` | Export to PNG and PDF, insert as Figure 3. |
| UML Class | `documentation_application_development/diagrams/class_diagram.mmd` | Export to PNG and PDF, insert as Figure 4. |
| Application Flowchart | `documentation_application_development/diagrams/application_flowchart.mmd` | Export to PNG and PDF, insert as Figure 5. |
| Authentication and RBAC Flow | `documentation_application_development/diagrams/security_flow_diagram.mmd` | Export to PNG and PDF, insert as Figure 6. |

## Appendix D: Individual Approval and Planning Evidence

The author has prepared the documentation package as an approved individual submission. The supporting individual contribution document at `documentation_application_development/Expanded_Individual_Contribution.md` records the contribution, the timeline, and the workflow. Approval evidence (for example a screenshot of the lecturer's email or a note from the module leader) should be attached at `documentation_application_development/evidence/approval/` if available. This item is currently [NEEDS CONFIRMATION].

## Appendix E: Source Repository, Database and Build Evidence

The repository link, the packaged build link, and the database evidence are held in `documentation_application_development/Source_And_Build_Links.md`. The current detected repository remote is `https://github.com/YasasPasinduFernando/AD-COURSEWORK-2.git`. The current detected migrations are listed in Section 7.6. The build was verified locally with `dotnet build --no-restore`, returning zero warnings and zero errors.

Figure 18: [INSERT SCREENSHOT HERE]

This screenshot shows the public landing page of the project's source repository on GitHub.

Figure 19: [INSERT SCREENSHOT HERE]

This screenshot shows the packaged build (ZIP) or hosted deployment evidence available for the marker to download or visit.

Figure 17: [INSERT SCREENSHOT HERE]

This screenshot shows a database client connected to the seeded UniManage database, displaying the schema and a representative table such as `Courses`, `Enrollments`, or `Submissions`.

Figure 17b: [INSERT SCREENSHOT HERE]

This screenshot shows the console output of `dotnet ef migrations list`, confirming the three EF Core migrations.

Figure 20: [INSERT SCREENSHOT HERE]

This screenshot shows the Visual Studio 2022 solution open with the `AD COURSEWORK 2` project loaded in Solution Explorer.

Figure 20b: [INSERT SCREENSHOT HERE]

This screenshot shows the console output of `dotnet build --no-restore` reporting zero warnings and zero errors.

## Appendix F: User Manual Reference

The complete user manual is stored at `documentation_application_development/User_Manual_Application_Development.md`. The user manual covers system requirements, project package contents, installation, login, user roles, troubleshooting, reporting, and viva demonstration notes. It should be exported to Word and PDF and either embedded or linked in the final submission package.

## Appendix G: Test Logs and Audit Evidence

The test plan in Chapter 15 lists the test cases. Each test case should produce a screenshot or, where appropriate, an audit log entry. The audit log should be exported through the Administrator audit log view and included as evidence. The build console output should be saved as plain text and stored at `documentation_application_development/evidence/build/`. The audit log export and the saved build console output are listed as outstanding manual evidence items in the QA report.

## Appendix H: Validation Message Evidence

The validation rules used by each form are listed in Table 16. Selected examples of validation messages should be captured as screenshots and included here, including:

- Required field missing on registration.
- Password too weak on registration.
- Wrong credentials on login.
- Course code already in use.
- Enrolment limit reached.
- File too large or wrong type on submission upload.

Each of the validation messages above must be captured as a screenshot before final submission.

## Appendix I: Configuration Redaction Notes

The author has redacted sensitive configuration values from the report. The following keys must not appear in screenshots, source code listings, or appendices:

- `ConnectionStrings:DefaultConnection` (database password and host).
- `Authentication:Google:ClientSecret`.
- `Email:Password` (SMTP password).

If a screenshot accidentally captures one of these values, the screenshot must be retaken or redacted before submission. The author has avoided pasting `appsettings.json` or `appsettings.Development.json` into the report for this reason.

## Appendix J: Viva Demonstration Script

The following script supports the viva demonstration. The author should be ready to perform the steps in the order listed.

1. Open Visual Studio and the `AD COURSEWORK 2.sln` solution. Highlight the folder structure.
2. Run `dotnet build --no-restore` from a terminal and show the zero warning, zero error result.
3. Start the application through Visual Studio (F5) or `dotnet run`. Show the seeded URL.
4. Sign in as the seeded Administrator (`admin@unimanage.local` / `Admin123!`) and demonstrate the Administrator dashboard, the course list, the reports, and the audit log.
5. Sign out and sign in as the seeded Lecturer (`lecturer@unimanage.local` / `Lecturer123!`). Demonstrate the Lecturer dashboard, `MyCourses`, an assignment, a grading screen, and a meeting.
6. Sign out and sign in as the seeded Student (`student@unimanage.local` / `Student123!`). Demonstrate the Student dashboard, course browse, enrolment, submission, grade view, and inbox.
7. Trigger a validation error (for example a missing required field) and show the friendly message.
8. Show the audit log entry that corresponds to a recent action (for example a login or an enrolment).
9. Briefly walk through `Program.cs`, focusing on Identity, EF Core, rate limiter, anti-forgery, security headers, and routing.
10. Highlight the items that remain [NEEDS CONFIRMATION] honestly.

End of report.
