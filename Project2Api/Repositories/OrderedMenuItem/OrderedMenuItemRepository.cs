using System.Data;
using Dapper;
using Project2Api.Models;

namespace Project2Api.Repositories;

public class OrderedMenuItemRepository : IOrderedMenuItemRepository
{
    private readonly IDbConnection _connection;

    public OrderedMenuItemRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<OrderedMenuItem?> CreateOrderedMenuItem(OrderedMenuItem orderedMenuItem)
    {
        using(UnitOfWork uow = new UnitOfWork(_connection)) 
        {
            try
            {
                string sql = "INSERT INTO ordered_menu_item (order_id, menu_item_name, quantity) VALUES (@order_id, @menu_item_name, @quantity)";
                var parameters = new { order_id = orderedMenuItem.OrderId, menu_item_name = orderedMenuItem.MenuItemName, quantity = orderedMenuItem.Quantity };

                int rowsAffected = await uow.Connection.ExecuteAsync(sql, parameters, uow.Transaction);

                uow.Commit();
                return orderedMenuItem;
            }
            catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }

    public Task<IEnumerable<OrderedMenuItem>> GetAllOrderedMenuItems()
    {
        throw new NotImplementedException();
    }

    public Task<OrderedMenuItem> GetOrderedMenuItemByOrderIdAndMenuItemName(Guid orderId, string menuItemName)
    {
        throw new NotImplementedException();
    }

    public Task<OrderedMenuItem> UpdateOrderedMenuItem(OrderedMenuItem orderedMenuItem)
    {
        throw new NotImplementedException();
    }
    
    public Task<OrderedMenuItem> DeleteOrderedMenuItem(OrderedMenuItem orderedMenuItem)
    {
        throw new NotImplementedException();
    }
}