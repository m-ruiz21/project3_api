using Moq;
using Project2Api.Contracts.MenuItem;
using Project2Api.DbTools;
using Project2Api.Services.MenuItems;
using Project2Api.Models;
using ErrorOr;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    [TestFixture]
    internal class CreateMenuItemTests
    {
        private Mock<IDbClient> _dbClientMock = null!;
        private MenuItemService _menuItemService = null!;

        [SetUp]
        public void SetUp()
        {
            _dbClientMock = new Mock<IDbClient>();
            _menuItemService = new MenuItemService(_dbClientMock.Object);
        }

        [Test]
        public void CreateMenuItem_WithValidMenuItemRequest_ReturnsMenuItem()
        {
            // Arrange
            MenuItemRequest menuItemRequest = new MenuItemRequest("pita", 1.0f, "base", 2000, new List<string> { "utensils", "plate" });

            ErrorOr<MenuItem> menuItem = MenuItem.From(menuItemRequest);

            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(1);

            // Act
            ErrorOr<MenuItem> result = _menuItemService.CreateMenuItem(menuItem.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Name, Is.EqualTo(menuItem.Value.Name));
            Assert.That(result.Value.Price, Is.EqualTo(menuItem.Value.Price));
            Assert.That(result.Value.Quantity, Is.EqualTo(menuItem.Value.Quantity));
            Assert.That(result.Value.Cutlery, Is.EqualTo(menuItem.Value.Cutlery));
        }

        [Test]
        public void CreateMenuItem_WithInvalidMenuItem_ReturnsError()
        {
            // Arrange
            MenuItemRequest menuItemRequest = new MenuItemRequest("burger", 0.1f, "meal", 1, new List<string> { "utensils", "plate" });
            MenuItem newMenuItem = MenuItem.From(menuItemRequest).Value;

            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(-1);

            // Act
            ErrorOr<MenuItem> result = _menuItemService.CreateMenuItem(newMenuItem);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.MenuItem.DbError));
        }
    }
}