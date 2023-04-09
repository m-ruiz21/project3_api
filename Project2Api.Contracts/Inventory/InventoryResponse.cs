namespace Project2Api.Contracts.Inventory
{
    public record InventoryResponse(
        string Name,
        int Quantity,
        string type
    );
}