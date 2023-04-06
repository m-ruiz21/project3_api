using ErrorOr;
using Project2Api.Models;
using Project2Api.ServiceErrors;

namespace Tests.Models.OrdersTests
{
    // tests for order model
    [TestFixture]
    internal class CreateOrderTests
    {
        [Test]
        public void CreateOrder_ValidOrder_ReturnsOrder()
        {
            // Arrange
            DateTime orderTime = DateTime.Now;
            List<string> orderItems = new List<string>();
            orderItems.Add("pita");
            Guid guid = Guid.NewGuid(); 
            
            // Act
            ErrorOr<Order> errorOrOrder = Order.Create(
                orderTime,
                orderItems,
                1.0f,
                guid
            );

            // Assert
            Assert.IsFalse(errorOrOrder.IsError);
            Assert.AreEqual(orderTime, errorOrOrder.Value.OrderTime);
            Assert.AreEqual(orderItems, errorOrOrder.Value.Items);
            Assert.AreEqual(1.0f, errorOrOrder.Value.Price);
            Assert.AreEqual(guid, errorOrOrder.Value.Id);
        }

        // test for invalid order time
        [Test]
        public void CreateOrder_InvalidOrderTime_ReturnsError()
        {
            // Arrange
            DateTime orderTime = DateTime.MinValue;
            List<string> orderItems = new List<string>();
            orderItems.Add("pita");
            Guid guid = Guid.NewGuid();

            // Act
            ErrorOr<Order> errorOrOrder = Order.Create(
                orderTime,
                orderItems,
                0.0f,
                guid
            );

            // Assert
            Assert.IsTrue(errorOrOrder.IsError);
            Assert.AreEqual(Errors.Orders.InvalidOrder, errorOrOrder.FirstError);
        }

        // test for invalid order price
        [Test]
        public void CreateOrder_InvalidOrderPrice_ReturnsError()
        {
            // Arrange
            DateTime orderTime = DateTime.Now;
            List<string> orderItems = new List<string>();
            orderItems.Add("pita");
            Guid guid = Guid.NewGuid();

            // Act
            ErrorOr<Order> errorOrOrder = Order.Create(
                orderTime,
                orderItems,
                0.0f,
                guid
            );

            // Assert
            Assert.IsTrue(errorOrOrder.IsError);
            Assert.AreEqual(Errors.Orders.InvalidOrder, errorOrOrder.FirstError);
        }
    }
}
