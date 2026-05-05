# Evidence Checklist

Student Name: Yasas Pasindu Fernando  
Student ID: 25026764  
Project: UniManage - University Course Management System  
Submission Type: Approved individual submission

Following approval to complete the coursework individually, the author completed the implementation, testing, documentation, and final preparation as an individual submission.

Table 9: Individual Evidence Checklist

| Evidence Item | Required Evidence | Current Location | Status |
|---|---|---|---|
| Final main technical report | Completed report in Word/PDF | `Main_Application_Development_Report.md` | Draft complete |
| Individual contribution report | Supporting individual contribution document | `Expanded_Individual_Contribution.md` | Draft complete |
| Installation guide and user manual | User manual converted to Word/PDF | `User_Manual_Application_Development.md` | Draft complete |
| Visual Studio solution/source code | Solution and project files | `AD COURSEWORK 2.sln`, `AD COURSEWORK 2.csproj` | Confirmed |
| Repository link | Public or accessible source repository | `Source_And_Build_Links.md` | [NEEDS CONFIRMATION] |
| Packaged build/deployment link | ZIP, hosted build, or packaged artefact link | `Source_And_Build_Links.md` | [NEEDS CONFIRMATION] |
| Database script / migration / database evidence | Migration list, database screenshot, backup, or script | `Data/Migrations/` | Migrations confirmed, screenshot needed |
| Architecture diagram | Exported Figure 1 | `diagrams/architecture_diagram.mmd` | Source complete, PNG/PDF export needed |
| ER diagram | Exported Figure 3 | `diagrams/er_diagram.mmd` | Source complete, PNG/PDF export needed |
| UML class diagram | Exported Figure 4 | `diagrams/class_diagram.mmd` | Source complete, PNG/PDF export needed |
| Use case diagram | Exported Figure 2 | `diagrams/use_case_diagram.mmd` | Source complete, PNG/PDF export needed |
| Application flowchart | Exported Figure 5 | `diagrams/application_flowchart.mmd` | Source complete, PNG/PDF export needed |
| Authentication/role access flow diagram | Exported Figure 6 | `diagrams/security_flow_diagram.mmd` | Source complete, PNG/PDF export needed |
| Login screenshot | Figure 7 | `screenshots/` | [NEEDS CONFIRMATION] |
| Registration screenshot | Figure 7 or user manual Figure UM3 | `screenshots/` | [NEEDS CONFIRMATION] |
| Student dashboard screenshot | Figure 8 | `screenshots/` | [NEEDS CONFIRMATION] |
| Lecturer dashboard screenshot | Figure 9 | `screenshots/` | [NEEDS CONFIRMATION] |
| Administrator dashboard screenshot | Figure 10 | `screenshots/` | [NEEDS CONFIRMATION] |
| Course management screenshot | Figure 11 | `screenshots/` | [NEEDS CONFIRMATION] |
| Enrollment screenshot | Figure 12 | `screenshots/` | [NEEDS CONFIRMATION] |
| Assignment/grading screenshot | Figure 13 | `screenshots/` | [NEEDS CONFIRMATION] |
| Reporting screenshot | Figure 14 | `screenshots/` | [NEEDS CONFIRMATION] |
| Communication module screenshot | Figure 15 | `screenshots/` | [NEEDS CONFIRMATION] |
| Validation/error handling screenshot | Figure 16 | `screenshots/` | [NEEDS CONFIRMATION] |
| Test plan and results | Completed testing table with actual results | Section 13 of main report | Manual results needed |
| Individual approval evidence if available | Approval screenshot, email, or note | `evidence/` | [NEEDS CONFIRMATION] |
| Viva demonstration notes | Short feature explanation and order | User manual and QA report | Draft prepared |

## Source Features Confirmed

- ASP.NET Core MVC project targeting .NET 8.
- Entity Framework Core with Pomelo MySQL provider.
- ASP.NET Core Identity and role constants.
- Controllers, models, view models, Razor views, migrations, reports, messaging, meetings, validation, and security infrastructure.
- Successful build with `dotnet build --no-restore`.
- Supporting hosted demonstration on Google Cloud through the intended custom-domain URL `https://lms.yasaboy.com/`, with DNS record evidence in Figure UM21, server reachability evidence in Figure UM22, and the earlier IP-based endpoint `http://34.171.6.234:8080` retained as backup evidence.

## Hosting and Deployment Evidence Checklist

The following items support the hosted demonstration described in Section 8.20 of the main report and Section 5A of the user manual. Each item is a manual capture step that must be completed before the final submission. The items remain `[ ]` until the evidence file has been saved into `documentation_application_development/screenshots/` or `documentation_application_development/evidence/`.

- [x] Figure UM21 Cloudflare DNS A record screenshot inserted
- [x] Figure UM22 Nginx default page screenshot inserted
- [x] Cloudflare proxy status checked and described accurately
- [ ] Nginx reverse proxy status verified or left as [VERIFY]
- [ ] Final custom domain application homepage screenshot inserted
- [ ] Figure UM19 custom HTTPS domain homepage screenshot showing `https://lms.yasaboy.com/` rendered in a browser, with the address bar and application UI visible.
- [ ] Hosted login page screenshot taken from the custom-domain URL.
- [ ] Domain / HTTPS browser padlock evidence (a clear capture of the padlock icon and the certificate summary) once UniManage is confirmed on HTTPS for the custom domain.
- [ ] Google Cloud VM or deployment dashboard screenshot showing the virtual machine in the running state.
- [ ] Docker container running evidence (`docker ps` output, the Docker desktop window, or an equivalent screenshot).
- [ ] Optional: earlier IP-based endpoint screenshot showing `http://34.171.6.234:8080` rendered in a browser, retained as backup evidence.
- [ ] Optional: Dockerfile screenshot or short code listing showing the .NET 8 base image and the application entry point.
- [ ] Optional: `docker build` command evidence showing the image being created locally or on the host.
- [ ] Confirmation that no secrets are visible in any of the screenshots above (database password, SMTP password, Google client secret, SSH key, DNS or provider credential, service account key).
- [ ] Local Visual Studio build evidence retained (from `dotnet build --no-restore` and from Visual Studio 2022 opening the solution successfully).

## Manual Evidence Still Required

- Final screenshots.
- Exported diagrams as PNG or PDF.
- Repository link confirmation.
- Packaged build or deployment link.
- Database screenshot, backup, or migration evidence.
- Individual approval evidence if available.
- Hosted demonstration screenshots and Docker / Google Cloud evidence as listed in the section above.
- HTTPS, custom domain, environment-based secret management, deployment automation, uptime monitoring, and database backup automation are tracked as future improvements rather than as outstanding evidence items.
- Final Word/PDF export.
