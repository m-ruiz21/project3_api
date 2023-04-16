using ErrorOr;
using Project2Api.ServiceErrors;
using Microsoft.AspNetCore.Mvc;
using Project2Api.DbTools;
using Project2Api.Models;
using System.Data;
using Project2Api.Repositories;

namespace Project2Api.Services.MenuItems;
public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuItemService(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<ErrorOr<MenuItem>> CreateMenuItemAsync(MenuItem menuItem)
    {
        int? rowsAffected = await _menuItemRepository.CreateMenuItemAsync(menuItem);

        if(rowsAffected == null || rowsAffected == 0)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }

        return menuItem;
    }

    public async Task<ErrorOr<MenuItem>> GetMenuItemAsync(string name)
    {
        if (name == null)
        {
            return ServiceErrors.Errors.MenuItem.InvalidMenuItem;
        }

        MenuItem? menuItem = await _menuItemRepository.GetMenuItemByNameAsync(name);

        if (menuItem == null)
        {
            return ServiceErrors.Errors.MenuItem.NotFound;
        } 

        return menuItem; 
    }

    public async Task<ErrorOr<Dictionary<string, List<MenuItem>>>> GetAllMenuItemsAsync()
    {        
        IEnumerable<MenuItem>? menuItems = await _menuItemRepository.GetMenuItemsAsync();
        if (menuItems == null)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }

        Dictionary<string, List<MenuItem>> menuByCategory = menuItems
            .GroupBy(m => m.Category)
            .ToDictionary(g => g.Key, g => g.ToList());

        return menuByCategory;
    }

    public async Task<ErrorOr<MenuItem>> UpdateMenuItemAsync(string name, MenuItem menuItem)
    {
        if (name == null || menuItem == null)
        {
            return ServiceErrors.Errors.MenuItem.InvalidMenuItem;
        }

        int? rowsAffected = await _menuItemRepository.UpdateMenuItemAsync(menuItem);
        if (rowsAffected == null)
        {
            return ServiceErrors.Errors.MenuItem.DbError;
        }
        else if (rowsAffected == 0)
        {
            return ServiceErrors.Errors.MenuItem.NotFound;
        }

        return menuItem;
    }

    public async Task<ErrorOr<IActionResult>> DeleteMenuItemAsync(string name)
    {
        if (name == null)
        {
            return ServiceErrors.Errors.MenuItem.InvalidMenuItem;
        }

        bool success = await _menuItemRepository.DeleteMenuItemAsync(name);
        if (!success)
        {
            return ServiceErrors.Errors.MenuItem.NotFound;
        }

        return new NoContentResult();
    }
}