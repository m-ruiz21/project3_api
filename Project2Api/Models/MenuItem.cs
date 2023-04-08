using System.Data;
using ErrorOr;
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
        if (name == null || price < 0 || category == null || quantity < 0 || MenuItemCutlery == null)
        {
            return ServiceErrors.Errors.MenuItem.InvalidMenuItem;
        }

        return new MenuItem(name, price, category, quantity, MenuItemCutlery);
    }

    public static ErrorOr<MenuItem> From(DataRow dataRow)
    {
        // check if dataRow is null 
        if (dataRow == null)
        {
            // print error message
            Console.WriteLine("[OrdersService] Failed to convert data table: data row is null");
            return Errors.Orders.DbError;
        }

        // check if any of the columns are null 
        if (dataRow["id"] == null || dataRow["date_time"] == null || dataRow["total_price"] == null)
        {
            // print error message
            Console.WriteLine("[OrdersService] Failed to convert data table: some fields are null");
            return Errors.Orders.NotFound;
        }

        // convert rest of row to order
        String name = dataRow["name"].ToString() ?? "";
        String RawPrice = dataRow["price"].ToString() ?? "";
        String RawCategory = dataRow["category"].ToString() ?? "";
        String RawQuantity = dataRow["quantity"].ToString() ?? "";

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