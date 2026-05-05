"""Final submission cleanup pass for the UniManage Markdown sources.

This helper performs bulk text transformations that are too tedious to do by hand:

- Converts body-line `[SCREENSHOT REQUIRED] Figure X: Title. Description.` patterns into
  the figure placeholder + caption pattern requested for the final submission:

      Figure X: [INSERT SCREENSHOT HERE]

      This screenshot shows <description>.

- Converts the optional responsive-design note in Section 9.5 into a labelled optional figure.
- Leaves table-cell status flags ([SCREENSHOT REQUIRED], [VERIFY], [NEEDS CONFIRMATION])
  in evidence/status tables alone, because those are the audit trail.

It does not edit Word fields or PDFs. Run it as:

    python documentation_application_development/conversion/final_cleanup_pass.py
"""

from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]

MAIN_REPORT = ROOT / "Main_Application_Development_Report.md"
USER_MANUAL = ROOT / "User_Manual_Application_Development.md"


def _build_caption(label: str, remainder: str) -> str:
    """Return a 'This screenshot shows ...' sentence for a screenshot caption."""
    remainder = remainder.strip().rstrip(".")
    if not remainder:
        body = label.strip().rstrip(".")
        return f"This screenshot shows the {body.lower()}."

    # If the remainder already starts with "This screenshot shows", accept it as-is.
    if remainder.lower().startswith("this screenshot shows"):
        sentence = remainder.split(". ", 1)
        if len(sentence) == 1:
            return sentence[0].rstrip(".") + "."
        return sentence[0].rstrip(".") + "."

    # If the remainder starts with an instruction such as "Capture the login page ..."
    instruction_starts = (
        "capture", "open", "run", "submit", "log in", "sign", "navigate", "trigger", "show",
    )
    lowered = remainder.lower()
    if any(lowered.startswith(prefix) for prefix in instruction_starts):
        # Replace the instruction verb with a description verb where possible.
        remainder = re.sub(r"^(?i)Capture\s+", "", remainder)
        return f"This screenshot shows {remainder.rstrip('.').strip()}."

    return f"This screenshot shows {remainder.rstrip('.').strip()}."


def transform_screenshot_lines(text: str) -> str:
    pattern = re.compile(
        r"^\[SCREENSHOT REQUIRED\]\s+(Figure\s+[A-Za-z0-9]+):\s+([^\n]*)$",
        re.MULTILINE,
    )

    def replace(match: re.Match) -> str:
        figure = match.group(1).strip()
        rest = match.group(2).strip()
        # The first sentence often holds the figure title. The second sentence (if present)
        # holds the existing "This screenshot shows ..." caption. Try to split on the first
        # full stop followed by a space and a capital letter.
        title_split = re.split(r"\.\s+(?=[A-Z])", rest, maxsplit=1)
        if len(title_split) == 2:
            title, description = title_split
        else:
            title = rest
            description = ""

        if description.lower().startswith("this screenshot shows"):
            caption = description.rstrip(".").strip() + "."
        elif description:
            caption = _build_caption(title, description)
        else:
            caption = _build_caption(title, "")

        return f"{figure}: [INSERT SCREENSHOT HERE]\n\n{caption}"

    return pattern.sub(replace, text)


def transform_optional_note(text: str) -> str:
    pattern = re.compile(
        r"^\[SCREENSHOT REQUIRED\]\s+\(optional\)\s+([^\n]*)$",
        re.MULTILINE,
    )

    def replace(match: re.Match) -> str:
        original = match.group(1).strip().rstrip(".")
        return (
            "Figure 9c (optional): [INSERT SCREENSHOT HERE]\n\n"
            f"This screenshot shows {original.lower()}."
        )

    return pattern.sub(replace, text)


def transform_appendix_h_marker(text: str) -> str:
    """Soften the trailing tracking line in Appendix H without removing it.

    The original line "[SCREENSHOT REQUIRED] for each item above." is a tracking note
    rather than a figure callout. Convert it to a softer note that survives the cleanup
    while still flagging the manual action.
    """
    return text.replace(
        "[SCREENSHOT REQUIRED] for each item above.",
        "Each of the validation messages above must be captured as a screenshot before final submission.",
    )


def main() -> None:
    for path in [MAIN_REPORT, USER_MANUAL]:
        original = path.read_text(encoding="utf-8")
        new_text = original
        new_text = transform_screenshot_lines(new_text)
        new_text = transform_optional_note(new_text)
        new_text = transform_appendix_h_marker(new_text)
        if new_text != original:
            path.write_text(new_text, encoding="utf-8")
            print(f"Updated: {path}")
        else:
            print(f"No changes required: {path}")


if __name__ == "__main__":
    main()
