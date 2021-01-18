using CsvImporter.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CsvImporter.Services
{
    public class StockService : IStockService
    {
        private readonly ILogger<StockService> logger;
        private readonly IStockProvider stockProvider;
        private readonly ICsvDataProvider csvDataProvider;
        private readonly IStockRespository stockRespository;

        public StockService(ILogger<StockService> logger, IStockProvider stockProvider, ICsvDataProvider csvDataProvider, IStockRespository stockRespository)
        {
            this.logger = logger;
            this.stockProvider = stockProvider;
            this.csvDataProvider = csvDataProvider;
            this.stockRespository = stockRespository;
        }

        public async Task ImportStock()
        {
            int batchSize = 6000000;
            DataTable stockTable;
            int index = 0;

            this.logger.LogDebug("Start truncate stock table");
            await this.stockRespository.TruncateAsync();
            this.logger.LogDebug("Finish truncate stock table");

            this.logger.LogDebug("Start insert stock process");
            using (var stockStream = await this.stockProvider.GetStockStream())
            {
                this.logger.LogDebug("Start get data range from stock stream");

                while ((stockTable = this.csvDataProvider.GetDataRange(stockStream, index, batchSize)).Rows.Count > 0)
                {
                    this.logger.LogDebug("Start bulk insert");
                    await this.stockRespository.BulkInsertAsync(stockTable);
                    this.logger.LogDebug("Finish bulk insert");
                    index = index + batchSize;
                }
            }
            this.logger.LogDebug("Finish insert stock process");
        }
    }
}
