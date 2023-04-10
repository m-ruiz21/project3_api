using ErrorOr;
using Project2Api.ServiceErrors;
using Microsoft.AspNetCore.Mvc;
using Project2Api.DbTools;
using Project2Api.Models;
using System.Data;

namespace Project2Api.Services.Inventory;
public class MenuItemService : IInventoryService
{
    private readonly IDbClient _dbClient;

    public MenuItemService(IDbClient dbClient)
    {
        _dbClient = dbClient;
    }

    public ErrorOr<IActionResult> DeleteInventoryItem(InventoryItem inventoryItem)
    {
        throw new NotImplementedException();
    }

    public ErrorOr<List<InventoryItem>> GetAllInventoryItems()
    {
        Task<DataTable> table = _dbClient.ExecuteQueryAsync(
            "SELECT id, name, quantity, 'cutlery' as type " +
            "FROM cutlery " +
            "UNION " +
            "SELECT id, name, quantity, 'menu item' as type " +
            "FROM menu_item;"
        );

        if (table.IsFaulted)
        {
            return ServiceErrors.Errors.Inventory.DbError;
        }

        // convert table to list of inventory Items
        List<InventoryItem> inventoryItems = new List<InventoryItem>();
        foreach (DataRow row in table.Result.Rows)
        {

        } 

        throw new NotImplementedException();
    }
}