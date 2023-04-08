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
}