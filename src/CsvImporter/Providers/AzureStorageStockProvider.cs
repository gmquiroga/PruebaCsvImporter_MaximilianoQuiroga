using Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using CsvImporter.Configuration;
using CsvImporter.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CsvImporter.Providers
{
    public class AzureStorageStockProvider: IStockProvider
    {
        private readonly StorageSettings storageSettings;
        private readonly ILogger<AzureStorageStockProvider> logger;
        private readonly BlobDownloadTransferOptions blobDownloadTransferOptions;
        private readonly BlockBlobClient blockBlobClient;

        public AzureStorageStockProvider(ILogger<AzureStorageStockProvider> logger, 
                                         IOptions<StorageSettings> storageSettings, 
                                         IOptions<BlobDownloadTransferOptions> blobDownloadTransferOptions)
        {
            // Set threading and default connection limit to 100 to ensure multiple threads and connections can be opened.
            // This is in addition to parallelism with the storage client library that is defined in the functions below.
            ThreadPool.SetMinThreads(100, 4);
            ServicePointManager.DefaultConnectionLimit = 100; // (Or More)

            this.logger = logger;
            this.storageSettings = storageSettings.Value;
            this.blobDownloadTransferOptions = blobDownloadTransferOptions.Value;
            this.blockBlobClient = new BlockBlobClient(new Uri(this.storageSettings.FilePath));
        }

        public async Task<Stream> GetStockStream()
        {
            if (!await this.blockBlobClient.ExistsAsync()) throw new FileNotFoundException($"Stock file not found: {this.storageSettings.FilePath}");

            MemoryStream resultStream = new MemoryStream();
            try
            {
                StorageTransferOptions options = new StorageTransferOptions();
                
                options.InitialTransferLength = this.blobDownloadTransferOptions.InitialTransferLength;
                options.MaximumTransferLength = this.blobDownloadTransferOptions.MaximumTransferLength;
                options.MaximumConcurrency = this.blobDownloadTransferOptions.MaximumConcurrency;

                this.logger.LogInformation("Start download from azure blob (this might take several minutes)");
                var azureResponse = await this.blockBlobClient.DownloadToAsync(resultStream, null, options);
                azureResponse.Dispose();

                this.logger.LogInformation("Finish download from azure blob");
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Error getting stock file : {0}", this.storageSettings.FilePath);
            }
            
            return resultStream;
        }
    }
}
