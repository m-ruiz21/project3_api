using System.Data;
using ErrorOr;
using Project2Api.Models;
using Project2Api.ServiceErrors;

namespace Tests.Models.OrdersTests
{
    // tests for order model
    [TestFixture]
    internal class FromRow 
    {
        public DataTable table = new DataTable();

        [OneTimeSetUp]        
        public void SetUp()
        {
            table.Columns.Add("id", typeof(Guid));
            table.Columns.Add("date_time", typeof(DateTime));
            table.Columns.Add("total_price", typeof(float));
        }

        // test for valid order request
        [Test]
        public void FromTable_ValidOrderRequest_ReturnsOrder()
        {
            DataRow row = table.NewRow();
            row["id"] = Guid.NewGuid();
            row["date_time"] = DateTime.Now;
            row["total_price"] = 1.0f;

            // Act
            ErrorOr<Order> result = Order.From(row);

            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Id, Is.EqualTo(row["id"]));
            Assert.That(result.Value.OrderTime, Is.EqualTo(row["date_time"]).Within(1).Seconds);
            Assert.That(result.Value.Price, Is.EqualTo(row["total_price"])); 
        }

        // test for invalid order request
        [Test]
        public void FromTable_InvalidOrderRequest_ReturnsError()
        {
            // Arrange
            DataRow row = table.NewRow();
            row["id"] = Guid.NewGuid();
            row["date_time"] = DateTime.Now;
            row["total_price"] = 0.0f;

            // Act
            ErrorOr<Order> result = Order.From(row);

            // Assert
            Assert.That(result.IsError, Is.True);
            Assert.That(result.FirstError, Is.EqualTo(Errors.Orders.InvalidOrder));
        }
    }
}