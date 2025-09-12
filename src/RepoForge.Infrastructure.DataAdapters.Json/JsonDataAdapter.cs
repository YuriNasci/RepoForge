using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.Infrastructure.DataAdapters.Json
{
    using RepoForge.Domain.Interfaces;
    using System.Text;
    using System.Text.Json;

    public class JsonDataAdapter : IJsonDataAdapter
    {
        private readonly IBlobRepository _blobRepository;

        public JsonDataAdapter(IBlobRepository blobRepository)
        {
            _blobRepository = blobRepository;
        }

        public async Task UploadJsonAsync<T>(string key, T data)
        {
            var json = JsonSerializer.Serialize(data);
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await _blobRepository.UploadAsync(key, ms);
        }

        public async Task<T?> DownloadJsonAsync<T>(string key)
        {
            using var stream = await _blobRepository.DownloadAsync(key);
            if (stream == null) return default;

            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
