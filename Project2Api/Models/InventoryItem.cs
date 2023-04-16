using System.Data;
using ErrorOr;
using Project2Api.Contracts.Inventory;

namespace Project2Api.Models;

public class InventoryItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; }

    /// <summary>
    /// Private constructor for InventoryItem
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="quantity"></param>
    private InventoryItem(string name, string type, int quantity)
    {
        this.Name = name;
        this.Type = type;
        this.Quantity = quantity;
    }

    /// <summary>
    /// Creates a new InventoryItem
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="quantity"></param>
    /// <returns>Error or new inventory item</returns>
    public static ErrorOr<InventoryItem> Create(
        string name,
        string type,
        int quantity
    )
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || quantity < 0)
        {
            return ServiceErrors.Errors.Inventory.InvalidInventoryItem;
        }

        return new InventoryItem(name, type, quantity);
    }

    /// <summary>
    /// Creates a new InventoryItem from an InventoryRequest
    /// </summary>
    /// <param name="inventoryRequest"></param>
    /// <returns>Error or new inventory item</returns>
    public static ErrorOr<InventoryItem> From(InventoryRequest inventoryRequest)
    {
        if (string.IsNullOrEmpty(inventoryRequest.Name) || string.IsNullOrEmpty(inventoryRequest.Type))
        {
            return ServiceErrors.Errors.Inventory.InvalidInventoryItem;
        }

        return Create(
            inventoryRequest.Name,
            inventoryRequest.Type,
            0 
        );
    }

    /// <summary>
    /// Creates a new InventoryItem from a DataRow
    /// </summary>
    /// <param name="row"></param>
    /// <returns>Error or new inventory item</returns>
    public static ErrorOr<InventoryItem> From(DataRow row)
    {
        if (!row.Table.Columns.Contains("name") || !row.Table.Columns.Contains("type") || !row.Table.Columns.Contains("quantity"))
        {
            return ServiceErrors.Errors.Inventory.InvalidInventoryItem;
        }

        string name = row["name"].ToString() ?? "";
        string type = row["type"].ToString() ?? "";
        string rawQuantity = row["quantity"].ToString() ?? "";

        // make sure none of them are emtpy or null
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(rawQuantity))
        {
            Console.WriteLine("[Inventory] Failed to Convert to Inventory Item: missing values");
            return ServiceErrors.Errors.Inventory.InvalidInventoryItem;
        }

        try 
        {
            return Create(
                name,  
                type, 
                int.Parse(rawQuantity)
                );
        } catch (Exception e) { 
            Console.WriteLine(e);
            return ServiceErrors.Errors.Inventory.UnexpectedError;
        }
    }
}