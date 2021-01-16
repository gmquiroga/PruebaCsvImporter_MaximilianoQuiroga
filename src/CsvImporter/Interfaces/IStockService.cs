using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CsvImporter.Interfaces
{
    public interface IStockService
    {
        Task ImportStock();
    }
}
