namespace AD_COURSEWORK_2.Infrastructure;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string htmlBody);

    // Sends an HTML email with one or more in-memory attachments.
    // This supports .ics calendar invites together with the HTML body.
    Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        IEnumerable<EmailAttachment> attachments);
}

public sealed record EmailAttachment(string FileName, string ContentType, byte[] Content);
