namespace AD_COURSEWORK_2.Infrastructure;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string htmlBody);
}
