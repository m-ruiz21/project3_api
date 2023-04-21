using Moq;
using Project2Api.Services.Orders;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.OrdersServiceTests
{
    
    [TestFixture]
    internal class DeleteOrderTests 
    {

        private Mock<IOrdersRepository> _ordersRepositoryMock = null!;

        private Mock<IMenuItemRepository> _menuItemRepositoryMock = null!;

        private OrdersService _ordersService = null!;

        [SetUp]
        public void SetUp()
        {
            _ordersRepositoryMock = new Mock<IOrdersRepository>();
            _menuItemRepositoryMock = new Mock<IMenuItemRepository>();
            _ordersService = new OrdersService(_ordersRepositoryMock.Object, _menuItemRepositoryMock.Object);
        } 

        [Test]
        public async Task DeleteOrder_WithValidOrderId_Returns204Status()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            _ordersRepositoryMock.Setup(x => x.DeleteOrderAsync(orderId)).ReturnsAsync(true);

            // Act
            ErrorOr<IActionResult> result = await _ordersService.DeleteOrderAsync(orderId);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteOrder_WithInvalidOrderId_Returns404Status()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            _ordersRepositoryMock.Setup(x => x.DeleteOrderAsync(orderId)).ReturnsAsync(false);

            // Act
            ErrorOr<IActionResult> result = await _ordersService.DeleteOrderAsync(orderId);

            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Orders.NotFound));
        }
    }
}