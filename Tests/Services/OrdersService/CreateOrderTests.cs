using NUnit.Framework;
using Moq;
using System.Data;
using Project2Api.Models;
using Project2Api.DbTools;
using ErrorOr;

namespace Project2Api.Services.Orders.Tests.OrdersServices.CreateOrder
{
    public class CreateOrderTests
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
        public void CreateOrder_WhenDbClientReturnsError_ReturnsError()
        {
            // Arrange
            String guid = Guid.NewGuid().ToString();
            Order order = new Order(Guid.Parse(guid), DateTime.Now, new List<string>(), 10.00f);
            _dbClientMock?.Setup(
                x => x.ExecuteQueryAsync(
                    It.IsAny<String>()
                    )
                )
                .Returns(new Task<DataTable>(() => new DataTable()));

            // Act
            ErrorOr<Order>? result = _ordersService?.CreateOrder(order);

            // Assert
            Assert.IsTrue(result?.IsError);
        }

        [Test]
        public void CreateOrder_WithInvalidOrder_ReturnsError()
        {
            // Arrange
            String guid = Guid.NewGuid().ToString();
            Order order = new Order(Guid.Parse(guid), DateTime.Now, new List<string>(), 10.00f);
            _dbClientMock?.Setup(
                x => x.ExecuteQueryAsync(
                    It.IsAny<String>()
                    )
                )
                .Returns(new Task<DataTable>(() => new DataTable()));

            // Act
            ErrorOr<Order>? result = _ordersService?.CreateOrder(order);

            // Assert
            Assert.IsTrue(result?.IsError);
        }

        [Test]
        public void CreateOrder_WithValidOrder_ReturnsOrder()
        {
            // Arrange
            String guid = Guid.NewGuid().ToString();
            List<string> items = new List<string>() { "falafel" };
            Order order = new Order(Guid.Parse(guid), DateTime.Now, items, 10.00f);
            DataTable orderTable = new DataTable();
            orderTable.Columns.Add("id");
            orderTable.Columns.Add("order_time");
            orderTable.Columns.Add("price");
            orderTable.Rows.Add(guid, "2021-01-01 00:00:00", "10.00");
            _dbClientMock?.Setup(
                x => x.ExecuteNonQueryAsync(
                    It.IsAny<String>()
                    )
                )
                .Returns(new Task<int>(() => 1));
 
            // Act
            ErrorOr<Order>? result = _ordersService?.CreateOrder(order); 
            
            // Assert
            Console.Out.WriteLine(result?.FirstError.Description); 
            Assert.IsFalse(result?.IsError);
            Assert.AreEqual(order, result?.Value);
        }
    }
}