using Moq;
using Project2Api.DbTools;
using Project2Api.Services.MenuItems;
using System.Data;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    
    [TestFixture]
    internal class DeleteMenuItemTests 
    {

        private Mock<IMenuItemRepository> _repositoryMock = null!;
        private MenuItemService _menuItemService = null!;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IMenuItemRepository>();
            _menuItemService = new MenuItemService(_repositoryMock.Object);
        }

        [Test]
        public async Task DeleteMenuItem_WithValidMenuItemId_Returns204Status()
        {
            // Arrange
            string name = "Test Name"; 
            _repositoryMock.Setup(x => x.DeleteMenuItemAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            ErrorOr<IActionResult> result = await _menuItemService.DeleteMenuItemAsync(name);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteMenuItem_WithInvalidMenuItemId_Returns404Status()
        {
            // Arrange
            string name = "Test Name"; 
            _repositoryMock.Setup(x => x.DeleteMenuItemAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            ErrorOr<IActionResult> result = await _menuItemService.DeleteMenuItemAsync(name);

            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.MenuItem.NotFound));
        }
    }
}