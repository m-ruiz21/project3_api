using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.Order;
using Project2Api.Models;
using Project2Api.Services.Orders;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        } 

        /// <summary>
        /// Creates order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns>Added Object</returns>
        [HttpPost()]
        public IActionResult CreateOrder(OrderRequest orderRequest)
        {
            if (orderRequest.Items.Count == 0 || orderRequest.Items == null)
            {
                return BadRequest(
                    new { error = "Order Must Not Be Empty"}
                );
            }

            if (orderRequest.Price == 0)
            {
                return BadRequest(
                    new { error = "Price Must Be Greater Than 0" }
                );
            }

            // create new order item 
            Order order = new Order(
                new Guid(),
                DateTime.Now,
                orderRequest.Items,
                orderRequest.Price
            );

            // TODO: save order to database
            
            OrderResponse orderResponse = new OrderResponse(
                order.Id,
                order.OrderTime,
                order.Items,
                order.Price
            );

            // return 201 response with orderResponse
            return CreatedAtAction(
                nameof(GetOrder),
                new { id = orderResponse.Id },
                orderResponse
            );
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
