using Microsoft.AspNetCore.Http;

namespace AD_COURSEWORK_2.Infrastructure;

public static class UploadedFileStore
{
    private const long MaxBytes = 15 * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        ".txt", ".md", ".csv", ".rtf",
        ".png", ".jpg", ".jpeg", ".gif", ".webp", ".bmp",
        ".zip", ".rar", ".7z",
        ".mp3", ".mp4", ".wav", ".m4a"
    };

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/vnd.ms-powerpoint",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "application/rtf",
        "text/plain",
        "text/markdown",
        "text/csv",
        "image/png",
        "image/jpeg",
        "image/gif",
        "image/webp",
        "image/bmp",
        "application/zip",
        "application/x-zip-compressed",
        "application/x-rar-compressed",
        "application/vnd.rar",
        "application/x-7z-compressed",
        "application/octet-stream",
        "audio/mpeg",
        "audio/mp4",
        "audio/wav",
        "audio/x-wav",
        "video/mp4"
    };

    public static async Task<(string StoredName, string? ContentType, long Size)?> SaveAsync(
        IWebHostEnvironment env,
        IFormFile file,
        string subfolder,
        CancellationToken ct = default)
    {
        if (file.Length == 0 || file.Length > MaxBytes)
            return null;

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(ext) || ext.Length > 10)
            return null;

        if (!AllowedExtensions.Contains(ext))
            return null;

        if (!string.IsNullOrWhiteSpace(file.ContentType)
            && !AllowedContentTypes.Contains(file.ContentType))
        {
            return null;
        }

        var stored = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
        var dir = Path.Combine(env.WebRootPath, "uploads", subfolder);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, stored);
        await using (var fs = File.Create(path))
        {
            await file.CopyToAsync(fs, ct);
        }

        return (stored, file.ContentType, file.Length);
    }

    public static string? GetPhysicalPath(IWebHostEnvironment env, string subfolder, string? storedName)
    {
        if (string.IsNullOrWhiteSpace(storedName))
            return null;

        if (storedName.Contains("..") || storedName.Contains('/') || storedName.Contains('\\'))
            return null;

        var path = Path.Combine(env.WebRootPath, "uploads", subfolder, storedName);
        return File.Exists(path) ? path : null;
    }

    public static void TryDelete(IWebHostEnvironment env, string subfolder, string? storedName)
    {
        var path = GetPhysicalPath(env, subfolder, storedName);
        if (path != null)
            File.Delete(path);
    }

    public static IEnumerable<string> GetAllowedExtensions() => AllowedExtensions;
}
