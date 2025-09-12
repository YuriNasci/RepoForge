using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.Infrastructure.DataAdapters.Csv
{
    using System.Formats.Asn1;
    using System.Globalization;
    using System.Text;
    using CsvHelper;
    using RepoForge.Domain.Interfaces;

    public class CsvDataAdapter : ICsvDataAdapter
    {
        private readonly IBlobRepository _blobRepository;

        public CsvDataAdapter(IBlobRepository blobRepository)
        {
            _blobRepository = blobRepository;
        }

        public async Task UploadCsvAsync<T>(string key, IEnumerable<T> data)
        {
            using var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms, Encoding.UTF8, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
                await writer.FlushAsync();
            }
            ms.Position = 0;
            await _blobRepository.UploadAsync(key, ms);
        }

        public async Task<IEnumerable<T>?> DownloadCsvAsync<T>(string key) where T : class, new()
        {
            using var stream = await _blobRepository.DownloadAsync(key);
            if (stream == null) return null;

            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<T>().ToList();
        }
    }
}
