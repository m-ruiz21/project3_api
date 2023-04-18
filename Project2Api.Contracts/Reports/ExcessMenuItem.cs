namespace Project2Api.Contracts.Reports
{
    public record ExcessMenuItem(
        string MenuItemName,
        int Quantity,
        int AmountSold
    );
}