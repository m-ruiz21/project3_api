using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;

namespace Project2Api.Services.Orders
{
    public interface IOrdersService
    {
        /// <summary>
        /// Creates order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Added Object or Error</returns>
        ErrorOr<Order> CreateOrder(Order order);
    
        /// <summary>
        /// Gets order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Order or Error</returns>
        ErrorOr<Order> GetOrder(Guid id);

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of Orders or Error</returns>
        ErrorOr<List<Order>> GetAllOrders(int pageNumber, int pageSize);

        /// <summary>
        /// Updates order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Updated Object or Error</returns>
        ErrorOr<Order> UpdateOrder(Guid id, Order order);

        /// <summary>
        /// Deletes order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success Action Result or Error</returns>
        ErrorOr<IActionResult> DeleteOrder(Guid id);
    }
}