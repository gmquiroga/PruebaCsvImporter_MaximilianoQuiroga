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
            try
            {
                this.stockRespository.UnitOfWork.BeginTransaction();

                this.logger.LogInformation("Start truncate stock table");
                await this.stockRespository.TruncateAsync();
                this.logger.LogInformation("Finish truncate stock table");

                this.logger.LogInformation("Start insert stock process");
                using (var stockStream = await this.stockProvider.GetStockStream())
                {
                    using (var dataReader = this.csvDataProvider.GetData(stockStream))
                    {
                        this.logger.LogInformation("Start bulk insert");
                        await this.stockRespository.BulkInsertAsync(dataReader);
                        this.logger.LogInformation("Finish bulk insert");
                    }
                }
                this.logger.LogInformation("Finish insert stock process");

                this.stockRespository.UnitOfWork.Commit();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Error import stock");
                this.stockRespository.UnitOfWork.Rollback();
            }
            
        }
    }
}
