using System.ComponentModel.DataAnnotations;

namespace LondonStock.Classes
{
    public class Stock
    {
        public int Id { get; set; }

        public string? StockSymbol { get; set; }

        public decimal Value { get; set; }

        public string? Currency { get; set; }
    }
}
