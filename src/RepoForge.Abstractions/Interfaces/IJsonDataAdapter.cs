using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.Abstractions
{
    /// <summary>
    /// High-level adapter for uploading and downloading JSON payloads over a blob store.
    /// </summary>
    public interface IJsonDataAdapter
    {
        /// <summary>
        /// Serializes the provided data to JSON and uploads it to the blob store.
        /// </summary>
        /// <typeparam name="T">Type of the payload to serialize.</typeparam>
        /// <param name="key">Logical object key (e.g., path) in the blob store.</param>
        /// <param name="data">Instance to serialize and upload.</param>
        Task UploadJsonAsync<T>(string key, T data);

        /// <summary>
        /// Downloads the JSON content stored at <paramref name="key"/> and deserializes it.
        /// </summary>
        /// <typeparam name="T">Target type for deserialization.</typeparam>
        /// <param name="key">Logical object key (e.g., path) in the blob store.</param>
        /// <returns>Deserialized instance or <c>null</c> if the object does not exist.</returns>
        Task<T?> DownloadJsonAsync<T>(string key);
    }
}
