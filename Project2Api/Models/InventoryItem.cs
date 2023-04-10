using System.Data;
using ErrorOr;

namespace Project2Api.Models;

public class InventoryItem
{
    public string Name { get; }
    public string Category { get; }
    public int Quantity { get; }

    private InventoryItem(string name, string category, int quantity)
    {
        this.Name = name;
        this.Category = category;
        this.Quantity = quantity;
    }

    public ErrorOr<InventoryItem> Create(
        string name,
        string category,
        int quantity
    )
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category) || quantity < 0)
        {
            return ServiceErrors.Errors.Inventory.InvalidInventoryItem;
        }

        return new InventoryItem(name, category, quantity);
    }

    public ErrorOr<InventoryItem> From(DataRow row)
    {
        if (!row.Table.Columns.Contains("name") || !row.Table.Columns.Contains("category") || !row.Table.Columns.Contains("quantity"))
        {
            return ServiceErrors.Errors.Inventory.InvalidInventoryItem;
        }

        string name = row["name"].ToString() ?? "";
        string category = row["category"].ToString() ?? "";
        string rawQuantity = row["quantity"].ToString() ?? "";

        // make sure none of them are emtpy or null
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(rawQuantity))
        {
            Console.WriteLine("[Inventory] Failed to Convert to Inventory Item: missing values");
            return ServiceErrors.Errors.Inventory.InvalidInventoryItem;
        }

        try 
        {
            return Create(
                name,  
                category, 
                int.Parse(rawQuantity)
                );
        } catch (Exception e) { 
            Console.WriteLine(e);
            return ServiceErrors.Errors.Inventory.UnexpectedError;
        }
    }
}