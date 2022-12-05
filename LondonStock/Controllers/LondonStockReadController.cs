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
        public async Task<IEnumerable<Stock>> Get()
        {
            return await _stockRepo.GetStocksAsync();
        }

        [HttpPost]
        [Route("GetStocks")]
        public async Task<IEnumerable<Stock>> Get(List<string> stockSymbols)
        {
            return (await _stockRepo.GetStocksAsync()).Where(s => stockSymbols.Contains(s.StockSymbol));
        }
    }
}
