using LondonStock.Classes;

namespace LondonStock.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);

        Task<List<Order>> GetOrdersAsync();

        List<Order> GetLocalOrders();
    }
}
