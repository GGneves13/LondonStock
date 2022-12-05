using teste.Classes;

namespace teste.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);

        List<Order> GetOrders();
    }
}
