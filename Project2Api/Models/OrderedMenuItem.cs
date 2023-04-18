using ErrorOr;
using Project2Api.ServiceErrors;

namespace Project2Api.Models;

public class OrderedMenuItem
{
    public Guid OrderId { get; set; }
    public string MenuItemName { get; set; }
    public int Quantity { get; set; }

    public OrderedMenuItem(
        Guid orderId, 
        string menuItemName, 
        int quantity)
    {
        OrderId = orderId;
        MenuItemName = menuItemName;
        Quantity = quantity;
    }

    public ErrorOr<OrderedMenuItem> Create(
        Guid orderId,
        string menuItemName,
        int quantity
    )
    {
        if (quantity == 0 || string.IsNullOrEmpty(menuItemName) || orderId == Guid.Empty)
        {
            return Errors.OrderedMenuItem.InvalidMenuItem;
        }

        return new OrderedMenuItem(
            orderId,
            menuItemName,
            quantity
        );
    }
}