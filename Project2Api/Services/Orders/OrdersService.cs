using Project2Api.Models;
using Project2Api.DbTools;
using ErrorOr;
using Project2Api.ServiceErrors;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Project2Api.Services.Orders;

public class OrdersService : IOrdersService
{
    private readonly IDbClient _dbClient;
    
    public OrdersService(IDbClient dbClient)
    {
        _dbClient = dbClient;
    }
    
    public ErrorOr<Order> CreateOrder(Order order)
    {
        // create order
        Task<int> orderTask = _dbClient.ExecuteNonQueryAsync(
            $"INSERT INTO orders (id, date_time, total_price) VALUES ('{order.Id}', '{order.OrderTime}', '{order.Price}')"
        );

        // check that orderTask was successful
        if (orderTask.Result <= 0)
        {
            return Errors.Orders.DbError;
        }

        // add items to ordered_menu_items table
        foreach (string item in order.Items)
        {
            Task<int> itemTask = _dbClient.ExecuteNonQueryAsync(
                $"INSERT INTO ordered_menu_item (order_id, menu_item_name) VALUES ('{order.Id}', '{item}')"
            );

            // reduce stock of menu item
            Task<int> reduceMenuItemTask = _dbClient.ExecuteNonQueryAsync(
                $"UPDATE menu_item SET quantity = quantity - 1 WHERE name = '{item}'"
            );

            // reduce stock of cutlery used by menu item 
            Task<int> reduceCutleryTask = _dbClient.ExecuteNonQueryAsync(
                $"UPDATE cutlery SET quantity = cutlery.quantity - mc.quantity FROM menu_item mi JOIN menu_item_cutlery mc ON mi.name = mc.menu_item_name WHERE cutlery.name = mc.cutlery_name AND mi.name = '{item}';"
            );

            // check that itemTask and reduceStockTask was successful
            if (itemTask.Result <= 0 || reduceMenuItemTask.Result <= 0)
            {
                return Errors.Orders.DbError;
            }
        }

        return order;
    }

    public ErrorOr<Order> GetOrder(Guid id)
    {
        // check input
        if (id == Guid.Empty)
        {
            return Errors.Orders.InvalidOrder;
        }

        // get order from database
        Task<DataTable> orderTask = _dbClient.ExecuteQueryAsync(
            $"SELECT * FROM orders WHERE id = '{id}'"
        );
 
        // check that orderTask was successful
        DataTable orderTable = orderTask.Result; 
        if (orderTable.Rows.Count == 0)
        {
            return Errors.Orders.NotFound;
        }

        ErrorOr<Order> order= Order.From(orderTable.Rows[0]);

        // check that order was successfully converted 
        if (order.IsError)
        {
            return order.Errors[0];
        }

        // populate order.items table by getting all menu items from ordered_menu_items table
        Task<DataTable> itemsTask = _dbClient.ExecuteQueryAsync(
            $"SELECT menu_item_name FROM ordered_menu_item WHERE order_id = '{id}'"
        );

        // check that itemsTask was successful
        DataTable itemsTable = itemsTask.Result;
        if (itemsTable.Rows.Count == 0)
        {
            return Errors.Orders.UnexpectedError;
        }

        // insert items into order.items
        foreach (DataRow row in itemsTable.Rows)
        {
            // make sure menu_item exists
            if (row["menu_item_name"] == null)
            {
                return Errors.Orders.UnexpectedError;
            }

            string menuItem = row["menu_item_name"].ToString() ?? "";
            order.Value.Items.Add(menuItem);
        }

        return order.Value;   
    }

    public ErrorOr<List<Order>> GetAllOrders(int pageNumber, int pageSize)
    {
        // get all orders from database
        Task<DataTable> ordersTask = _dbClient.ExecuteQueryAsync(
            $"SELECT * FROM orders ORDER BY date_time DESC LIMIT {pageSize} OFFSET {pageSize * (pageNumber - 1)}"
        );

        // check that ordersTask was successful
        if (ordersTask.IsFaulted)
        {
            return Errors.Orders.DbError;
        }

        DataTable ordersTable = ordersTask.Result;

        // convert ordersTable to list of orders
        List<Order> orders = new List<Order>();

        // convert each row to order
        foreach (DataRow row in ordersTable.Rows)
        {
            ErrorOr<Order> order = Order.From(row); 

            if (order.IsError)
            {
                return order.Errors[0];
            }

            orders.Add(order.Value);
        }

        return orders;
    }

    public ErrorOr<Order> UpdateOrder(Guid id, Order order)
    {
        // update order
        Task<int> updateTask = _dbClient.ExecuteNonQueryAsync(
            $"UPDATE orders SET date_time = '{order.OrderTime}', total_price = '{order.Price}' WHERE id = '{id}'"
        );

        // delete all ordered menu items for this order
        Task<int> deleteTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM ordered_menu_item WHERE order_id = '{id}'"
        );

        // make sure updateTask and deleteTask were successful
        if (updateTask.Result == 0 || deleteTask.Result == 0)
        {
            return Errors.Orders.NotFound;
        }
        else if (updateTask.Result == -1 || deleteTask.Result == -1)
        {
            return Errors.Orders.DbError;
        }
        
        // add new ordered menu items for this order
        foreach (string item in order.Items)
        {
            Task<int> itemTask = _dbClient.ExecuteNonQueryAsync(
                $"INSERT INTO ordered_menu_item (order_id, menu_item_name) VALUES ('{order.Id}', '{item}')"
            );

            // make sure itemTask was successful
            if (itemTask.Result <= 0)
            {
                return Errors.Orders.DbError;
            }
        }

        return order;
    }

    public ErrorOr<IActionResult> DeleteOrder(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Errors.Orders.InvalidOrder;
        }

        // delete all orders in ordered_menu_item table with this order id
        Task<int> deleteItemsTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM ordered_menu_item WHERE order_id = '{id}'"
        );

        // make sure deleteItemsTask was successful
        if (deleteItemsTask.Result == -1)
        {
            return Errors.Orders.DbError;
        } 
        else if (deleteItemsTask.Result == 0)
        {
            return Errors.Orders.NotFound;
        }

        // delete order
        Task<int> deleteTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM orders WHERE id = '{id}'"
        );

        // check that orderTask was successful
        if (deleteTask.Result == 0)
        {
            return Errors.Orders.NotFound;
        }

        if (deleteTask.Result == -1)
        {
            return Errors.Orders.DbError;
        }

        return new NoContentResult();
    }
}