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

            await this.stockRespository.TruncateAsync();

            using (var stockStream = await this.stockProvider.GetStockStream())
            {
                for (int index = 0; index < 18000000; index += batchSize)
                {
                    stockTable = this.csvDataProvider.GetDataRange(stockStream, index, batchSize);

                    await this.stockRespository.BulkInsertAsync(stockTable);
                }   
            }
        }

        //public async Task ImportStock()
        //{
        //    DataTable stockTable;

        //    await this.stockRespository.TruncateAsync();

        //    using (var stockStream = await this.stockProvider.GetStockStream())
        //    { 
        //        stockTable = this.csvDataProvider.GetDataTable(stockStream, this.GetStockDataTableStructure());
        //    }

        //    await this.stockRespository.BulkInsertAsync(stockTable);
        //}

        private DataTable GetStockDataTableStructure()
        {
            DataTable stockDataTable = new DataTable();
            stockDataTable.Columns.Add("PointOfSale", typeof(string));
            stockDataTable.Columns.Add("Product", typeof(string));
            stockDataTable.Columns.Add("Date", typeof(DateTime));
            stockDataTable.Columns.Add("Stock", typeof(int));

            return stockDataTable;
        }
    }
}
