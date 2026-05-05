namespace AD_COURSEWORK_2.Infrastructure;

public interface IAuditLogger
{
    Task LogAsync(string category, string action, string? detail = null, bool success = true,
        string? userId = null, string? userName = null);
}
