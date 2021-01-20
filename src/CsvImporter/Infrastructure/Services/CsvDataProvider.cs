using CsvHelper;
using CsvHelper.Configuration;
using CsvImporter.Interfaces;
using CsvImporter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace CsvImporter.Infrastructure.Services
{
    public class CsvDataProvider: ICsvDataProvider, IDisposable
    {
        private readonly ILogger<CsvDataProvider> logger;
        private StreamReader streamReder;
        private CsvReader csvReader;
        private CsvDataReader csvDataReader;

        public CsvDataProvider(ILogger<CsvDataProvider> logger)
        {
            this.logger = logger;
        }

        public IDataReader GetData(Stream sourceCsvStream)
        {
            if (sourceCsvStream == null) throw new NoNullAllowedException();

            this.logger.LogDebug("GetData from stock stream....");
            sourceCsvStream.Seek(0, SeekOrigin.Begin);

            this.streamReder = new StreamReader(sourceCsvStream, null, true, -1, true);

            this.csvReader = new CsvReader(this.streamReder, CultureInfo.CurrentCulture);
            this.csvReader.Configuration.Delimiter = ";";

            this.csvDataReader = new CsvDataReader(csvReader);

            return this.csvDataReader;
        }

        public void Dispose()
        {
            this.streamReder.Dispose();
            this.csvReader.Dispose();
            this.csvDataReader.Dispose();
        }
    }
}
