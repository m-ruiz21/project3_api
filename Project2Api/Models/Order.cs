using System.Data;
using ErrorOr;
using Project2Api.Contracts.Order;
using Project2Api.ServiceErrors;

namespace Project2Api.Models;

public class Order
{
    public Guid Id { get; }
    public DateTime OrderTime { get; }
    public List<string> Items { get; }
    public float Price { get; }

    private Order(
        Guid id, 
        DateTime orderTime, 
        List<string> items, 
        float price)
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
        float price,
        Guid? id = null
    )
    {
        if (price == 0.0f || orderTime == DateTime.MinValue)
        {
            return Errors.Orders.InvalidOrder("Price or order time is invalid");
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
        return Create(
            DateTime.Now,
            order.Items,
            order.Price
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
            order.Price,
            id
        );
    }

    /// <summary>
    /// Creates order from data row
    /// </summary>
    /// <param name="dataRow"></param>
    /// <returns>Created order</returns>
    public static ErrorOr<Order> From(DataRow dataRow)
    {
        // check if dataRow is null 
        if (dataRow == null)
        {
            // print error message
            Console.WriteLine("[OrdersService] Failed to convert data table: data row is null");
            return Errors.Orders.DbError;
        }

        // check if any of the columns are null 
        if (dataRow["id"] == null || dataRow["date_time"] == null || dataRow["total_price"] == null)
        {
            // print error message
            Console.WriteLine("[OrdersService] Failed to convert data table: some fields are null");
            return Errors.Orders.InvalidOrder("Some fields are null");
        }

        // convert rest of row to order
        String RawId = dataRow["id"].ToString() ?? "";
        String? rawOrderTime = dataRow["date_time"].ToString() ?? "";
        String? rawPrice = dataRow["total_price"].ToString() ?? "";

        // create order
        try
        {
            return Create(
                DateTime.Parse(rawOrderTime),
                new List<string>(),
                float.Parse(rawPrice),
                Guid.Parse(RawId)
            ); 
        } catch (System.FormatException e) {
            Console.Out.WriteLine(e);
            return Errors.Orders.UnexpectedError;
        }
    }
}