namespace Project2Api.Models;

public class Cutlery
{
    public string Name { get; set; }
    public int Quantity { get; set; }

    /// <summary>
    /// Creates cutlery
    /// </summary>
    /// <param name="name"></param>
    /// <param name="quantity"></param>
    /// <returns>Created cutlery</returns>
    public Cutlery(
        string name,
        int quantity)
    {
        this.Name = name;
        this.Quantity = quantity;
    }
}