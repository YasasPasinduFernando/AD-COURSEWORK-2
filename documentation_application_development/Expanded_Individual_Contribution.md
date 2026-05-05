# Expanded Individual Contribution

Coursework: CS6004ES Application Development Coursework 2  
Case study: UniManage - University Course Management System  
Author: [NEEDS CONFIRMATION]  
Student ID: [NEEDS CONFIRMATION]  
Group: [NEEDS CONFIRMATION]

## Purpose

This document provides a structured draft for the individual contribution section. The repository scan can identify implemented features, but it cannot prove which team member completed each part. Therefore, all personal ownership claims must be confirmed by the author and supported by evidence such as commits, task records, screenshots, issue notes or dated file history.

## Contribution Summary

The author contributed to the design, implementation, testing and documentation of the UniManage application. The exact feature ownership is currently `[NEEDS CONFIRMATION]`. The final version should retain only the work that the author personally completed or significantly supported.

## Evidence-Supported Application Areas

The current repository contains implementation evidence for the following areas:

| Area | Repository evidence | Author ownership |
|---|---|---|
| ASP.NET Core MVC solution setup | `.sln`, `.csproj`, `Program.cs` | [NEEDS CONFIRMATION] |
| Identity authentication and roles | `AccountController.cs`, `AppRoles.cs`, `ApplicationUser.cs`, `DbInitializer.cs` | [NEEDS CONFIRMATION] |
| Course management | `CoursesController.cs`, Course views and models | [NEEDS CONFIRMATION] |
| Enrolment | `EnrollmentsController.cs`, `Enrollment.cs`, Browse view | [NEEDS CONFIRMATION] |
| Assignment management | `AssignmentsController.cs`, Assignment views and view models | [NEEDS CONFIRMATION] |
| Submission and grading | `SubmissionsController.cs`, `Submission.cs`, grading view | [NEEDS CONFIRMATION] |
| Reporting and exports | `ReportsController.cs`, `CsvWriter.cs`, `PdfReport.cs` | [NEEDS CONFIRMATION] |
| Messaging | `MessagesController.cs`, `Message.cs`, message views | [NEEDS CONFIRMATION] |
| Meeting scheduling | `MeetingsController.cs`, `Meeting.cs`, calendar helpers | [NEEDS CONFIRMATION] |
| Audit logging and security | `AuditLogger.cs`, `AuditLogsController.cs`, `SecurityHeadersMiddleware.cs` | [NEEDS CONFIRMATION] |
| UI styling | `Views/Shared/_Layout.cshtml`, `wwwroot/css/site.css`, static assets | [NEEDS CONFIRMATION] |
| Documentation | `documentation_application_development/` files | [NEEDS CONFIRMATION] |

## Suggested Individual Contribution Narrative

The following draft should be edited after confirming the author's actual work:

The author was responsible for `[NEEDS CONFIRMATION]` within the UniManage ASP.NET Core MVC application. This contribution focused on translating the UCMS case study into working MVC components, database-backed entities and role-specific user workflows. The author worked with the Visual Studio solution structure and followed the existing pattern of controllers, view models, Razor views and Entity Framework Core data access.

For implementation, the author contributed to `[NEEDS CONFIRMATION]`. The work included designing or updating relevant models, controller actions, validation rules and user interface pages. The implementation used ASP.NET Core Identity to support role-based access and Entity Framework Core to persist the system data in MySQL. Where appropriate, the author also considered validation, exception handling and user feedback through model annotations, `ModelState` checks and status messages.

For testing and evidence preparation, the author contributed to `[NEEDS CONFIRMATION]`. The project was built successfully using `dotnet build --no-restore`, and the documentation pack identifies the screenshots and appendices still required for final submission. Any final claim about testing should be supported by screenshots, build logs or a test table.

## Technical Skills Demonstrated

The application evidence supports the following technical skill areas:

- ASP.NET Core MVC project structure.
- C# controllers, models and view models.
- Razor view development.
- Entity Framework Core relationship configuration.
- ASP.NET Core Identity and role-based authorisation.
- Input validation and anti-forgery protection.
- File upload validation and controlled file access.
- CSV and PDF export generation.
- Email notification integration.
- Academic technical documentation.

The author's personal use of each skill remains `[NEEDS CONFIRMATION]`.

## Contribution Evidence To Attach

The author should attach or reference:

1. Git commit history filtered by the author's account. [NEEDS CONFIRMATION]
2. Screenshots of completed feature pages. [NEEDS CONFIRMATION]
3. Build output evidence. Completed: `dotnet build --no-restore` succeeded on 5 May 2026.
4. Code snippets or file references showing personally completed features. [NEEDS CONFIRMATION]
5. Meeting notes, task allocation notes or issue tracker records. [NEEDS CONFIRMATION]
6. Testing notes for features implemented by the author. [NEEDS CONFIRMATION]
7. Final report sections written or edited by the author. [NEEDS CONFIRMATION]

## Neutral Group Work Statement

The final report may include a neutral statement such as:

The author contributed to the application within the agreed group scope and focused on the areas evidenced in the submitted artefacts. Any incomplete or unclear tasks are recorded as confirmation items so that the final submission remains accurate and evidence-based.

