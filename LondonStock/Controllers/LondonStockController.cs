﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory.Query.Internal;
using System.Net;
using teste.Classes;
using teste.Repositories.Interfaces;
using teste.Services.Interfaces;

namespace teste.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LondonStockController : ControllerBase
    {
        private readonly ILogger<LondonStockController> _logger;
        private readonly IStockRepository _stockRepo;
        private readonly IOrderServices _orderServices;

        public LondonStockController(
            ILogger<LondonStockController> logger,
            IStockRepository stockRepo,
            IOrderServices orderServices)
        {
            _logger = logger;
            _stockRepo = stockRepo;
            _orderServices = orderServices;
        }

        [HttpGet]
        [Route("GetAllStocks")]
        public IEnumerable<Stock> Get()
        {
            return _stockRepo.GetStocks();
        }

        [HttpPost]
        [Route("GetStocks")]
        public IEnumerable<Stock> Get(List<string> stockSymbols)
        {
            return _stockRepo.GetStocks().Where(s => stockSymbols.Contains(s.StockSymbol));
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
