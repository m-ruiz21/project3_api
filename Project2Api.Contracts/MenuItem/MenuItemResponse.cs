namespace Project2Api.Contracts.MenuItem
{
    public record MenuItemResponse(
        string Name,
        float Price,
        string Category,
        int Quantity,
        List<string> Cutlery
    );
}