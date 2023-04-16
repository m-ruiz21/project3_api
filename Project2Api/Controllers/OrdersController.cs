using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.Order;
using Project2Api.Models;
using Project2Api.Services.Orders;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ApiController 
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
        public async Task<IActionResult> CreateOrder(OrderRequest orderRequest)
        {
            ErrorOr<Order> order = Order.From(orderRequest);

            if (order.IsError)
            {
                return Problem(order.Errors);
            }

            ErrorOr<Order> orderErrorOr = await _ordersService.CreateOrderAsync(order.Value);

            return orderErrorOr.Match(
                value => Ok(MapOrderToOrderResponse(value)),
                errors => Problem(errors)
            );
        }
        
        /// <summary>
        /// Gets order with specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Requested object</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            ErrorOr<Order> orderErrorOr = await _ordersService.GetOrderAsync(id);

            return orderErrorOr.Match(
                value => Ok(MapOrderToOrderResponse(value)),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of orderResponses</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllOrders(int pageNumber = 1, int pageSize = 50)
        {
            // get all orders
            ErrorOr<List<Order>> ordersErrorOr = await _ordersService.GetAllOrdersAsync(pageNumber, pageSize);

            // get list of orders and map list to order Responses if successful, else return error 
            return ordersErrorOr.Match(
                value => Ok(value
                    .Select(order => MapOrderToOrderResponse(order))
                    .ToList()),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Updates order with specific ID
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="id"></param>
        /// <returns>Updated order object</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderRequest orderRequest, Guid id)
        { 
            // convert order request to order
            ErrorOr<Order> order = Order.From(orderRequest, id);

            if (order.IsError)
            {
                return Problem(order.Errors);
            }

            // update order
            ErrorOr<Order> orderErrorOr = await _ordersService.UpdateOrderAsync(id, order.Value);

            // return Ok(orderResponse) if succcessful, otherwise return error
            return orderErrorOr.Match(
                value => Ok(MapOrderToOrderResponse(value)),
                errors => Problem(errors)
            );
        }


        /// <summary>
        /// Deletes order with specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Succes / Failure code</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            // delete order
            ErrorOr<IActionResult> orderErrorOr = await _ordersService.DeleteOrderAsync(id);

            // return No Content if successful, otherwise return error 
            return orderErrorOr.Match(
                value => NoContent(),
                errors => Problem(errors)
            ); 
        }

        /// <summary>
        /// Maps order to order response
        /// </summary>
        /// <param name="order"></param>
        /// <returns>OrderResponse</returns>
        private static OrderResponse MapOrderToOrderResponse(Order order)
        {
            return new OrderResponse(
                order.Id,
                order.OrderTime,
                order.Items,
                order.Price
            );
        }  
    }
}
