using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace CsvImporter.Interfaces
{
    public interface ICsvDataProvider
    {
        IDataReader GetData(Stream sourceCsvStream);
    }
}
