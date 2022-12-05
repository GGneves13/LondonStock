using LondonStock.Classes;
using LondonStock.Repositories.Interfaces;

namespace LondonStock.Repositories
{
    public class StockRepository : IStockRepository
    {
        private ApiContext _context;
        public StockRepository(ApiContext context)
        {
            _context = context;
        }

        public void AddStock(Stock stock)
        {
            _context.Stocks.Add(stock);
            _context.SaveChanges();
        }

        public List<Stock> GetStocks()
        {
            return _context.Stocks.ToList();
        }

        public void UpdateStockPriceById(int id, decimal price)
        {
            var stock = _context.Stocks.First(s => s.Id == id);
            
            stock.Value = price;
            _context.SaveChanges();
        }
    }
}
