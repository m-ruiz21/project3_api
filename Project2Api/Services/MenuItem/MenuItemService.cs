using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.DbTools;
using Project2Api.Models;

namespace Project2Api.Services.Orders;
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
            $"INSERT INTO menu_Item (name, price, quantity) VALUES ('{menuItem.Name}', '{menuItem.Price}', '{menuItem.Quantity}')"
        );

        if(menuItemTask.Result == 0)
        {
            return ServiceErrors.Errors.Orders.DbError;
        }

        // add items to ordered_menu_items table
        foreach (string item in menuItem.Cutlery)
        {
            Task<int> itemTask = _dbClient.ExecuteNonQueryAsync(
                $"INSERT INTO menu_item (menu_item_name, cutlery_name) VALUES ('{menuItem.Name}', '{item}')"
            );


            // check that itemTask was successful
            if (menuItemTask.Result == 0)
            {
                return ServiceErrors.Errors.Orders.UnexpectedError;
            }
        }

        return menuItem;
    }

    public ErrorOr<IActionResult> DeleteMenuItem(Guid id)
    {
        throw new NotImplementedException();
    }

    public ErrorOr<List<MenuItem>> GetAllMenuItems(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public ErrorOr<MenuItem> GetMenuItem(Guid id)
    {
        throw new NotImplementedException();
    }

    public ErrorOr<MenuItem> UpdateMenuItem(Guid id, Order order)
    {
        throw new NotImplementedException();
    }
}