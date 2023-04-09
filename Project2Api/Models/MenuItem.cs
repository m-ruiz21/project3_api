using System.Data;
using ErrorOr;
using Project2Api.Contracts.MenuItem;
using Project2Api.ServiceErrors;

namespace Project2Api.Models;

public class MenuItem
{
    public string Name { get; }
    public float Price { get; }
    public string Category { get; }
    public int Quantity { get; }
    public List<string> Cutlery { get; }

    private MenuItem(
        string name,
        float price,
        string category,
        int quantity,
        List<string> MenuItemCutlery)
    {
        this.Name = name;
        this.Price = price;
        this.Category = category;
        this.Quantity = quantity;
        this.Cutlery = MenuItemCutlery;
    }

    public static ErrorOr<MenuItem> Create(
        string name,
        float price,
        string category,
        int quantity,
        List<string> MenuItemCutlery)
    {
        if (string.IsNullOrEmpty(name) || price < 0 || string.IsNullOrEmpty(name) || quantity < 0 || MenuItemCutlery == null)
        {
            return ServiceErrors.Errors.MenuItem.InvalidMenuItem;
        }

        return new MenuItem(name, price, category, quantity, MenuItemCutlery);
    }

    public static ErrorOr<MenuItem> From(MenuItemRequest menuItemRequest)
    {
        return Create(
            menuItemRequest.Name,
            menuItemRequest.Price,
            menuItemRequest.Category,
            menuItemRequest.Quantity,
            menuItemRequest.Cutlery
        );
    }
    
    public static ErrorOr<MenuItem> From(DataRow dataRow)
    {
        // check if dataRow is null 
        if (dataRow == null)
        {
            // print error message
            Console.WriteLine("[MenuItem] Failed to convert data table: data row is null");
            return Errors.Orders.DbError;
        }

        // check to see if our necessary columns exist
        if (!dataRow.Table.Columns.Contains("name") || !dataRow.Table.Columns.Contains("price") || !dataRow.Table.Columns.Contains("category") || !dataRow.Table.Columns.Contains("quantity"))
        {
            // print error message
            Console.WriteLine("[MenuItem] Failed to convert data table: missing columns");
            return Errors.Orders.DbError;
        }

        // convert rest of row to order
        String name = dataRow["name"].ToString() ?? "";
        String RawPrice = dataRow["price"].ToString() ?? "";
        String RawCategory = dataRow["category"].ToString() ?? "";
        String RawQuantity = dataRow["quantity"].ToString() ?? "";

        // check if any values are null or empty using string.IsNullOrEmpty
        if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(RawPrice) || String.IsNullOrEmpty(RawCategory) || String.IsNullOrEmpty(RawQuantity))
        {
            // print error message
            Console.WriteLine("[MenuItem] Failed to convert data table: missing values");
            // print out values
            Console.WriteLine($"name: {name}");
            Console.WriteLine($"price: {RawPrice}");
            Console.WriteLine($"category: {RawCategory}");
            Console.WriteLine($"quantity: {RawQuantity}");
            return Errors.MenuItem.NotFound;
        }

        // create order
        try
        {
            return Create(
                name,
                float.Parse(RawPrice),
                RawCategory,
                int.Parse(RawQuantity),
                new List<string>()
            ); 
        } catch (System.FormatException e) {
            Console.Out.WriteLine(e);
            return Errors.Orders.UnexpectedError;
        }
    }
}