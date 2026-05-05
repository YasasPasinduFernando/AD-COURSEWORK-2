using System.Globalization;
using System.Text;

namespace AD_COURSEWORK_2.Infrastructure;

/// <summary>
/// Generates RFC 5545 (.ics / iCalendar) content and deep-link URLs to add an
/// event to Google Calendar / Outlook / Yahoo. No external API calls or
/// OAuth required — works with every modern calendar app.
/// </summary>
public static class CalendarLink
{
    private const string ProductId = "-//UniManage//University Course Management System//EN";

    public sealed record CalendarEvent(
        string Uid,
        string Title,
        string? Description,
        string? Location,
        DateTime StartUtc,
        DateTime EndUtc,
        string? OrganizerName,
        string? OrganizerEmail,
        string? Url);

    /// <summary>
    /// Build a complete .ics document for the given event. Suitable for
    /// download or as an email attachment of type text/calendar.
    /// </summary>
    public static string BuildIcs(CalendarEvent e)
    {
        var sb = new StringBuilder();
        sb.Append("BEGIN:VCALENDAR\r\n")
          .Append("VERSION:2.0\r\n")
          .Append("PRODID:").Append(ProductId).Append("\r\n")
          .Append("METHOD:PUBLISH\r\n")
          .Append("CALSCALE:GREGORIAN\r\n")
          .Append("BEGIN:VEVENT\r\n")
          .Append("UID:").Append(EscapeText(e.Uid)).Append("\r\n")
          .Append("DTSTAMP:").Append(FormatUtc(DateTime.UtcNow)).Append("\r\n")
          .Append("DTSTART:").Append(FormatUtc(e.StartUtc)).Append("\r\n")
          .Append("DTEND:").Append(FormatUtc(e.EndUtc)).Append("\r\n")
          .Append("SUMMARY:").Append(EscapeText(e.Title)).Append("\r\n");

        var description = BuildDescription(e);
        if (!string.IsNullOrWhiteSpace(description))
            sb.Append("DESCRIPTION:").Append(EscapeText(description)).Append("\r\n");

        if (!string.IsNullOrWhiteSpace(e.Location))
            sb.Append("LOCATION:").Append(EscapeText(e.Location!)).Append("\r\n");

        if (!string.IsNullOrWhiteSpace(e.Url))
            sb.Append("URL:").Append(e.Url).Append("\r\n");

        if (!string.IsNullOrWhiteSpace(e.OrganizerEmail))
        {
            var name = EscapeText(e.OrganizerName ?? "Organizer");
            sb.Append("ORGANIZER;CN=").Append(name).Append(":mailto:")
              .Append(e.OrganizerEmail).Append("\r\n");
        }

        sb.Append("STATUS:CONFIRMED\r\n")
          .Append("TRANSP:OPAQUE\r\n")
          .Append("BEGIN:VALARM\r\n")
          .Append("ACTION:DISPLAY\r\n")
          .Append("DESCRIPTION:").Append(EscapeText("Reminder: " + e.Title)).Append("\r\n")
          .Append("TRIGGER:-PT15M\r\n")
          .Append("END:VALARM\r\n")
          .Append("END:VEVENT\r\n")
          .Append("END:VCALENDAR\r\n");

        return sb.ToString();
    }

    /// <summary>
    /// Build a one-click "Add to Google Calendar" deep-link URL.
    /// Opens the prefilled new-event dialog in the user's Google Calendar.
    /// </summary>
    public static string GoogleCalendarUrl(CalendarEvent e)
    {
        var dates = $"{FormatUtc(e.StartUtc)}/{FormatUtc(e.EndUtc)}";
        var details = BuildDescription(e);

        var qs = new List<string>
        {
            "action=TEMPLATE",
            "text=" + Uri.EscapeDataString(e.Title ?? string.Empty),
            "dates=" + Uri.EscapeDataString(dates)
        };
        if (!string.IsNullOrWhiteSpace(details))
            qs.Add("details=" + Uri.EscapeDataString(details));
        if (!string.IsNullOrWhiteSpace(e.Location))
            qs.Add("location=" + Uri.EscapeDataString(e.Location!));
        if (!string.IsNullOrWhiteSpace(e.Url))
            qs.Add("sprop=" + Uri.EscapeDataString("website:" + e.Url));

        return "https://calendar.google.com/calendar/render?" + string.Join("&", qs);
    }

    /// <summary>
    /// Build a one-click "Add to Outlook (Office 365 / live.com)" deep-link.
    /// </summary>
    public static string OutlookCalendarUrl(CalendarEvent e)
    {
        var qs = new List<string>
        {
            "path=" + Uri.EscapeDataString("/calendar/action/compose"),
            "rru=" + Uri.EscapeDataString("addevent"),
            "subject=" + Uri.EscapeDataString(e.Title ?? string.Empty),
            "startdt=" + Uri.EscapeDataString(e.StartUtc.ToString("o", CultureInfo.InvariantCulture)),
            "enddt=" + Uri.EscapeDataString(e.EndUtc.ToString("o", CultureInfo.InvariantCulture))
        };
        var details = BuildDescription(e);
        if (!string.IsNullOrWhiteSpace(details))
            qs.Add("body=" + Uri.EscapeDataString(details));
        if (!string.IsNullOrWhiteSpace(e.Location))
            qs.Add("location=" + Uri.EscapeDataString(e.Location!));

        return "https://outlook.live.com/calendar/0/deeplink/compose?" + string.Join("&", qs);
    }

    private static string BuildDescription(CalendarEvent e)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(e.Description))
            parts.Add(e.Description!);
        if (!string.IsNullOrWhiteSpace(e.Url))
            parts.Add("Join: " + e.Url);
        if (!string.IsNullOrWhiteSpace(e.OrganizerName))
            parts.Add("Hosted by: " + e.OrganizerName);
        return string.Join("\n\n", parts);
    }

    private static string FormatUtc(DateTime dt)
    {
        var utc = dt.Kind == DateTimeKind.Utc ? dt : dt.ToUniversalTime();
        return utc.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture);
    }

    private static string EscapeText(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace(",", "\\,")
            .Replace(";", "\\;")
            .Replace("\r\n", "\\n")
            .Replace("\n", "\\n");
    }
}
