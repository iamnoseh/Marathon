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

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
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

        var filePath = Path.Combine(_environment.WebRootPath, fileUrl.TrimStart('/'));
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
