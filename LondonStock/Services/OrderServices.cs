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

        public async void AddOrder(Order order)
        {
            order.Id = (await _orderRepo.GetOrdersAsync()).Count() + 1;
            await _orderRepo.AddOrderAsync(order);
        }

        public async void CalculateNewStockPrice(int stockId)
        {
            var sums = _orderRepo.GetLocalOrders()
                .Where(o => o.StockId == stockId)
                .GroupBy(r => 1)
                .Select(g => new
                {
                    SumPrice = g.Sum(x => x.Price),
                    SumNumberOfShares = g.Sum(x => x.NumberOfShares)
                }).First();

            var stock = await _stockRepo.GetStockByIdAsync(stockId);
            stock.Value = sums.SumPrice / sums.SumNumberOfShares;

            _stockRepo.UpdateStock(stock);
        }
    }
}
