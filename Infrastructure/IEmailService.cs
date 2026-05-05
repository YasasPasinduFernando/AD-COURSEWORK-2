namespace AD_COURSEWORK_2.Infrastructure;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string htmlBody);

    /// <summary>
    /// Send an HTML email with one or more in-memory attachments.
    /// Used for delivering .ics calendar invites alongside the HTML body so
    /// Gmail / Outlook / Apple Mail can offer "Add to calendar" automatically.
    /// </summary>
    Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        IEnumerable<EmailAttachment> attachments);
}

public sealed record EmailAttachment(string FileName, string ContentType, byte[] Content);
