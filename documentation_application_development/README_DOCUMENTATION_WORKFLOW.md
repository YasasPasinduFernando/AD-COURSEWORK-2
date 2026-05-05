# Documentation Workflow - CS6004ES Application Development Coursework 2

This folder contains a structured documentation pack for the UniManage - University Course Management System application. The documents are based on the current repository scan and should be completed with screenshots, diagrams, contribution evidence and final formatting before submission.

## Folder Structure

- `PROJECT_SCAN_SUMMARY.md` - factual scan summary of the current repository.
- `Main_Application_Development_Report.md` - main academic report draft.
- `Expanded_Individual_Contribution.md` - individual contribution draft and evidence template.
- `User_Manual_Application_Development.md` - user manual for administrator, lecturer and student roles.
- `Evidence_Checklist.md` - checklist for screenshots, code evidence, testing and appendices.
- `Source_And_Build_Links.md` - repository, solution, build and run references.
- `diagrams/` - folder for architecture, ERD and workflow diagrams.
- `screenshots/` - folder for coursework screenshots.
- `evidence/` - folder for build logs, database evidence and contribution evidence.
- `conversion/` - Word conversion and formatting guidance.
- `final_document/` - final Word/PDF export location.

## Evidence Rules

The report should not claim features that cannot be evidenced. If a feature is not visible in the repository, screenshots, build output, database state or project notes, it should remain marked as:

`[NEEDS CONFIRMATION]`

This marker is used for missing screenshots, unverified contribution details, unconfirmed deployment evidence, missing test evidence and any unclear coursework requirement.

## Recommended Workflow

1. Read `PROJECT_SCAN_SUMMARY.md` and confirm whether the scan accurately reflects the implemented application.
2. Capture screenshots into `screenshots/` using the names suggested in `screenshots/README_SCREENSHOTS.md`.
3. Create diagrams in `diagrams/` using the list in `diagrams/README_DIAGRAMS.md`.
4. Add build logs, migration screenshots, test notes and exported reports into `evidence/`.
5. Update `Expanded_Individual_Contribution.md` with the author's confirmed work.
6. Replace all `[NEEDS CONFIRMATION]` markers that can be supported by evidence.
7. Move unsupported or incomplete items into limitations rather than presenting them as completed.
8. Convert the main report and supporting documents into the required Word/PDF format.
9. Place the final file in `final_document/`.

## Writing Style

Use UK English and neutral academic wording. The report should use "the author" rather than first-person pronouns. It should describe implementation evidence and technical decisions without assigning blame for incomplete group work.

Preferred style:

- "The application implements role-based access control through ASP.NET Core Identity."
- "The repository scan did not detect an automated test project. This remains `[NEEDS CONFIRMATION]`."
- "The author contributed to `[NEEDS CONFIRMATION]`, supported by `[evidence path]`."

Avoid unsupported wording:

- "The system is fully tested" unless test evidence is attached.
- "All requirements were completed" unless the coursework brief has been mapped and verified.
- Personal or group blame statements.

## Final Checks

Before submission, confirm:

- The GitHub/source link is accessible.
- The solution builds.
- Screenshots are clear and labelled.
- Sensitive configuration values are redacted.
- The final document uses consistent heading numbering.
- References to files, screenshots and appendices match the actual folder contents.
