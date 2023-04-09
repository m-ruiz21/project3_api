namespace Project2Api.Contracts.Inventory
{
    public record InventoryRequest(
        string Name,
        int Quantity,
        string type
    );
}