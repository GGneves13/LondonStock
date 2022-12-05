using LondonStock.Classes;

namespace LondonStock.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);

        List<Order> GetOrders();
    }
}
