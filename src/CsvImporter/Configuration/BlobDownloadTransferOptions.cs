using System;
using System.Collections.Generic;
using System.Text;

namespace CsvImporter.Configuration
{
    public class BlobDownloadTransferOptions
    {
        public int InitialTransferLength { get; set; }
        public int MaximumTransferLength { get; set; }
        public int MaximumConcurrency { get; set; }
    }
}
