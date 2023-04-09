using Moq;
using Project2Api.Contracts.Order;
using Project2Api.DbTools;
using Project2Api.Services.Orders;
using System.Data;
using Project2Api.Models;
using ErrorOr;

namespace Project2Api.Tests.Services.OrdersServiceTests
{
    [TestFixture]
    internal class CreateOrderTests 
    {
        private Mock<IDbClient> _dbClientMock = null!;
        private OrdersService _ordersService = null!;

        [SetUp]
        public void SetUp()
        {
            _dbClientMock = new Mock<IDbClient>();
            _ordersService = new OrdersService(_dbClientMock.Object);
        }

        [Test]
        public void CreateOrder_WithValidOrderRequest_ReturnsOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest(new List<string> { "pita", "meatball" }, 1.0f);

            ErrorOr<Order> order = Order.From(orderRequest);

            _dbClientMock.Setup(x => x.ExecuteQueryAsync(It.IsAny<string>())).ReturnsAsync(new DataTable());
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(1);

            // Act
            ErrorOr<Order> result = _ordersService.CreateOrder(order.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Id, Is.EqualTo(order.Value.Id));
            Assert.That(result.Value.OrderTime, Is.EqualTo(order.Value.OrderTime).Within(1).Seconds);
            Assert.That(result.Value.Price, Is.EqualTo(order.Value.Price));
        }

        [Test]
        public void CreateOrder_WithInvalidOrderRequest_ReturnsError()
        {
            // Arrange
            var orderRequest = new OrderRequest(new List<string>{"burger"}, 0.1f);

            ErrorOr<Order> order = Order.From(orderRequest);

            _dbClientMock.Setup(x => x.ExecuteQueryAsync(It.IsAny<string>())).ReturnsAsync(new DataTable());
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(-1);

            // Act
            ErrorOr<Order> result = _ordersService.CreateOrder(order.Value);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Orders.DbError));
        }
    }
}
