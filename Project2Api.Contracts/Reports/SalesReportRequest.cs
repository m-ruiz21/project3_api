namespace Project2Api.Contracts.Reports
{
    public record SalesReportRequest(
        DateTime StartDate,
        DateTime EndDate,
        string menuItem
    );
}