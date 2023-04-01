using Project2Api.Contracts.Order;

namespace Project2Api.Models;

public class Order
{
    public Guid Id { get; }
    public DateTime OrderTime { get; }
    public List<string> Items { get; }
    public float Price { get; }

    public Order(Guid id, DateTime orderTime, List<string> items, float price)
    {
        Id = id;
        OrderTime = orderTime;
        Items = items;
        Price = price;
    }
}