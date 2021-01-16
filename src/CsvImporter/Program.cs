using Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using CsvHelper;
using CsvHelper.Configuration;
using CsvImporter.Interfaces;
using CsvImporter.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;


namespace CsvImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            var services = new ServiceCollection();
            services.AddCustomServices(configuration);

            var serviceProvider = services.BuildServiceProvider();

            var stockService = serviceProvider.GetService<IStockService>();
            stockService.ImportStock().GetAwaiter().GetResult();
            


            /*
            MemoryStream resultStream = new MemoryStream();

            var fileBytes = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Stock.CSV");
            resultStream.Write(fileBytes, 0, fileBytes.Length);
            */

            /*
            var fileStream = File.OpenRead(Directory.GetCurrentDirectory() + "\\Stock.CSV");


            using (var reader = new StreamReader(fileStream, null, true, -1, true))
            {
                using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    csvReader.Configuration.Delimiter = ";";

                    var stocks = csvReader.GetRecords<StockDto>().Skip(0).Take(10);

                    var table =  stocks.CopyToDataTable<StockDto>();
                }
            }*/


            //using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "\\Stock.CSV"))
            //using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            //{
            //    csv.Configuration.Delimiter = ";";
            //    //csv.Configuration.RegisterClassMap<StockMap>();
            //    var records = csv.GetRecords<Stock1>();


            //    Console.WriteLine("Cantidad de registros: {0}", records.Count());
            //}

            /*
            

            using (SqlConnection destinationConnection =
                       new SqlConnection(connectionString))
            {
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy =
                           new SqlBulkCopy(destinationConnection))
                {
                    //bulkCopy.WriteToServerAsync()
                }
            }*/




            //Stream outputStream = File.Create(@"C:\WorkMaxi\TestBlobFile.csv");

            /*
            MemoryStream outputStream = new MemoryStream();
            DownloadRange(outputStream, 0, 1000000).GetAwaiter().GetResult();
            outputStream.Seek(0, SeekOrigin.Begin);
            */

            //CsvParserOptions csvParserOptions = new CsvParserOptions(false, ';');
            //CsvStockMapping csvMapper = new CsvStockMapping();
            //TinyCsvParser.CsvParser<StockDto> csvParser = new TinyCsvParser.CsvParser<StockDto>(csvParserOptions, csvMapper);

            //var records = csvParser.ReadFromStream(outputStream, Encoding.UTF8);
            //var list = records.Select(x => x.Result).ToList<StockDto>();


            //var dt = new DataTable();
            //dt.Columns.Add("PointOfSale", typeof(string));
            //dt.Columns.Add("Product", typeof(string));
            //dt.Columns.Add("Date", typeof(DateTime));
            //dt.Columns.Add("Stock", typeof(int));

            //var mData = list.CopyToDataTable<StockDto>(dt, null);
            /*
            using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "\\Stock.CSV"))
            //using (var reader = new StreamReader(outputStream))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.Delimiter = ";";
                //csv.Configuration.BadDataFound = null;

                var stocks = csv.GetRecords<StockDto>().Skip(3).Take(10);

                var table = stocks.CopyToDataTable<StockDto>();

                
                

                //csv.Configuration.BufferSize
                //csv.Configuration.RegisterClassMap<StockMap>();
                //var records = csv.GetRecords<Stock1>();

                //Console.WriteLine("Cantidad de registros: {0}", records.Count());
            }*/

            //outputStream.Dispose();


            //using (SqlConnection destinationConnection =
            //           new SqlConnection(connectionString))
            //{
            //    destinationConnection.Open();

            //    using (SqlBulkCopy bulkCopy =
            //               new SqlBulkCopy(destinationConnection))
            //    {
            //        bulkCopy.BatchSize = 100000;
            //        bulkCopy.WriteToServerAsync(sr)
            //    }
            //}
        }

        public static void DownloadBlobAnonymously()
        {
            /*
            BlockBlobClient blob = new BlockBlobClient(new Uri(@"https://storage10082020.blob.core.windows.net/y9ne9ilzmfld/Stock.CSV"));
            
            blob.DownloadTo(@"C:\Temp\logfile.txt");*/
        }

        public static async Task<Stream> DownloadRange(Stream outputStream, long offset, long length)
        {
            /*
            var blockBlobClient = new BlockBlobClient(
                                            _configurationKeys.StorageConnectionString
                                            , containerName.ToString(),
        
                                            , blobName.ToString());
            */
            BlockBlobClient blockBlobClient = new BlockBlobClient(new Uri(@"https://storage10082020.blob.core.windows.net/y9ne9ilzmfld/Stock.CSV"));

            //var httpRange = new Azure.HttpRange(offset, length);
            //var output = blockBlobClient.Download(httpRange);
            //output.Value.Content.CopyTo(outputStream);
            
            StorageTransferOptions options = new StorageTransferOptions();
            options.InitialTransferLength = 1024 * 1024;
            options.MaximumConcurrency = 30;
            options.MaximumTransferLength = 4 * 1024 * 1024;


            await blockBlobClient.DownloadToAsync(outputStream, null, options);

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    output.Value.Content.CopyTo(ms);
            //    var test = ms.ToArray();
            //}

            return outputStream;
        }
    }

    public class CsvStockMapping : CsvMapping<StockDto>
    {
        public CsvStockMapping()
            : base()
        {
            MapProperty(0, x => x.PointOfSale);
            MapProperty(1, x => x.Product);
            MapProperty(2, x => x.Date);
            MapProperty(3, x => x.Stock);
        }
    }

    public class StockMap : ClassMap<StockDto>
    {
        public StockMap()
        {
            Map(sm => sm.PointOfSale).Name("PointOfSale");
            Map(sm => sm.Product).Name("Product");
            Map(sm => sm.Date).Name("Date");
            Map(sm => sm.Stock).Name("Stock");
        }
    }
}
