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
using Microsoft.Extensions.Logging;
using Serilog;


namespace CsvImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                var services = new ServiceCollection();

                services.AddLogging(configure => configure.AddSerilog());
                services.AddCustomOptions(configuration);
                services.AddCustomServices(configuration);

                var serviceProvider = services.BuildServiceProvider();

                var stockService = serviceProvider.GetService<IStockService>();

                Console.WriteLine("Importing stocks to database...");

                stockService.ImportStock().GetAwaiter().GetResult();

                Console.WriteLine("Finish to import stocks");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            
        }
    }
}
