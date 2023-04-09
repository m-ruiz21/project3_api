using Moq;
using Project2Api.DbTools;
using Project2Api.Services.MenuItems;
using System.Data;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    
    [TestFixture]
    internal class DeleteMenuItemTests 
    {

        private Mock<IDbClient> _dbClientMock = null!;
        private MenuItemService _menuItemService = null!;

        [SetUp]
        public void SetUp()
        {
            _dbClientMock = new Mock<IDbClient>();
            _dbClientMock.Setup(x => x.ExecuteQueryAsync(It.IsAny<string>())).ReturnsAsync(new DataTable());
            _menuItemService = new MenuItemService(_dbClientMock.Object);
        } 

        [Test]
        public void DeleteMenuItem_WithValidMenuItemId_Returns204Status()
        {
            // Arrange
            string name = "Test Name"; 
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(1);

            // Act
            ErrorOr<IActionResult> result = _menuItemService.DeleteMenuItem(name);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void DeleteMenuItem_WithInvalidMenuItemId_Returns404Status()
        {
            // Arrange
            string name = "Test Name"; 
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(0);

            // Act
            ErrorOr<IActionResult> result = _menuItemService.DeleteMenuItem(name);

            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.MenuItem.NotFound));
        }
    }
}