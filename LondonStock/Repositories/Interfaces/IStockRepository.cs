using LondonStock.Classes;

namespace LondonStock.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetStocksAsync();
        Task<Stock> GetStockByIdAsync(int id);
        Task AddStockAsync(Stock stock);
        void UpdateStock(Stock stock);
        Task UpdateStockPriceByIdAsync(int id, decimal price);
    }
}
