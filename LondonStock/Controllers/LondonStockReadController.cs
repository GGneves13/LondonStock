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
    public class LondonStockReadController : ControllerBase
    {
        private readonly ILogger<LondonStockReadController> _logger;
        private readonly IStockRepository _stockRepo;

        public LondonStockReadController(
            ILogger<LondonStockReadController> logger,
            IStockRepository stockRepo)
        {
            _logger = logger;
            _stockRepo = stockRepo;
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
    }
}
