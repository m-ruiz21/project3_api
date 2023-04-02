using NUnit.Framework;
using Moq;
using System.Data;
using Project2Api.Models;
using Project2Api.DbTools;

namespace Project2Api.Services.Orders.Tests.OrdersServiceTests
{
    public class ConvertDataTableToOrderTests
    {
        private OrdersService? _ordersService;
        private Mock<IDbClient>? _dbClientMock;

        [SetUp]
        public void Setup()
        {
            _dbClientMock = new Mock<IDbClient>();
            _ordersService = new OrdersService(_dbClientMock.Object);
        }

        [Test]
        public void ConvertDataTableToOrder_WhenTableIsEmpty_ReturnsNull()
        {
            // Arrange
            DataTable orderTable = new DataTable();

            // Act
            Order? order = _ordersService?.ConvertDataTableToOrder(orderTable);

            // Assert
            Assert.IsNull(order);
        }

        [Test]
        public void ConvertDataTableToOrder_WhenTableHasNullColumn_ReturnsNull()
        {
            // Arrange
            DataTable orderTable = new DataTable();
            orderTable.Columns.Add("id");
            orderTable.Columns.Add("order_time");
            orderTable.Columns.Add("price");
            orderTable.Rows.Add(null, null, null);

            // Act
            Order? order = _ordersService?.ConvertDataTableToOrder(orderTable);

            // Assert
            Assert.IsNull(order);
        }

        [Test]
        public void ConvertDataTableToOrder_WhenTableHasValidData_ReturnsOrder()
        {
            // Arrange
            DataTable orderTable = new DataTable();
            orderTable.Columns.Add("id");
            orderTable.Columns.Add("order_time");
            orderTable.Columns.Add("price");
            String guid = Guid.NewGuid().ToString();
            orderTable.Rows.Add(guid, "2021-01-01 00:00:00", "10.00");

            // Act
            Order? order = _ordersService?.ConvertDataTableToOrder(orderTable);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(Guid.Parse(guid), order?.Id);
            Assert.AreEqual(DateTime.Parse("2021-01-01 00:00:00"), order?.OrderTime);
            Assert.AreEqual(10.00, order?.Price);
        }

        [Test]
        public void ConvertDataTableToOrder_WhenTableHasInvalidData_ReturnsNull()
        {
            // Arrange
            DataTable orderTable = new DataTable();
            orderTable.Columns.Add("id");
            orderTable.Columns.Add("order_time");
            orderTable.Columns.Add("price");
            orderTable.Rows.Add("1", "2021-01-01 00:00:00", "10.00");

            // Act
            Order? order = _ordersService?.ConvertDataTableToOrder(orderTable);

            // Assert
            Assert.IsNull(order);
        }
    }
}