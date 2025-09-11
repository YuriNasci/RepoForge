namespace RepoForge.Domain.Interfaces;

public interface IBlobRepository
{
    Task UploadAsync(string key, Stream data);
    Task<Stream?> DownloadAsync(string key);
    Task DeleteAsync(string key);
}