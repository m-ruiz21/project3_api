using System.Data;
using ErrorOr;

namespace Project2Api.Models;

public class InventoryItem
{
    public string Name { get; }
    public int Quantity { get; }
    public string Type { get; }

    private InventoryItem(string name, string type, int quantity)
    {
        this.Name = name;
        this.Type = type;
        this.Quantity = quantity;
    }

    public ErrorOr<InventoryItem> Create(
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

    public ErrorOr<InventoryItem> From(DataRow row)
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