using System;
using System.Collections.Generic;
using System.Text;

namespace CsvImporter.Models
{
    public class StockDto
    {
        public StockDto()
        {

        }

        public string PointOfSale { get; set; }
        public string Product { get; set; }
        public DateTime Date { get; set; }
        public int Stock { get; set; }

        //public string PointOfSaleId { get; set; }
        //public string ProductId { get; set; }
        //public DateTime Date { get; set; }
        //public int StockAmount { get; set; }
    }
}
