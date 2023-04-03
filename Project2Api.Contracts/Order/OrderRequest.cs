namespace Project2Api.Contracts.Order
{
    public record OrderRequest(
        List<string> Items, 
        float Price
    );
}
