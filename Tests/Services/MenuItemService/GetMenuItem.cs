using Moq;
using Project2Api.DbTools;
using Project2Api.Services.MenuItems;
using System.Data;
using Project2Api.Models;
using ErrorOr;

namespace Project2Api.Tests.Services.MenuItemServiceTests
{
    
    [TestFixture]
    internal class GetMenuItemTest 
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
        public void GetMenuItem_WithValidMenuItemId_Returns200Status()
        {
            // Arrange
            // create mock menu item
            string name = "Test Name";
            float price = 10.00f;
            string category = "Test Category";
            int quantity = 10;
            List<string> cutlery = new List<string>() {"Test Cutlery"};
            ErrorOr<MenuItem> errorOrMenuItem = MenuItem.Create(name, price, category, quantity, cutlery);

            MenuItem menuItem = errorOrMenuItem.Value;

            // set up return for get menu item
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Price");
            dataTable.Columns.Add("Category");
            dataTable.Columns.Add("Quantity");
            DataRow dataRow = dataTable.NewRow();
            dataRow["Name"] = menuItem.Name;
            dataRow["Price"] = menuItem.Price;
            dataRow["Category"] = menuItem.Category;
            dataRow["Quantity"] = menuItem.Quantity;
            dataTable.Rows.Add(dataRow);

            _dbClientMock.Setup(x => x.ExecuteQueryAsync($"SELECT * FROM menu_item WHERE name = '{name}'")).ReturnsAsync(dataTable);

            // set up return for cutlery
            DataTable cutleryDataTable = new DataTable();
            cutleryDataTable.Columns.Add("cutlery_name");
            DataRow cutleryDataRow = cutleryDataTable.NewRow();
            cutleryDataRow["cutlery_name"] = cutlery[0];
            cutleryDataTable.Rows.Add(cutleryDataRow);

            _dbClientMock.Setup(x => x.ExecuteQueryAsync($"SELECT cutlery_name FROM menu_item_cutlery WHERE menu_item_name = '{name}'")).ReturnsAsync(cutleryDataTable);

            // Act
            ErrorOr<MenuItem> result = _menuItemService.GetMenuItem(name);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Name, Is.EqualTo(name));
            Assert.That(result.Value.Price, Is.EqualTo(price));
            Assert.That(result.Value.Category, Is.EqualTo(category));
            Assert.That(result.Value.Quantity, Is.EqualTo(quantity));
            Assert.That(result.Value.Cutlery, Is.EqualTo(cutlery));
        }

        [Test]
        public void GetMenuItem_WithInvalidMenuItemId_Returns404Status()
        {
            // Arrange
            // create mock menu item
            string name = "Test Name";

            // set up return for get menu item
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Price");
            dataTable.Columns.Add("Category");
            dataTable.Columns.Add("Quantity");

            _dbClientMock.Setup(x => x.ExecuteQueryAsync($"SELECT * FROM menu_item WHERE name = '{name}'")).ReturnsAsync(dataTable);

            // Act
            ErrorOr<MenuItem> result = _menuItemService.GetMenuItem(name);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.MenuItem.NotFound));
        }
    }
}