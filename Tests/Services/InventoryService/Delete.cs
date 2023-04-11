using Moq;
using Project2Api.DbTools;
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

        private Mock<IDbClient> _dbClientMock = null!;
        private InventoryService _inventoryService = null!;

        [SetUp]
        public void SetUp()
        {
            _dbClientMock = new Mock<IDbClient>();
            _dbClientMock.Setup(x => x.ExecuteQueryAsync(It.IsAny<string>())).ReturnsAsync(new DataTable());
            _inventoryService = new InventoryService(_dbClientMock.Object);
        } 

        [Test]
        public void DeleteInventory_WithValidInventoryItem_Returns204Status()
        {
            // Arrange
            ErrorOr<InventoryItem> item = InventoryItem.Create("Tests", "Cutlery", 0);
            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(1);


            // Act
            ErrorOr<IActionResult> result = _inventoryService.DeleteInventoryItem(item.Value);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void DeleteInventory_WithInvalidInventoryType_Returns404Status()
        {
            // Arrange
            ErrorOr<InventoryItem> item = InventoryItem.Create("test", "test", 0);

            _dbClientMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>())).ReturnsAsync(-1);

            // Act
            ErrorOr<IActionResult> result = _inventoryService.DeleteInventoryItem(item.Value);

            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Inventory.DbError));
        }
    }
}