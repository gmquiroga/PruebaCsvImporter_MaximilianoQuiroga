using Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using CsvImporter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CsvImporter.Providers
{
    public class AzureStorageStockProvider: IStockProvider
    {
        private readonly BlockBlobClient blockBlobClient;

        public AzureStorageStockProvider(string stockFilePath)
        {
            blockBlobClient = new BlockBlobClient(new Uri(stockFilePath));
        }

        public async Task<Stream> GetStockStream()
        {
            MemoryStream resultStream = new MemoryStream();

            StorageTransferOptions options = new StorageTransferOptions();
            options.InitialTransferLength = 1024 * 1024;
            options.MaximumConcurrency = 30;
            options.MaximumTransferLength = 4 * 1024 * 1024;

            await this.blockBlobClient.DownloadToAsync(resultStream, null, options);

            return resultStream;
        }
    }
}
