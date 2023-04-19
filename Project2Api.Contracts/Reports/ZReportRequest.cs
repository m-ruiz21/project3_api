namespace Project2Api.Contracts.Reports
{
    public record ZReportRequest(
        DateTime StartDate,
        DateTime EndDate
    );
}