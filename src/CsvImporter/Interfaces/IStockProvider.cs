using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CsvImporter.Interfaces
{
    public interface IStockProvider
    {
        Task<Stream> GetStockStream();
    }
}
