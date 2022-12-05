using LondonStock.Classes;

namespace LondonStock.Repositories.Interfaces
{
    public interface IStockRepository
    {
        List<Stock> GetStocks();

        void AddStock(Stock stock);

        void UpdateStockPriceById(int id, decimal price);
    }
}
