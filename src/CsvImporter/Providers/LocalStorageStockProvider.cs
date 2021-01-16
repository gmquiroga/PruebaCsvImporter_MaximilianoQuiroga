using CsvImporter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CsvImporter.Providers
{
    public class LocalStorageStockProvider : IStockProvider
    {
        private readonly string stockFilePath;

        public LocalStorageStockProvider(string stockFilePath)
        {
            this.stockFilePath = stockFilePath;
        }

        public async Task<Stream> GetStockStream()
        {
            var resultStream = File.OpenRead(this.stockFilePath);

            return await Task.FromResult(resultStream);
        }
    }
}
