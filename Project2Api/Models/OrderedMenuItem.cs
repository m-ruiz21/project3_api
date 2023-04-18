using ErrorOr;
using Project2Api.ServiceErrors;

namespace Project2Api.Models;

public class OrderedMenuItem
{
    public Guid OrderId { get; set; }
    public string MenuItemName { get; set; }
    public int Quantity { get; set; }

    public OrderedMenuItem(
        Guid order_id, 
        String menu_item_name, 
        int quantity)
    {
        OrderId = order_id;
        MenuItemName = menu_item_name;
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