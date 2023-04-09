using Moq;
using Project2Api.DbTools;
using Project2Api.Services.Orders;
using System.Data;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    
    [TestFixture]
    internal class DeleteMenuItemTests 
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
        public void DeleteOrder_WithValidOrderId_Returns204Status()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(1);

            // Act
            ErrorOr<IActionResult> result = _ordersService.DeleteOrder(orderId);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void DeleteOrder_WithInvalidOrderId_Returns404Status()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(0);

            // Act
            ErrorOr<IActionResult> result = _ordersService.DeleteOrder(orderId);

            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Orders.NotFound));
        }
    }
}