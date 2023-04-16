using System.Data;
using Dapper;
using Project2Api.Models;

namespace Project2Api.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly IDbConnection _connection;

    public OrdersRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public Task<int?> CreateOrderAsync(Order order)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "INSERT INTO orders (id, date_time, total_price) VALUES (@id, @date_time, @total_price)"; 
                var parameters = new { id = order.Id, date_time = order.OrderTime, total_price = order.Price };

                int rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction);

                foreach (string item in order.Items)
                {
                    sql = "INSERT INTO ordered_menu_item (order_id, menu_item_name, quantity) VALUES (@order_id, @menu_item_name, 1)";
                    var orderItemsParameters = new { order_id = order.Id, menu_item_name = item};

                    uow.Connection.Execute(sql, orderItemsParameters, uow.Transaction);

                    sql = "UPDATE menu_item SET quantity = quantity - 1 WHERE name = @menu_item_name";
                    var updateMenuItemParameters = new { menu_item_name = item };

                    uow.Connection.Execute(sql, updateMenuItemParameters, uow.Transaction);

                    sql = @"
                    UPDATE cutlery 
                    SET quantity = cutlery.quantity - mc.quantity 
                    FROM menu_item mi 
                    JOIN menu_item_cutlery mc 
                    ON mi.name = mc.menu_item_name 
                    WHERE cutlery.name = mc.cutlery_name AND mi.name = @menu_item_name";

                    uow.Connection.Execute(sql, updateMenuItemParameters, uow.Transaction);
                }
                
                uow.Commit();
                return Task.FromResult<int?>(rowsAffected);
            } catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message); 
                return Task.FromResult<int?>(null);
            }
        }
    }

    public Task<Order?> GetOrderByIdAsync(Guid id)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM orders WHERE id = @id";
                var parameters = new { id = id };

                Order order = uow.Connection.QueryFirstOrDefault<Order>(sql, parameters, uow.Transaction);

                sql = "SELECT menu_item_name FROM ordered_menu_item WHERE order_id = @order_id";
                var orderItemsParameters = new { order_id = order.Id };

                order.Items = uow.Connection.Query<string>(sql, orderItemsParameters, uow.Transaction).ToList();

                uow.Commit();
                return Task.FromResult<Order?>(order);
            } catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message); 
                return Task.FromResult<Order?>(null);
            }
        }
    }

    public Task<IEnumerable<Order>?> GetOrdersAsync()
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM orders ORDER BY date_time DESC;";

                IEnumerable<Order> orders = uow.Connection.Query<Order>(sql, uow.Transaction);

                uow.Commit();
                return Task.FromResult<IEnumerable<Order>?>(orders);
            } catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message); 
                return Task.FromResult<IEnumerable<Order>?>(null);
            }
        }
    }

    public Task<int?> UpdateOrderAsync(Order order)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "UPDATE orders SET total_price = @total_price WHERE id = @id";
                var parameters = new { date_time = order.OrderTime, total_price = order.Price, id = order.Id };
                int rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction);

                sql = "DELETE FROM ordered_menu_items WHERE order_id = @order_id";
                var deleteOrderItemsParameters = new { order_id = order.Id };
                uow.Connection.Execute(sql, deleteOrderItemsParameters, uow.Transaction);

                foreach (string item in order.Items)
                {
                    sql = "INSERT INTO ordered_menu_item (order_id, menu_item_name, quantity) VALUES (@order_id, @menu_item_name, 1)";
                    var addOrderItemsParameters = new { order_id = order.Id, menu_item_name = item};

                    uow.Connection.Execute(sql, addOrderItemsParameters, uow.Transaction);
                }

                uow.Commit();
                return Task.FromResult<int?>(rowsAffected);
            } catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message); 
                return Task.FromResult<int?>(null);
            }
        }
    }
    
    public Task<bool> DeleteOrderAsync(Guid id)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "DELETE FROM ordered_menu_item WHERE order_id = @id";
                var parameters = new { id = id };
                int rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction);

                sql = "DELETE FROM orders WHERE id = @id";
                rowsAffected += uow.Connection.Execute(sql, parameters, uow.Transaction);

                uow.Commit();
                return Task.FromResult<bool>(rowsAffected > 0);
            } catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message); 
                return Task.FromResult<bool>(false);
            }
        }
    }
}