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

    public Task<IEnumerable<OrderedMenuItem>?> GetOrderedMenuItemsAsync()
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM ordered_menu_item";

                IEnumerable<OrderedMenuItem> orderedMenuItems = uow.Connection.Query<OrderedMenuItem>(sql, uow.Transaction);

                return Task.FromResult<IEnumerable<OrderedMenuItem>?>(orderedMenuItems);
            }
            catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<IEnumerable<OrderedMenuItem>?>(null);
            }
        }
    }

    public Task<OrderedMenuItem?> GetOrderedMenuItemByOrderIdAndMenuItemName(Guid orderId, string menuItemName)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM ordered_menu_item WHERE order_id = @OrderId AND menu_item_name = @MenuItemName";
                var parameters = new { OrderId = orderId, MenuItemName = menuItemName };

                OrderedMenuItem orderedMenuItem = uow.Connection.QueryFirstOrDefault<OrderedMenuItem>(sql, parameters, uow.Transaction);

                return Task.FromResult<OrderedMenuItem?>(orderedMenuItem);
            }
            catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<OrderedMenuItem?>(null);
            }
        }
    }

    public Task<OrderedMenuItem?> UpdateOrderedMenuItem(OrderedMenuItem orderedMenuItem)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "UPDATE ordered_menu_item SET quantity = @Quantity WHERE order_id = @OrderId AND menu_item_name = @MenuItemName";
                var parameters = new { Quantity = orderedMenuItem.Quantity, OrderId = orderedMenuItem.OrderId, MenuItemName = orderedMenuItem.MenuItemName };

                int rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction);

                uow.Commit();
                return Task.FromResult<OrderedMenuItem?>(orderedMenuItem);
            }
            catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<OrderedMenuItem?>(null);
            }
        }
    }
    
    public Task<OrderedMenuItem?> DeleteOrderedMenuItem(OrderedMenuItem orderedMenuItem)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "DELETE FROM ordered_menu_item WHERE order_id = @OrderId AND menu_item_name = @MenuItemName";
                var parameters = new { OrderId = orderedMenuItem.OrderId, MenuItemName = orderedMenuItem.MenuItemName };

                int rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction);

                uow.Commit();
                return Task.FromResult<OrderedMenuItem?>(orderedMenuItem);
            }
            catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<OrderedMenuItem?>(null);
            }
        }
    }
}