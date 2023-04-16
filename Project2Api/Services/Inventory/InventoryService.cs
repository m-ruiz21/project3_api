using ErrorOr;
using Project2Api.ServiceErrors;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;
using Project2Api.Repositories;

namespace Project2Api.Services.Inventory;
public class InventoryService: IInventoryService
{
    private readonly ICutleryRepository _cutleryRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public InventoryService(ICutleryRepository  cutleryRepository, IMenuItemRepository menuItemRepository)
    {
        _cutleryRepository = cutleryRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<ErrorOr<IActionResult>> DeleteInventoryItem(InventoryItem inventoryItem)
    {
        bool deleted;
        if (inventoryItem.Type == "cutlery")
        {
            deleted = await _cutleryRepository.DeleteCutleryAsync(inventoryItem.Name);
        }
        else if (inventoryItem.Type == "menu item")
        {
            deleted = await _menuItemRepository.DeleteMenuItemAsync(inventoryItem.Name);
        }
        else
        {
            return ServiceErrors.Errors.Inventory.InvalidType;
        }

        if (!deleted)
        {
            return ServiceErrors.Errors.Inventory.DbError;
        }

        return new NoContentResult();
    }

    public async Task<ErrorOr<List<InventoryItem>>> GetAllInventoryItems()
    {
        IEnumerable<Cutlery>? allCutlery = await _cutleryRepository.GetCutleryAsync();
        IEnumerable<MenuItem>? allMenuItems = await _menuItemRepository.GetMenuItemsAsync();
        if (allCutlery == null || allMenuItems == null)
        {
            return ServiceErrors.Errors.Inventory.DbError;
        }
 
        List<InventoryItem> inventoryItems = new List<InventoryItem>();
        
        foreach (Cutlery cutlery in allCutlery)
        {
            ErrorOr<InventoryItem> result = InventoryItem.Create(cutlery.Name, "cutlery", cutlery.Quantity);
            if (result.IsError)
            {
                return Errors.Inventory.DbError;
            }
            inventoryItems.Add(result.Value);
        }

        foreach (MenuItem menuItem in allMenuItems)
        {
            var result = InventoryItem.Create(menuItem.Name, "menu item", menuItem.Quantity);
            if (result.IsError)
            {
                return Errors.Inventory.DbError;
            }
            inventoryItems.Add(result.Value);
        }

        return inventoryItems;
    }
}