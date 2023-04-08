using ErrorOr;
using Project2Api.ServiceErrors;
using Microsoft.AspNetCore.Mvc;
using Project2Api.DbTools;
using Project2Api.Models;
using System.Data;

namespace Project2Api.Services.MenuItems;
public class MenuItemService : IMenuItemService
{
    private readonly IDbClient _dbClient;

    public MenuItemService(IDbClient dbClient)
    {
        _dbClient = dbClient;
    }

    public ErrorOr<MenuItem> CreateMenuItem(MenuItem menuItem)
    {

        // create menu_item
        Task<int> menuItemTask = _dbClient.ExecuteNonQueryAsync(
            $"INSERT INTO menu_item (name, price, quantity) VALUES ('{menuItem.Name}', '{menuItem.Price}', '{menuItem.Quantity}')"
        );

        if(menuItemTask.Result == 0)
        {
            return ServiceErrors.Errors.Orders.DbError;
        }

        // add items to ordered_menu_items table
        foreach (string cutlery in menuItem.Cutlery)
        {
            Task<int> cutleryTask = _dbClient.ExecuteNonQueryAsync(
                $"INSERT INTO menu_item_cutlery (menu_item_name, cutlery_name) VALUES ('{menuItem.Name}', '{cutlery}')"
            );

            // check that itemTask was successful
            if (cutleryTask.Result == 0)
            {
                return ServiceErrors.Errors.Orders.DbError;
            }
        }

        return menuItem;
    }

    public ErrorOr<MenuItem> GetMenuItem(string name)
    {
        // check input
        if (name == null)
        {
            return ServiceErrors.Errors.MenuItem.InvalidMenuItem;
        }

        // get menu item from database
        Task<DataTable> menuItemTask = _dbClient.ExecuteQueryAsync(
            $"SELECT * FROM menu_item WHERE name = '{name}'"
        );
 
        // check that menuItemTask was successful
        DataTable menuItemTable = menuItemTask.Result; 
        if (menuItemTable.Rows.Count == 0)
        {
            return Errors.MenuItem.NotFound;
        }

        ErrorOr<MenuItem> menuItem = MenuItem.From(menuItemTable.Rows[0]);

        // check that order was successfully converted 
        if (menuItem.IsError)
        {
            return menuItem.Errors[0];
        }

        // populate order.items table by getting all menu items from ordered_menu_items table
        Task<DataTable> cutleryTask = _dbClient.ExecuteQueryAsync(
            $"SELECT cutlery_name FROM menu_item_cutlery WHERE menu_item_name = '{name}'"
        );

        // check that itemsTask was successful
        DataTable cutleryTable = cutleryTask.Result;
        if (cutleryTable.Rows.Count == 0)
        {
            return Errors.Orders.UnexpectedError;
        }

        // insert items into order.items
        foreach (DataRow row in cutleryTable.Rows)
        {
            // make sure menu_item exists
            if (row["cutlery_name"] == null)
            {
                return Errors.Orders.UnexpectedError;
            }

            string cutlery = row["cutlery_name"].ToString() ?? "";
            menuItem.Value.Cutlery.Add(cutlery);
        }

        return menuItem.Value;   
    }

    public ErrorOr<List<MenuItem>> GetAllMenuItems(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }


    public ErrorOr<MenuItem> UpdateMenuItem(string name, MenuItem menuItem)
    {
        throw new NotImplementedException();
    } 
    public ErrorOr<IActionResult> DeleteMenuItem(string name)
    {
        throw new NotImplementedException();
    }
}