using Moq;
using Project2Api.Contracts.Order;
using Project2Api.Services.Orders;
using Project2Api.Models;
using ErrorOr;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.OrdersServiceTests
{
    [TestFixture]
    internal class UpdateOrderTests 
    {
        private Mock<IOrdersRepository> _ordersRepositoryMock= null!;
        private OrdersService _ordersService = null!;

        [SetUp]
        public void SetUp()
        {
            _ordersRepositoryMock = new Mock<IOrdersRepository>(); 
            _ordersService = new OrdersService(_ordersRepositoryMock.Object);    
        }

        [Test]
        public async Task UpdateOrder_WithValidOrderRequest_ReturnsOrder()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            UpdateOrderRequest updateOrderRequest = new UpdateOrderRequest(dateTime, new List<string> { "pita", "meatball" }, 1.0M);
            Guid guid = Guid.NewGuid();
            
            _ordersRepositoryMock.Setup(x => x.UpdateOrderAsync(It.IsAny<Order>())).ReturnsAsync(1);

            ErrorOr<Order> order = Order.From(updateOrderRequest, guid);

            // Act
            ErrorOr<Order> result = await _ordersService.UpdateOrderAsync(guid, order.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Id, Is.EqualTo(order.Value.Id));
            Assert.That(result.Value.OrderTime, Is.EqualTo(order.Value.OrderTime).Within(1).Seconds);
            Assert.That(result.Value.Price, Is.EqualTo(order.Value.Price));
        }

        [Test]
        public async Task UpdateOrder_WithInvalidOrderRequest_ReturnsError()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            UpdateOrderRequest updateOrderRequest = new UpdateOrderRequest(dateTime, new List<string> { "burger" }, 0.1M);
            Guid guid = Guid.NewGuid();

            ErrorOr<Order> order = Order.From(updateOrderRequest, guid);

            _ordersRepositoryMock.Setup(x => x.UpdateOrderAsync(It.IsAny<Order>())).ReturnsAsync(0);

            // Act
            ErrorOr<Order> result = await _ordersService.UpdateOrderAsync(guid, order.Value);

            // Assert
            Assert.That(result.IsError, Is.True);
        }
    }
}