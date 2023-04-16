using Moq;
using Project2Api.Contracts.Order;
using Project2Api.DbTools;
using Project2Api.Services.Orders;
using System.Data;
using Project2Api.Models;
using ErrorOr;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.OrdersServiceTests
{
    [TestFixture]
    internal class CreateOrderTests 
    {
        private Mock<IOrdersRepository> _ordersRepositoryMock = null!;
        private OrdersService _ordersService = null!;

        [SetUp]
        public void SetUp()
        {
            _ordersRepositoryMock = new Mock<IOrdersRepository>();
            _ordersService = new OrdersService(_ordersRepositoryMock.Object);
        }

        [Test]
        public async Task CreateOrder_WithValidOrderRequest_ReturnsOrder()
        {
            // Arrange
            var orderRequest = new OrderRequest(new List<string> { "pita", "meatball" }, 1.0M);

            ErrorOr<Order> order = Order.From(orderRequest);

            _ordersRepositoryMock.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(1);

            // Act
            ErrorOr<Order> result = await _ordersService.CreateOrderAsync(order.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Id, Is.EqualTo(order.Value.Id));
            Assert.That(result.Value.OrderTime, Is.EqualTo(order.Value.OrderTime).Within(1).Seconds);
            Assert.That(result.Value.Price, Is.EqualTo(order.Value.Price));
        }

        [Test]
        public async Task CreateOrder_WithInvalidOrderRequest_ReturnsError()
        {
            // Arrange
            var orderRequest = new OrderRequest(new List<string>{"burger"}, 0.1M);

            ErrorOr<Order> order = Order.From(orderRequest);

            _ordersRepositoryMock.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(0);

            // Act
            ErrorOr<Order> result = await _ordersService.CreateOrderAsync(order.Value);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Orders.DbError));
        }
    }
}
