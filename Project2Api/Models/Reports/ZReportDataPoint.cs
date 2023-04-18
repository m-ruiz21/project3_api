namespace Project2Api.Models.Reports;

public class ZReportDataPoint 
{
    public decimal Sales { get; set; }
    public DateTime Date { get; set; }

    public ZReportDataPoint(decimal sales, DateTime dateTime)
    {
        Sales = sales;
        Date = dateTime;
    }
}