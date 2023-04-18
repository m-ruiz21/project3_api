using System.Linq;
using ErrorOr;
using Project2Api.Models;
using Project2Api.Models.Reports;
using Project2Api.Repositories;
using Project2Api.ServiceErrors;

namespace Project2Api.Services.Reports;

public class ReportsService : IReportsService
{
    private IOrdersRepository _ordersRepository;

    public ReportsService(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<ErrorOr<XReport>> GetXReport()
    {
        IEnumerable<Order>? orders = await _ordersRepository.GetOrdersAsync();

        if (orders == null) 
        {
            return Errors.Reports.DbError;
        }

        decimal todaySales = orders
            .Where(o => o.OrderTime.Date == DateTime.Today)
            .Sum(o => o.Price);
        
        XReport xReport = new XReport(todaySales);
        
        return xReport;
    }

    public async Task<ErrorOr<List<ZReportDataPoint>>> GetZReport(DateTime startDate, DateTime endDate)
    {
        IEnumerable<Order>? orders = await _ordersRepository.GetOrdersAsync(); 

        if (orders == null) 
        {
            return Errors.Reports.DbError;
        }

        var historicalSales = orders
            .Where(o => o.OrderTime.Date >= startDate && o.OrderTime.Date <= endDate)
            .GroupBy(o => o.OrderTime.Date)
            .Select(g => new ZReportDataPoint(
                g.Sum(o => o.Price), g.Key
                ))
            .OrderBy(g => g.Date)
            .ToList();

        return historicalSales;
    }
}