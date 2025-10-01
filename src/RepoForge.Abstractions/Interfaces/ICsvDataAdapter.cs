using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoForge.Abstractions
{
    /// <summary>
    /// High-level adapter for uploading and downloading CSV payloads over a blob store.
    /// </summary>
    public interface ICsvDataAdapter
    {
        /// <summary>
        /// Writes the sequence of records as CSV and uploads it to the blob store.
        /// </summary>
        /// <typeparam name="T">Record type to serialize.</typeparam>
        /// <param name="key">Logical object key (e.g., path) in the blob store.</param>
        /// <param name="data">Records to write.</param>
        Task UploadCsvAsync<T>(string key, IEnumerable<T> data);

        /// <summary>
        /// Downloads the CSV from the blob store and materializes it as a sequence of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Target record type. Must be a class with a public parameterless constructor.</typeparam>
        /// <param name="key">Logical object key (e.g., path) in the blob store.</param>
        /// <returns>The materialized records, or <c>null</c> if the object does not exist.</returns>
        Task<IEnumerable<T>?> DownloadCsvAsync<T>(string key) where T : class, new();
    }
}
