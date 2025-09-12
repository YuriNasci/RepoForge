using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.Domain.Interfaces
{
    public interface ICsvDataAdapter
    {
        Task UploadCsvAsync<T>(string key, IEnumerable<T> data);
        Task<IEnumerable<T>?> DownloadCsvAsync<T>(string key) where T : class, new();
    }
}
