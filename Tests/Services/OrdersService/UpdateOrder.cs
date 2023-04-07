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
    internal class UpdateOrderTests 
    {
        private Mock<IDbClient> _dbClientMock = null!;
        private OrdersService _ordersService = null!;

        [SetUp]
        public void SetUp()
        {
            _dbClientMock = new Mock<IDbClient>();
            _dbClientMock.Setup(x => x.ExecuteQueryAsync(It.IsAny<string>())).ReturnsAsync(new DataTable());
            
            _ordersService = new OrdersService(_dbClientMock.Object);
        }

        [Test]
        public void UpdateOrder_WithValidOrderRequest_ReturnsOrder()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            UpdateOrderRequest updateOrderRequest = new UpdateOrderRequest(dateTime, new List<string> { "pita", "meatball" }, 1.0f);
            Guid guid = Guid.NewGuid();
            
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(1);
            
            ErrorOr<Order> order = Order.From(updateOrderRequest, guid);

            // Act
            ErrorOr<Order> result = _ordersService.UpdateOrder(guid, order.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Id, Is.EqualTo(order.Value.Id));
            Assert.That(result.Value.OrderTime, Is.EqualTo(order.Value.OrderTime).Within(1).Seconds);
            Assert.That(result.Value.Price, Is.EqualTo(order.Value.Price));
        }

        [Test]
        public void UpdateOrder_WithInvalidOrderRequest_ReturnsError()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            UpdateOrderRequest updateOrderRequest = new UpdateOrderRequest(dateTime, new List<string> { "burger" }, 0.1f);
            Guid guid = Guid.NewGuid();

            ErrorOr<Order> order = Order.From(updateOrderRequest, guid);

            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(0);

            // Act
            ErrorOr<Order> result = _ordersService.UpdateOrder(guid, order.Value);

            // Assert
            Assert.That(result.IsError, Is.True);
        }
    }
}