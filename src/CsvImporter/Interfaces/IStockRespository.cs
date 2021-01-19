using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CsvImporter.Interfaces
{
    public interface IStockRespository
    {
        Task BulkInsertAsync(IDataReader stockReader);
        Task TruncateAsync();
    }
}
