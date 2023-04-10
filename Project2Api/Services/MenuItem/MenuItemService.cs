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
            $"INSERT INTO menu_item (name, price, quantity, category) VALUES ('{menuItem.Name}', '{menuItem.Price}', '{menuItem.Quantity}', '{menuItem.Category}')"
        );

        if(menuItemTask.Result <= 0)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }

        // add items to ordered_menu_items table
        foreach (string cutlery in menuItem.Cutlery)
        {
            Task<int> cutleryTask = _dbClient.ExecuteNonQueryAsync(
                $"INSERT INTO menu_item_cutlery (menu_item_name, cutlery_name) VALUES ('{menuItem.Name}', '{cutlery}')"
            );

            // check that itemTask was successful
            if (cutleryTask.Result <= 0)
            {
                return ServiceErrors.Errors.MenuItem.DbError;
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
        if (menuItemTask.IsFaulted)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }

        // check if menu item was found
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
        if (cutleryTable.Rows.Count <= 0)
        {
            return Errors.MenuItem.DbError;
        }

        // insert items into order.items
        foreach (DataRow row in cutleryTable.Rows)
        {
            // make sure menu_item exists
            if (row["cutlery_name"] == null)
            {
                return Errors.MenuItem.UnexpectedError;
            }

            string cutlery = row["cutlery_name"].ToString() ?? "";
            menuItem.Value.Cutlery.Add(cutlery);
        }

        return menuItem.Value;   
    }

    public ErrorOr<Dictionary<string, List<MenuItem>>> GetAllMenuItems()
    {        
        Dictionary<string, List<MenuItem>> menuItems = new Dictionary<string, List<MenuItem>>();
        foreach (string category in new string[] { "base", "protein", "topping", "dressing", "drink", "extra", "side" })
        {
            // get menu items from database
            Task<DataTable> menuItemTask = _dbClient.ExecuteQueryAsync(
                $"SELECT * FROM menu_item WHERE category = '{category}'"
            );

            // make sure query was successful
            if (menuItemTask.IsFaulted)
            {
                return ServiceErrors.Errors.MenuItem.DbError;
            }

            DataTable menuItemTable = menuItemTask.Result; 

            List<MenuItem> menuItemList = new List<MenuItem>();
            // insert items into menuItems
            foreach (DataRow row in menuItemTable.Rows)
            {
                ErrorOr<MenuItem> menuItem = MenuItem.From(row);

                if (menuItem.IsError)
                {
                    return menuItem.FirstError;
                }

                menuItemList.Add(menuItem.Value);
            }

            menuItems.Add(category, menuItemList);
        } 

        return menuItems; 
    }

    public ErrorOr<MenuItem> UpdateMenuItem(string name, MenuItem menuItem)
    {
        // update menu item
        Task<int> menuItemTask = _dbClient.ExecuteNonQueryAsync(
            $"UPDATE menu_item SET price = '{menuItem.Price}', quantity = '{menuItem.Quantity}' WHERE name = '{name}'"
        );

        // check that menuItemTask was successful
        if (menuItemTask.Result == -1)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }
        else if (menuItemTask.Result == 0)
        {
            return ServiceErrors.Errors.MenuItem.NotFound;
        }

        // delete all cutlery from menu_item_cutlery table
        Task<int> cutleryTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM menu_item_cutlery WHERE menu_item_name = '{name}'"
        );

        // check that cutleryTask was successful
        if (cutleryTask.Result == -1)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }

        // add items to ordered_menu_items table
        foreach (string cutlery in menuItem.Cutlery)
        {
            Task<int> cutleryTask2 = _dbClient.ExecuteNonQueryAsync(
                $"INSERT INTO menu_item_cutlery (menu_item_name, cutlery_name) VALUES ('{name}', '{cutlery}')"
            );

            // check that itemTask was successful
            if (cutleryTask2.Result == -1)
            {
                return ServiceErrors.Errors.MenuItem.DbError;
            }
            else if (cutleryTask2.Result == 0)
            {
                return ServiceErrors.Errors.MenuItem.NotFound;
            }
        }

        return menuItem;
    }

    public ErrorOr<IActionResult> DeleteMenuItem(string name)
    {
        // delete cutlery from menu_item_cutlery table
        Task<int> cutleryTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM menu_item_cutlery WHERE menu_item_name = '{name}'"
        );

        // check that cutleryTask was successful
        if (cutleryTask.Result == -1)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }
        else if (cutleryTask.Result == 0)
        {
            return ServiceErrors.Errors.MenuItem.NotFound;
        }

        // delete menu item from menu_item table
        Task<int> menuItemTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM menu_item WHERE name = '{name}'"
        );

        // check that menuItemTask was successful
        if (menuItemTask.Result == -1)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }
        else if (menuItemTask.Result == 0)
        {
            return ServiceErrors.Errors.MenuItem.NotFound;
        }

        return new NoContentResult();
    }
}