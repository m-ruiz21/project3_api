using System.Data;
using ErrorOr;
using Project2Api.Models;
using Project2Api.ServiceErrors;

namespace Tests.Models.MenuItemTests
{
    // tests MenuItem.From(TableRow) model
    [TestFixture]
    internal class FromRow 
    {
        public DataTable table = new DataTable();

        [OneTimeSetUp]        
        public void SetUp()
        {
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("price", typeof(float));
            table.Columns.Add("category", typeof(string));
            table.Columns.Add("quantity", typeof(int));
        }

        // test for valid menu item request
        [Test]
        public void FromTable_ValidMenuItemRequest_ReturnsMenuItem()
        {
            DataRow row = table.NewRow();
            row["name"] = "test";
            row["price"] = 1.0f;
            row["category"] = "test";
            row["quantity"] = 1;

            // Act
            ErrorOr<MenuItem> result = MenuItem.From(row);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Name, Is.EqualTo(row["name"]));
            Assert.That(result.Value.Price, Is.EqualTo(row["price"]));
            Assert.That(result.Value.Category, Is.EqualTo(row["category"]));
            Assert.That(result.Value.Quantity, Is.EqualTo(row["quantity"]));
        }

        // test for invalid menu item request
        [Test]
        public void FromTable_InvalidMenuItemRequest_ReturnsError()
        {
            // Arrange
            DataRow row = table.NewRow();
            row["name"] = null;
            row["price"] = 0.0f;
            row["category"] = null;
            row["quantity"] = 0;

            // Act
            ErrorOr<MenuItem> result = MenuItem.From(row);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(Errors.MenuItem.NotFound));
        }
    }
}