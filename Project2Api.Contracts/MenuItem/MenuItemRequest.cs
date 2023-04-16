namespace Project2Api.Contracts.MenuItem
{
    public record MenuItemRequest(
        string Name,
        decimal Price,
        string Category,
        int Quantity,
        List<string> Cutlery
    );
}