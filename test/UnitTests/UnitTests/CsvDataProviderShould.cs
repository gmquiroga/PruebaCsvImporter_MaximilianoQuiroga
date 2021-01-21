using CsvImporter.Infrastructure.Services;
using CsvImporter.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class CsvDataProviderShould
    {
        private readonly ICsvDataProvider csvDataProvider;
        private readonly Mock<ILogger<CsvDataProvider>> mockLogger;

        public CsvDataProviderShould()
        {
            this.mockLogger = new Mock<ILogger<CsvDataProvider>>();
            this.csvDataProvider = new CsvDataProvider(this.mockLogger.Object);
        }

        [Fact]
        public void GetData_ThrowsGivenNullStream()
        {
            Assert.Throws<NoNullAllowedException>(() => this.csvDataProvider.GetData(null));
        }
    }
}
