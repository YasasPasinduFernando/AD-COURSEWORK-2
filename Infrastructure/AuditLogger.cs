using AD_COURSEWORK_2.Data;
using AD_COURSEWORK_2.Models;
using System.Security.Claims;

namespace AD_COURSEWORK_2.Infrastructure;

public sealed class AuditLogger : IAuditLogger
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _http;
    private readonly ILogger<AuditLogger> _logger;

    public AuditLogger(ApplicationDbContext db, IHttpContextAccessor http, ILogger<AuditLogger> logger)
    {
        _db = db;
        _http = http;
        _logger = logger;
    }

    public async Task LogAsync(string category, string action, string? detail = null, bool success = true,
        string? userId = null, string? userName = null)
    {
        try
        {
            var ctx = _http.HttpContext;
            var principal = ctx?.User;
            var resolvedUserId = userId
                ?? principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var resolvedUserName = userName
                ?? principal?.Identity?.Name;
            var ip = ctx?.Connection.RemoteIpAddress?.ToString();
            var ua = ctx?.Request.Headers.UserAgent.ToString();

            var entry = new AuditLog
            {
                CreatedAtUtc = DateTime.UtcNow,
                Category = Truncate(category, 80) ?? string.Empty,
                Action = Truncate(action, 120) ?? string.Empty,
                Detail = Truncate(detail, 2000),
                Success = success,
                UserId = resolvedUserId,
                UserName = Truncate(resolvedUserName, 256),
                IpAddress = Truncate(ip, 64),
                UserAgent = Truncate(ua, 512)
            };

            _db.AuditLogs.Add(entry);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write audit log for {Category}/{Action}", category, action);
        }
    }

    private static string? Truncate(string? value, int max)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        return value.Length <= max ? value : value[..max];
    }
}
