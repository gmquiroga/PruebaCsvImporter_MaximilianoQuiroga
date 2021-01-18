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

namespace CsvImporter.Infrastructure.Services
{
    public class CsvDataProvider: ICsvDataProvider
    {
        public DataTable GetDataTable(Stream sourceCsvStream, DataTable table) 
        {
            sourceCsvStream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(sourceCsvStream))
            {
                using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    csvReader.Configuration.Delimiter = ";";

                    using (var csvDataReader = new CsvDataReader(csvReader))
                    {
                        table.Load(csvDataReader);
                    }
                }
            }

            return table;
        }

        public DataTable GetDataRange(Stream sourceCsvStream, int index, int count)
        {
            sourceCsvStream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(sourceCsvStream, null, true, -1, true))
            {
                using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    csvReader.Configuration.Delimiter = ";";

                    var stocks = csvReader.GetRecords<StockDto>().Skip(index).Take(count);

                    return stocks.CopyToDataTable<StockDto>();
                }
            }
        }
    }
}
