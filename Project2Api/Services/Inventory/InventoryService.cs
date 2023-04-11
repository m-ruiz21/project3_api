using ErrorOr;
using Project2Api.ServiceErrors;
using Microsoft.AspNetCore.Mvc;
using Project2Api.DbTools;
using Project2Api.Models;
using System.Data;

namespace Project2Api.Services.Inventory;
public class InventoryService: IInventoryService
{
    private readonly IDbClient _dbClient;

    public InventoryService(IDbClient dbClient)
    {
        _dbClient = dbClient;
    }

    public ErrorOr<IActionResult> DeleteInventoryItem(InventoryItem inventoryItem)
    {
        string type = inventoryItem.Type;
        string table = (type == "menu item") ? "menu_item" : "cutlery";

        // delete items from menu_item table
        Task<int> inventoryTask = _dbClient.ExecuteNonQueryAsync(
            $"DELETE FROM {table} WHERE name = '{inventoryItem.Name}'"
        );

        // check that inventoryTask was successful
        if (inventoryTask.Result == -1)
        {
            return ServiceErrors.Errors.Inventory.DbError;
        }
        else if (inventoryTask.Result == 0)
        {
            return ServiceErrors.Errors.Inventory.NotFound;
        }

        return new NoContentResult();
    }

    public ErrorOr<List<InventoryItem>> GetAllInventoryItems()
    {
        Task<DataTable?> table = _dbClient.ExecuteQueryAsync(
            "SELECT id, name, quantity, 'cutlery' as type " +
            "FROM cutlery " +
            "UNION " +
            "SELECT id, name, quantity, 'menu item' as type " +
            "FROM menu_item;"
        );

        if (table.Result == null)
        {
            return ServiceErrors.Errors.Inventory.DbError;
        }

        // convert table to list of inventory Items
        List<InventoryItem> inventoryItems = new List<InventoryItem>();
        foreach (DataRow row in table.Result.Rows)
        {
            ErrorOr<InventoryItem> inventoryItem = InventoryItem.From(row);
            if (inventoryItem.IsError)
            {
                return inventoryItem.FirstError;
            }

            inventoryItems.Add(inventoryItem.Value);
        } 

        return inventoryItems;
    }
}