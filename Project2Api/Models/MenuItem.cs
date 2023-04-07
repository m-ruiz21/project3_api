namespace Project2Api.Models;

public class MenuItem
{
    string name { get; }
    float price { get; }
    string category { get; }
    int quantity { get; }
    List<string> MenuItemCutlery { get; }

    private MenuItem(
        string name,
        float price,
        string category,
        int quantity,
        List<string> MenuItemCutlery)
    {
        this.name = name;
        this.price = price;
        this.category = category;
        this.quantity = quantity;
        this.MenuItemCutlery = MenuItemCutlery;
    }
}