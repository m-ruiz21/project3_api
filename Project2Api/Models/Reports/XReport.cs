using ErrorOr;
using Project2Api.Contracts.Cutlery;
using Project2Api.ServiceErrors;

namespace Project2Api.Models.Reports;

public class XReport 
{
    public decimal Sales { get; set; }

    public XReport(decimal sales)
    {
        Sales = sales;
    }
}