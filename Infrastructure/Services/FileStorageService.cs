using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    private string GetWebRootPath()
    {
        return _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default)
    {
        var webRootPath = GetWebRootPath();
        var uploadsFolder = Path.Combine(webRootPath, "uploads", folder);
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var outputFileStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(outputFileStream, cancellationToken);
        }

        return $"/uploads/{folder}/{uniqueFileName}";
    }

    public void DeleteFile(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return;

        var webRootPath = GetWebRootPath();
        var filePath = Path.Combine(webRootPath, fileUrl.TrimStart('/'));
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
