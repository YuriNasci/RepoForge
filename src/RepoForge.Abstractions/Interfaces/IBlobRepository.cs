namespace RepoForge.Abstractions;

/// <summary>
/// Abstraction for a blob storage capable of uploading, downloading and deleting
/// binary objects addressed by a string key.
/// </summary>
public interface IBlobRepository
{
    /// <summary>
    /// Uploads the content of the provided stream to the blob storage using the given key.
    /// </summary>
    /// <param name="key">Logical identifier or path used by the underlying store (e.g., S3 object key).</param>
    /// <param name="data">Readable stream positioned at the beginning of the content to upload.</param>
    Task UploadAsync(string key, Stream data);

    /// <summary>
    /// Downloads the content stored under the specified key.
    /// </summary>
    /// <param name="key">Logical identifier or path used by the underlying store.</param>
    /// <returns>A readable stream for the object content, or <c>null</c> if the key does not exist.</returns>
    Task<Stream?> DownloadAsync(string key);

    /// <summary>
    /// Deletes the object stored under the specified key if it exists.
    /// </summary>
    /// <param name="key">Logical identifier or path used by the underlying store.</param>
    Task DeleteAsync(string key);
}