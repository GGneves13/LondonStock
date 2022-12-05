using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LondonStock.Repositories.Interfaces;
using LondonStock.Services;
using LondonStock.Services.Interfaces;
using NSubstitute;
using LondonStock.Classes;

namespace LondonStockUnitTests.Services
{
    public class OrderServicesTests
    {
        private ILogger<OrderServices> _logger;
        private IOrderRepository _orderRepo;
        private IStockRepository _stockRepo;
        private IOrderServices _services;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For< ILogger<OrderServices>>();
            _orderRepo = Substitute.For<IOrderRepository>();
            _stockRepo = Substitute.For<IStockRepository>();

            _services = new OrderServices(_logger, _stockRepo, _orderRepo);
        }

        [Test]
        public void SuccessfulAddOrder()
        {
            //Arrange
            var orderList = CreateOrders();
            var order = new Order { BrokerId = 1, NumberOfShares = 2, Price = 20, StockId = 1 };
            _orderRepo.GetOrdersAsync().Returns(orderList);

            //Act
            _services.AddOrder(order);

            //Assert
            _orderRepo.Received(1).GetOrdersAsync();
            _orderRepo.Received(1).AddOrderAsync(Arg.Is<Order>(o => o.Id == orderList.Count() + 1
                                                            && o.BrokerId == order.BrokerId
                                                            && o.NumberOfShares == order.NumberOfShares
                                                            && o.Price == order.Price
                                                            && o.StockId == order.StockId));
        }

        [Test]
        public void SuccessfulCalculateNewStockPrice()
        {
            //Arrange
            var orderList = CreateOrders();
            var stockId = 1;
            var stock = new Stock { Id = 1 };
            var newExpectedValues = orderList.Where(o => o.StockId == stockId)
                .GroupBy(r => 1)
                .Select(g => new
                {
                    SumPrice = g.Sum(x => x.Price),
                    SumNumberOfShares = g.Sum(x => x.NumberOfShares)
                }).First();

            _orderRepo.GetLocalOrders().Returns(orderList);
            _stockRepo.GetStockByIdAsync(stockId).Returns(stock);

            //Act
            _services.CalculateNewStockPrice(stockId);

            //Assert
            _orderRepo.Received(1).GetLocalOrders();
            _stockRepo.Received(1).UpdateStock(Arg.Is<Stock>(o => o.Id == stockId
                                                            && o.Value == newExpectedValues.SumPrice / newExpectedValues.SumNumberOfShares));
        }

        private List<Order> CreateOrders()
        {
            var list = new List<Order>();

            list.Add(new Order { Id = 1, BrokerId = 1, NumberOfShares = 2, Price = 20, StockId = 1 });
            list.Add(new Order { Id = 2, BrokerId = 1, NumberOfShares = 4, Price = 550, StockId = 2 });
            list.Add(new Order { Id = 2, BrokerId = 1, NumberOfShares = 4, Price = 36, StockId = 1 });

            return list;
        }
    }
}
