namespace Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default);
    void DeleteFile(string fileUrl);
}
