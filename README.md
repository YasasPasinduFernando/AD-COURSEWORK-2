# UniManage — University Course Management System

UniManage is a secure, role-based ASP.NET Core MVC web application that helps a
university manage students, lecturers, courses, enrollments, assignments,
submissions, grading, communication, and analytics.

Built with **.NET 8 / ASP.NET Core MVC**, **EF Core 8 + Pomelo MySQL**, and
**ASP.NET Identity**.

---

## Table of contents

1. [Features](#features)
2. [Architecture](#architecture)
3. [Security](#security)
4. [Getting started](#getting-started)
5. [Default seeded accounts](#default-seeded-accounts)
6. [Project structure](#project-structure)
7. [Reports & analytics](#reports--analytics)
8. [Audit log](#audit-log)
9. [Configuration reference](#configuration-reference)

---

## Features

### Authentication & access control
- Email/password registration and login with **ASP.NET Identity**.
- Optional **Google** single sign-on (existing accounts only).
- **Role-based access control**: `Administrator`, `Lecturer`, `Student`.
- **Account lockout** after 5 failed attempts (15-minute cool-down).
- Password reset via email token.
- Sign-in alert email with IP and user agent.

### Dashboards
- **Student** — enrolled courses, upcoming deadlines, recent grades, calendar
  of assignment due dates and uploads.
- **Lecturer** — taught courses with enrollment counts, pending vs. graded
  submissions, monthly activity calendar.
- **Administrator** — totals (users, courses, enrollments, assignments) and
  most popular courses.

### Course management
- Administrators create, edit, and delete courses with code, name, credits,
  enrollment limit, lecturer assignment, and prerequisite course.
- Lecturers upload course materials with title and file (allow-listed types).
- Students see materials only for courses they are enrolled in.
- Server-side **search** (code, name, lecturer) and **pagination** on the
  admin courses list.

### Enrollment
- Students browse the course catalog with filters (open / enrolled / blocked)
  and search.
- Server-side validation enforces:
  - Enrollment limit
  - Prerequisite course
  - No duplicate enrollment

### Assignments & grading
- Lecturers create, edit, and delete assignments per course with due date and
  max points.
- Students submit text and/or a file attachment (file extension and size
  validated).
- Lecturers grade with score and feedback; once graded, students can no longer
  resubmit that submission.

### Communication
- Direct messaging between **students and the lecturers of their enrolled
  courses** (and vice versa). Other recipients are blocked at the controller
  level.
- Inbox lists conversations with the latest message preview and an unread
  badge.
- Threaded conversation view marks messages read on open.

### Reports & analytics
- **Course popularity** — bar chart of enrollments vs. capacity.
- **Student performance** — top-15 student averages with overall cohort
  average.
- **Lecturer workload** — courses, assignments, submissions per lecturer
  (admins see all, lecturers see themselves).
- **Enrollments over time** — daily enrollment line chart.
- Every report has a **CSV export** (UTF-8 with BOM, CRLF, RFC4180 quoting).

### Audit log (admin only)
- Every sensitive action is recorded with timestamp, user, IP, user agent,
  category, action, detail, and success flag.
- Searchable by category, user, action, IP, and detail; paginated.

### Profile
- View and edit full name and phone number.
- Change password (re-signs current session).

---

## Architecture

```
+-----------------------------+        +----------------------------+
|     Browser (Bootstrap)     |  HTTPS |   ASP.NET Core MVC + Razor |
|  Chart.js, Bootstrap Icons  |<------>|     Controllers + Views    |
+-----------------------------+        +-------------+--------------+
                                                     |
                                       EF Core 8     |
                                       (Pomelo)      v
                                              +-------------+
                                              |   MySQL 8   |
                                              +-------------+
```

- **Identity** stores users and roles in the same MySQL database.
- **Migrations** under `Data/Migrations` create the schema, including the
  audit log table.
- **Middleware pipeline**: HTTPS redirect → static files → security headers →
  routing → rate limiter → authentication → authorization.

---

## Security

The application follows OWASP-aligned hardening:

| Concern                         | Mitigation                                                                 |
| ------------------------------- | -------------------------------------------------------------------------- |
| Brute-force login               | Identity lockout, 5 attempts, 15-min cool-down, audit log                  |
| Rate limiting                   | Fixed window per-IP on `/Account/*` and per-user on file uploads           |
| CSRF                            | `@Html.AntiForgeryToken()` + `[ValidateAntiForgeryToken]` on every POST    |
| Cookie hardening                | `HttpOnly`, `SameSite=Lax`/`Strict`, `Secure` (when HTTPS)                 |
| Transport                       | `UseHttpsRedirection`, `UseHsts` in production                             |
| XSS                             | Razor auto-encoding, `X-Content-Type-Options: nosniff`, strict CSP         |
| Clickjacking                    | `X-Frame-Options: DENY`, `frame-ancestors 'none'`                          |
| MIME sniffing                   | `X-Content-Type-Options: nosniff`                                          |
| Referrer leakage                | `Referrer-Policy: strict-origin-when-cross-origin`                         |
| Browser feature abuse           | `Permissions-Policy: camera=(), microphone=(), geolocation=(), payment=()` |
| Cross-origin window attacks     | `Cross-Origin-Opener-Policy: same-origin`                                  |
| SQL injection                   | EF Core parameterised queries everywhere                                   |
| File upload                     | **Allow-list** of extensions and content types, 15 MB cap, GUID file name  |
| Path traversal in downloads     | Stored file names sanitised; `..` / path separators rejected               |
| Privilege escalation            | `[Authorize(Roles=...)]` on every controller/action; resource ownership    |
|                                 | checks (e.g. lecturer can only grade their own course's submissions)       |
| Audit                           | Every login, logout, create/update/delete, grade, enroll, etc. is logged   |

### Content Security Policy

```
default-src 'self';
img-src 'self' data: https:;
font-src 'self' https://fonts.gstatic.com https://cdn.jsdelivr.net data:;
style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net;
script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net;
connect-src 'self';
frame-ancestors 'none';
form-action 'self' https://accounts.google.com;
base-uri 'self';
object-src 'none';
```

`'unsafe-inline'` is allowed for Razor inline scripts/styles only; no
third-party scripts beyond `cdn.jsdelivr.net` (Bootstrap Icons, Chart.js) are
permitted.

---

## Getting started

### Prerequisites

- .NET 8 SDK
- MySQL 8.x (running locally or remotely)
- (Optional) `dotnet ef` CLI: `dotnet tool install --global dotnet-ef`

### 1. Clone and restore

```bash
git clone <repo-url>
cd "AD COURSEWORK 2"
dotnet restore
```

### 2. Configure the database

Edit `appsettings.Development.json` (gitignored) or `appsettings.json` and set:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=unimanage;User=root;Password=YOUR_PASSWORD;TreatTinyAsBoolean=true;CharSet=utf8mb4;"
}
```

### 3. Apply migrations

The application will run migrations automatically on startup. To do it
manually:

```bash
dotnet ef database update
```

### 4. (Optional) Configure email

To enable welcome emails, login alerts, password resets, and material upload
notifications, set the SMTP settings:

```json
"Email": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSsl": true,
  "SenderName": "UniManage LMS",
  "SenderEmail": "you@example.com",
  "Username": "you@example.com",
  "Password": "YOUR_APP_PASSWORD"
}
```

### 5. (Optional) Configure Google sign-in

```json
"Authentication": {
  "Google": {
    "ClientId": "...",
    "ClientSecret": "..."
  }
}
```

If `ClientSecret` is empty the Google button shows a friendly message.

### 6. Run

```bash
dotnet run
```

Then visit `https://localhost:5001` (or the port shown in the console).

---

## Default seeded accounts

On first run, `DbInitializer` creates these accounts (change passwords after
first login!):

| Role          | Email                          | Password       |
| ------------- | ------------------------------ | -------------- |
| Administrator | `admin@unimanage.local`        | `Admin123!`    |
| Lecturer      | `lecturer@unimanage.local`     | `Lecturer123!` |
| Student       | `student@unimanage.local`      | `Student123!`  |

Two demo courses (`CS101`, `CS201`) and one assignment are also seeded.

---

## Project structure

```
AD COURSEWORK 2/
├── Controllers/
│   ├── AccountController.cs        — register, login, logout, reset, Google
│   ├── AssignmentsController.cs    — lecturer CRUD + student "Mine" view
│   ├── AuditLogsController.cs      — admin audit log browser
│   ├── CoursesController.cs        — admin CRUD + materials + details
│   ├── DashboardController.cs      — Student/Lecturer/Administrator dashboards
│   ├── EnrollmentsController.cs    — student catalog + enroll
│   ├── HomeController.cs           — public home
│   ├── MessagesController.cs       — inbox / thread / compose
│   ├── ProfileController.cs        — profile + change password
│   ├── ReportsController.cs        — analytics views + CSV exports
│   └── SubmissionsController.cs    — submit, grade, file downloads
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── ApplicationDbContextFactory.cs
│   ├── DbInitializer.cs            — roles, demo users, demo courses
│   └── Migrations/
├── Infrastructure/
│   ├── AuditLogger.cs              — IAuditLogger implementation
│   ├── CsvWriter.cs                — RFC-4180 CSV writer with UTF-8 BOM
│   ├── EmailSettings.cs / SmtpEmailService.cs / EmailTemplates.cs
│   ├── SecurityHeadersMiddleware.cs
│   └── UploadedFileStore.cs        — file allow-list, save, fetch, delete
├── Models/                         — domain entities + AuditLog + AppRoles
├── ViewModels/                     — input/output VMs (RegisterVM, etc.)
├── Views/                          — Razor views grouped by controller
├── wwwroot/                        — static assets, uploaded files (`uploads/`)
├── Program.cs                      — DI, middleware pipeline, rate limiter
├── appsettings.json
└── README.md
```

---

## Reports & analytics

Open **Reports** from the navbar (admins) or **Workload** (lecturers).

| Report              | Visualisation                | Export |
| ------------------- | ---------------------------- | ------ |
| Course popularity   | Grouped bar (enrolled vs. capacity) | CSV |
| Student performance | Bar chart (avg %)            | CSV    |
| Lecturer workload   | Grouped bar (courses/assignments/subs) | CSV |
| Enrollments         | Line chart by day            | CSV    |

CSVs are UTF-8 with BOM so they open correctly in Excel.

---

## Audit log

Browse from the navbar **Audit** (admins). Every entry contains:

- Timestamp (UTC)
- Category (`Auth`, `Course`, `Assignment`, `Submission`, `Enrollment`,
  `Material`, `Profile`, `Security`)
- Action label
- User name and id (if known)
- IP address and user agent
- Free-form detail (e.g. `assignmentId=12 grade=85/100`)
- Success flag (failures shown highlighted)

Logged events include: registration, login success, login failure, account
lockout, logout, password reset, course CRUD, material upload (and rejection),
enrollment, submission, grading, profile update, password change.

---

## Configuration reference

All configuration is in `appsettings.json` and `appsettings.Development.json`.
Sensitive values (DB password, SMTP password, Google client secret) should be
moved to **User Secrets** (`dotnet user-secrets`) or environment variables in
production.

| Section / key                                | Purpose                                  |
| -------------------------------------------- | ---------------------------------------- |
| `ConnectionStrings:DefaultConnection`        | MySQL connection string                  |
| `Email:Host` / `Port` / `EnableSsl`          | SMTP host configuration                  |
| `Email:SenderName` / `SenderEmail`           | "From" header                            |
| `Email:Username` / `Password`                | SMTP credentials                         |
| `Authentication:Google:ClientId` / `Secret`  | Google OAuth client                      |
| `Logging:LogLevel:Default`                   | Log level (Information / Warning / ...)  |

---

## License

Academic coursework — provided as-is for educational purposes.
