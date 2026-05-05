using System.Security.Cryptography;
using System.Text;

namespace AD_COURSEWORK_2.Infrastructure;

public static class MeetLinkGenerator
{
    private const string Alphabet = "abcdefghijkmnopqrstuvwxyz";

    public static string GenerateGoogleMeetUrl()
    {
        var code = $"{RandomChunk(3)}-{RandomChunk(4)}-{RandomChunk(3)}";
        return $"https://meet.google.com/{code}";
    }

    public static bool IsValidMeetingUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;
        if (!Uri.TryCreate(url, UriKind.Absolute, out var u)) return false;
        if (u.Scheme != "https" && u.Scheme != "http") return false;
        var host = u.Host.ToLowerInvariant();
        return host == "meet.google.com"
               || host.EndsWith(".meet.google.com", StringComparison.Ordinal)
               || host == "zoom.us"
               || host.EndsWith(".zoom.us", StringComparison.Ordinal)
               || host == "teams.microsoft.com"
               || host.EndsWith(".teams.microsoft.com", StringComparison.Ordinal)
               || host == "teams.live.com"
               || host == "us02web.zoom.us"
               || host == "us04web.zoom.us"
               || host == "us05web.zoom.us"
               || host == "us06web.zoom.us"
               || host == "webex.com"
               || host.EndsWith(".webex.com", StringComparison.Ordinal);
    }

    private static string RandomChunk(int len)
    {
        var sb = new StringBuilder(len);
        Span<byte> bytes = stackalloc byte[len];
        RandomNumberGenerator.Fill(bytes);
        for (var i = 0; i < len; i++)
        {
            sb.Append(Alphabet[bytes[i] % Alphabet.Length]);
        }
        return sb.ToString();
    }
}
