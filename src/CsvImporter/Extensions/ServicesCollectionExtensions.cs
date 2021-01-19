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
using CsvImporter.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<StorageSettings>(configuration.GetSection(nameof(StorageSettings)));
            services.Configure<SqlBulkSettings>(configuration.GetSection(nameof(SqlBulkSettings)));

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["Data:ConnectionString"];
            services.AddScoped((sp) => new SqlConnection(connectionString));
            services.AddScoped<IStockRespository, StockRepository>();
            
            //services.AddScoped<IStockProvider, AzureStorageStockProvider>();
            services.AddScoped<IStockProvider, LocalStorageStockProvider>();

            services.AddScoped<ICsvDataProvider, CsvDataProvider>();
            services.AddScoped<IStockService, StockService>();


            return services;
        }
        
    }
}
