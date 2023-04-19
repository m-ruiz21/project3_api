using Project2Api.Models;

namespace Project2Api.Repositories;

public interface IOrdersRepository
{
    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="order">The order to create.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int?> CreateOrderAsync(Order order);

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>A list of orders.</returns>
    Task<IEnumerable<Order>?> GetOrdersAsync();

    /// <summary>
    /// Gets all orders in a date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <returns> Orders in given date range as IEnumerable </returns>
    Task<IEnumerable<Order>?> GetOrdersInDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Gets an order by id.
    /// </summary>
    /// <param name="id">The id of the order.</param>
    /// <returns>An order.</returns>
    Task<Order?> GetOrderByIdAsync(Guid id);

    /// <summary>
    /// Updates an order.
    /// </summary>
    /// <param name="order">The order to update.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int?> UpdateOrderAsync(Order order);

    /// <summary>
    /// Deletes an order.
    /// </summary>
    /// <param name="id">The id of the order to delete.</param>
    /// <returns>The number of rows affected.</returns>
    Task<bool> DeleteOrderAsync(Guid id);
}