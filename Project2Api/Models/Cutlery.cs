using ErrorOr;
using Project2Api.Contracts.Cutlery;
using Project2Api.ServiceErrors;

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
    
    public static ErrorOr<Cutlery> Create(
        string name,
        int quantity
    )
    {
        if (String.IsNullOrEmpty(name) || quantity < 0)
        {
            return Errors.Cutlery.InvalidCutlery;
        }

        return new Cutlery(
            name,
            quantity
        );
    }

    /// <summary>
    /// Creates cutlery From cutleryRequest
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Created cutlery</returns>
    public static ErrorOr<Cutlery> From(CutleryRequest cutleryRequest)
    {
        return Create(
            cutleryRequest.Name,
            cutleryRequest.Quantity
        );
    }
}