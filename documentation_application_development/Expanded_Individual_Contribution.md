# Supporting Document: Individual Contribution Report

Student Name: Yasas Pasindu Fernando  
Student ID: 25026764  
Module: CS6004ES Application Development  
Assessment: Coursework 2  
Project: UniManage - University Course Management System  
Submission Context: Approved individual submission

## 1. Introduction

This supporting document records the author's individual contribution to the final UniManage coursework submission. It is written as an approved individual submission and is based on the available source code, documentation, build evidence, and prepared submission artefacts.

## 2. Project Context

UniManage is a University Course Management System implemented as an ASP.NET Core MVC application using C#, Razor views, Entity Framework Core, and a MySQL database provider. The system supports academic workflows for Student, Lecturer, and Administrator roles.

## 3. Individual Submission Context

Following approval to complete the coursework individually, the author completed the implementation, testing, documentation, and final preparation as an individual submission.

## 4. Personal Motivation and Project Selection

The author selected and completed UniManage because it provides a practical academic management problem with clear technical requirements. The project allows demonstration of MVC structure, database relationships, authentication, validation, reporting, and user interface development.

## 5. Summary of Main Contribution

The author's final contribution focused on reviewing the source code, aligning the implementation with the coursework brief, preparing technical documentation, preparing diagrams, building the evidence checklist, and identifying remaining manual evidence required for submission.

Table IC1: Area and Contribution Summary

| Area | Contribution Summary | Evidence / Figure Reference |
|---|---|---|
| Requirement analysis | Mapped confirmed features to coursework requirements | Table 1, Table 7 |
| Architecture and diagrams | Prepared Mermaid diagram sources | Figures 1 to 6, Appendix C |
| Database design | Documented EF Core entities and relationships | Figure 3, Table 3 |
| Authentication | Documented Identity login, registration, and roles | Figure 6, Section 10.2 |
| Dashboards | Documented Student, Lecturer, and Administrator dashboard flows | Figures 8 to 10 |
| Course management | Documented course CRUD and material upload | Figure 11 |
| Enrollment | Documented student enrolment workflow | Figure 12 |
| Assignment and grading | Documented assignment, submission, and grading flow | Figure 13 |
| Reporting | Documented analytics and export features | Figure 14 |
| Communication | Documented messages and meeting workflow | Figure 15 |
| Security and validation | Documented RBAC, validation, anti-forgery, rate limiting, audit logging | Figure 16 |
| Testing and evidence | Prepared test plan and evidence checklist | Table 8, Table 9 |
| User manual | Prepared installation and role-based user guide | Appendix F |

## 6. Contribution to Requirement Analysis

The author reviewed the project structure and identified confirmed requirements through controllers, models, views, and configuration files. Confirmed requirements include login, role-based dashboards, course management, enrolment, assignments, grading, reporting, communication, validation, and security. Items not found or not manually evidenced were marked as [NEEDS CONFIRMATION].

## 7. Contribution to System Architecture and Diagrams

The author prepared editable Mermaid diagram sources for architecture, use case, ERD, UML class, application flow, and authentication flow. These diagrams are stored in `documentation_application_development/diagrams/` and can be exported to PNG or PDF for the report.

## 8. Contribution to Database Design

The author documented the database design using actual source-code evidence from `ApplicationDbContext.cs`, model classes, and EF Core migrations. The documentation records the MySQL EF Core approach and marks SQL Server or LocalDB evidence as [NEEDS CONFIRMATION] because it was not detected.

## 9. Contribution to User Registration and Login

The author documented the registration, login, Google login, forgotten password, reset password, and logout flows implemented in `AccountController.cs`. The author also linked these features to Identity configuration in `Program.cs`.

## 10. Contribution to Role-Based Access Control

The author documented role constants in `AppRoles.cs`, seeded roles in `DbInitializer.cs`, and controller-level `[Authorize]` attributes. The documentation explains Student, Lecturer, and Administrator access in a viva-friendly way.

## 11. Contribution to Student Dashboard

The author documented the student dashboard implementation using `DashboardController.Student`, `StudentDashboardViewModel`, and `Views/Dashboard/Student.cshtml`. The documented flow includes enrolled courses, deadlines, grades, messages, activity, and meetings.

## 12. Contribution to Lecturer Dashboard

The author documented the lecturer dashboard using `DashboardController.Lecturer`, `LecturerDashboardViewModel`, and `Views/Dashboard/Lecturer.cshtml`. The documented flow includes teaching courses, submissions, grading status, messages, and meetings.

## 13. Contribution to Administrator Dashboard

The author documented the administrator dashboard using `DashboardController.Administrator`, `AdminDashboardViewModel`, and `Views/Dashboard/Administrator.cshtml`. The documentation covers counts, popular courses, reports, and audit activity.

## 14. Contribution to Course Management

The author documented `CoursesController.cs`, the `Course` entity, course input validation, administrator CRUD operations, lecturer course access, and material upload.

## 15. Contribution to Enrollment System

The author documented `EnrollmentsController.cs`, the `Enrollment` entity, course browsing, duplicate enrolment prevention, capacity checking, and prerequisite checking.

## 16. Contribution to Assignment and Grading

The author documented `AssignmentsController.cs`, `SubmissionsController.cs`, `Assignment.cs`, `Submission.cs`, assignment creation, submission upload, grading, feedback, and secure file access.

## 17. Contribution to Reporting and Analytics

The author documented reports implemented in `ReportsController.cs`, including course popularity, student performance, lecturer workload, enrolments, pass/fail analysis, and assignment attendance. CSV and PDF export helpers were also documented.

## 18. Contribution to Communication Module

The author documented `MessagesController.cs`, the `Message` entity, message inbox, conversation threads, replies, and compose workflow. Meeting scheduling was also documented as an additional communication feature through `MeetingsController.cs`.

## 19. Contribution to Security, Validation, and Error Handling

The author documented security and validation features including Identity, RBAC, anti-forgery tokens, rate limiting, security headers, upload validation, audit logging, `ModelState`, data annotations, and error handling middleware.

## 20. Contribution to Testing and Debugging

The author verified that the project builds successfully using `dotnet build --no-restore`, with 0 warnings and 0 errors. Manual screenshots, browser testing, database screenshots, and exported report evidence remain [NEEDS CONFIRMATION] until captured.

## 21. Contribution to UI/UX, Screenshots, and Evidence Preparation

The author reviewed the UI structure in Razor views, `_Layout.cshtml`, `site.css`, and static assets. The screenshots README and evidence checklist define the screenshots required for final submission.

## 22. Contribution to Documentation, User Manual, and Research

The author prepared the main technical report, this individual contribution report, the user manual, evidence checklist, source/build links file, diagram README, screenshot README, evidence README, conversion guide, manual checklist, and final QA report.

## 23. Challenges, Learning, and Final Preparation

The main challenge was aligning the actual scanned project with the coursework wording while avoiding unsupported claims. The author learned the importance of evidence-based documentation, careful source-code review, and clear marking of uncertain items.

## 24. Supporting Evidence

Table IC2: Evidence Type and Location

| Evidence Type | Purpose | Location |
|---|---|---|
| Source code | Confirms implemented features | Project root and controller/model/view folders |
| Build evidence | Confirms project compiles | `dotnet build --no-restore` output |
| Diagrams | Supports architecture and design explanation | `documentation_application_development/diagrams/` |
| Screenshots | Supports final report figures | `documentation_application_development/screenshots/` |
| Evidence checklist | Tracks completion | `Evidence_Checklist.md` |
| User manual | Installation and usage guidance | `User_Manual_Application_Development.md` |
| Final QA report | Submission readiness check | `final_document/FINAL_QA_REPORT.txt` |

## 25. Summary

The author completed the final documentation and evidence preparation as an approved individual submission. The source-code review confirms a substantial ASP.NET Core MVC UCMS implementation, while remaining screenshot, link, deployment, and database evidence tasks are clearly marked for manual completion.
