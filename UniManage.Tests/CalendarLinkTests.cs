using AD_COURSEWORK_2.Infrastructure;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class CalendarLinkTests
{
    [Fact]
    public void BuildIcs_contains_summary_start_and_optional_url()
    {
        var start = new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);
        var evt = new CalendarLink.CalendarEvent(
            Uid: "meeting-1@test",
            Title: "Live tutorial",
            Description: "Bring questions",
            Location: "Online",
            StartUtc: start,
            EndUtc: end,
            OrganizerName: "Dr Smith",
            OrganizerEmail: "smith@example.com",
            Url: "https://meet.example.com/abc");

        var ics = CalendarLink.BuildIcs(evt);

        ics.Should().Contain("BEGIN:VCALENDAR");
        ics.Should().Contain("BEGIN:VEVENT");
        ics.Should().Contain("SUMMARY:Live tutorial");
        ics.Should().Contain("DTSTART:");
        ics.Should().Contain("URL:https://meet.example.com/abc");
    }

    [Fact]
    public void GoogleCalendarUrl_contains_title_and_calendar_host()
    {
        var start = new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);
        var evt = new CalendarLink.CalendarEvent(
            Uid: "uid",
            Title: "Team sync",
            Description: null,
            Location: null,
            StartUtc: start,
            EndUtc: end,
            null,
            null,
            Url: "https://meet.example.com/x");

        var url = CalendarLink.GoogleCalendarUrl(evt);

        url.Should().StartWith("https://calendar.google.com/calendar/render?");
        url.Should().Contain("text=");
        url.Should().Contain(Uri.EscapeDataString("Team sync"));
    }
}
