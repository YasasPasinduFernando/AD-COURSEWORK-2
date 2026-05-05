# Source and Build Links

Project: UniManage - University Course Management System  
Student: Yasas Pasindu Fernando  
Student ID: 25026764

Following approval to complete the coursework individually, the author completed the implementation, testing, documentation, and final preparation as an individual submission.

## Project Repository

[PASTE REPOSITORY LINK HERE]

Detected Git remote from source scan:

`https://github.com/YasasPasinduFernando/AD-COURSEWORK-2.git`

## Packaged Build / Deployment

Local build command verified:

```text
dotnet build --no-restore
```

Build result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Hosted Demonstration

Intended primary hosted demonstration URL:

```text
https://lms.yasaboy.com/
```

Previous IP-Based Endpoint:

```text
http://34.171.6.234:8080
```

Hosting Platform:

Google Cloud (Compute Engine virtual machine).

Deployment Method:

Docker container deployment.

Domain / HTTPS:

The hostname `lms.yasaboy.com` is evidenced as an A record to `34.171.6.234` in Figure UM21 (DNS only in Cloudflare). Figure UM22 shows the default Nginx page on the same host label over HTTP with a Not secure browser warning; it is not evidence of the UniManage application or of HTTPS on the custom domain. Figure UM19 should show the UniManage home page on `https://lms.yasaboy.com/` before that URL is treated as fully application-evidenced in the submission package.

Deployment Notes:

The custom-domain URL is included as the intended supporting demonstration address. The earlier IP-based endpoint is retained as a backup evidence item in case the custom-domain endpoint is temporarily unreachable. The application should also be tested locally in Visual Studio because the coursework assessment environment is based on Visual Studio 2017 or higher. The cloud virtual machine and the Docker container must be running for the hosted URL to respond. If both hosted endpoints are unreachable on the day of marking, the local Visual Studio installation route in the user manual is the supported alternative.

Reverse Proxy and DNS:

Figure UM21 provides Cloudflare DNS record evidence (DNS only, not proxied). Figure UM22 provides Nginx installation and reachability evidence only; reverse proxy completion remains `[VERIFY]` in Section 8.20 and Appendix E.4 until configuration or application routing evidence is added.

Security Note:

Do not include database passwords, SSH keys, Google Cloud service account keys, SMTP credentials, Google OAuth client secrets, or private DNS or provider credentials in the report or in the public repository. Any screenshot of `appsettings.json`, of the Google Cloud console, of the SSH session, of the DNS provider dashboard, or of the reverse proxy configuration must be redacted before insertion.

## Database Script / Backup

[PASTE LINK OR FILE PATH HERE]

Detected migration folder:

`Data/Migrations/`

Detected migrations:

- `20260408041036_InitialUniManage.cs`
- `20260501122117_AddAuditLogAndSecurity.cs`
- `20260501125135_AddMeetings.cs`
- `ApplicationDbContextModelSnapshot.cs`

Raw SQL script or database backup: [NEEDS CONFIRMATION]

## Evidence Folder

[PASTE LINK HERE]

Local folder:

`documentation_application_development/evidence/`

## User Manual

User_Manual_Application_Development.docx

Source Markdown:

`documentation_application_development/User_Manual_Application_Development.md`

## Local Project Paths

Solution:

`AD COURSEWORK 2.sln`

Project:

`AD COURSEWORK 2.csproj`

Launch settings:

`Properties/launchSettings.json`

Detected local URLs:

- `https://localhost:7212`
- `http://localhost:5103`

## Configuration Warning

Do not paste private database passwords, SMTP passwords, Google client secrets, or other sensitive settings into the final report. Use redacted screenshots where configuration evidence is required.
