using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project2Api.Models;

namespace Project2Api.Repositories;

public interface IOrderedMenuItemRepository
{
    Task<IEnumerable<OrderedMenuItem>> GetAllOrderedMenuItems();
    Task<OrderedMenuItem> GetOrderedMenuItemByOrderIdAndMenuItemName(Guid orderId, string menuItemName);
    Task<OrderedMenuItem?> CreateOrderedMenuItem(OrderedMenuItem orderedMenuItem);
    Task<OrderedMenuItem> UpdateOrderedMenuItem(OrderedMenuItem orderedMenuItem);
    Task<OrderedMenuItem> DeleteOrderedMenuItem(OrderedMenuItem orderedMenuItem);
}