using Microsoft.Extensions.Logging;
using LondonStock.Repositories.Interfaces;
using NSubstitute;
using LondonStock.Classes;
using LondonStock.Controllers;
using LondonStock.Services.Interfaces;
using System.Net;

namespace LondonStockUnitTests.Controllers
{
    public class LondonStockUpdateControllerTests
    {
        private ILogger<LondonStockUpdateController> _logger;
        private IStockRepository _stockRepo;
        private IOrderServices _orderServices;
        private IUnitOfWork _unitOfWork;
        private LondonStockUpdateController _controller;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For< ILogger<LondonStockUpdateController>>();
            _stockRepo = Substitute.For<IStockRepository>();
            _orderServices = Substitute.For<IOrderServices>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _controller = new LondonStockUpdateController(_logger, _unitOfWork, _stockRepo, _orderServices);
        }

        #region AddNewStock

        [Test]
        public async Task SuccessfullyAddStock()
        {
            //Arrange
            var stockSymbol = "NIO";
            var stockValue = 10;
            _stockRepo.GetStocksAsync().Returns(new List<Stock>());

            //Act
            var result = await _controller.AddNewStock(stockSymbol, stockValue);

            //Assert
            await _stockRepo.Received(2).GetStocksAsync();
            await _stockRepo.Received(1).AddStockAsync(Arg.Is<Stock>(o => o.Id == 1
                                                            && o.StockSymbol == stockSymbol
                                                            && o.Value == stockValue));
            await _unitOfWork.Received(1).CompleteAsync();
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task FailBecauseStockAlreadyExists()
        {
            //Arrange
            var stockSymbol = "NIO";
            var stockValue = 10;
            _stockRepo.GetStocksAsync().Returns(CreateStocks());

            //Act
            var result = await _controller.AddNewStock(stockSymbol, stockValue);

            //Assert
            await _stockRepo.Received(1).GetStocksAsync();
            await _stockRepo.Received(0).AddStockAsync(Arg.Any<Stock>());
            await _unitOfWork.Received(0).CompleteAsync();
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task FailBecauseEmptyStockCode()
        {
            //Arrange
            var stockSymbol = "";
            var stockValue = 10;
            _stockRepo.GetStocksAsync().Returns(CreateStocks());

            //Act
            var result = await _controller.AddNewStock(stockSymbol, stockValue);

            //Assert
            await _stockRepo.Received(0).GetStocksAsync();
            await _stockRepo.Received(0).AddStockAsync(Arg.Any<Stock>());
            await _unitOfWork.Received(0).CompleteAsync();
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-1.5)]
        public async Task FailBecauseInvalidStockValue(decimal value)
        {
            //Arrange
            var stockSymbol = "NIO";
            var stockValue = value;
            _stockRepo.GetStocksAsync().Returns(CreateStocks());

            //Act
            var result = await _controller.AddNewStock(stockSymbol, stockValue);

            //Assert
            await _stockRepo.Received(0).GetStocksAsync();
            await _stockRepo.Received(0).AddStockAsync(Arg.Any<Stock>());
            await _unitOfWork.Received(0).CompleteAsync();
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
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
