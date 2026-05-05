using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Options;

namespace AD_COURSEWORK_2.Infrastructure;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailSettings> options, ILogger<SmtpEmailService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public Task SendAsync(string toEmail, string subject, string htmlBody)
        => SendAsync(toEmail, subject, htmlBody, Array.Empty<EmailAttachment>());

    public async Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        IEnumerable<EmailAttachment> attachments)
    {
        if (string.IsNullOrWhiteSpace(_settings.Host) ||
            string.IsNullOrWhiteSpace(_settings.SenderEmail) ||
            string.IsNullOrWhiteSpace(_settings.Username) ||
            string.IsNullOrWhiteSpace(_settings.Password))
        {
            _logger.LogWarning("SMTP email skipped because Email settings are incomplete.");
            return;
        }

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password)
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        var streams = new List<MemoryStream>();
        try
        {
            foreach (var att in attachments ?? Array.Empty<EmailAttachment>())
            {
                if (att.Content == null || att.Content.Length == 0)
                    continue;

                var ms = new MemoryStream(att.Content);
                streams.Add(ms);
                var attachment = new Attachment(ms, att.FileName, att.ContentType);
                if (att.ContentType.StartsWith("text/calendar", StringComparison.OrdinalIgnoreCase))
                {
                    attachment.ContentDisposition!.Inline = false;
                    attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                }
                message.Attachments.Add(attachment);
            }

            await client.SendMailAsync(message);
        }
        finally
        {
            foreach (var s in streams) s.Dispose();
        }
    }
}
