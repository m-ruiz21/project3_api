using Moq;
using Project2Api.Services.Orders;
using Project2Api.Models;
using ErrorOr;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.OrdersServiceTests
{
    
    [TestFixture]
    internal class GetOrderTest 
    {

        private Mock<IOrdersRepository> _orderRepositoryMock = null!;
        private OrdersService _ordersService = null!;

        [SetUp]
        public void SetUp()
        {
            _orderRepositoryMock = new Mock<IOrdersRepository>();
            _ordersService = new OrdersService(_orderRepositoryMock.Object);
        }
    
        [Test]
        public async Task GetOrder_Exists_ReturnsOrder()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
 
            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(
                new Order
                (
                    orderId,
                    DateTime.Now,
                    10.00M
                )
            );

            // Act
            ErrorOr<Order> result = await _ordersService.GetOrderAsync(orderId);
    
            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Id, Is.EqualTo(orderId));
            Assert.That(result.Value.Price, Is.EqualTo(10.00M));
        }

        [Test]
        public async Task GetOrder_DoesNotExist_ReturnsError()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            
            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((Order?)null);

            // Act
            ErrorOr<Order> result = await _ordersService.GetOrderAsync(orderId);
    
            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Orders.NotFound));
        }
    }
} 