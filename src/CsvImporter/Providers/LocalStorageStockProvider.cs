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
    public class LocalStorageStockProvider : IStockProvider
    {
        private readonly StorageSettings storageSettings;
        private readonly ILogger<LocalStorageStockProvider> logger;

        public LocalStorageStockProvider(ILogger<LocalStorageStockProvider> logger, IOptions<StorageSettings> storageSettings)
        {
            this.storageSettings = storageSettings.Value;
            this.logger = logger;
        }

        public async Task<Stream> GetStockStream()
        {
            Stream resultStream = null;
            try
            {
                resultStream = File.OpenRead(this.storageSettings.FilePath);
            }
            catch (FileNotFoundException exception)
            {
                this.logger.LogError(exception, "Stock file not found: {0}", this.storageSettings.FilePath);
                throw;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Error open stock file : {0}", this.storageSettings.FilePath);
                throw;
            }

            return await Task.FromResult(resultStream);
        }
    }
}
