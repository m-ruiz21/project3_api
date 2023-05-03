using System.Data;
using ErrorOr;
using Project2Api.Contracts.Order;
using Project2Api.ServiceErrors;

namespace Project2Api.Models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderTime { get; set; }
    public List<string> Items { get; set; }
    public decimal Price { get; set; }    

    public Order(
        Guid id, 
        DateTime date_time, 
        decimal total_price)
    {
        Id = id;
        OrderTime = date_time;
        Price = total_price;
        Items = new List<string>();
    }


    private Order(
        Guid id, 
        DateTime orderTime, 
        List<string> items, 
        decimal price)
    {
        Id = id;
        OrderTime = orderTime;
        Items = items;
        Price = price;
    }
    
    /// <summary>
    /// Creates order
    /// </summary>
    /// <param name="orderTime"></param>
    /// <param name="items"></param>
    /// <param name="price"></param>
    /// <param name="id"></param>
    /// <returns>Created order</returns>
    public static ErrorOr<Order> Create(
        DateTime orderTime,
        List<string> items,
        decimal price,
        Guid? id = null
    )
    {
        if (price < 0.0M || orderTime == DateTime.MinValue || items.Count == 0)
        {
            return Errors.Orders.InvalidOrder;
        }

        return new Order(
            id ?? Guid.NewGuid(), 
            orderTime, 
            items, 
            price
        );
    }

    /// <summary>
    /// Creates order from request
    /// </summary>
    /// <param name="order"></param>
    /// <returns>Created order</returns>
    public static ErrorOr<Order> From(OrderRequest order)
    {
        TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        DateTime orderDateTimeCST = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
        
        return Create(
            orderDateTimeCST,
            order.Items,
            0.0M 
        );
    }

    /// <summary>
    /// Creates order from request
    /// </summary>
    /// <param name="order"></param>
    /// <param name="id"></param>
    /// <returns>Created order</returns>
    public static ErrorOr<Order> From(UpdateOrderRequest order, Guid id)
    {
        return Create(
            order.dateTime, 
            order.Items, 
            0.0M,
            id
        );
    }
}