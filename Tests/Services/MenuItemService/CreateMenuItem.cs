using Moq;
using Project2Api.Contracts.MenuItem;
using Project2Api.Services.MenuItems;
using Project2Api.Models;
using ErrorOr;
using Project2Api.Repositories;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    [TestFixture]
    internal class CreateMenuItemTests
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
        public async Task CreateMenuItem_WithValidMenuItemRequest_ReturnsMenuItem()
        {
            // Arrange
            MenuItemRequest menuItemRequest = new MenuItemRequest("pita", 1.0M, "base", 2000, new List<string> { "utensils", "plate" });

            ErrorOr<MenuItem> menuItem = MenuItem.From(menuItemRequest);
            
            _repositoryMock.Setup(x => x.CreateMenuItemAsync(It.IsAny<MenuItem>())).ReturnsAsync(1);

            // Act
            ErrorOr<MenuItem> result = await _menuItemService.CreateMenuItemAsync(menuItem.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Name, Is.EqualTo(menuItem.Value.Name));
            Assert.That(result.Value.Price, Is.EqualTo(menuItem.Value.Price));
            Assert.That(result.Value.Quantity, Is.EqualTo(menuItem.Value.Quantity));
            Assert.That(result.Value.Cutlery, Is.EqualTo(menuItem.Value.Cutlery));
        }

        [Test]
        public async Task CreateMenuItem_WithInvalidMenuItem_ReturnsError()
        {
            // Arrange
            MenuItemRequest menuItemRequest = new MenuItemRequest("burger", 0.1M, "meal", 1, new List<string> { "utensils", "plate" });
            MenuItem newMenuItem = MenuItem.From(menuItemRequest).Value;

            _repositoryMock.Setup(x => x.CreateMenuItemAsync(It.IsAny<MenuItem>())).ReturnsAsync(0);

            // Act
            ErrorOr<MenuItem> result = await _menuItemService.CreateMenuItemAsync(newMenuItem);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.MenuItem.DbError));
        }
    }
}