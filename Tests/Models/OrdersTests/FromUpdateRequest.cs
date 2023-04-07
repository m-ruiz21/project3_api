using ErrorOr;
using Project2Api.Contracts.Order;
using Project2Api.Models;
using Project2Api.ServiceErrors;

namespace Tests.Models.OrdersTests
{
    // tests for order model
    [TestFixture]
    internal class FromUpdateRequest 
    {
        // test for valid order request
        [Test]
        public void FromUpdateRequest_ValidOrderRequest_ReturnsOrder()
        {
            // Arrange
            List<string> orderItems = new List<string>();
            orderItems.Add("pita");
            DateTime orderTime = DateTime.Now;
            Guid guid = Guid.NewGuid();
            float price = 0.1f;
            UpdateOrderRequest orderUpdateRequest = new UpdateOrderRequest(orderTime, orderItems, price);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderUpdateRequest, guid);

            // Assert
            Assert.That(! errorOrOrder.IsError);
            Assert.That(orderItems, Is.EqualTo(errorOrOrder.Value.Items));
            Assert.That(price, Is.EqualTo(errorOrOrder.Value.Price).Within(0.01f));
            Assert.That(orderTime, Is.EqualTo(errorOrOrder.Value.OrderTime));
            Assert.That(guid, Is.EqualTo(errorOrOrder.Value.Id));
        }

        // test for invalid order request
        [Test]
        public void FromUpdateRequest_InvalidOrderRequest_ReturnsError()
        {
            // Arrange
            List<string> orderItems = new List<string>();
            orderItems.Add("pita");
            DateTime orderTime = DateTime.Now;
            Guid guid = Guid.NewGuid();
            float price = 0.0f;
            UpdateOrderRequest orderUpdateRequest = new UpdateOrderRequest(orderTime, orderItems, price);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderUpdateRequest, guid);

            // Assert
            Assert.That(errorOrOrder.IsError);
            Assert.That(Errors.Orders.InvalidOrder, Is.EqualTo(errorOrOrder.FirstError));
        }
    }
}