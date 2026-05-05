# User Manual - UniManage Application

Coursework: CS6004ES Application Development Coursework 2  
Application: UniManage - University Course Management System  
Framework: ASP.NET Core MVC on .NET 8

## 1. Purpose

This user manual explains how to run and use the UniManage web application. It is written for coursework demonstration and evidence preparation. The application supports three main roles: Student, Lecturer and Administrator.

## 2. Prerequisites

Before running the application, the following should be available:

- Visual Studio 2022 or a compatible .NET 8 development environment.
- .NET 8 SDK.
- MySQL 8.x.
- Access to the project solution `AD COURSEWORK 2.sln`.
- A configured connection string for the local or coursework database.

Sensitive values such as database passwords, SMTP passwords and Google client secrets should not be included in screenshots or final submission documents.

## 3. Running the Application

### Visual Studio

1. Open `AD COURSEWORK 2.sln`.
2. Confirm that the startup project is `AD COURSEWORK 2`.
3. Check that `appsettings.json` or user secrets contain the correct MySQL connection string.
4. Run the project using the HTTPS or HTTP launch profile.
5. The detected local launch URLs are:
   - `https://localhost:7212`
   - `http://localhost:5103`

### Command Line

From the project root:

```text
dotnet restore
dotnet build
dotnet run
```

The repository scan verified a successful build with:

```text
dotnet build --no-restore
```

## 4. Database Initialisation

The application uses Entity Framework Core migrations. At startup, `DbInitializer.cs` calls database migration logic and seeds default roles and sample records.

Default roles:

- Administrator
- Lecturer
- Student

Seeded demo accounts:

| Role | Email | Password |
|---|---|---|
| Administrator | `admin@unimanage.local` | `Admin123!` |
| Lecturer | `lecturer@unimanage.local` | `Lecturer123!` |
| Student | `student@unimanage.local` | `Student123!` |

Seeded academic data includes two demo courses, `CS101` and `CS201`, and one assignment.

## 5. General Navigation

After login, the application redirects the user to the correct dashboard based on their role. The shared navigation bar changes depending on whether the user is a student, lecturer or administrator.

Common options include:

- Dashboard
- Profile
- Change password
- Sign out

The system also displays success and error alerts when actions are completed or blocked.

## 6. Student Guide

### 6.1 Login

1. Open the application URL.
2. Select Sign in.
3. Enter the student email and password.
4. Submit the login form.

### 6.2 Student Dashboard

The student dashboard provides:

- Enrolled courses.
- Upcoming assignment deadlines.
- Recent grades.
- Unread message count.
- Activity feed.
- Upcoming meetings.
- Calendar events.

### 6.3 Browse and Enrol on Courses

1. Select Catalog.
2. Review available courses.
3. Check course capacity and prerequisite status.
4. Select Enrol for an eligible course.

The system blocks enrolment if:

- The student is already enrolled.
- The course is full.
- A prerequisite course has not been enrolled.

### 6.4 View Course Details

1. Open an enrolled course.
2. Review course information, lecturer details, materials and assignments.
3. Download available materials where permitted.

### 6.5 Submit an Assignment

1. Select Assignments.
2. Choose the relevant assignment.
3. Enter text content and/or upload an attachment.
4. Submit the form.

The system requires either text content or a file. File uploads are restricted by type and size.

### 6.6 View Grades and Feedback

After the lecturer grades a submission, the student can view:

- Grade.
- Maximum points.
- Feedback.
- Submission status.

### 6.7 Messaging

1. Select Messages.
2. Open an existing conversation or compose a new message.
3. Select an allowed lecturer recipient.
4. Send the message.

Students can communicate with lecturers connected to their enrolled courses.

### 6.8 Meetings

1. Select Meetings.
2. Review upcoming course meetings.
3. Use Join to open the meeting link.
4. Download or use calendar evidence where required.

## 7. Lecturer Guide

### 7.1 Lecturer Dashboard

The lecturer dashboard provides:

- Taught courses.
- Enrolment counts.
- Assignment counts.
- Pending and graded submissions.
- Recent submissions.
- Upcoming meetings.
- Activity and calendar events.

### 7.2 Manage Own Courses

1. Select My Courses.
2. Open a course.
3. Review enrolled student count, assignments and materials.

Lecturers can only manage their assigned courses.

### 7.3 Upload Course Materials

1. Open a course details page.
2. Enter a material title.
3. Select a file.
4. Submit the upload form.

The upload process validates extension, content type and size.

### 7.4 Create and Edit Assignments

1. Open a course.
2. Select the assignment management option.
3. Create a new assignment with title, description, due date and maximum points.
4. Edit or delete assignments where necessary.

### 7.5 Grade Student Submissions

1. Open an assignment.
2. Review submitted work.
3. Open the grading page.
4. Enter a grade within the permitted range.
5. Add feedback.
6. Save the grade.

### 7.6 Messaging Students

Lecturers can message students enrolled in their own courses. The message inbox shows conversation threads and unread status.

### 7.7 Schedule Meetings

1. Select Meetings.
2. Create a meeting for one of the lecturer's courses.
3. Enter title, description, scheduled time and duration.
4. Use auto-generated meeting link or provide a valid Google Meet, Zoom, Teams or Webex URL.
5. Choose whether to notify students.

The system can produce calendar attachments and links for meeting reminders.

### 7.8 Lecturer Workload Report

Lecturers can access the workload report for their own teaching workload. Administrators can access broader report data.

## 8. Administrator Guide

### 8.1 Administrator Dashboard

The administrator dashboard includes:

- User count.
- Course count.
- Enrolment count.
- Assignment count.
- Submission count.
- Message count.
- Popular courses.
- Recent audit activity.

### 8.2 Manage Courses

1. Select Courses.
2. Search or browse course records.
3. Create a course by entering code, name, description, credits, enrolment limit, lecturer and optional prerequisite.
4. Edit existing course details.
5. Delete a course where appropriate.

### 8.3 Reports

The Reports area includes:

- Course popularity.
- Student performance.
- Lecturer workload.
- Enrolments.
- Pass/fail analysis.
- Assignment attendance.

Reports can be exported as CSV and PDF where supported by the page.

### 8.4 Audit Logs

1. Select Audit.
2. Filter by category or search text.
3. Review user, action, detail, IP address and timestamp information.

The audit log supports evidence of authentication, course, material, submission, enrolment, profile, security, meeting and message events where logged.

### 8.5 User Management

A dedicated administrator user-management screen was not detected in the repository. Administrator creation is currently evidenced through seed data. This remains `[NEEDS CONFIRMATION]` for the final submission.

## 9. Profile and Password Management

Authenticated users can:

- View profile information.
- Update full name and phone number.
- Change password.
- Sign out.

Forgotten password and reset password flows are implemented through the account controller and email service.

## 10. Troubleshooting

| Issue | Suggested action |
|---|---|
| Application does not start | Check .NET 8 SDK, build output and launch profile. |
| Database connection fails | Confirm MySQL is running and the connection string is correct. |
| Migrations fail | Confirm database permissions and EF Core tooling. |
| Login fails | Check seeded account credentials or registered user details. |
| Google login fails | Confirm Google client configuration, or use local login. |
| Email does not send | Confirm SMTP settings; avoid exposing credentials in evidence. |
| Upload rejected | Check file extension, size and content type. |
| Access denied | Confirm the user has the expected role. |

## 11. Evidence Capture Notes

For coursework evidence, capture screenshots of the main workflows for each role. Avoid including private passwords, SMTP credentials, Google secrets or database passwords. Use the screenshot checklist in `screenshots/README_SCREENSHOTS.md`.

