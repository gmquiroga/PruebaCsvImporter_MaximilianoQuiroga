using CsvImporter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CsvImporter
{
    class Program
    {
        private static ILogger<Program> logger;

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
                logger = serviceProvider.GetService<ILogger<Program>>();

                logger.LogInformation("Importing stocks to database...");

                stockService.ImportStock().GetAwaiter().GetResult();

                logger.LogInformation("Finish to import stocks");
            }
            catch (Exception exception)
            {
                logger.LogInformation(exception.Message);
            }
        }
    }
}
