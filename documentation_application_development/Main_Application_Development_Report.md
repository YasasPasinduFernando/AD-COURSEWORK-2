# Title Page

UniManage - University Course Management System  
Application Development Coursework 2 Report  
Module: CS6004ES Application Development  
Assessment: Coursework 2  
Technology Used: ASP.NET Core MVC, C#, Visual Studio, Entity Framework Core, MySQL  
Centre: ESOFT Metro Campus / London Metropolitan University  
Student Name: Yasas Pasindu Fernando  
Student ID: 25026764  
Submission Type: Individual submission approved by lecturer/module team  
Lecturer / Module Leader: [INSERT IF REQUIRED]  
Submission Date: [INSERT DATE]

# Declaration / Academic Integrity Note

This report is based on the author's own implementation, source-code review, testing preparation, and documentation work for the UniManage application. External sources, official documentation, and tutorials are acknowledged where relevant. The final submission should include only evidence that can be supported by the project files, screenshots, build output, database evidence, and approved individual submission records.

# Acknowledgement

The author acknowledges the lecturer and module team for guidance on the coursework requirements and for the approval and direction to complete the final coursework submission individually. This documentation has been prepared in a professional and evidence-based manner using the available source code and supporting artefacts.

# Abstract

UniManage is a University Course Management System developed as an ASP.NET Core MVC web application using C# in Visual Studio. The system addresses common academic administration needs such as user registration and login, role-based access, course management, enrolment, assignment handling, grading, dashboards, reporting, messaging, and validation. The scanned source code confirms Student, Lecturer, and Administrator roles through ASP.NET Core Identity, with separate dashboard views and controller actions for key academic workflows. The application uses Entity Framework Core with a MySQL provider and includes migrations, a database context, seed data, reporting exports, file upload handling, email support, security headers, and audit logging. This report documents the system design, implementation, testing plan, evidence requirements, and viva-ready explanation of the solution. Features not confirmed by the source scan are marked as [NEEDS CONFIRMATION].

# Table of Contents

The final Word document should generate the Table of Contents from the Markdown heading structure.

# List of Figures

Figure 1: System Architecture Diagram  
Figure 2: Use Case Diagram  
Figure 3: Entity Relationship Diagram  
Figure 4: UML Class Diagram  
Figure 5: Application Flowchart  
Figure 6: Authentication and Role-Based Access Flow  
Figure 7: Login or Registration Screen  
Figure 8: Student Dashboard  
Figure 9: Lecturer Dashboard  
Figure 10: Administrator Dashboard  
Figure 11: Course Management Screen  
Figure 12: Enrollment Screen  
Figure 13: Assignment and Grading Screen  
Figure 14: Reporting and Analytics Screen  
Figure 15: Communication Module Screen  
Figure 16: Validation or Error Handling Evidence  
Figure 17: Database Evidence  
Figure 18: Source Repository Evidence  
Figure 19: Packaged Build or Deployment Evidence

# List of Tables

Table 1: Functional Requirements  
Table 2: Non-Functional Requirements  
Table 3: Core Database Entities  
Table 4: Main Controllers and Responsibilities  
Table 5: Main Models and Properties  
Table 6: Main Methods and Purpose  
Table 7: Coursework Task Mapping  
Table 8: Test Plan and Results  
Table 9: Individual Evidence Checklist

# Abbreviations

AD: Application Development  
UI: User Interface  
UX: User Experience  
API: Application Programming Interface  
CRUD: Create, Read, Update, Delete  
DB: Database  
ERD: Entity Relationship Diagram  
MVC: Model View Controller  
ORM: Object Relational Mapping  
SQL: Structured Query Language  
HTML: HyperText Markup Language  
CSS: Cascading Style Sheets  
JS: JavaScript  
JSON: JavaScript Object Notation  
RBAC: Role-Based Access Control

# 1. Introduction

This report documents UniManage, a University Course Management System created for CS6004ES Application Development Coursework 2. The application has been reviewed from the actual source-code repository. The scanned project is an ASP.NET Core MVC web application targeting .NET 8, with C# controllers, Razor views, Entity Framework Core models, and a MySQL database provider. The system follows the MVC pattern and supports role-based academic workflows for students, lecturers, and administrators.

The report is written for an approved individual submission. It explains the problem, requirements, design, database structure, implementation, testing plan, evidence checklist, limitations, and future improvements.

# 2. Project Motivation

A university course management system is useful because academic institutions need structured tools for managing courses, student enrolment, teaching materials, assignments, grading, communication, and reporting. UniManage provides a practical application development scenario because it combines user authentication, database relationships, validation, secure access, user interface design, and business logic in one system.

Following approval to complete the coursework individually, the author completed the implementation, testing, documentation, and final preparation as an individual submission. This made it necessary to document the application clearly, review the source code carefully, and identify confirmed features separately from items requiring manual confirmation.

# 3. Problem Description

The academic management problem includes several connected tasks:

- Course handling, including course creation, update, deletion, lecturer assignment, credits, enrolment limits, and prerequisites.
- Student enrolment with prevention of duplicate enrolment and capacity checks.
- Assignment creation and student submission.
- Grading with score and feedback.
- Dashboards for Student, Lecturer, and Administrator roles.
- Reports and analytics for academic monitoring.
- Communication between students and lecturers.
- Secure access so each role can only use relevant features.
- Validation and error handling for forms, files, access control, and database operations.

# 4. Aim and Objectives

Aim:

To design, implement, document, and evaluate UniManage as an ASP.NET MVC-based University Course Management System that supports role-based academic workflows.

Objectives:

- To implement registration and login using a secure authentication mechanism.
- To implement role-based access for Student, Lecturer, and Administrator users.
- To provide role-specific dashboards for academic users.
- To implement course management for administrator and lecturer workflows.
- To implement an enrolment system for students.
- To implement assignment submission and grading.
- To implement reporting and analytics features.
- To implement communication features where supported by the source code.
- To apply validation, error handling, and security controls.
- To prepare testing evidence, user documentation, diagrams, and viva-ready explanations.

# 5. Application Overview

UniManage is a web application for university course management. The target users are students, lecturers, and administrators. Students can register or log in, view their dashboard, browse courses, enrol, view assignments, submit work, view grades, message lecturers, and view meetings. Lecturers can view teaching dashboards, manage assignments for their own courses, upload materials, grade submissions, send messages, schedule meetings, and view workload reports. Administrators can manage courses, view an administrator dashboard, generate reports, and review audit logs.

The system boundary covers academic course management functions inside the web application. External services such as SMTP email and Google login are configured in code but require local configuration. The final submission package should include the report, individual contribution document, user manual, diagrams, screenshots, source repository link, build evidence, and database evidence.

# 6. Requirement Analysis

## 6.1 Functional Requirements

Table 1: Functional Requirements

| ID | Functional Requirement | Evidence in System |
|---|---|---|
| FR1 | User registration and login | `AccountController.cs`, `RegisterViewModel.cs`, `LoginViewModel.cs`, `Views/Account/Register.cshtml`, `Views/Account/Login.cshtml` |
| FR2 | Role-based access | `AppRoles.cs`, `Program.cs`, `[Authorize]` attributes in controllers |
| FR3 | Student dashboard | `DashboardController.Student`, `StudentDashboardViewModel`, `Views/Dashboard/Student.cshtml` |
| FR4 | Lecturer dashboard | `DashboardController.Lecturer`, `LecturerDashboardViewModel`, `Views/Dashboard/Lecturer.cshtml` |
| FR5 | Administrator dashboard | `DashboardController.Administrator`, `AdminDashboardViewModel`, `Views/Dashboard/Administrator.cshtml` |
| FR6 | Course management | `CoursesController.cs`, `Course.cs`, `CourseInputViewModel.cs`, `Views/Courses/` |
| FR7 | Enrollment system | `EnrollmentsController.cs`, `Enrollment.cs`, `CourseBrowseRowViewModel.cs`, `Views/Enrollments/Browse.cshtml` |
| FR8 | Assignment and grading | `AssignmentsController.cs`, `SubmissionsController.cs`, `Assignment.cs`, `Submission.cs`, `GradeSubmissionViewModel.cs` |
| FR9 | Reporting and analytics | `ReportsController.cs`, report view models, `CsvWriter.cs`, `PdfReport.cs`, `Views/Reports/` |
| FR10 | Communication module | `MessagesController.cs`, `Message.cs`, `Views/Messages/`; `MeetingsController.cs` for meetings |
| FR11 | Security and data protection | Identity configuration, anti-forgery attributes, rate limiting, security headers, audit logging |
| FR12 | Validation and exception handling | Data annotations, `ModelState`, `UseExceptionHandler`, `Error.cshtml`, controller checks |

## 6.2 Non-Functional Requirements

Table 2: Non-Functional Requirements

| ID | Non-Functional Requirement | Rationale / Evidence |
|---|---|---|
| NFR1 | Usability | Role-specific navigation and dashboards in `_Layout.cshtml` and dashboard views |
| NFR2 | Performance | EF Core queries use filtering, paging, `AsNoTracking` in several read-only queries |
| NFR3 | Security | Identity, RBAC, anti-forgery, rate limiting, security headers, file access checks |
| NFR4 | Maintainability | MVC folder structure, separate view models, infrastructure services |
| NFR5 | Reliability | EF Core retry-on-failure for MySQL and exception handling middleware |
| NFR6 | Scalability | Layered structure supports additional controllers and services; hosting scalability is [NEEDS CONFIRMATION] |
| NFR7 | Accessibility | Skip link and Bootstrap layout detected; full accessibility audit is [NEEDS CONFIRMATION] |
| NFR8 | Data integrity | EF Core relationships, unique course code, unique enrolment per student/course, validation attributes |

# 7. Research and Background

ASP.NET MVC is a common design approach for separating web application concerns into models, views, and controllers. In this project, the implementation uses ASP.NET Core MVC, which is the modern version of the framework. C# provides the application logic, while Razor views generate the user interface. Entity Framework Core acts as the ORM, allowing C# entity classes to map to database tables. ASP.NET Core Identity provides authentication and role support. These technologies are suitable for coursework because they demonstrate practical web development, database access, validation, security, and maintainable software structure.

Official Microsoft documentation should be cited in the final reference list for ASP.NET Core MVC, C#, ASP.NET Core Identity, and Entity Framework Core. MySQL provider documentation should be cited because the scanned project uses Pomelo Entity Framework Core MySQL rather than SQL Server or LocalDB.

# 8. System Architecture and Design

## 8.1 System Architecture Diagram

Figure 1: System Architecture Diagram

This diagram explains how browser users interact with the ASP.NET Core MVC application, controllers, views, models, services, Entity Framework Core, uploads, authentication, and the MySQL database. Editable Mermaid/draw.io source is listed in Appendix C.

## 8.2 Use Case Diagram

Figure 2: Use Case Diagram

This diagram identifies the main actions available to Student, Lecturer, and Administrator users. It supports viva discussion by linking each actor to confirmed features such as login, dashboards, course browsing, enrolment, assignments, reports, and communication. Editable Mermaid/draw.io source is listed in Appendix C.

## 8.3 Entity Relationship Diagram

Figure 3: Entity Relationship Diagram

This diagram describes the main data relationships between application users, courses, enrolments, assignments, submissions, messages, materials, meetings, audit logs, and report query outputs. Editable Mermaid/draw.io source is listed in Appendix C.

## 8.4 UML Class Diagram

Figure 4: UML Class Diagram

This diagram summarises the main controllers, models, services, and `ApplicationDbContext` classes found in the source code. Separate role-specific controller classes and a separate grading entity were not detected, so the diagram only presents confirmed source-code classes. Editable Mermaid/draw.io source is listed in Appendix C.

## 8.5 Application Flowchart

Figure 5: Application Flowchart

This diagram shows the application flow from opening the web application to authentication, role selection, dashboard access, validation, database operations, report generation, and logout. Editable Mermaid/draw.io source is listed in Appendix C.

## 8.6 Authentication and Role-Based Access Flow

Figure 6: Authentication and Role-Based Access Flow

This diagram explains how login requests are validated, checked through Identity, converted into an authentication cookie, and redirected to the correct dashboard. It also shows unauthorised access handling. Editable Mermaid/draw.io source is listed in Appendix C.

# 9. Database Design

Table 3: Core Database Entities

| Entity | Primary Key | Main Foreign Keys | Purpose |
|---|---|---|---|
| `ApplicationUser` | `Id` | Identity relationships | Stores user profile details and Identity user data |
| `Course` | `CourseId` | `LecturerId`, `PrerequisiteId` | Stores course details, lecturer, capacity, and prerequisite |
| `Enrollment` | `EnrollmentId` | `StudentId`, `CourseId` | Links students to courses |
| `Assignment` | `AssignmentId` | `CourseId` | Stores coursework tasks and due dates |
| `Submission` | `SubmissionId` | `AssignmentId`, `StudentId`, `GradedById` | Stores student submissions, grade, and feedback |
| `CourseMaterial` | `CourseMaterialId` | `CourseId`, `UploadedById` | Stores uploaded material metadata |
| `Message` | `MessageId` | `SenderId`, `ReceiverId` | Stores direct messages |
| `Meeting` | `MeetingId` | `CourseId`, `LecturerId` | Stores scheduled meeting details |
| `AuditLog` | `AuditLogId` | `UserId` | Stores audit activity |

The database access approach is Entity Framework Core through `ApplicationDbContext`, which inherits from `IdentityDbContext<ApplicationUser>`. Migrations are present in `Data/Migrations/`. The project uses Pomelo Entity Framework Core MySQL. SQL Server or LocalDB scripts were not found in the scan and remain [NEEDS CONFIRMATION] if required by the lecturer.

# 10. Implementation

## 10.1 ASP.NET MVC Structure

The application follows MVC structure with `Controllers`, `Models`, `ViewModels`, `Views`, `Data`, `Infrastructure`, and `wwwroot` folders. `Program.cs` configures services, middleware, Identity, EF Core, rate limiting, security headers, routes, and database seeding.

Evidence placeholder: Figure 18 Source Repository Evidence.

## 10.2 User Registration and Login

Registration and login are implemented in `AccountController.cs`. The view models are `RegisterViewModel`, `LoginViewModel`, `ForgotPasswordViewModel`, and `ResetPasswordViewModel`. Views are stored under `Views/Account/`. Identity handles password sign-in, lockout, roles, password reset, and optional Google sign-in where configured.

Evidence placeholder: Figure 7 Login or Registration Screen.

## 10.3 Student Dashboard

The student dashboard is implemented by `DashboardController.Student` and `Views/Dashboard/Student.cshtml`. It uses `StudentDashboardViewModel` to display enrolled courses, deadlines, grades, messages, activity, calendar events, and meetings.

Evidence placeholder: Figure 8 Student Dashboard.

## 10.4 Lecturer Dashboard

The lecturer dashboard is implemented by `DashboardController.Lecturer` and `Views/Dashboard/Lecturer.cshtml`. It displays teaching courses, enrolment counts, assignment counts, pending submissions, graded submissions, recent submissions, messages, meetings, and activity.

Evidence placeholder: Figure 9 Lecturer Dashboard.

## 10.5 Administrator Dashboard

The administrator dashboard is implemented by `DashboardController.Administrator` and `Views/Dashboard/Administrator.cshtml`. It shows counts for users, courses, enrolments, assignments, submissions, messages, popular courses, and recent audit activity.

Evidence placeholder: Figure 10 Administrator Dashboard.

## 10.6 Course Management

Course management is implemented in `CoursesController.cs`. Administrators can create, edit, delete, search, and page through courses. Lecturers can access `MyCourses` and upload materials for their own courses. The `Course` entity stores code, name, description, credits, enrolment limit, lecturer, prerequisite, enrolments, assignments, materials, and meetings.

Evidence placeholder: Figure 11 Course Management Screen.

## 10.7 Enrollment System

The enrolment workflow is implemented in `EnrollmentsController.cs`. Students can browse courses and enrol if the course is open, the student is not already enrolled, the capacity has not been reached, and any prerequisite has been satisfied.

Evidence placeholder: Figure 12 Enrollment Screen.

## 10.8 Assignment and Grading

Assignment creation and management are implemented in `AssignmentsController.cs`. Student submissions and lecturer grading are implemented in `SubmissionsController.cs`. The `Submission` model stores text content, file metadata, status, grade, feedback, and grading details. A separate `Grade` model was not found because grade fields are stored in `Submission`.

Evidence placeholder: Figure 13 Assignment and Grading Screen.

## 10.9 Reporting and Analytics

`ReportsController.cs` implements course popularity, student performance, lecturer workload, enrolments, pass/fail, and assignment attendance reports. `CsvWriter.cs` provides CSV export and `PdfReport.cs` provides PDF export through QuestPDF.

Evidence placeholder: Figure 14 Reporting and Analytics Screen.

## 10.10 Communication Module

`MessagesController.cs` implements inbox, thread, reply, compose, and mark all read actions. Students can message lecturers linked to enrolled courses. Lecturers can message students enrolled in their courses. `MeetingsController.cs` provides meeting scheduling and joining as an additional communication feature.

Evidence placeholder: Figure 15 Communication Module Screen.

## 10.11 Security and Data Protection

Security is supported by ASP.NET Core Identity, RBAC, anti-forgery tokens, rate limiting, secure cookie configuration, security headers, file upload validation, controlled file downloads, and audit logging. Exception handling is configured in `Program.cs` through production and development error handling paths.

Evidence placeholder: Figure 16 Validation or Error Handling Evidence.

# 11. Detailed Description of Classes, Properties, and Methods

Table 4: Main Controllers and Responsibilities

| Controller | Main Responsibility | Key Actions / Methods | Related Views | Coursework Task |
|---|---|---|---|---|
| `AccountController` | Authentication and account flows | `Register`, `Login`, `Logout`, `ForgotPassword`, `ResetPassword`, `GoogleCallback` | `Views/Account/` | Registration/Login |
| `DashboardController` | Role dashboard routing and dashboard data | `Index`, `Student`, `Lecturer`, `Administrator` | `Views/Dashboard/` | Dashboards |
| `CoursesController` | Course CRUD, lecturer courses, materials | `Index`, `MyCourses`, `Details`, `Create`, `Edit`, `Delete`, `UploadMaterial` | `Views/Courses/` | Course Management |
| `EnrollmentsController` | Student course browsing and enrolment | `Browse`, `Enroll` | `Views/Enrollments/Browse.cshtml` | Enrollment |
| `AssignmentsController` | Assignment creation and management | `ForCourse`, `Create`, `Edit`, `Delete`, `Mine` | `Views/Assignments/` | Assignments |
| `SubmissionsController` | Submission, grading, file access | `Submit`, `Grade`, `ForAssignment`, `CourseMaterialFile`, `SubmissionFile` | `Views/Submissions/` | Grading |
| `ReportsController` | Reports and exports | `CoursePopularity`, `StudentPerformance`, `LecturerWorkload`, `Enrollments`, `PassFail`, `AssignmentAttendance` | `Views/Reports/` | Reporting |
| `MessagesController` | Messaging workflow | `Inbox`, `Thread`, `Reply`, `Compose`, `MarkAllRead` | `Views/Messages/` | Communication |
| `MeetingsController` | Meeting scheduling and join links | `Index`, `Create`, `Edit`, `Delete`, `Join`, `Ics` | `Views/Meetings/` | Communication |
| `ProfileController` | Profile and password change | `Index`, `ChangePassword` | `Views/Profile/` | Account management |
| `AuditLogsController` | Audit log review | `Index` | `Views/AuditLogs/Index.cshtml` | Security evidence |

Table 5: Main Models and Properties

| Model / Entity | Key Properties | Purpose | Database Relationship |
|---|---|---|---|
| `ApplicationUser` | `FullName`, `Email`, `PhoneNumber` | Extends Identity user | Related to courses, enrolments, submissions, messages, materials |
| `Course` | `Code`, `Name`, `Credits`, `EnrollmentLimit`, `LecturerId`, `PrerequisiteId` | Course record | One lecturer, many enrolments, assignments, materials, meetings |
| `Enrollment` | `StudentId`, `CourseId`, `EnrolledAtUtc` | Student-course link | Many-to-one with user and course |
| `Assignment` | `CourseId`, `Title`, `DueDateUtc`, `MaxPoints` | Coursework task | Many assignments per course |
| `Submission` | `AssignmentId`, `StudentId`, `Status`, `Grade`, `Feedback` | Student submission and grading record | Many submissions per assignment |
| `Message` | `SenderId`, `ReceiverId`, `Subject`, `Content`, `IsRead` | Communication record | Links sender and receiver users |
| `CourseMaterial` | `CourseId`, `Title`, `StoredFileName`, `UploadedById` | Uploaded teaching material | Many materials per course |
| `Meeting` | `CourseId`, `LecturerId`, `Title`, `ScheduledAtUtc`, `MeetingUrl` | Online meeting record | Many meetings per course |
| `AuditLog` | `Category`, `Action`, `Detail`, `UserId`, `Success` | Audit trail | Linked to user where available |

Table 6: Main Methods and Purpose

| Class / File | Method | Purpose | Input / Output | Error Handling / Validation |
|---|---|---|---|---|
| `Program.cs` | service configuration | Registers DbContext, Identity, email, audit, rate limiting and MVC | App configuration to service container | Throws if connection string missing |
| `DbInitializer.cs` | `SeedAsync` | Applies migrations and creates demo roles/data | Service provider to seeded database | Throws on failed user creation |
| `AccountController` | `Login` | Authenticates user | Login form to dashboard redirect | `ModelState`, lockout, audit failure |
| `AccountController` | `Register` | Creates Student or Lecturer user | Register form to signed-in user | Role check, password validation, audit |
| `CoursesController` | `Create` | Creates course | Course form to database record | Lecturer and prerequisite validation |
| `CoursesController` | `UploadMaterial` | Uploads course material | File and title to stored metadata | File type, size, owner, rate limit |
| `EnrollmentsController` | `Enroll` | Enrols student on course | Course ID to enrolment record | Duplicate, capacity, prerequisite checks |
| `AssignmentsController` | `Create` | Creates assignment | Assignment form to database record | Course ownership and model validation |
| `SubmissionsController` | `Submit` | Saves student submission | Text/file to submission record | Enrolment, file validation, graded lock |
| `SubmissionsController` | `Grade` | Records grade and feedback | Grade form to updated submission | Lecturer ownership and score range |
| `ReportsController` | report actions | Builds analytics rows and exports | Query output to view/CSV/PDF | Role-based access |
| `MessagesController` | `Compose` | Sends message | Recipient, subject, content | Allowed-recipient validation |
| `MeetingsController` | `Create` | Schedules meeting | Meeting form to meeting record | Course ownership, URL and time validation |
| `AuditLogger.cs` | `LogAsync` | Saves audit record | Category/action/detail to audit table | Internal try/catch logging |

# 12. Logical Solution Explanation

The solution connects the case study requirements to a practical MVC implementation. Authentication identifies the user, RBAC routes the user to the correct dashboard, controllers apply validation and business rules, Entity Framework Core persists records, and Razor views present role-specific functionality. Course management creates the academic structure. Enrolment links students to courses. Assignments and submissions support teaching and assessment. Reports provide administrative insight. Messaging and meetings support academic communication. This forms a logical workflow from secure access to academic task completion.

# 13. Testing

Table 8: Test Plan and Results

| Test ID | Test Area | Test Scenario | Input / Setup | Expected Result | Actual Result | Status |
|---|---|---|---|---|---|---|
| T1 | Registration | Register Student or Lecturer | Valid register form | User created and signed in | [NEEDS CONFIRMATION] | Pending screenshot |
| T2 | Login | Login with seeded user | Demo credentials | Correct dashboard displayed | [NEEDS CONFIRMATION] | Pending screenshot |
| T3 | Role-based access | Student opens admin course list | Student account | Access denied or forbidden | [NEEDS CONFIRMATION] | Pending screenshot |
| T4 | Course management | Administrator creates course | Valid course form | Course saved | [NEEDS CONFIRMATION] | Pending screenshot |
| T5 | Enrollment | Student enrols in available course | Course with capacity | Enrolment saved | [NEEDS CONFIRMATION] | Pending screenshot |
| T6 | Assignment creation | Lecturer creates assignment | Valid assignment form | Assignment saved | [NEEDS CONFIRMATION] | Pending screenshot |
| T7 | Assignment submission | Student submits text/file | Enrolled student | Submission saved | [NEEDS CONFIRMATION] | Pending screenshot |
| T8 | Grading | Lecturer grades submission | Valid grade range | Grade and feedback saved | [NEEDS CONFIRMATION] | Pending screenshot |
| T9 | Reporting | Administrator opens reports | Admin account | Report rows displayed | [NEEDS CONFIRMATION] | Pending screenshot |
| T10 | Messaging | Send message | Allowed recipient | Message thread updated | [NEEDS CONFIRMATION] | Pending screenshot |
| T11 | Validation | Submit invalid form | Missing required fields | Validation errors displayed | [NEEDS CONFIRMATION] | Pending screenshot |
| T12 | Security | Upload invalid file | Unsupported file | Upload rejected | [NEEDS CONFIRMATION] | Pending screenshot |
| T13 | Database save/retrieve | Save course or enrolment | Valid database connection | Record appears after reload | [NEEDS CONFIRMATION] | Pending database evidence |
| T14 | Error handling | Request missing entity | Invalid ID | Not found or safe error view | [NEEDS CONFIRMATION] | Pending screenshot |
| T15 | Build | Compile solution | `dotnet build --no-restore` | 0 errors | Build succeeded with 0 warnings and 0 errors | Confirmed |

# 14. Evaluation Against Coursework Brief

Table 7: Coursework Task Mapping

| Assessment Requirement | Evidence in Project | Strength |
|---|---|---|
| UI | Razor views, Bootstrap, custom CSS | Strong source evidence |
| Registration/Login | `AccountController`, Identity | Strong source evidence |
| Student Dashboard | `DashboardController.Student`, student view | Strong source evidence |
| Lecturer Dashboard | `DashboardController.Lecturer`, lecturer view | Strong source evidence |
| Administrator Dashboard | `DashboardController.Administrator`, administrator view | Strong source evidence |
| Course Management | `CoursesController`, course views | Strong source evidence |
| Enrollment System | `EnrollmentsController` | Strong source evidence |
| Assignment and Grading | `AssignmentsController`, `SubmissionsController` | Strong source evidence |
| Reporting and Analytics | `ReportsController`, CSV/PDF helpers | Strong source evidence |
| Communication Module | `MessagesController`, `MeetingsController` | Strong source evidence |
| Security and Data Protection | Identity, RBAC, anti-forgery, security headers | Strong source evidence |
| Installation/User Manual | User manual file in documentation folder | Draft prepared |
| Logical Solution | MVC workflow documented in this report | Draft prepared |
| Diagrams | Mermaid diagram files in `diagrams/` | Source prepared, export needed |
| Class/property/method description | Tables 4 to 6 | Prepared |
| Reflection | Section 18 | Prepared |
| Programming style | MVC structure and naming | Evidenced |
| Validation and exception handling | Data annotations, `ModelState`, exception middleware | Evidenced |

# 15. Programming Style and Maintainability

The project uses meaningful controller and model names aligned with MVC responsibilities. The code separates data models, view models, controllers, Razor views, and infrastructure services. Validation is implemented through attributes and controller checks. Error handling is visible through middleware, safe response types, and specific `try/catch` handling around email and audit operations. The structure supports maintainability because common behaviour such as uploads, reports, email, and audit logging is placed in infrastructure classes rather than being repeated in views.

# 16. Challenges Faced

The main individual challenges were confirming the actual implemented feature set, aligning the source code with the coursework brief, preparing evidence-based documentation, and separating confirmed functionality from items requiring manual evidence. Another challenge was documenting the database accurately because the scanned project uses EF Core with MySQL, while the general coursework technology list may refer to SQL Server or LocalDB.

# 17. Limitations

- Automated test projects were not detected. [NEEDS CONFIRMATION]
- Final screenshots have not been inserted. [NEEDS CONFIRMATION]
- Repository and packaged build links must be pasted before submission. [NEEDS CONFIRMATION]
- SQL Server or LocalDB scripts were not detected. EF Core MySQL migrations were detected instead. [NEEDS CONFIRMATION]
- Dedicated administrator user-management screens were not detected. [NEEDS CONFIRMATION]
- Deployment or hosted URL evidence was not detected. [NEEDS CONFIRMATION]

# 18. Own Reflection on Experience

The author gained practical experience in reviewing and documenting an ASP.NET MVC-style application with role-based academic workflows. The approved individual completion route required careful source-code checking, precise evidence handling, and disciplined documentation. The process strengthened understanding of MVC structure, Entity Framework Core relationships, Identity authentication, validation, reporting, and viva preparation. The author also learned the importance of marking uncertain features clearly rather than presenting unsupported claims.

# 19. Individual Contribution

"The full individual contribution report is attached as a supporting document at the end of this submission."

The supporting report explains the author's approved individual completion context and summarises the final implementation, testing, documentation, and submission preparation work.

# 20. Conclusion

UniManage provides a structured University Course Management System with confirmed evidence for authentication, role dashboards, course management, enrolment, assignments, grading, reporting, messaging, meetings, validation, security, and database access. The project demonstrates practical application development using ASP.NET Core MVC, C#, Visual Studio, Entity Framework Core, and MySQL. The documentation package prepares the work for an approved individual submission by linking claims to actual source files and by identifying evidence that still requires manual completion.

# 21. Future Improvements

- Add automated unit and integration tests.
- Add a dedicated administrator user-management module if required.
- Add deployment evidence and a hosted demonstration environment.
- Add accessibility audit results.
- Add database backup or SQL export evidence.
- Add more advanced report filtering and chart exports.
- Add final screenshots and exported diagrams to the Word report.

# 22. References

Microsoft, 2026. ASP.NET Core MVC documentation. Available at: [official Microsoft documentation link].  
Microsoft, 2026. C# documentation. Available at: [official Microsoft documentation link].  
Microsoft, 2026. ASP.NET Core Identity documentation. Available at: [official Microsoft documentation link].  
Microsoft, 2026. Entity Framework Core documentation. Available at: [official Microsoft documentation link].  
Pomelo Foundation, 2026. Pomelo Entity Framework Core MySQL provider documentation. Available at: [official provider documentation link].  
QuestPDF, 2026. QuestPDF documentation. Available at: [official QuestPDF documentation link].

# 23. Appendices

Appendix A: Selected Source Code Evidence  
Appendix B: Screenshots  
Appendix C: Diagram Sources  
Appendix D: Individual Approval and Planning Evidence  
Appendix E: Source Code Repository and Packaged Build Evidence  
Appendix F: User Manual
