# Source and Build Links

Coursework: CS6004ES Application Development Coursework 2  
Application: UniManage - University Course Management System

## Source Repository

Git remote detected:

`https://github.com/YasasPasinduFernando/AD-COURSEWORK-2.git`

Current local branch detected:

`main`

Repository accessibility for marking: [NEEDS CONFIRMATION]

## Local Project Paths

Repository root:

`C:\Users\HP\source\repos\AD COURSEWORK 2`

Solution:

`C:\Users\HP\source\repos\AD COURSEWORK 2\AD COURSEWORK 2.sln`

Project:

`C:\Users\HP\source\repos\AD COURSEWORK 2\AD COURSEWORK 2.csproj`

README:

`C:\Users\HP\source\repos\AD COURSEWORK 2\README.md`

Documentation folder:

`C:\Users\HP\source\repos\AD COURSEWORK 2\documentation_application_development`

## Main Source Folders

- `Controllers/`
- `Models/`
- `ViewModels/`
- `Views/`
- `Data/`
- `Infrastructure/`
- `wwwroot/`
- `Properties/`

## Build Commands

Recommended restore and build:

```text
dotnet restore
dotnet build
```

Verified build command from the repository scan:

```text
dotnet build --no-restore
```

Verified result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Run Commands

Command-line run:

```text
dotnet run
```

Visual Studio run:

1. Open `AD COURSEWORK 2.sln`.
2. Select the `https` or `http` launch profile.
3. Run the project.

## Local Launch URLs

Detected in `Properties/launchSettings.json`:

- `https://localhost:7212`
- `http://localhost:5103`
- IIS Express SSL port: `44349`

## Database Setup

The application uses Entity Framework Core migrations with MySQL. The database context is `ApplicationDbContext`.

Useful commands:

```text
dotnet ef database update
dotnet ef migrations list
```

The repository contains:

- `Data/ApplicationDbContext.cs`
- `Data/ApplicationDbContextFactory.cs`
- `Data/DbInitializer.cs`
- `Data/Migrations/20260408041036_InitialUniManage.cs`
- `Data/Migrations/20260501122117_AddAuditLogAndSecurity.cs`
- `Data/Migrations/20260501125135_AddMeetings.cs`
- `Data/Migrations/ApplicationDbContextModelSnapshot.cs`

## Build Artefacts

Build artefact folders detected:

- `bin/Debug/net8.0/`
- `obj/`
- `artifacts_build/`

The `artifacts_build/` folder contains compiled application files, runtime configuration files, static web assets metadata and dependency DLLs.

## Configuration Notes

Configuration files detected:

- `appsettings.json`
- `appsettings.Development.json`
- `Properties/launchSettings.json`

These files may contain local database, SMTP or external authentication values. Final coursework evidence should show redacted examples only.

## External Services

External service configuration detected in code:

- MySQL database connection.
- SMTP email service.
- Optional Google login.
- Calendar links for Google Calendar and Outlook.
- Meeting URL support for Google Meet, Zoom, Teams and Webex.

Actual live service configuration and deployed environment are `[NEEDS CONFIRMATION]`.

