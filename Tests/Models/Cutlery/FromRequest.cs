using ErrorOr;
using Project2Api.Contracts.Cutlery;
using Project2Api.Models;
using Project2Api.ServiceErrors;

namespace Tests.Models.CutleryTests
{
    // tests for order model
    [TestFixture]
    internal class FromRequest 
    {
        // test for valid order request
        [Test]
        public void FromRequest_ValidCutleryRequest_ReturnsCutlery()
        {
            // Arrange 
            CutleryRequest cutleryRequest = new CutleryRequest("bowl", 12); 

            // Act
            ErrorOr<Cutlery> errorOrCutlery = Cutlery.From(cutleryRequest); 

            // Assert
            Assert.That(! errorOrCutlery.IsError);
            Assert.That(errorOrCutlery.Value.Name, Is.EqualTo("bowl")); 
            Assert.That(errorOrCutlery.Value.Quantity, Is.EqualTo(12));
        }

        // test for invalid order request
        [Test]
        public void FromRequest_InvalidOrderRequest_ReturnsError()
        {
            // Arrange
            CutleryRequest cutleryRequest = new CutleryRequest("", -1);

            // Act
            ErrorOr<Cutlery> errorOrCutlery = Cutlery.From(cutleryRequest);

            // Assert
            Assert.That(errorOrCutlery.IsError);
            Assert.That(Errors.Cutlery.InvalidCutlery, Is.EqualTo(errorOrCutlery.FirstError));
        }
    }
}