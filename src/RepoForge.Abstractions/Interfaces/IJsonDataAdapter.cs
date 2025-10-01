using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.Abstractions
{
    public interface IJsonDataAdapter
    {
        Task UploadJsonAsync<T>(string key, T data);
        Task<T?> DownloadJsonAsync<T>(string key);
    }
}
