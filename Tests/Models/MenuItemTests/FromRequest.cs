using ErrorOr;
using Project2Api.Contracts.Order;
using Project2Api.Models;
using Project2Api.ServiceErrors;

namespace Tests.Models.MenuItemTests
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
            OrderRequest orderRequest = new OrderRequest(orderItems, 0.1M);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderRequest);

            // Assert
            Assert.That(! errorOrOrder.IsError);
            Assert.That(orderItems, Is.EqualTo(errorOrOrder.Value.Items));
            Assert.That(0.1f, Is.EqualTo(errorOrOrder.Value.Price).Within(0.01M));
            Assert.That(errorOrOrder.Value.OrderTime, ! Is.EqualTo(DateTime.Now)); 
            Assert.That(Guid.Empty, ! Is.EqualTo(errorOrOrder.Value.Id));
        }

        // test for invalid order request
        [Test]
        public void FromRequest_InvalidOrderRequest_ReturnsError()
        {
            // Arrange
            List<string> orderItems = new List<string>();
            orderItems.Add("pita");
            OrderRequest orderRequest = new OrderRequest(orderItems, 0.0M);

            // Act
            ErrorOr<Order> errorOrOrder = Order.From(orderRequest);

            // Assert
            Assert.That(errorOrOrder.IsError);
            Assert.That(Errors.Orders.InvalidOrder, Is.EqualTo(errorOrOrder.FirstError));
        }
    }
}