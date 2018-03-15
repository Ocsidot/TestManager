using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestManager.Lib.ViewModels;

namespace TestManager.Lib.Services
{
    public class DataRetrieverService : IDataRetriever
    {
        public async Task<PostmanCollection[]> GetCollections(string path)
        {
            string[] fileSystemEntries = Directory.GetFileSystemEntries(path, "*.json");

            var tasks = fileSystemEntries.Select(fileName => ReadFileAsync(fileName));
            return await Task.WhenAll(tasks.ToArray());
        }

        private async Task<PostmanCollection> ReadFileAsync(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                string contents = await reader.ReadToEndAsync().ConfigureAwait(false);
                return PostmanCollection.FromJson(contents);
            }
        }
    }
}
