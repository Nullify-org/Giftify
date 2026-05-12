using Giftify.Interfaces.Services;

namespace Giftify.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly IWebHostEnvironment _env;
    private const string UploadFolder = "images/products";
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; 

    public ImageUploadService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string?> UploadAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return null;

        // Validate extension
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            throw new InvalidOperationException($"File type '{ext}' is not allowed. Allowed: {string.Join(", ", AllowedExtensions)}");

        // Validate size
        if (file.Length > MaxFileSizeBytes)
            throw new InvalidOperationException($"File size exceeds the 5 MB limit.");

        // Build save path
        var uploadsDir = Path.Combine(_env.WebRootPath, UploadFolder);
        Directory.CreateDirectory(uploadsDir); // creates folder if missing

        var uniqueName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadsDir, uniqueName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{UploadFolder}/{uniqueName}"; // relative URL for <img src="...">
    }

    public void Delete(string? relativeUrl)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl))
            return;

        // Convert "/images/products/abc.jpg"  →  absolute disk path
        var fullPath = Path.Combine(_env.WebRootPath, relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}