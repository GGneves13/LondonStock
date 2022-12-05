using LondonStock.Classes;

namespace LondonStock.Services.Interfaces
{
    public interface IOrderServices
    {
        void AddOrder(Order order);

        void CalculateNewStockPrice(int stockId);
    }
}
