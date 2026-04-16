using Microsoft.AspNetCore.Http;

namespace AD_COURSEWORK_2.Infrastructure;

public static class UploadedFileStore
{
    private const long MaxBytes = 15 * 1024 * 1024;

    public static async Task<(string StoredName, string? ContentType, long Size)?> SaveAsync(
        IWebHostEnvironment env,
        IFormFile file,
        string subfolder,
        CancellationToken ct = default)
    {
        if (file.Length == 0 || file.Length > MaxBytes)
            return null;

        var ext = Path.GetExtension(file.FileName);
        if (ext.Length > 120)
            return null;

        var stored = $"{Guid.NewGuid():N}{ext}";
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
        var path = Path.Combine(env.WebRootPath, "uploads", subfolder, storedName);
        return File.Exists(path) ? path : null;
    }

    public static void TryDelete(IWebHostEnvironment env, string subfolder, string? storedName)
    {
        var path = GetPhysicalPath(env, subfolder, storedName);
        if (path != null)
            File.Delete(path);
    }
}
