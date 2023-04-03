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

    public Order? ConvertDataRowToOrder(DataRow dataRow)
    {
        // check if dataRow is null 
        if (dataRow == null)
        {
            // print error message
            Console.WriteLine("[OrdersService] Failed to convert data table: data row is null");
            return null;
        }

        // check if any of the columns are null 
        if (dataRow["id"] == null || dataRow["date_time"] == null || dataRow["total_price"] == null)
        {
            // print error message
            Console.WriteLine("[OrdersService] Failed to convert data table: some fields are null");
            return null;
        }

        // convert rest of row to order
        String RawId = dataRow["id"].ToString() ?? "";
        String? rawOrderTime = dataRow["date_time"].ToString() ?? "";
        String? rawPrice = dataRow["total_price"].ToString() ?? "";

        // create order
        try
        {
            Order order = new Order(Guid.Parse(RawId),
                                    DateTime.Parse(rawOrderTime),
                                    new List<string>(),
                                    float.Parse(rawPrice)
                                );
            return order; 
        } catch (System.FormatException e) {
            Console.Out.WriteLine(e);
            return null;
        }
    }

    public ErrorOr<Order> CreateOrder(Order order)
    {
        // make sure order is valid and no fields are emtpy
        if (order.Id == Guid.Empty || order.OrderTime == DateTime.MinValue || order.Items.Count == 0 || order.Price == 0.0f)
        {
            return Errors.Orders.InvalidOrder;
        }

        // create order
        Task<int> orderTask = _dbClient.ExecuteNonQueryAsync(
            $"INSERT INTO orders (id, date_time, total_price) VALUES ('{order.Id}', '{order.OrderTime}', '{order.Price}')"
        );

        // check that orderTask was successful
        if (orderTask.Result == 0)
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
            if (itemTask.Result == 0 || reduceMenuItemTask.Result == 0)
            {
                return Errors.Orders.UnexpectedError;
            }
        }

        return order;
    }

    public ErrorOr<Order> GetOrder(Guid id)
    {
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

        Order? order = ConvertDataRowToOrder(orderTable.Rows[0]);

        // check that order was successfully converted 
        if (order == null)
        {
            return Errors.Orders.UnexpectedError;
        }

        // populate order.items table by getting all menu items from ordered_menu_items table
        Task<DataTable> itemsTask = _dbClient.ExecuteQueryAsync(
            $"SELECT menu_item_name FROM ordered_menu_item WHERE order_id = '{id}';"
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
            order.Items.Add(menuItem);
        }

        return order;   
    }

    public ErrorOr<List<Order>> GetAllOrders(int pageNumber, int pageSize)
    {
        // get all orders from database
        Task<DataTable> ordersTask = _dbClient.ExecuteQueryAsync(
            $"SELECT * FROM orders ORDER BY date_time DESC LIMIT {pageNumber} OFFSET {pageSize}"
        );

        // check that ordersTask was successful
        DataTable ordersTable = ordersTask.Result;
        if (ordersTable.Rows.Count == 0)
        {
            return Errors.Orders.UnexpectedError;
        }

        // convert ordersTable to list of orders
        List<Order> orders = new List<Order>();

        // convert each row to order
        foreach (DataRow row in ordersTable.Rows)
        {
            Order? order = ConvertDataRowToOrder(row);

            if (order == null)
            {
                return Errors.Orders.DbError;
            }

            orders.Add(order);
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
            return Errors.Orders.InvalidOrder;
        }
        
        // add new ordered menu items for this order
        foreach (string item in order.Items)
        {
            Task<int> itemTask = _dbClient.ExecuteNonQueryAsync(
                $"INSERT INTO ordered_menu_item (order_id, menu_item_name) VALUES ('{order.Id}', '{item}')"
            );

            // make sure itemTask was successful
            if (itemTask.Result == 0)
            {
                return Errors.Orders.UnexpectedError;
            }
        }

        return order;
    }

    public ErrorOr<IActionResult> DeleteOrder(Guid id)
    {
        // delete order
        Task<int> deleteTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM orders WHERE id = '{id}'"
        );

        // check that orderTask was successful
        if (deleteTask.Result == 0)
        {
            return Errors.Orders.NotFound;
        }

        return new NoContentResult();
    }
}