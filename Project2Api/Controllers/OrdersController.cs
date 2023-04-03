using ErrorOr;
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
            // create new order item 
            Order order = new Order(
                Guid.NewGuid(),
                DateTime.Now,
                orderRequest.Items,
                orderRequest.Price
            );

            // save order to database
            ErrorOr<Order> orderErrorOr = _ordersService.CreateOrder(order);

            // check if order was created successfully
            // if not, return status code based on error type
            if (orderErrorOr.IsError)
            {
                if (orderErrorOr.FirstError.Code == "Order.DbError")
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new { error = orderErrorOr.Errors[0].Description }
                    );
                }
                else
                {
                    return BadRequest(
                        new { error = orderErrorOr.Errors[0].Description }
                    );
                }
            }

            OrderResponse orderResponse = new OrderResponse(
                order.Id,
                order.OrderTime,
                order.Items,
                order.Price
            );

            // return 201 response with orderResponse
            return CreatedAtAction(
                nameof(GetOrder),
                new { id = order.Id },
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
            // get order with id=id
            ErrorOr<Order> orderErrorOr = _ordersService.GetOrder(id);

            // check if order existed
            if (orderErrorOr.IsError)
            {
                return NotFound(
                    new { error = orderErrorOr.Errors[0].Description }
                );
            }

            OrderResponse orderResponse = new OrderResponse(
                orderErrorOr.Value.Id,
                orderErrorOr.Value.OrderTime,
                orderErrorOr.Value.Items,
                orderErrorOr.Value.Price
            );

            return Ok(orderResponse);
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of orderResponses</returns>
        [HttpGet()]
        public IActionResult GetAllOrders(int pageNumber = 1, int pageSize = 50)
        {
            // get all orders
            ErrorOr<List<Order>> ordersErrorOr = _ordersService.GetAllOrders(pageNumber, pageSize);

            // check if orders were successfully retreieved
            if (ordersErrorOr.IsError)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { error = ordersErrorOr.Errors[0].Description }
                );
            }

            // get list of orders and map to list of orderResponses
            List<OrderResponse> orderResponses = ordersErrorOr.Value
                .Select(order => new OrderResponse(
                    order.Id,
                    order.OrderTime,
                    order.Items,
                    order.Price
                ))
                .ToList();
            
            return Ok(orderResponses);
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
            // make sure id is not empty
            if (id == Guid.Empty)
            {
                return BadRequest(
                    new { error = "Id Must Not Be Empty" }
                );
            }

            // make sure orderRequest is not empty
            if (orderRequest == null)
            {
                return BadRequest(
                    new { error = "Order Must Not Be Empty" }
                );
            }
            
            // convert order request to order
            Order order = new Order(
                id,
                DateTime.Now,
                orderRequest.Items,
                orderRequest.Price
            );

            // update order
            ErrorOr<Order> orderErrorOr = _ordersService.UpdateOrder(id, order);

            // check for errors 
            if (orderErrorOr.IsError)
            {
                return NotFound(
                    new { error = orderErrorOr.Errors[0].Description }
                );
            }

            // convert to order Response
            OrderResponse orderResponse = new OrderResponse(
                orderErrorOr.Value.Id,
                orderErrorOr.Value.OrderTime,
                orderErrorOr.Value.Items,
                orderErrorOr.Value.Price
            );

            return Ok(orderResponse);
        }


        /// <summary>
        /// Deletes order with specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Succes / Failure code</returns>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteOrder(Guid id)
        {
            // make sure id is not empty
            if (id == Guid.Empty)
            {
                return BadRequest(
                    new { error = "Id Must Not Be Empty" }
                );
            }

            // delete order
            ErrorOr<IActionResult> orderErrorOr = _ordersService.DeleteOrder(id);

            // check for db or not found errors
            if (orderErrorOr.IsError)
            {
                if (orderErrorOr.FirstError.Code == "Order.DbError")
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new { error = orderErrorOr.Errors[0].Description }
                    );
                }
                else
                {
                    return NotFound(
                        new { error = orderErrorOr.Errors[0].Description }
                    );
                }
            }

            return orderErrorOr.Value;
        } 
    }
}
