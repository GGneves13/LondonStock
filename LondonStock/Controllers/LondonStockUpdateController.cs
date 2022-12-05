using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory.Query.Internal;
using System.Net;
using LondonStock.Classes;
using LondonStock.Repositories.Interfaces;
using LondonStock.Services.Interfaces;

namespace LondonStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LondonStockUpdateController : ControllerBase
    {
        private readonly ILogger<LondonStockUpdateController> _logger;
        private readonly IStockRepository _stockRepo;
        private readonly IOrderServices _orderServices;

        public LondonStockUpdateController(
            ILogger<LondonStockUpdateController> logger,
            IStockRepository stockRepo,
            IOrderServices orderServices)
        {
            _logger = logger;
            _stockRepo = stockRepo;
            _orderServices = orderServices;
        }

        [HttpPost]
        [Route("AddNewStock")]
        public HttpResponseMessage AddNewStock(string stockSymbol, decimal value)
        {
            if(stockSymbol?.Length == 0)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "Invalid stockSymbol.");

            var stock = _stockRepo.GetStocks().FirstOrDefault(s => s.StockSymbol == stockSymbol);

            if (stock != null)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "LondonStock already trades this stock");

            if (value <= 0)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "Invalid stock value.");

            _stockRepo.AddStock(
                new Stock
                {
                    Id = _stockRepo.GetStocks().Count + 1,
                    StockSymbol = stockSymbol,
                    Value = value,
                    Currency = "GBP"
                }
                );

            return NewHttpResponseMessage(HttpStatusCode.OK, "Stock was added successfully.");
        }

        [HttpPost]
        [Route("AddNewOrder")]
        public HttpResponseMessage AddNewOrder(string stockSymbol, decimal price, decimal numberOfShares, int brokerId)
        {
            if (stockSymbol?.Length == 0)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "Invalid order stockSymbol");

            var stock = _stockRepo.GetStocks().FirstOrDefault(s => s.StockSymbol == stockSymbol);

            if (stock == null)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "LondonStock doesnt trade this stock yet");

            if (price <= 0)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "Invalid order value");

            if (numberOfShares <= 0)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "Invalid order amount");

            if (brokerId <= 0)
                return NewHttpResponseMessage(HttpStatusCode.BadRequest, "Invalid order brokerId");

            _orderServices.AddOrder(
                new Order
                {
                    StockId = stock.Id,
                    Price = price,
                    NumberOfShares = numberOfShares,
                    BrokerId = brokerId
                });

            _orderServices.CalculateNewStockPrice(stock.Id);

            return NewHttpResponseMessage(HttpStatusCode.OK, "Order was added successfully.");
        }

        private HttpResponseMessage NewHttpResponseMessage(HttpStatusCode statusCode, string? message) 
        {
            var msg = new HttpResponseMessage(statusCode);

            if(message?.Length > 0)
                msg.Content = new StringContent(message);

            return msg;
        }
    }
}
