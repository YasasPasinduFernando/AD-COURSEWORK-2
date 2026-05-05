using System.Text;
using System.Text.Encodings.Web;

namespace AD_COURSEWORK_2.Infrastructure;

/// <summary>
/// Builds branded, email-client-safe HTML for all UniManage emails.
/// Every method returns a complete HTML document with inline styles so it
/// renders consistently in Gmail, Outlook, Apple Mail, and mobile clients.
/// </summary>
public static class EmailTemplates
{
    private const string BrandName = "UniManage";
    private const string BrandTagline = "University Course Management System";
    private const string ColorPrimary = "#1e3a8a";
    private const string ColorPrimaryBright = "#2563eb";
    private const string ColorAccent = "#22d3ee";
    private const string ColorText = "#0b1220";
    private const string ColorMuted = "#64748b";
    private const string ColorBorder = "#e5ecf5";
    private const string ColorSurface = "#f6f8fc";
    private const string ColorSuccess = "#047857";
    private const string ColorWarn = "#b45309";
    private const string ColorDanger = "#be123c";

    public static string BuildWelcomeEmail(string fullName, string role)
    {
        var name = SafeName(fullName);
        var roleSafe = HtmlEncoder.Default.Encode(role ?? string.Empty);

        var body = new StringBuilder()
            .Append(P($"Hi <strong>{name}</strong>,"))
            .Append(P($"Welcome aboard! Your <strong>{BrandName}</strong> account has been created successfully as a <strong>{roleSafe}</strong>."))
            .Append(KeyValueBlock(new (string, string)[]
            {
                ("Name", name),
                ("Role", roleSafe),
                ("Joined", HtmlEncoder.Default.Encode(DateTime.Now.ToString("f")))
            }))
            .Append(P("You can now sign in and start exploring your personalized dashboard."))
            .ToString();

        return BuildShell(
            preheader: $"Welcome to {BrandName} — your account is ready.",
            accent: GradientPrimary,
            icon: "\u2713",
            eyebrow: "Account ready",
            heading: $"Welcome to {BrandName}, {name}!",
            subheading: "Your learning space is ready to go.",
            bodyHtml: body,
            cta: null,
            footerExtras: "If you did not create this account, please ignore this email or contact your administrator.");
    }

    public static string BuildLoginAlertEmail(string fullName, DateTime whenLocal, string ipAddress, string? userAgent = null)
    {
        var name = SafeName(fullName);
        var time = HtmlEncoder.Default.Encode(whenLocal.ToString("f"));
        var ip = HtmlEncoder.Default.Encode(string.IsNullOrWhiteSpace(ipAddress) ? "Unknown" : ipAddress);
        var agent = HtmlEncoder.Default.Encode(string.IsNullOrWhiteSpace(userAgent) ? "Web browser" : userAgent);

        var body = new StringBuilder()
            .Append(P($"Hi <strong>{name}</strong>,"))
            .Append(P($"We detected a new sign-in to your <strong>{BrandName}</strong> account. If this was you, no action is required."))
            .Append(KeyValueBlock(new (string, string)[]
            {
                ("When", time),
                ("IP address", ip),
                ("Device", agent)
            }))
            .Append(Callout("If you don't recognise this sign-in, reset your password immediately and contact your administrator.", ColorWarn, "#fffbeb", "#fde68a"))
            .ToString();

        return BuildShell(
            preheader: "A new sign-in to your UniManage account was detected.",
            accent: GradientPrimary,
            icon: "!",
            eyebrow: "Security alert",
            heading: "New sign-in detected",
            subheading: "Here are the details we captured for this session.",
            bodyHtml: body,
            cta: null,
            footerExtras: "You're receiving this email because login alerts are enabled for your account.");
    }

    public static string BuildPasswordResetEmail(string fullName, string resetUrl)
    {
        var name = SafeName(fullName);
        var urlSafe = HtmlEncoder.Default.Encode(resetUrl ?? string.Empty);

        var body = new StringBuilder()
            .Append(P($"Hi <strong>{name}</strong>,"))
            .Append(P($"We received a request to reset the password for your <strong>{BrandName}</strong> account. Use the button below to choose a new password."))
            .Append(P($"<span style=\"color:{ColorMuted};font-size:13px;\">This link is valid for a limited time and can only be used once. If it expires, you can request another from the sign-in page.</span>"))
            .Append(Callout("If you didn't request a password reset, you can safely ignore this email — your password won't change.", ColorMuted, "#f8fafc", ColorBorder))
            .ToString();

        return BuildShell(
            preheader: "Reset your UniManage password.",
            accent: GradientViolet,
            icon: "\u21BB",
            eyebrow: "Password reset",
            heading: "Reset your password",
            subheading: "Click the button below to set a new password.",
            bodyHtml: body,
            cta: new CtaButton("Reset password", urlSafe),
            footerExtras: "For your security, we never email your existing password. This link expires automatically.");
    }

    public static string BuildMeetingInviteEmail(
        string fullName,
        string courseCode,
        string courseName,
        string meetingTitle,
        string? meetingDescription,
        DateTime scheduledLocal,
        int durationMinutes,
        string meetingUrl,
        string? lecturerName,
        string? googleCalendarUrl = null,
        string? outlookCalendarUrl = null,
        string? icsDownloadUrl = null)
    {
        var name = SafeName(fullName);
        var codeSafe = HtmlEncoder.Default.Encode(courseCode ?? string.Empty);
        var courseSafe = HtmlEncoder.Default.Encode(courseName ?? string.Empty);
        var titleSafe = HtmlEncoder.Default.Encode(meetingTitle ?? string.Empty);
        var lecturerSafe = HtmlEncoder.Default.Encode(string.IsNullOrWhiteSpace(lecturerName) ? "Your lecturer" : lecturerName);
        var urlSafe = HtmlEncoder.Default.Encode(meetingUrl ?? string.Empty);
        var descSafe = string.IsNullOrWhiteSpace(meetingDescription)
            ? null
            : HtmlEncoder.Default.Encode(meetingDescription);

        var body = new StringBuilder()
            .Append(P($"Hi <strong>{name}</strong>,"))
            .Append(P($"<strong>{lecturerSafe}</strong> has scheduled a live session for <strong>{codeSafe} — {courseSafe}</strong>. Save the time and join from the link below."))
            .Append(KeyValueBlock(new (string, string)[]
            {
                ("Topic", titleSafe),
                ("Course", $"{codeSafe} — {courseSafe}"),
                ("When", HtmlEncoder.Default.Encode(scheduledLocal.ToString("dddd, MMM d, yyyy · h:mm tt"))),
                ("Duration", $"{durationMinutes} minutes"),
                ("Hosted by", lecturerSafe),
                ("Meet link", $"<a href=\"{urlSafe}\" style=\"color:{ColorPrimaryBright};text-decoration:none;font-weight:600;\">{urlSafe}</a>")
            }))
            .Append(descSafe != null ? Callout(descSafe, ColorPrimaryBright, "#eef4ff", "#cfddf6") : "")
            .Append(BuildAddToCalendarBlock(googleCalendarUrl, outlookCalendarUrl, icsDownloadUrl))
            .Append(P("The .ics calendar file is attached — most email apps will offer to add it to your calendar in one tap."))
            .ToString();

        return BuildShell(
            preheader: $"Live session: {titleSafe} — {scheduledLocal:MMM d, h:mm tt}",
            accent: GradientPrimary,
            icon: "\u25B6",
            eyebrow: "Live session",
            heading: $"You're invited: {titleSafe}",
            subheading: $"{codeSafe} · {scheduledLocal:dddd, MMM d, h:mm tt}",
            bodyHtml: body,
            cta: new CtaButton("Join meeting", urlSafe),
            footerExtras: "You're receiving this email because you're enrolled in this course on UniManage.");
    }

    private static string BuildAddToCalendarBlock(string? googleUrl, string? outlookUrl, string? icsUrl)
    {
        if (string.IsNullOrWhiteSpace(googleUrl) && string.IsNullOrWhiteSpace(outlookUrl) && string.IsNullOrWhiteSpace(icsUrl))
            return string.Empty;

        var sb = new StringBuilder();
        sb.Append($@"<div style=""margin:6px 0 16px;padding:14px 16px;background:#f6f8fc;border:1px solid {ColorBorder};border-radius:12px;"">
  <div style=""font-size:11px;font-weight:700;letter-spacing:0.12em;text-transform:uppercase;color:{ColorMuted};margin-bottom:10px;"">Add to your calendar</div>
  <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"">
    <tr>");

        if (!string.IsNullOrWhiteSpace(googleUrl))
        {
            sb.Append($@"<td style=""padding-right:8px;""><a href=""{HtmlEncoder.Default.Encode(googleUrl)}"" target=""_blank"" rel=""noopener"" style=""display:inline-block;padding:9px 14px;border-radius:8px;background:#ffffff;border:1px solid {ColorBorder};color:{ColorText};text-decoration:none;font-size:13px;font-weight:700;"">📅 Google Calendar</a></td>");
        }

        if (!string.IsNullOrWhiteSpace(outlookUrl))
        {
            sb.Append($@"<td style=""padding-right:8px;""><a href=""{HtmlEncoder.Default.Encode(outlookUrl)}"" target=""_blank"" rel=""noopener"" style=""display:inline-block;padding:9px 14px;border-radius:8px;background:#ffffff;border:1px solid {ColorBorder};color:{ColorText};text-decoration:none;font-size:13px;font-weight:700;"">📆 Outlook</a></td>");
        }

        if (!string.IsNullOrWhiteSpace(icsUrl))
        {
            sb.Append($@"<td><a href=""{HtmlEncoder.Default.Encode(icsUrl)}"" target=""_blank"" rel=""noopener"" style=""display:inline-block;padding:9px 14px;border-radius:8px;background:#ffffff;border:1px solid {ColorBorder};color:{ColorText};text-decoration:none;font-size:13px;font-weight:700;"">⬇ Download .ics</a></td>");
        }

        sb.Append("</tr></table></div>");
        return sb.ToString();
    }

    public static string BuildSubmissionReceivedEmail(
        string lecturerFullName,
        string studentName,
        string courseCode,
        string courseName,
        string assignmentTitle,
        DateTime submittedLocal,
        bool isLate,
        string? gradeUrl)
    {
        var name = SafeName(lecturerFullName);
        var studentSafe = HtmlEncoder.Default.Encode(studentName ?? string.Empty);
        var codeSafe = HtmlEncoder.Default.Encode(courseCode ?? string.Empty);
        var courseSafe = HtmlEncoder.Default.Encode(courseName ?? string.Empty);
        var titleSafe = HtmlEncoder.Default.Encode(assignmentTitle ?? string.Empty);

        var body = new StringBuilder()
            .Append(P($"Hi <strong>{name}</strong>,"))
            .Append(P($"A new submission has arrived for <strong>{titleSafe}</strong> in <strong>{codeSafe} — {courseSafe}</strong>."))
            .Append(KeyValueBlock(new (string, string)[]
            {
                ("Student", studentSafe),
                ("Assignment", titleSafe),
                ("Course", $"{codeSafe} — {courseSafe}"),
                ("Submitted", HtmlEncoder.Default.Encode(submittedLocal.ToString("dddd, MMM d, h:mm tt"))),
                ("Status", isLate ? "<span style=\"color:" + ColorWarn + ";font-weight:700;\">Late submission</span>" : "<span style=\"color:" + ColorSuccess + ";font-weight:700;\">On time</span>")
            }))
            .Append(P("Review the submission and provide grade and feedback when convenient."))
            .ToString();

        CtaButton? cta = null;
        if (!string.IsNullOrWhiteSpace(gradeUrl))
            cta = new CtaButton("Open and grade", HtmlEncoder.Default.Encode(gradeUrl));

        return BuildShell(
            preheader: $"New submission from {studentSafe} — {titleSafe}",
            accent: GradientViolet,
            icon: "\u25B2",
            eyebrow: "New submission",
            heading: $"New submission from {studentSafe}",
            subheading: $"{codeSafe} · {titleSafe}",
            bodyHtml: body,
            cta: cta,
            footerExtras: "You're receiving this because you teach this course on UniManage.");
    }

    public static string BuildGradeReleasedEmail(
        string studentFullName,
        string courseCode,
        string courseName,
        string assignmentTitle,
        decimal grade,
        decimal maxPoints,
        string? feedback,
        string? lecturerName,
        string? viewUrl)
    {
        var name = SafeName(studentFullName);
        var codeSafe = HtmlEncoder.Default.Encode(courseCode ?? string.Empty);
        var courseSafe = HtmlEncoder.Default.Encode(courseName ?? string.Empty);
        var titleSafe = HtmlEncoder.Default.Encode(assignmentTitle ?? string.Empty);
        var lecturerSafe = HtmlEncoder.Default.Encode(string.IsNullOrWhiteSpace(lecturerName) ? "Your lecturer" : lecturerName);

        var pct = maxPoints > 0 ? (int)Math.Round((double)(grade / maxPoints) * 100) : 0;
        var pctColor = pct >= 75 ? ColorSuccess : pct >= 50 ? ColorPrimaryBright : pct >= 40 ? ColorWarn : ColorDanger;
        var pctLabel = pct >= 75 ? "Excellent" : pct >= 50 ? "Good" : pct >= 40 ? "Pass" : "Needs improvement";

        var body = new StringBuilder()
            .Append(P($"Hi <strong>{name}</strong>,"))
            .Append(P($"<strong>{lecturerSafe}</strong> has graded your submission for <strong>{titleSafe}</strong> in <strong>{codeSafe} — {courseSafe}</strong>."))
            .Append($@"<div style=""text-align:center;padding:20px 16px;background:{ColorSurface};border:1px solid {ColorBorder};border-radius:12px;margin:6px 0 16px;"">
  <div style=""font-size:11px;font-weight:700;letter-spacing:0.12em;text-transform:uppercase;color:{ColorMuted};"">Your score</div>
  <div style=""font-size:38px;font-weight:800;color:{pctColor};letter-spacing:-0.02em;line-height:1.05;margin-top:4px;"">{HtmlEncoder.Default.Encode(grade.ToString("0.##"))} / {HtmlEncoder.Default.Encode(maxPoints.ToString("0.##"))}</div>
  <div style=""font-size:14px;font-weight:700;color:{pctColor};margin-top:4px;"">{pct}% · {pctLabel}</div>
</div>")
            .Append(KeyValueBlock(new (string, string)[]
            {
                ("Assignment", titleSafe),
                ("Course", $"{codeSafe} — {courseSafe}"),
                ("Graded by", lecturerSafe),
                ("When", HtmlEncoder.Default.Encode(DateTime.Now.ToString("f")))
            }))
            .Append(string.IsNullOrWhiteSpace(feedback)
                ? Callout("No feedback comments were left for this submission.", ColorMuted, "#f8fafc", ColorBorder)
                : $@"<div style=""margin:6px 0 16px;padding:14px 16px;background:#fffbeb;border:1px solid #fde68a;border-left:4px solid {ColorWarn};border-radius:10px;color:{ColorText};font-size:14px;line-height:1.55;"">
  <div style=""font-size:11px;font-weight:700;letter-spacing:0.1em;text-transform:uppercase;color:{ColorWarn};margin-bottom:6px;"">Lecturer feedback</div>
  <div style=""white-space:pre-wrap;"">{HtmlEncoder.Default.Encode(feedback)}</div>
</div>")
            .ToString();

        CtaButton? cta = null;
        if (!string.IsNullOrWhiteSpace(viewUrl))
            cta = new CtaButton("View submission", HtmlEncoder.Default.Encode(viewUrl));

        return BuildShell(
            preheader: $"Your grade for {titleSafe}: {grade}/{maxPoints} ({pct}%)",
            accent: pct >= 50 ? GradientEmerald : GradientViolet,
            icon: pct >= 50 ? "\u2713" : "\u270E",
            eyebrow: "Grade released",
            heading: $"Your grade for {titleSafe}",
            subheading: $"{codeSafe} · {grade}/{maxPoints} · {pct}%",
            bodyHtml: body,
            cta: cta,
            footerExtras: "You're receiving this email because you submitted to this assignment on UniManage.");
    }

    public static string BuildMaterialUploadEmail(
        string fullName,
        string courseCode,
        string courseName,
        string materialTitle,
        string? lecturerName,
        string? courseUrl)
    {
        var name = SafeName(fullName);
        var codeSafe = HtmlEncoder.Default.Encode(courseCode ?? string.Empty);
        var courseSafe = HtmlEncoder.Default.Encode(courseName ?? string.Empty);
        var titleSafe = HtmlEncoder.Default.Encode(materialTitle ?? string.Empty);
        var lecturerSafe = HtmlEncoder.Default.Encode(string.IsNullOrWhiteSpace(lecturerName) ? "Your lecturer" : lecturerName);

        var body = new StringBuilder()
            .Append(P($"Hi <strong>{name}</strong>,"))
            .Append(P($"New learning material has been uploaded for <strong>{codeSafe} — {courseSafe}</strong>. Sign in to review it at your convenience."))
            .Append(KeyValueBlock(new (string, string)[]
            {
                ("Course", $"{codeSafe} — {courseSafe}"),
                ("Material", titleSafe),
                ("Uploaded by", lecturerSafe),
                ("When", HtmlEncoder.Default.Encode(DateTime.Now.ToString("f")))
            }))
            .Append(P("Keep up the great work — staying on top of new content is key to success."))
            .ToString();

        CtaButton? cta = null;
        if (!string.IsNullOrWhiteSpace(courseUrl))
        {
            cta = new CtaButton("Open course", HtmlEncoder.Default.Encode(courseUrl));
        }

        return BuildShell(
            preheader: $"New material in {codeSafe}: {titleSafe}",
            accent: GradientEmerald,
            icon: "\u25CE",
            eyebrow: "Course update",
            heading: "New lesson material uploaded",
            subheading: $"{codeSafe} · {courseSafe}",
            bodyHtml: body,
            cta: cta,
            footerExtras: "You're receiving this email because you're enrolled in this course on UniManage.");
    }

    // ============================================================
    // Shell + primitives
    // ============================================================

    private const string GradientPrimary = "linear-gradient(135deg,#0b1a44 0%,#1e3a8a 45%,#4f46e5 80%,#22d3ee 130%)";
    private const string GradientViolet = "linear-gradient(135deg,#312e81 0%,#6d28d9 50%,#8b5cf6 100%)";
    private const string GradientEmerald = "linear-gradient(135deg,#064e3b 0%,#047857 50%,#10b981 100%)";

    private sealed record CtaButton(string Label, string Url);

    private static string BuildShell(
        string preheader,
        string accent,
        string icon,
        string eyebrow,
        string heading,
        string subheading,
        string bodyHtml,
        CtaButton? cta,
        string footerExtras)
    {
        var year = DateTime.UtcNow.Year;
        var preheaderSafe = HtmlEncoder.Default.Encode(preheader ?? string.Empty);
        var eyebrowSafe = HtmlEncoder.Default.Encode(eyebrow ?? string.Empty);
        var headingSafe = HtmlEncoder.Default.Encode(heading ?? string.Empty);
        var subheadingSafe = HtmlEncoder.Default.Encode(subheading ?? string.Empty);
        var footerExtrasSafe = HtmlEncoder.Default.Encode(footerExtras ?? string.Empty);

        var ctaBlock = cta is null
            ? string.Empty
            : $@"
<tr>
  <td align=""center"" style=""padding:4px 32px 28px;"">
    <a href=""{cta.Url}"" style=""display:inline-block;padding:13px 28px;background:{GradientPrimary};color:#ffffff;text-decoration:none;border-radius:10px;font-weight:700;font-family:'Plus Jakarta Sans',Inter,Arial,sans-serif;font-size:15px;letter-spacing:-0.01em;box-shadow:0 10px 22px rgba(37,99,235,0.22);"">
      {HtmlEncoder.Default.Encode(cta.Label)}
    </a>
  </td>
</tr>";

        return $@"<!doctype html>
<html lang=""en"">
<head>
  <meta charset=""utf-8"" />
  <meta name=""viewport"" content=""width=device-width,initial-scale=1"" />
  <meta name=""color-scheme"" content=""light"" />
  <meta name=""supported-color-schemes"" content=""light"" />
  <title>{headingSafe}</title>
</head>
<body style=""margin:0;padding:0;background:{ColorSurface};font-family:'Plus Jakarta Sans',Inter,-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Arial,sans-serif;color:{ColorText};"">
  <div style=""display:none;max-height:0;overflow:hidden;opacity:0;color:transparent;"">{preheaderSafe}</div>
  <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""background:{ColorSurface};padding:28px 12px;"">
    <tr>
      <td align=""center"">
        <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""600"" style=""max-width:600px;width:100%;background:#ffffff;border:1px solid {ColorBorder};border-radius:18px;overflow:hidden;box-shadow:0 24px 60px rgba(15,23,42,0.08);"">
          <tr>
            <td style=""padding:18px 28px;border-bottom:1px solid {ColorBorder};background:#ffffff;"">
              <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                <tr>
                  <td align=""left"" style=""vertical-align:middle;"">
                    <span style=""display:inline-block;width:30px;height:30px;border-radius:9px;background:linear-gradient(135deg,#22d3ee,#8b5cf6);vertical-align:middle;margin-right:10px;line-height:30px;text-align:center;color:#ffffff;font-size:14px;font-weight:800;letter-spacing:-0.02em;"">UM</span>
                    <span style=""font-weight:800;font-size:17px;letter-spacing:-0.02em;color:{ColorText};vertical-align:middle;"">{BrandName}</span>
                  </td>
                  <td align=""right"" style=""vertical-align:middle;color:{ColorMuted};font-size:12px;font-weight:600;text-transform:uppercase;letter-spacing:0.08em;"">
                    {BrandTagline}
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td style=""padding:0;"">
              <div style=""background:{accent};padding:30px 28px 28px;color:#ffffff;"">
                <div style=""font-size:11px;font-weight:700;letter-spacing:0.14em;text-transform:uppercase;opacity:0.88;"">{eyebrowSafe}</div>
                <div style=""display:inline-block;width:52px;height:52px;border-radius:14px;background:rgba(255,255,255,0.18);border:1px solid rgba(255,255,255,0.28);text-align:center;line-height:52px;font-size:26px;margin:10px 0 14px;"">{icon}</div>
                <div style=""font-size:24px;font-weight:800;letter-spacing:-0.02em;line-height:1.2;"">{headingSafe}</div>
                <div style=""font-size:14px;opacity:0.9;margin-top:6px;"">{subheadingSafe}</div>
              </div>
            </td>
          </tr>
          <tr>
            <td style=""padding:28px 32px 10px;font-size:15px;line-height:1.6;color:{ColorText};"">
              {bodyHtml}
            </td>
          </tr>
          {ctaBlock}
          <tr>
            <td style=""padding:20px 32px 28px;border-top:1px solid {ColorBorder};background:{ColorSurface};font-size:12px;line-height:1.55;color:{ColorMuted};"">
              <div style=""margin-bottom:8px;color:{ColorMuted};"">{footerExtrasSafe}</div>
              <div style=""color:{ColorMuted};"">© {year} <strong style=""color:{ColorText};"">{BrandName}</strong> · {BrandTagline}</div>
              <div style=""margin-top:6px;color:#94a3b8;font-size:11px;"">This is an automated message — please do not reply directly.</div>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
    }

    // ------- small helpers -------
    private static string SafeName(string? name)
    {
        var v = string.IsNullOrWhiteSpace(name) ? "there" : name!;
        return HtmlEncoder.Default.Encode(v);
    }

    private static string P(string innerHtml) =>
        $"<p style=\"margin:0 0 14px;color:{ColorText};font-size:15px;line-height:1.6;\">{innerHtml}</p>";

    private static string KeyValueBlock(IEnumerable<(string Label, string Value)> pairs)
    {
        var sb = new StringBuilder();
        sb.Append($"<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"background:{ColorSurface};border:1px solid {ColorBorder};border-radius:12px;margin:6px 0 16px;\">");
        var items = pairs.ToList();
        for (var i = 0; i < items.Count; i++)
        {
            var (label, value) = items[i];
            var borderTop = i == 0 ? "" : $"border-top:1px dashed {ColorBorder};";
            sb.Append("<tr>")
              .Append($"<td style=\"padding:10px 14px;{borderTop}color:{ColorMuted};font-size:12px;font-weight:700;letter-spacing:0.06em;text-transform:uppercase;width:120px;\">{HtmlEncoder.Default.Encode(label)}</td>")
              .Append($"<td style=\"padding:10px 14px;{borderTop}color:{ColorText};font-size:14px;font-weight:600;\">{value}</td>")
              .Append("</tr>");
        }
        sb.Append("</table>");
        return sb.ToString();
    }

    private static string Callout(string text, string accent, string bg, string border)
    {
        return $"<div style=\"margin:6px 0 16px;padding:12px 14px;background:{bg};border:1px solid {border};border-left:4px solid {accent};border-radius:10px;color:{accent};font-size:13px;line-height:1.5;font-weight:600;\">{text}</div>";
    }
}
