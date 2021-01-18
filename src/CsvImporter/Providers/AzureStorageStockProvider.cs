﻿using Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using CsvImporter.Configuration;
using CsvImporter.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CsvImporter.Providers
{
    public class AzureStorageStockProvider: IStockProvider
    {
        private readonly StorageSettings storageSettings;
        private readonly ILogger<LocalStorageStockProvider> logger;
        private readonly BlockBlobClient blockBlobClient;

        public AzureStorageStockProvider(ILogger<LocalStorageStockProvider> logger, IOptions<StorageSettings> storageSettings)
        {
            this.storageSettings = storageSettings.Value;
            this.logger = logger;
            blockBlobClient = new BlockBlobClient(new Uri(this.storageSettings.FilePath));
        }

        public async Task<Stream> GetStockStream()
        {
            MemoryStream resultStream = new MemoryStream();
            try
            {
                StorageTransferOptions options = new StorageTransferOptions();
                options.InitialTransferLength = 1024 * 1024;
                options.MaximumConcurrency = 30;
                options.MaximumTransferLength = 4 * 1024 * 1024;

                await this.blockBlobClient.DownloadToAsync(resultStream, null, options);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Error getting stock file : {0}", this.storageSettings.FilePath);
            }
            
            return resultStream;
        }
    }
}
