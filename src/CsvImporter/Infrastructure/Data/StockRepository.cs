using CsvImporter.Configuration;
using CsvImporter.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly SqlBulkSettings sqlBulkSettings;
        private const string STOCK_TABLE_NAME = "dbo.Stock";

        public IUnitOfWork UnitOfWork
        {
            get{ return this.unitOfWork; }
        }

        public StockRepository(ILogger<StockRepository> logger, IUnitOfWork unitOfWork, IOptions<SqlBulkSettings> sqlBulkSettings)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.sqlBulkSettings = sqlBulkSettings.Value;
        }

        public async Task BulkInsertAsync(IDataReader stockReader)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.unitOfWork.Connection, SqlBulkCopyOptions.TableLock, this.unitOfWork.Transaction))
                {
                    bulkCopy.BulkCopyTimeout = this.sqlBulkSettings.Timeout;
                    bulkCopy.BatchSize = this.sqlBulkSettings.BatchSize;
                    bulkCopy.DestinationTableName = STOCK_TABLE_NAME;
                    await bulkCopy.WriteToServerAsync(stockReader);
                }
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
                SqlCommand truncateCommand = new SqlCommand($"TRUNCATE TABLE {STOCK_TABLE_NAME}", this.unitOfWork.Connection, this.unitOfWork.Transaction);

                await truncateCommand.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Error truncate stock table");
                throw;
            }
        }

    }
}
