using System;
using System.Collections.Generic;
using System.Text;

namespace CsvImporter.Configuration
{
    public class SqlBulkSettings
    {
        public int Timeout { get; set; }
        public int BatchSize { get; set; }
    }
}
