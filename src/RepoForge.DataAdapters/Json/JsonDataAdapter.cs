using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.DataAdapters.Json
{
    using RepoForge.Abstractions;
    using System.Text;
    using System.Text.Json;

    /// <summary>
    /// Default implementation of <see cref="IJsonDataAdapter"/> that uses <see cref="IBlobRepository"/>
    /// for storage and <see cref="JsonSerializer"/> for serialization.
    /// </summary>
    public class JsonDataAdapter : IJsonDataAdapter
    {
        private readonly IBlobRepository _blobRepository;

        /// <summary>
        /// Creates a new <see cref="JsonDataAdapter"/>.
        /// </summary>
        /// <param name="blobRepository">Blob storage repository to read/write JSON payloads.</param>
        public JsonDataAdapter(IBlobRepository blobRepository)
        {
            _blobRepository = blobRepository;
        }

        /// <inheritdoc />
        public async Task UploadJsonAsync<T>(string key, T data)
        {
            var json = JsonSerializer.Serialize(data);
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await _blobRepository.UploadAsync(key, ms);
        }

        /// <inheritdoc />
        public async Task<T?> DownloadJsonAsync<T>(string key)
        {
            using var stream = await _blobRepository.DownloadAsync(key);
            if (stream == null) return default;

            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
