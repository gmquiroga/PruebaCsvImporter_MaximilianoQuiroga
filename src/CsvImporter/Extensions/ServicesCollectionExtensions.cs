using CsvImporter.Infrastructure.Data;
using CsvImporter.Interfaces;
using CsvImporter.Providers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CsvImporter.Infrastructure.Services;
using CsvImporter.Services;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["Data:ConnectionString"];
            string stockFilePath = configuration["Data:StockFilePath"];

            services.AddScoped((sp) => new SqlConnection(connectionString));

            services.AddScoped<IStockRespository, StockRepository>();
            //services.AddScoped<IStockProvider>((sp => new AzureStorageStockProvider(stockFilePath)));
            services.AddScoped<IStockProvider>((sp => new LocalStorageStockProvider(Directory.GetCurrentDirectory() + "\\Stock.CSV")));
            services.AddScoped<ICsvDataProvider, CsvDataProvider>();
            services.AddScoped<IStockService, StockService>();


            return services;
        }
        
    }
}
