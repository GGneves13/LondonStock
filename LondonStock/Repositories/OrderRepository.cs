using LondonStock.Classes;
using LondonStock.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LondonStock.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ApiContext _context;
        public OrderRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            //_context.SaveChanges();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public List<Order> GetLocalOrders()
        {
            return _context.Orders.Local.ToList();
        }
    }
}
