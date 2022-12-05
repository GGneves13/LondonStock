using Microsoft.Extensions.Logging;
using LondonStock.Repositories.Interfaces;
using NSubstitute;
using LondonStock.Classes;
using LondonStock.Controllers;

namespace LondonStockUnitTests.Controllers
{
    public class LondonStockReadControllerTests
    {
        private ILogger<LondonStockReadController> _logger;
        private IStockRepository _stockRepo;
        private LondonStockReadController _controller;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For< ILogger<LondonStockReadController>>();
            _stockRepo = Substitute.For<IStockRepository>();

            _controller = new LondonStockReadController(_logger, _stockRepo);
        }

        #region Get()

        [Test]
        public async Task SuccessfulGetZeroStocks()
        {
            //Arrange
            _stockRepo.GetStocksAsync().Returns(new List<Stock>());

            //Act
            var result = await _controller.Get();

            //Assert
            await _stockRepo.Received(1).GetStocksAsync();
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task SuccessfulGetStocks()
        {
            //Arrange
            _stockRepo.GetStocksAsync().Returns(CreateStocks());

            //Act
            var result = await _controller.Get();

            //Assert
            await _stockRepo.Received(1).GetStocksAsync();
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        #endregion

        #region Get(List<string)

        [Test]
        public async Task SuccessfulGetSpecificZeroStocks()
        {
            //Arrange
            var filterList = new List<string> { "0A6L", "NIO" };
            _stockRepo.GetStocksAsync().Returns(new List<Stock>());

            //Act
            var result = await _controller.Get(filterList);

            //Assert
            await _stockRepo.Received(1).GetStocksAsync();
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task SuccessfulGetSpecificStocks()
        {
            //Arrange
            var filterList = new List<string> { "0A6L", "NIO" };
            _stockRepo.GetStocksAsync().Returns(CreateStocks());

            //Act
            var result = await _controller.Get(filterList);

            //Assert
            await _stockRepo.Received(1).GetStocksAsync();
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        #endregion

        private List<Stock> CreateStocks()
        {
            var list = new List<Stock>();

            list.Add(new Stock { Id = 1, StockSymbol = "NIO", Value = 10, Currency = "GBP" });
            list.Add(new Stock { Id = 2, StockSymbol = "TSLA", Value = 400, Currency = "GBP" });
            list.Add(new Stock { Id = 2, StockSymbol = "AAPL", Value = 500, Currency = "GBP" });

            return list;
        }
    }
}
