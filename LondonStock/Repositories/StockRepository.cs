using LondonStock.Classes;
using LondonStock.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LondonStock.Repositories
{
    public class StockRepository : IStockRepository
    {
        private ApiContext _context;
        public StockRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task AddStockAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            //_context.SaveChanges();
        }

        public void UpdateStock(Stock stock)
        {
            _context.Stocks.Update(stock);
            //_context.SaveChanges();
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.FirstAsync(s => s.Id == id);
        }

        public async Task UpdateStockPriceByIdAsync(int id, decimal price)
        {
            var stock = await _context.Stocks.FirstAsync(s => s.Id == id);
            
            stock.Value = price;
            //_context.SaveChanges();
        }
    }
}
