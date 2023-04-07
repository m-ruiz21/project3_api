using Moq;
using Project2Api.Contracts.Order;
using Project2Api.DbTools;
using Project2Api.Services.Orders;
using System.Data;
using Project2Api.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Project2Api.Tests.Services.OrdersServiceTests
{
    
    [TestFixture]
    internal class GetOrderTest 
    {

        private Mock<IDbClient> _dbClientMock = null!;
        private OrdersService _ordersService = null!;

        [SetUp]
        public void SetUp()
        {
            _dbClientMock = new Mock<IDbClient>();
            _ordersService = new OrdersService(_dbClientMock.Object);
        }
    
        [Test]
        public void GetOrder_Exists_ReturnsOrder()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id");
            dataTable.Columns.Add("date_time");
            dataTable.Columns.Add("total_price");
            dataTable.Rows.Add(orderId, DateTime.Now, 10.00);
            
            // set up return for get order
            _dbClientMock.Setup(x => x.ExecuteQueryAsync($"SELECT * FROM orders WHERE id = '{orderId}'")).ReturnsAsync(dataTable);

            // set up moq return for get menu items
            DataTable menuItemsDataTable = new DataTable();
            menuItemsDataTable.Columns.Add("menu_item_name");
            menuItemsDataTable.Columns.Add("quantity");
            menuItemsDataTable.Columns.Add("order_id");
            menuItemsDataTable.Rows.Add("falafel", 1, orderId);

            _dbClientMock.Setup(x => x.ExecuteQueryAsync($"SELECT menu_item_name FROM ordered_menu_item WHERE order_id = '{orderId}'")).ReturnsAsync(menuItemsDataTable);

            // Act
            ErrorOr<Order> result = _ordersService.GetOrder(orderId);
    
            // Assert
            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value.Id, Is.EqualTo(orderId));
            Assert.That(result.Value.Price, Is.EqualTo(10.00));
        }

        [Test]
        public void GetOrder_DoesNotExist_ReturnsError()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id");
            dataTable.Columns.Add("date_time");
            dataTable.Columns.Add("total_price");
            
            // set up return for get order
            _dbClientMock.Setup(x => x.ExecuteQueryAsync($"SELECT * FROM orders WHERE id = '{orderId}'")).ReturnsAsync(dataTable);

            // Act
            ErrorOr<Order> result = _ordersService.GetOrder(orderId);
    
            // Assert
            Assert.That(result.IsError);
            Assert.That(result.FirstError, Is.EqualTo(ServiceErrors.Errors.Orders.NotFound));
        }
    }
} 