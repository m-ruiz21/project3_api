using Moq;
using Project2Api.Repositories;
using Project2Api.Services.Inventory;
using System.Data;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;

namespace Project2Api.Tests.Services.InventoryServiceTests
{
    
    [TestFixture]
    internal class DeleteInventoryTests 
    {

        private Mock<IMenuItemRepository> _menuItemRepositoryMock = null!;
        private Mock<ICutleryRepository> _cutleryRepositoryMock = null!;

        private InventoryService _inventoryService = null!;

        [SetUp]
        public void SetUp()
        {
            _cutleryRepositoryMock = new Mock<ICutleryRepository>();
            _menuItemRepositoryMock = new Mock<IMenuItemRepository>(); 
            _inventoryService = new InventoryService(_cutleryRepositoryMock.Object, _menuItemRepositoryMock.Object);
        } 

        [Test]
        public async Task DeleteCutleryInventory_WithValidInventoryItem_Returns204Status()
        {
            // Arrange
            ErrorOr<InventoryItem> item = InventoryItem.Create("tests", "cutlery", 0);
            _cutleryRepositoryMock.Setup(x => x.DeleteCutleryAsync(It.IsAny<string>())).ReturnsAsync(true);
            
            if (item.IsError)
            {
                Console.WriteLine(item.FirstError);
                Assert.Fail();
            }

            // Act
            ErrorOr<IActionResult> result = await _inventoryService.DeleteInventoryItem(item.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteMenuItemInventory_WithValidInventoryItem_Returns204Status()
        {
            // Arrange
            ErrorOr<InventoryItem> item = InventoryItem.Create("tests", "menu item", 1);
            _menuItemRepositoryMock.Setup(x => x.DeleteMenuItemAsync(It.IsAny<string>())).ReturnsAsync(true);
            
            if (item.IsError)
            {
                Console.WriteLine(item.FirstError);
                Assert.Fail();
            }

            // Act
            ErrorOr<IActionResult> result = await _inventoryService.DeleteInventoryItem(item.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteInventory_WithInvalidInventoryType_Returns404Status()
        {
            // Arrange
            ErrorOr<InventoryItem> item = InventoryItem.Create("test", "cutlery", 0);

            if (item.IsError)
            {
                Console.WriteLine(item.FirstError);
                Assert.Fail();
            }

            _cutleryRepositoryMock.Setup(x => x.DeleteCutleryAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            ErrorOr<IActionResult> result = await _inventoryService.DeleteInventoryItem(item.Value);

            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Inventory.DbError));
        }
    }
}