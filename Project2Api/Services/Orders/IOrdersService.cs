using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;
using System.Data;

namespace Project2Api.Services.Orders
{
    public interface IOrdersService
    {

        /// <summary>
        /// Converts DataTable to Order
        /// </summary>
        /// <param name="orderTable"></param>
        /// <returns>Order</returns>
        Order? ConvertDataTableToOrder(DataTable orderTable);

        /// <summary>
        /// Creates order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Added Object</returns>
        ErrorOr<Order> CreateOrder(Order order);
    
        /// <summary>
        /// Gets order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Order</returns>
        ErrorOr<Order> GetOrder(Guid id);

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of Orders</returns>
        ErrorOr<List<Order>> GetAllOrders();

        /// <summary>
        /// Updates order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Updated Object</returns>
        ErrorOr<Order> UpdateOrder(Guid id, Order order);

        /// <summary>
        /// Deletes order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success Action Result or Error</returns>
        ErrorOr<IActionResult> DeleteOrder(Guid id);
    }
}