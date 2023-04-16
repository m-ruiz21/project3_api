namespace Project2Api.Contracts.MenuItem
{
    public record MenuItemResponse(
        string Name,
        decimal Price,
        string Category,
        int Quantity,
        List<string> Cutlery
    );
}