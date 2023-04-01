using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.Order;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        /// <summary>
        /// Creates order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns>Added Object</returns>
        [HttpPost()]
        public IActionResult CreateOrder(OrderRequest orderRequest)
        {
            return Ok(orderRequest);
        }
        
        /// <summary>
        /// Gets order with specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Requested object</returns>
        [HttpGet("{id:guid}")]
        public IActionResult GetOrder(Guid id)
        {
            return Ok(id);
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of orderResponses</returns>
        [HttpGet()]
        public IActionResult GetAllOrders()
        {
            return Ok();
        }

        /// <summary>
        /// Updates order with specific ID
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="id"></param>
        /// <returns>Updated order object</returns>
        [HttpPut("{id:guid}")]
        public IActionResult UpdateOrder(OrderRequest orderRequest, Guid id)
        {
            return Ok(orderRequest);
        }


        /// <summary>
        /// Deletes order with specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Succes / Failure code</returns>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteOrder(Guid id)
        {
            return Ok(id);
        }
    }
}
