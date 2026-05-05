# Supporting Document: Individual Contribution Report

Student Name: Yasas Pasindu Fernando  
Student ID: 25026764  
Module: CS6004ES Application Development  
Assessment: Coursework 2  
Project: UniManage - University Course Management System (UCMS)  
Centre: ESOFT Metro Campus / London Metropolitan University  
Submission Type: Approved individual submission  
Companion Document: `Main_Application_Development_Report.md`

---

## 1. Introduction

This supporting document records the author's individual contribution to the final UniManage coursework submission. It complements the main technical report by focusing specifically on the work that the author has performed, the decisions taken, the evidence gathered, and the personal learning experienced during the project. The document is written as an approved individual submission and uses neutral, evidence-based wording throughout.

The contribution report has been organised so that the marker can move quickly from one academic area to the next. Each numbered section addresses a specific area of the project. Two consolidated tables, Table IC1 and Table IC2, summarise the contribution and the supporting evidence so that the marker has both a high-level and a section-by-section view.

The report avoids any group blame, group conflict, or comparative wording. It does not include a group contribution table or a work-splitting table. It focuses on the work that the author has produced and on how that work has been verified through source code, build evidence, and prepared screenshots.

## 2. Project Context

UniManage is a University Course Management System implemented as an ASP.NET Core MVC web application targeting .NET 8. It uses C# for application logic, Razor views and Bootstrap for the user interface, Entity Framework Core for data access through the Pomelo MySQL provider, and ASP.NET Core Identity for authentication and roles. The system supports academic workflows for Student, Lecturer, and Administrator users, including dashboards, course management, enrolment, assignments, submissions, grading, reporting, internal messaging, meeting scheduling, profile management, and audit logging.

The CS6004ES Application Development brief sets a structured list of marking items that includes the application user interface, registration and login, role dashboards, course management, the enrolment system, assignment and grading, reporting and analytics, the communication module, security and data protection, validation and exception handling, programming style, the installation guide and user manual, the concise logical solution, the architecture diagrams, the detailed description of classes and methods, and the personal reflection. The author has aligned the implementation and the documentation with these items in a way that is intended to be straightforward to mark.

The technical baseline is recorded in the main report. Twelve controllers, twelve domain models, sixteen view models, twelve infrastructure components, forty-six Razor views, three Entity Framework Core migrations, and a `Program.cs` startup file have been confirmed by source-code review. The build has been verified locally with `dotnet build --no-restore`, returning zero warnings and zero errors.

## 3. Approved Individual Submission Context

The CS6004ES Application Development Coursework 2 was originally set as group work. Following approval to complete the coursework individually, the author took responsibility for the final implementation, testing, documentation, and submission preparation. The author refers to themselves as "the author" throughout this contribution report. The author does not name other students, does not describe any conflict or disagreement, and does not include any work-splitting table.

The approved individual submission context has shaped the structure of this contribution report in two ways. First, every academic area is described as the author's own work because that is what has actually been delivered and verified. Second, the report makes a clear distinction between work that is provable from the source code and work that requires manual evidence such as screenshots and exported diagrams. Items that cannot be confirmed at the time of writing are honestly marked [VERIFY] or [NEEDS CONFIRMATION].

## 4. Personal Motivation and Project Selection

The author chose to complete UniManage as the coursework project for several reasons. First, a university course management system is a realistic academic problem that combines authentication, role-based access, structured database relationships, validation, reporting, and user interface design in a single application. Second, the problem aligns naturally with the marking items in the brief, which makes it easier to demonstrate coverage of each marking area through source-code evidence. Third, the technologies selected for the project, namely ASP.NET Core MVC, C#, Razor, Entity Framework Core, MySQL, and ASP.NET Core Identity, are widely used in industry and are therefore valuable to the author's professional development beyond the coursework.

Personal motivation also played a part. The author has been interested in modern .NET web development since the start of the module and chose UniManage because it allowed the author to apply that interest to a focused academic problem. The approved individual completion route reinforced the author's commitment to deliver a complete, evidence-based submission rather than a partial implementation.

## 5. Summary of Main Contribution

The author's contribution covers the full life cycle of the project. This includes the requirement analysis, the architectural design, the database design, the implementation of every feature, the validation and security pipeline, the user interface, the testing, the evidence preparation, and the documentation. Table IC1 summarises the contribution by area, with cross-references to the relevant figures in the main report and a status flag for marker convenience.

Table IC1: Summary of Individual Contribution

| Area | Contribution Summary | Evidence / Figure Reference | Status |
|---|---|---|---|
| Requirement analysis | Mapped 30 functional requirements (FR1 to FR30) and 10 non-functional requirements (NFR1 to NFR10) to source-code evidence and to marking items. | Tables 1, 2, 7, 11, 14, 15 in the main report | Confirmed |
| Architecture and system design | Prepared six diagrams (architecture, use case, ERD, UML class, application flowchart, authentication flow) as Mermaid sources and documented design decisions. | Figures 1 to 6, Section 6 of the main report, Appendix C | Sources prepared, exports pending |
| Database design | Documented EF Core configuration, nine academic entities, relationships, unique indexes, and three migrations. | Figure 3, Table 3, Section 7 of the main report | Confirmed |
| User registration and login | Documented `AccountController` with Register, Login, Logout, ForgotPassword, ResetPassword, and optional Google login support. | Figures 7, 7b, 7c, 7d, Section 8.3 of the main report | Confirmed; screenshots pending |
| Role-based access control | Documented `AppRoles`, role seeding through `DbInitializer`, `[Authorize]` attributes, and ownership filters. | Section 8.4 of the main report, Table 17 | Confirmed |
| Student dashboard | Documented `DashboardController.Student`, `StudentDashboardViewModel`, and `Views/Dashboard/Student.cshtml`. | Figures 8, 8b, Section 8.5 of the main report | Confirmed; screenshot pending |
| Lecturer dashboard | Documented `DashboardController.Lecturer`, `LecturerDashboardViewModel`, and `Views/Dashboard/Lecturer.cshtml`. | Figures 9, 9b, Section 8.6 of the main report | Confirmed; screenshot pending |
| Administrator dashboard | Documented `DashboardController.Administrator`, `AdminDashboardViewModel`, and `Views/Dashboard/Administrator.cshtml`. | Figures 10, 10b, Section 8.7 of the main report | Confirmed; user-management screen [NEEDS CONFIRMATION] |
| Course management | Documented `CoursesController` with Index, MyCourses, Create, Edit, Delete, UploadMaterial, search, paging, and ownership filters. | Figures 11, 11b, 11c, 11d, Section 8.8 of the main report | Confirmed; screenshots pending |
| Enrolment system | Documented `EnrollmentsController` with Browse and Enroll actions, capacity, duplicate, and prerequisite checks. | Figures 12, 12b, 12c, Section 8.9 of the main report | Confirmed; screenshots pending |
| Assignment, submission and grading | Documented `AssignmentsController`, `SubmissionsController`, ownership checks, file validation, grade range, feedback, and graded lock. | Figures 13, 13b, 13c, 13d, 13e, Sections 8.10 and 8.11 of the main report | Confirmed; screenshots pending |
| Reporting and analytics | Documented `ReportsController` with six reports plus CSV and PDF export through QuestPDF. | Figures 14, 14b, 14c, 14d, Section 8.12 of the main report | Confirmed; export evidence pending |
| Communication and meetings | Documented `MessagesController`, allowed-recipient validation, `MeetingsController`, ICS export. | Figures 15, 15b, 15c, 15d, Sections 8.13 and 8.14 of the main report | Confirmed; screenshots pending |
| Security, validation and error handling | Documented Identity options, lockout, anti-forgery, rate limiter, security headers middleware, exception handling, validation rules. | Figures 16, 16b, Sections 12 and 13 of the main report, Tables 16, 17 | Confirmed; validation screenshot pending |
| UI/UX and usability | Documented layout, alerts partial, role-aware navigation, validation feedback, responsive design, accessibility considerations. | Section 9 of the main report | Confirmed; layout screenshots pending |
| Testing and debugging | Prepared test plan with 32 cases (T1 to T32), build verification, manual functional, validation, role, and security testing. | Section 15 of the main report, Table 8 | Build confirmed; remaining cases pending screenshots |
| Documentation and user manual | Prepared the main report (24,000+ words), this contribution report, the user manual, evidence checklists, source/build links, diagram README, screenshot README, evidence README, conversion guide, and final QA reports. | `documentation_application_development/` folder | Confirmed |
| Evidence preparation and viva readiness | Built screenshot, diagram, source, build, and database evidence checklists; prepared a viva demonstration script. | Appendix B, Appendix J of the main report | Sources prepared; manual capture pending |

## 6. Contribution to Requirement Analysis

The author derived the requirements from three sources. The first source is the coursework brief, which sets the marking items for CS6004ES Application Development Coursework 2. The second source is a structured review of the source-code repository, which confirms what is actually implemented. The third source is the case-study analysis of an academic course management problem, which identifies the practical needs of Students, Lecturers, and Administrators.

The author produced thirty functional requirements (FR1 to FR30) covering registration, login, lockout, role-based redirection, dashboards, course management with paging and search, lecturer ownership, course material upload, enrolment with capacity, duplicate, and prerequisite checks, assignment management, submission upload, grading with feedback, secure file download, six reports, CSV and PDF export, internal messaging, allowed-recipient validation, meeting scheduling, ICS export, profile and password management, audit logging, friendly error pages, rate limiting, and anti-forgery protection. The author also produced ten non-functional requirements (NFR1 to NFR10) covering usability, performance, security, maintainability, reliability, data integrity, accessibility, auditability, configurability, and deployability.

The author then mapped the requirements onto the marking items through Table 7 (Coursework Marking Alignment) and Table 15 (Requirement Traceability Summary) in the main report. This traceability is intended to make the marker's job easier, because every marking item can be traced to specific requirements, source files, and test cases.

## 7. Contribution to Architecture and System Design

The author prepared the system architecture, the use case model, the entity relationship diagram, the UML class diagram, the application flowchart, and the authentication and role-based access flow. Each diagram is stored as a Mermaid source file in `documentation_application_development/diagrams/`. The author produced the diagrams from the actual source code rather than from an abstract design, which keeps the diagrams faithful to the implementation.

The architectural narrative explains how the request flows through the middleware pipeline (HTTPS redirection, static files, security headers, routing, rate limiting, authentication, authorisation), through the controller, through Entity Framework Core, and finally to the MySQL database. The narrative also identifies cross-cutting concerns such as audit logging, validation, and exception handling. Design decisions are documented in Section 6.7 of the main report, including the choice of MVC over Razor Pages, the choice of Pomelo MySQL over SQL Server / LocalDB, the choice of a single `DashboardController` rather than three role-specific controllers, the decision to store grade fields directly in `Submission` rather than in a separate `Grade` entity, the decision to add a custom security headers middleware, and the choice of QuestPDF for PDF reports.

## 8. Contribution to Database Design

The author documented the database design from `Data/ApplicationDbContext.cs`, the model classes in `Models/`, and the three EF Core migrations under `Data/Migrations/`. The contribution includes a clear list of nine academic entities (Table 3), a relationship narrative covering the lecturer-course link, the optional self-reference for prerequisites, the unique student-course enrolment, the assignment-course relationship, the submission-assignment-student-grader relationship, the message sender and receiver relationships, the meeting-course-lecturer relationships, and the optional audit log user reference.

The author also documented the constraints used in the database, including the unique index on `Course.Code`, the unique index on `Enrollment(StudentId, CourseId)`, the maximum-length attributes on string fields, the required attributes on key fields, and the range constraints on numeric fields such as `MaxPoints` on assignments and the grade on submissions. The seeding logic in `DbInitializer` is documented because it provides the reproducible demonstration data needed for the viva.

The author has clearly recorded that UniManage uses MySQL through the Pomelo provider rather than SQL Server / LocalDB. Where the brief refers to SQL Server / LocalDB, the author has documented the alternative approach in honest terms instead of overclaiming.

## 9. Contribution to User Registration and Login

The author documented the registration, login, logout, password reset, and optional Google login flows implemented in `Controllers/AccountController.cs`. The contribution links each action to its view, view model, and Identity API. The author has also documented the security configuration in `Program.cs` that supports the flow, including the strong password rules (minimum eight characters, at least one digit, at least one uppercase letter, at least one lowercase letter), the unique email constraint, the lockout policy (five failed attempts, fifteen minute lockout), and the application cookie configuration (HTTP-only, `SameSiteMode.Lax`, secure when the request is secure, sliding expiration of eight hours).

The author has identified the audit log entries written by the registration and login flows. Successful registrations write a positive audit entry, failed registrations record the validation failure, successful logins write a positive audit entry, and failed logins write a negative audit entry. This combination supports the marker's review by providing a concrete record of who registered or signed in, when, and whether the action succeeded.

## 10. Contribution to Role-Based Access Control

The author documented the three-layer approach to role-based access control. First, `Models/AppRoles.cs` defines the role name constants (Administrator, Lecturer, Student). Second, `Data/DbInitializer.cs` seeds the three roles and creates the three demonstration users. Third, controller actions use `[Authorize(Roles = AppRoles.X)]` to restrict access to the appropriate role. Where the role check is not enough, controller actions add an ownership filter through `LecturerId == userId` or `StudentId == userId` so that a lecturer cannot see another lecturer's data and a student cannot see another student's data.

The author has also documented the `Views/Account/AccessDenied.cshtml` page, which presents a friendly response when a user tries to open an action they are not authorised to use. The combination of role attributes, ownership filters, and the access denied page provides defence in depth at the application layer.

## 11. Contribution to Student Dashboard

The author documented the student dashboard implementation through `DashboardController.Student`, the `StudentDashboardViewModel` defined in `ViewModels/DashboardViewModels.cs`, and `Views/Dashboard/Student.cshtml`. The dashboard view model exposes enrolled courses, upcoming deadlines, recent grades, recent messages, calendar entries, and meetings. EF Core read queries use `AsNoTracking` where possible to reduce the change tracker overhead, and queries are scoped to the signed-in student's user identifier so that no cross-account data is exposed.

The author has identified the supporting partial `Views/Shared/_DashboardCalendar.cshtml`, which renders a small calendar block reused by the dashboards. This partial is one example of the maintainability practices documented in Section 14 of the main report.

## 12. Contribution to Lecturer Dashboard

The author documented the lecturer dashboard implementation through `DashboardController.Lecturer`, the `LecturerDashboardViewModel`, and `Views/Dashboard/Lecturer.cshtml`. The lecturer dashboard view model exposes the courses owned by the signed-in lecturer, enrolment counts, assignment counts, pending submissions, recently graded submissions, recent messages, recent meetings, and recent audit activity relevant to the lecturer's work. The author has identified that the dashboard uses ownership filters to ensure that the lecturer only sees their own data and is therefore consistent with the role-based access control approach documented in Section 10 of this contribution report.

## 13. Contribution to Administrator Dashboard

The author documented the administrator dashboard implementation through `DashboardController.Administrator`, the `AdminDashboardViewModel`, and `Views/Dashboard/Administrator.cshtml`. The administrator dashboard exposes counts for users, courses, enrolments, assignments, submissions, and messages, identifies popular courses by enrolment count, and presents recent audit log entries. Where a dedicated user-management screen is referenced in the brief, the author has not confirmed one in the source code; this is recorded as [NEEDS CONFIRMATION] in the main report and in this contribution report so that the marker has an honest record.

## 14. Contribution to Course Management

The author documented the course management implementation through `Controllers/CoursesController.cs`, the `Course` entity, the `CourseInputViewModel`, the `MaterialUploadViewModel`, and the `Views/Courses/` folder. The actions documented include `Index` with search and paging, `Details`, `Create`, `Edit`, `Delete`, `MyCourses`, and `UploadMaterial`. The author has documented the unique course code constraint at the database level, the prerequisite handling, the lecturer assignment, the capacity limit, and the file upload pipeline through `Infrastructure/UploadedFileStore.cs`.

The author has also documented that the upload action is gated by the `upload` rate-limiting policy configured in `Program.cs`, which permits up to thirty uploads per minute per user. This protects the file store against abuse without preventing reasonable academic use.

## 15. Contribution to Enrolment System

The author documented the enrolment workflow in `Controllers/EnrollmentsController.cs`, including the `Browse` action that lists eligible courses for the signed-in student and the `Enroll` action that creates an enrolment record after the business rules have been checked. The business rules are: the course exists, the course is open (capacity has not been reached), the student has not already enrolled, and any prerequisite has been satisfied. The combination of the unique index on `Enrollment(StudentId, CourseId)` and the application-level duplicate check prevents duplicates by defence in depth.

The author has produced eligibility logic that uses a single composed LINQ query rather than multiple queries to keep the database round-trip count low. The author has also documented the audit log entries that record successful and unsuccessful enrolment attempts.

## 16. Contribution to Assignment, Submission and Grading

The author documented the assignment lifecycle in `Controllers/AssignmentsController.cs`, including `ForCourse`, `Mine`, `Create`, `Edit`, and `Delete`, with ownership checks that ensure a lecturer can only manage assignments for their own courses. The submission lifecycle is documented in `Controllers/SubmissionsController.cs`, including `Submit`, `Grade`, `ForAssignment`, `CourseMaterialFile`, and `SubmissionFile`. The author has documented the file validation through `Infrastructure/UploadedFileStore.cs`, including extension allow-listing, size limits, and stored file naming to prevent path traversal.

The grading workflow is documented through `GradeSubmissionViewModel`, the score range validation, the feedback length validation, and the graded lock that prevents a student from modifying a submission after it has been graded. Each grading action writes an audit log entry that records the lecturer, the submission, and the grade.

## 17. Contribution to Reporting and Analytics

The author documented the reporting suite in `Controllers/ReportsController.cs`, including six reports: course popularity, student performance, lecturer workload, enrolments, pass and fail, and assignment attendance. Each report uses an EF Core query with `GroupBy` and aggregate functions and projects into a report view model. CSV export is provided through `Infrastructure/CsvWriter.cs`, which writes properly escaped CSV content. PDF export is provided through `Infrastructure/PdfReport.cs`, which uses QuestPDF to render the report layout. The author has identified that the QuestPDF licence type is set to Community in `Program.cs`, which keeps the project within the QuestPDF licensing terms.

## 18. Contribution to Communication and Meetings

The author documented the messaging implementation in `Controllers/MessagesController.cs`, including `Inbox`, `Thread`, `Reply`, `Compose`, and `MarkAllRead`. The allowed-recipient rule is documented: a Student may message a Lecturer who teaches a course on which the Student is enrolled, and a Lecturer may message a Student enrolled on an owned course. The author has identified that this rule is implemented at the database level through a join during the recipient look-up, so the user interface does not offer an unauthorised recipient.

The author has also documented the meeting implementation in `Controllers/MeetingsController.cs`, including `Index`, `Create`, `Edit`, `Delete`, `Join`, and `Ics`. The ICS export is provided through `Infrastructure/CalendarLink.cs`, which produces an iCalendar file for import into a personal calendar tool. The meeting URL is stored as part of the `Meeting` entity rather than being provisioned by the application; this is consistent with the documented scope.

## 19. Contribution to Security, Validation and Error Handling

The author documented the security pipeline in detail. Identity uses strong password rules and lockout. Anti-forgery is enabled globally through `AddAntiforgery` in `Program.cs`, with the cookie configured as HTTP-only, `SameSiteMode.Strict`, and secure when the request is secure. Rate limiting is configured with two named policies, `auth` (ten requests per minute per IP) and `upload` (thirty requests per minute per user). The custom `Infrastructure/SecurityHeadersMiddleware.cs` adds standard headers to every response. HTTPS redirection and HSTS are enabled. The application cookie is HTTP-only with `SameSiteMode.Lax` and secure when the request is secure.

Validation is documented through data annotations on view models, `ModelState` checks in controller actions, server-side ownership checks where role membership is not enough, and file upload validation through `UploadedFileStore`. Error handling is documented through the production exception handler `app.UseExceptionHandler("/Home/Error")`, the developer exception page in development, and the friendly `Views/Shared/Error.cshtml` page. The author has also documented the audit logger as a key part of the error and event-tracking story.

## 20. Contribution to UI/UX and Usability

The author documented the user interface design through `Views/Shared/_Layout.cshtml`, `Views/Shared/_Alerts.cshtml`, `Views/Shared/_DashboardCalendar.cshtml`, the role-specific dashboard views, the form views, and the static assets in `wwwroot/css/site.css` and `wwwroot/js/site.js`. The author has documented the role-aware navigation that adapts the menu items to the signed-in user's role, the alert partial that displays TempData feedback, the responsive Bootstrap grid that supports narrower viewports, the skip link near the top of the layout that helps keyboard users, and the label association used throughout the form views to support assistive technology. A formal WCAG 2.1 AA audit has not been carried out and is recorded as [VERIFY].

## 21. Contribution to Testing and Debugging

The author prepared a manual testing strategy with thirty-two test cases (T1 to T32) recorded in Section 15.9 of the main report. The cases cover registration, login, lockout, logout, password reset, role dashboards, role-aware navigation, course list, course create, course code uniqueness, course delete, enrolment success and rejection, assignment create, ownership rejection, submission upload, grading, reporting, CSV export, messaging compose, messaging restriction, anti-forgery, rate limit, access denied, validation message, friendly error, responsive layout, and build verification. The build verification (T32) is confirmed by `dotnet build --no-restore` returning zero warnings and zero errors. The remaining cases require screenshots to be captured during the final preparation week.

Debugging during development included resolving transient MySQL connection errors at startup by enabling EF Core retry on failure, splitting a single rate limiter policy into two named policies (`auth` and `upload`) to avoid spurious 429 responses on dashboard pages, ordering the migrations so that the audit table exists before any audit calls run, and wrapping the audit write in a try/catch block so that an audit failure cannot crash the calling action. Each problem and its resolution is documented in Section 17.1 of the main report.

## 22. Contribution to Documentation and User Manual

The author prepared the documentation package at `documentation_application_development/`. The package contains the main technical report at `Main_Application_Development_Report.md`, this contribution report at `Expanded_Individual_Contribution.md`, the installation guide and user manual at `User_Manual_Application_Development.md`, the evidence checklist at `Evidence_Checklist.md`, the source and build links file at `Source_And_Build_Links.md`, the diagram readme at `diagrams/README_DIAGRAMS.md`, the screenshot readme at `screenshots/README_SCREENSHOTS.md`, the evidence readme at `evidence/README_EVIDENCE.md`, the Word formatting guide at `conversion/WORD_FORMATTING_GUIDE.txt`, the Word conversion script at `conversion/convert_to_docx.py`, and a number of final QA artefacts under `final_document/`, including the documentation expansion plan, the lecturer marking alignment report, the manual final checklist, the individual wording cleanup report, and the final QA report.

The author has applied a consistent style across these documents, namely UK English spelling, "the author" voice in the body of the technical report, first person voice limited to the reflection chapter, no em dashes, no strange symbols, and no group blame.

## 23. Contribution to Evidence Preparation and Viva Readiness

The author prepared the evidence package by listing every screenshot, every diagram export, every source-code listing, every database evidence item, every build evidence item, and every viva preparation item. The list is held in three places. First, the main report's Appendix B lists thirty-eight screenshot placeholders with the corresponding view file. Second, the main report's Appendix C lists six diagram exports. Third, the evidence checklist tracks the status of each item.

The author has also prepared the viva demonstration script in Appendix J of the main report. The script begins with the solution open in Visual Studio, includes a build verification, demonstrates each role using the seeded demonstration users, triggers a validation error to show the friendly response, presents a corresponding audit log entry, and ends with a summary of items that remain [NEEDS CONFIRMATION] honestly.

The seeded demonstration users that support the viva are `admin@unimanage.local` (Administrator), `lecturer@unimanage.local` (Lecturer), and `student@unimanage.local` (Student). The seeded passwords are documented in `Data/DbInitializer.cs` and in the user manual.

## 24. Challenges, Learning and Final Preparation

The author faced a small set of recurrent challenges during the project. Aligning the source-code review with the coursework wording required care because the project uses Pomelo MySQL rather than SQL Server / LocalDB. The author resolved this by recording the implementation honestly and by explaining the alternative approach in the main report. Capturing evidence in a marker-friendly way required discipline because screenshots need to be redacted and named consistently. The author resolved this by preparing a screenshot README and by listing the figures in Appendix B. Maintaining a consistent voice across the documents required attention because the report mixes a third-person body with a first-person reflection. The author resolved this by reviewing each chapter against the wording rules in the documentation expansion plan.

The learning from this project has been substantial. On the technical side, the author has gained experience with MVC routing, Razor views, EF Core with the Pomelo provider, Identity, anti-forgery, rate limiting, custom middleware, dependency injection, asynchronous programming, and PDF generation through QuestPDF. On the professional side, the author has gained experience with evidence-based academic writing, traceability between requirements and implementation, and the discipline of marking unverifiable claims rather than embellishing them. On the personal side, the author has gained confidence in completing a substantial individual delivery within an academic deadline.

The final preparation stage focuses on capturing screenshots, exporting Mermaid diagrams, completing the title page placeholders, completing the references, pasting the final repository and packaged build links, and converting the report into Word and PDF for upload.

## 25. Supporting Evidence

Table IC2 lists the evidence prepared for assessment, the purpose of each item, the location, and the manual action that is still required.

Table IC2: Evidence Prepared for Assessment

| Evidence Type | Purpose | Location | Manual Action Needed |
|---|---|---|---|
| Source code | Confirms implemented features and supports source-code traceability | Project root, including `Controllers/`, `Models/`, `ViewModels/`, `Views/`, `Data/`, `Infrastructure/`, `wwwroot/`, `Program.cs`, `AD COURSEWORK 2.sln`, `AD COURSEWORK 2.csproj` | None for source itself; ensure final commit before submission |
| Build verification | Confirms that the project compiles cleanly | `dotnet build --no-restore` console output (zero warnings, zero errors) | Capture build console as plain text into `documentation_application_development/evidence/build/` |
| Architecture and design diagrams | Supports Section 6 of the main report | `documentation_application_development/diagrams/*.mmd` | Export each diagram to PNG and PDF, save into `documentation_application_development/diagrams/exported/` |
| Screenshots | Supports figures referenced from the main report and the user manual | `documentation_application_development/screenshots/` | Capture every screenshot listed in Appendix B of the main report and Section 7-9 of the user manual, redact personal data |
| Evidence checklist | Tracks the status of every required evidence item | `documentation_application_development/Evidence_Checklist.md` | Update each row as evidence is captured |
| Source and build links | Records repository, build, and database evidence URLs | `documentation_application_development/Source_And_Build_Links.md` | Paste final repository link, packaged build link, and database evidence link |
| User manual | Supports the installation guide and user manual marking item | `documentation_application_development/User_Manual_Application_Development.md` | Convert to Word and PDF; insert UM screenshots |
| Evidence checklist for figures | Tracks figure-by-figure capture | Appendix B of main report | Replace [SCREENSHOT REQUIRED] with the actual figure once captured |
| Database evidence | Supports the database design marking item | `documentation_application_development/evidence/database/` (planned) | Capture migration list output and a redacted view of one or two tables |
| Audit log evidence | Supports the security and audit traceability story | Audit log review screen `Views/AuditLogs/Index.cshtml` | Capture an audit log export screenshot |
| Validation message evidence | Supports the validation and exception handling marking item | Each form view | Capture screenshots of representative validation messages |
| Individual approval evidence | Supports the approved individual submission context | `documentation_application_development/evidence/approval/` (planned) | Attach approval email or note if available; otherwise leave [VERIFY] |
| Repository link | Supports submission packaging | `documentation_application_development/Source_And_Build_Links.md` | Paste final repository link |
| Packaged build / hosted deployment | Supports submission packaging | `documentation_application_development/Source_And_Build_Links.md` | Paste packaged build (ZIP) or hosted deployment link |
| Final Word and PDF report | Supports the formal report submission | `documentation_application_development/final_document/Application_Development_Final_Report.docx` (and PDF) | Re-export from Markdown after evidence is inserted |

## 26. Summary

The author has completed UniManage as an approved individual submission for CS6004ES Application Development Coursework 2. The contribution covers requirement analysis, architecture and design, database design, implementation of every feature, validation and security, user interface and usability, manual testing and debugging, documentation, and evidence preparation. The technical baseline is a working ASP.NET Core MVC 8 application with twelve controllers, twelve domain models, sixteen view models, twelve infrastructure components, forty-six Razor views, three EF Core migrations, and a complete `Program.cs` startup configuration that enables Identity, EF Core retry on failure, rate limiting, anti-forgery, security headers, HTTPS redirection, and HSTS.

Following approval to complete the coursework individually, the author took responsibility for the final implementation, testing, documentation, and submission preparation. Items that remain to be completed before the final upload are the screenshot capture, the diagram export, the title page placeholders, the references date placeholders, the repository and build links, and the conversion of the Markdown documents to Word and PDF. The current status of each item is recorded in Table IC2 above and in the evidence checklist.

End of individual contribution report.
