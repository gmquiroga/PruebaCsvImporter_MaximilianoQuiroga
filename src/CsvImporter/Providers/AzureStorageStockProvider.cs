using Azure.Core.Pipeline;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using CsvImporter.Configuration;
using CsvImporter.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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
                await this.blockBlobClient.DownloadToAsync(resultStream, null, options);
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
