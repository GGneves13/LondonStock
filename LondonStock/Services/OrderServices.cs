using LondonStock.Classes;
using LondonStock.Controllers;
using LondonStock.Repositories.Interfaces;
using LondonStock.Services.Interfaces;

namespace LondonStock.Services
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
        }

        public void CalculateNewStockPrice(int stockId)
        {
            var sums = _orderRepo.GetOrders()
                .Where(o => o.StockId == stockId)
                .GroupBy(r => 1)
                .Select(g => new
                {
                    SumPrice = g.Sum(x => x.Price),
                    SumNumberOfShares = g.Sum(x => x.NumberOfShares)
                }).First();

            _stockRepo.UpdateStockPriceById(stockId, sums.SumPrice / sums.SumNumberOfShares);
        }
    }
}
