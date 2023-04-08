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