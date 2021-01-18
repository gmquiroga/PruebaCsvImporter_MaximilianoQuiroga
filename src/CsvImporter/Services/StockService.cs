using CsvImporter.Interfaces;
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
        private readonly IStockProvider stockProvider;
        private readonly ICsvDataProvider csvDataProvider;
        private readonly IStockRespository stockRespository;

        public StockService(IStockProvider stockProvider, ICsvDataProvider csvDataProvider, IStockRespository stockRespository)
        {
            this.stockProvider = stockProvider;
            this.csvDataProvider = csvDataProvider;
            this.stockRespository = stockRespository;
        }

        public async Task ImportStock()
        {
            int batchSize = 5000000;
            DataTable stockTable;
            int index = 0;

            await this.stockRespository.TruncateAsync();

            using (var stockStream = await this.stockProvider.GetStockStream())
            {
                while ((stockTable = this.csvDataProvider.GetDataRange(stockStream, index, batchSize)).Rows.Count > 0)
                {
                    await this.stockRespository.BulkInsertAsync(stockTable);
                    index = index + batchSize;
                }
            }
        }
    }
}
