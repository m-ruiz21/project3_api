namespace Project2Api.Models.Reports;

public class XReport 
{
    public decimal Sales { get; set; }

    public XReport(decimal sales)
    {
        Sales = sales;
    }
}