using CsvImporter.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsvImporter.Infrastructure.Data
{
    public class StockRepository : IStockRespository
    {
        private readonly ILogger<StockRepository> logger;
        private readonly SqlConnection connection;
        private const string STOCK_TABLE_NAME = "dbo.Stock";

        public StockRepository(ILogger<StockRepository> logger, SqlConnection connection)
        {
            this.logger = logger;
            this.connection = connection;
        }

        public async Task BulkInsertAsync(DataTable stockTable)
        {
            try 
            {
                this.connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.connection, SqlBulkCopyOptions.TableLock, null))
                {
                    bulkCopy.DestinationTableName = STOCK_TABLE_NAME;
                    bulkCopy.BatchSize = 5000;
                    await bulkCopy.WriteToServerAsync(stockTable);
                }

                this.connection.Close();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Error bulk insert stock table");
                throw;
            }
        }

        public async Task TruncateAsync()
        {
            try
            {
                this.connection.Open();

                SqlCommand truncateCommand = new SqlCommand($"TRUNCATE TABLE {STOCK_TABLE_NAME}", this.connection);

                await truncateCommand.ExecuteNonQueryAsync();

                this.connection.Close();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Error truncate stock table");
                throw;
            }
        }

    }
}
