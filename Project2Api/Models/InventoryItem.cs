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
        if (string.IsNullOrEmpty(name) 
            || (type != "cutlery" && type != "menu item") 
            || string.IsNullOrEmpty(type) 
            || quantity < 0)
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
}