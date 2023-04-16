using Moq;
using Project2Api.DbTools;
using Project2Api.Services.MenuItems;
using System.Data;
using Project2Api.Models;
using ErrorOr;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    
    [TestFixture]
    internal class GetMenuItemTest 
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
        public async Task GetMenuItem_WithValidMenuItemId_Returns200Status()
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

            _repositoryMock.Setup(x => x.GetMenuItemByNameAsync(It.IsAny<string>())).ReturnsAsync(menuItem);

            // Act
            ErrorOr<MenuItem> result = await _menuItemService.GetMenuItemAsync(name);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Name, Is.EqualTo(name));
            Assert.That(result.Value.Price, Is.EqualTo(price));
            Assert.That(result.Value.Category, Is.EqualTo(category));
            Assert.That(result.Value.Quantity, Is.EqualTo(quantity));
            Assert.That(result.Value.Cutlery, Is.EqualTo(cutlery));
        }

        [Test]
        public async Task GetMenuItem_WithInvalidMenuItemId_Returns404Status()
        {
            // Arrange
            // create mock menu item
            string name = "Test Name";

            _repositoryMock.Setup(x => x.GetMenuItemByNameAsync(It.IsAny<string>())).ReturnsAsync((MenuItem?)null);

            // Act
            ErrorOr<MenuItem> result = await _menuItemService.GetMenuItemAsync(name);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.MenuItem.NotFound));
        }
    }
}