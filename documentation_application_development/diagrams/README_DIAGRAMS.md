# Diagram Register

This folder contains Mermaid source diagrams for UniManage - University Course Management System.

| Diagram filename | Report figure number | Purpose | Insert in report |
|---|---:|---|---|
| `architecture_diagram.mmd` | Figure 1 | Shows browser users, ASP.NET Core MVC layer, controllers, views, models, services, EF Core, database, authentication, and uploads | Section 8.1 System Architecture Diagram |
| `use_case_diagram.mmd` | Figure 2 | Shows Student, Lecturer, and Administrator use cases | Section 8.2 Use Case Diagram |
| `er_diagram.mmd` | Figure 3 | Shows entities and relationships based on source models and Identity roles | Section 8.3 Entity Relationship Diagram |
| `class_diagram.mmd` | Figure 4 | Shows actual controllers, models, services, and `ApplicationDbContext` found in the scanned code | Section 8.4 UML Class Diagram |
| `application_flowchart.mmd` | Figure 5 | Shows login, role dashboards, validation, database operations, reports, and logout | Section 8.5 Application Flowchart |
| `security_flow_diagram.mmd` | Figure 6 | Shows authentication, role detection, auth cookie creation, and unauthorised access handling | Section 8.6 Authentication and Role-Based Access Flow |

## Export Instruction to PNG/PDF

1. Open each `.mmd` file in a Mermaid-compatible editor or draw.io Mermaid import.
2. Export each diagram as PNG or PDF.
3. Use the same base filename, for example `architecture_diagram.png`.
4. Insert the exported figure into the matching report section.
5. Add the figure caption under the image in Word.

## Verification Notes

- Diagram sources use actual scanned project class names where available.
- Separate role-named controller files and a separate grading entity file were not detected, so they are not shown as implemented classes.
- Student, Lecturer, and Administrator are implemented as Identity roles rather than separate entity classes.
