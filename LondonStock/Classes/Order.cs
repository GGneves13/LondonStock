using System.ComponentModel.DataAnnotations;

namespace LondonStock.Classes
{
    public class Order
    {
        public int Id { get; set; }

        public int StockId { get; set; }

        public decimal Price { get; set; }

        public decimal NumberOfShares { get; set; }

        public int BrokerId { get; set; }
    }
}
