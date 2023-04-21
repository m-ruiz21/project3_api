using ErrorOr;
using Project2Api.Contracts.Order;
using Project2Api.Models;
using Project2Api.ServiceErrors;

namespace Tests.Models.OrdersTests
{
    // tests for order model
    [TestFixture]
    internal class FromRequest 
    {
        // test for valid order request
        [Test]
        public void FromRequest_ValidOrderRequest_ReturnsOrder()
        {
            // Arrange
            List<string> orderItems = new List<string>();
            orderItems.Add("pita");
            OrderRequest orderRequest = new OrderRequest(orderItems);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderRequest);

            // Assert
            Assert.That(! errorOrOrder.IsError);
            Assert.That(orderItems, Is.EqualTo(errorOrOrder.Value.Items));
            Assert.That(errorOrOrder.Value.OrderTime, ! Is.EqualTo(DateTime.Now)); 
            Assert.That(Guid.Empty, ! Is.EqualTo(errorOrOrder.Value.Id));
        }

        // test for invalid order request
        [Test]
        public void FromRequest_InvalidOrderRequest_ReturnsError()
        {
            // Arrange
            List<string> orderItems = new List<string>();
            OrderRequest orderRequest = new OrderRequest(orderItems);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderRequest);

            // Assert
            Assert.That(errorOrOrder.IsError);
            Assert.That(Errors.Orders.InvalidOrder, Is.EqualTo(errorOrOrder.FirstError));
        }
    }
}