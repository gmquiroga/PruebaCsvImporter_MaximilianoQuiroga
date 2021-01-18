using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace CsvImporter.Interfaces
{
    public interface ICsvDataProvider
    {
        DataTable GetDataRange(Stream sourceCsvStream, int index, int count);
    }
}
