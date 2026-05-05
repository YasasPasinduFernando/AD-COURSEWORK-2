# Evidence Folder Guidance

This folder should store supporting evidence that does not belong directly inside the main report.

## Suggested Evidence Files

- `build_output.txt`
- `database_migrations.txt`
- `manual_test_cases.xlsx` or `manual_test_cases.md`
- `report_exports/` for generated CSV/PDF samples
- `commit_evidence/` for screenshots or exported commit logs
- `database_schema/` for ERD or table screenshots
- `redacted_configuration_sample.txt`

## Evidence Rules

- Keep evidence factual and dated.
- Do not include passwords or private keys.
- Redact database, SMTP and Google authentication secrets.
- Use the same names in the report appendices and evidence folder.

## Current Status

The project build was verified successfully on 5 May 2026 using `dotnet build --no-restore`. A saved build log should still be added here if the final submission requires attached evidence. [NEEDS CONFIRMATION]

