using ErrorOr;
using Project2Api.Contracts.MenuItem;
using Project2Api.ServiceErrors;

namespace Project2Api.Models;

public class MenuItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public int Quantity { get; set; }
    public List<string> Cutlery { get; set; }

    /// <summary>
    /// Creates menu item (Created for use by Dapper)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="price"></param>
    /// <param name="category"></param>
    /// <param name="quantity"></param>
    /// <param name="MenuItemCutlery"></param>
    /// <returns>Created menu item</returns>
    public MenuItem(
        string name,
        int quantity,
        decimal price,
        string category)
    {
        this.Name = name;
        this.Price = price;
        this.Category = category;
        this.Quantity = quantity;
        this.Cutlery = new List<string>();
    }

    /// <summary>
    /// Private constructor for menu item
    /// </summary>
    /// <param name="name"></param>
    /// <param name="price"></param>
    /// <param name="category"></param>
    /// <param name="quantity"></param>
    /// <param name="MenuItemCutlery"></param>
    private MenuItem(
        string name,
        decimal price,
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

    /// <summary>
    /// Creates menu item
    /// </summary>
    /// <param name="name"></param>
    /// <param name="price"></param>
    /// <param name="category"></param>
    /// <param name="quantity"></param>
    /// <param name="MenuItemCutlery"></param>
    /// <returns>Created menu item</returns>
    public static ErrorOr<MenuItem> Create(
        string name,
        decimal price,
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

    /// <summary>
    /// Creates menu item from request
    /// </summary>
    /// <param name="menuItemRequest"></param>
    /// <returns>Created menu item</returns>
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
}