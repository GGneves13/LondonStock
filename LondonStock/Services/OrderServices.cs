using teste.Classes;
using teste.Controllers;
using teste.Repositories.Interfaces;
using teste.Services.Interfaces;

namespace teste.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ILogger<OrderServices> _logger;
        private readonly IOrderRepository _orderRepo;
        private readonly IStockRepository _stockRepo;
        public OrderServices(
            ILogger<OrderServices> logger,
            IStockRepository stockRepo,
            IOrderRepository orderRepo)
        {
            _logger = logger;
            _stockRepo = stockRepo;
            _orderRepo = orderRepo;
        }

        public void AddOrder(Order order)
        {
            order.Id = _orderRepo.GetOrders().Count() + 1;
            _orderRepo.AddOrder(order);

            var sums = _orderRepo.GetOrders()
                .Where(o => o.StockId == order.StockId)
                .GroupBy(r => 1)
                .Select(g => new
                {
                    SumPrice = g.Sum(x => x.Price),
                    SumNumberOfShares = g.Sum(x => x.NumberOfShares)
                }).First();

            _stockRepo.UpdateStockPriceById(order.StockId, sums.SumPrice / sums.SumNumberOfShares);
        }
    }
}
