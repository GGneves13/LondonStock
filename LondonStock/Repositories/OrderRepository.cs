using teste.Classes;
using teste.Repositories.Interfaces;

namespace teste.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ApiContext _context;
        public OrderRepository(ApiContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }
    }
}
