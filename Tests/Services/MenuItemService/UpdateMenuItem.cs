using Moq;
using Project2Api.ServiceErrors;
using Project2Api.DbTools;
using Project2Api.Services.MenuItems;
using System.Data;
using Project2Api.Models;
using ErrorOr;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    [TestFixture]
    internal class UpdateMenuItemTests 
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
        public async Task UpdateMenuItem_WithValidMenuItemId_Returns200Status()
        {
            // Arrange
            // create mock menu item
            string name = "Test Name";
            decimal price = 10.00M;
            string category = "Test Category";
            int quantity = 10;
            List<string> cutlery = new List<string>() {"Test Cutlery"};
            ErrorOr<MenuItem> errorOrMenuItem = MenuItem.Create(name, price, category, quantity, cutlery);

            MenuItem menuItem = errorOrMenuItem.Value;

            _repositoryMock.Setup(x => x.UpdateMenuItemAsync(It.IsAny<MenuItem>())).ReturnsAsync(1);
            
            // Act
            ErrorOr<MenuItem> result = await _menuItemService.UpdateMenuItem(name, menuItem);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Name, Is.EqualTo(menuItem.Name));
            Assert.That(result.Value.Price, Is.EqualTo(menuItem.Price));
            Assert.That(result.Value.Category, Is.EqualTo(menuItem.Category));
            Assert.That(result.Value.Quantity, Is.EqualTo(menuItem.Quantity));
            Assert.That(result.Value.Cutlery, Is.EqualTo(menuItem.Cutlery));
        }

        [Test]
        public async Task UpdateMenuItem_WithInvalidMenuItem_Returns404Status()
        {
            // Arrange
            string name = "Test Name";
            decimal price = 10.00M;
            string category = "Test Category";
            int quantity = 10;
            List<string> cutlery = new List<string>() {"Test Cutlery"};
            ErrorOr<MenuItem> errorOrMenuItem = MenuItem.Create(name, price, category, quantity, cutlery);

            MenuItem menuItem = errorOrMenuItem.Value;

            _repositoryMock.Setup(x => x.UpdateMenuItemAsync(It.IsAny<MenuItem>())).ReturnsAsync(0);

            // Act
            ErrorOr<MenuItem> result = await _menuItemService.UpdateMenuItem(name, menuItem);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(Errors.MenuItem.NotFound));
        }
    }
}