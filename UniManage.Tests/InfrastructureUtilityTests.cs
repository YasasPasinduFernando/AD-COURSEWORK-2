using System.Text;
using System.Text.Encodings.Web;
using AD_COURSEWORK_2.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace UniManage.Tests;

public class InfrastructureUtilityTests
{
    // ----- CsvWriter -----

    [Fact]
    public void CsvWriter_Build_ShouldIncludeHeadersAndUtf8Bom()
    {
        var headers = new[] { "ColA", "ColB" };
        var rows = new List<object?[]>
        {
            new object?[] { 1, "x" }
        };

        var bytes = CsvWriter.Build(headers, rows);

        bytes.Length.Should().BeGreaterThan(3);
        bytes[0].Should().Be(0xEF);
        bytes[1].Should().Be(0xBB);
        bytes[2].Should().Be(0xBF);

        var text = Encoding.UTF8.GetString(bytes);
        var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        lines[0].TrimStart('\uFEFF').Should().Be("ColA,ColB");
        lines.Should().Contain(l => l == "1,x");
    }

    // ----- CalendarLink -----

    [Fact]
    public void CalendarLink_BuildIcs_ShouldContainTitleStartAndUrl()
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
    public void CalendarLink_GoogleCalendarUrl_ShouldContainEncodedTitleAndCalendarHost()
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
            OrganizerName: null,
            OrganizerEmail: null,
            Url: "https://meet.example.com/x");

        var url = CalendarLink.GoogleCalendarUrl(evt);

        url.Should().StartWith("https://calendar.google.com/calendar/render?");
        url.Should().Contain(Uri.EscapeDataString("Team sync"));
    }

    // ----- MeetLinkGenerator -----

    [Fact]
    public void MeetLinkGenerator_GenerateGoogleMeetUrl_ShouldReturnNonEmptyMeetUrl()
    {
        var url = MeetLinkGenerator.GenerateGoogleMeetUrl();

        url.Should().NotBeNullOrWhiteSpace();
        url.Should().StartWith("https://meet.google.com/");
        MeetLinkGenerator.IsValidMeetingUrl(url).Should().BeTrue();
    }

    [Fact]
    public void MeetLinkGenerator_IsValidMeetingUrl_ShouldRejectUnknownHostsAndEmptyValues()
    {
        MeetLinkGenerator.IsValidMeetingUrl(string.Empty).Should().BeFalse();
        MeetLinkGenerator.IsValidMeetingUrl("not-a-url").Should().BeFalse();
        MeetLinkGenerator.IsValidMeetingUrl("https://example.com/abc").Should().BeFalse();

        MeetLinkGenerator.IsValidMeetingUrl("https://zoom.us/j/123").Should().BeTrue();
        MeetLinkGenerator.IsValidMeetingUrl("https://teams.microsoft.com/l/meetup-join/123").Should().BeTrue();
    }

    // ----- EmailTemplates -----

    [Fact]
    public void EmailTemplates_BuildPasswordResetEmail_ShouldContainResetGuidanceAndEncodedLink()
    {
        var resetUrl = "https://lms.example.com/Account/ResetPassword?token=abc";

        var html = EmailTemplates.BuildPasswordResetEmail("Alex Student", resetUrl);

        html.Should().Contain("Reset your UniManage password");
        html.Should().Contain("Password reset");
        html.Should().Contain("Reset password");
        html.Should().Contain(HtmlEncoder.Default.Encode(resetUrl));
    }

    // ----- SecurityHeadersMiddleware -----

    [Fact]
    public async Task SecurityHeadersMiddleware_ShouldAddExpectedResponseHeaders()
    {
        var nextInvoked = false;
        Task Next(HttpContext ctx)
        {
            nextInvoked = true;
            return Task.CompletedTask;
        }

        var middleware = new SecurityHeadersMiddleware(Next);
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(httpContext);

        nextInvoked.Should().BeTrue();
        httpContext.Response.Headers["X-Frame-Options"].ToString().Should().Be("DENY");
        httpContext.Response.Headers["X-Content-Type-Options"].ToString().Should().Be("nosniff");
        httpContext.Response.Headers["Referrer-Policy"].ToString().Should().Be("strict-origin-when-cross-origin");
        httpContext.Response.Headers["Content-Security-Policy"].ToString().Should().Contain("default-src 'self'");
    }

    // ----- SubmissionGradingRules -----

    [Theory]
    [InlineData(0, 100, true)]
    [InlineData(50, 100, true)]
    [InlineData(100, 100, true)]
    [InlineData(-0.01, 100, false)]
    [InlineData(100.01, 100, false)]
    public void SubmissionGradingRules_IsValidGrade_ShouldRespectInclusiveZeroToMax(decimal grade, decimal maxPoints, bool expected)
    {
        SubmissionGradingRules.IsValidGrade(grade, maxPoints).Should().Be(expected);
    }
}
