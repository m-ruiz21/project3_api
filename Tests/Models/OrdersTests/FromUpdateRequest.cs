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
            UpdateOrderRequest orderUpdateRequest = new UpdateOrderRequest(orderTime, orderItems);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderUpdateRequest, guid);

            // Assert
            Assert.That(! errorOrOrder.IsError);
            Assert.That(orderItems, Is.EqualTo(errorOrOrder.Value.Items));
            Assert.That(orderTime, Is.EqualTo(errorOrOrder.Value.OrderTime));
            Assert.That(guid, Is.EqualTo(errorOrOrder.Value.Id));
        }

        // test for invalid order request
        [Test]
        public void FromUpdateRequest_InvalidOrderRequest_ReturnsError()
        {
            // Arrange
            List<string> orderItems = new List<string>();
            DateTime orderTime = DateTime.Now;
            Guid guid = Guid.NewGuid();
            UpdateOrderRequest orderUpdateRequest = new UpdateOrderRequest(orderTime, orderItems);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderUpdateRequest, guid);

            // Assert
            Assert.That(errorOrOrder.IsError);
            Assert.That(Errors.Orders.InvalidOrder, Is.EqualTo(errorOrOrder.FirstError));
        }
    }
}