using System.Linq;
using ErrorOr;
using Project2Api.Contracts.Reports;
using Project2Api.Models;
using Project2Api.Models.Reports;
using Project2Api.Repositories;
using Project2Api.ServiceErrors;

namespace Project2Api.Services.Reports;

public class ReportsService : IReportsService
{
    private IOrdersRepository _ordersRepository;
    private IMenuItemRepository _menuItemsRepository;
    private IOrderedMenuItemRepository _orderedMenuItemsRepository;

    public ReportsService(
        IOrdersRepository ordersRepository, 
        IMenuItemRepository menuItemsRepository, 
        IOrderedMenuItemRepository orderedMenuItemsRepository
    )
    {
        _ordersRepository = ordersRepository;
        _menuItemsRepository = menuItemsRepository;
        _orderedMenuItemsRepository = orderedMenuItemsRepository;
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

    public async Task<ErrorOr<List<ZReportDataPoint>>> GetZReport(int pageNumber, int pageSize)
    {
        IEnumerable<Order>? orders = await _ordersRepository.GetOrdersAsync(); 

        if (orders == null) 
        {
            return Errors.Reports.DbError;
        }

        DateTime endDate = DateTime.Now.AddDays(-((pageNumber - 1) * pageSize));

        DateTime startDate = endDate.AddDays(-pageSize); 




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

    public async Task<ErrorOr<List<ExcessMenuItem>>> GetExcessReport(DateTime fromDate)
    {
        if (fromDate == DateTime.MinValue || fromDate > DateTime.Now) 
        {
            return Errors.Reports.InvalidRequest;
        }

        IEnumerable<MenuItem>? menuItems = await _menuItemsRepository.GetMenuItemsAsync();
        IEnumerable<OrderedMenuItem>? orderedMenuItems = await _orderedMenuItemsRepository.GetOrderedMenuItemsAsync();
        IEnumerable<Order>? orders = await _ordersRepository.GetOrdersAsync();

        if (menuItems == null || orderedMenuItems == null || orders == null) 
        {
            return Errors.Reports.DbError;
        }

        decimal stockThreshold = 0.1m; // 10% stock threshold

        var query = 
            from menuItem in menuItems
            join orderedMenuItem in (
                    from orderedMenuItem in orderedMenuItems
                    join order in orders on orderedMenuItem.OrderId equals order.Id
                    where order.OrderTime >= fromDate
                    select new { orderedMenuItem, order.OrderTime }
                )
                on menuItem.Name equals orderedMenuItem.orderedMenuItem.MenuItemName into menuItemGroup
            from orderedMenuItem in menuItemGroup.DefaultIfEmpty()
            group new { menuItem, orderedMenuItem } by menuItem.Name into itemGroup
            let totalSold = itemGroup.Sum(x => x.orderedMenuItem?.orderedMenuItem.Quantity ?? 0)
            let stockSoldRatio = totalSold / (decimal)itemGroup.First().menuItem.Quantity
            where stockSoldRatio < stockThreshold
            select new ExcessMenuItem
            (
                MenuItemName: itemGroup.Key,
                Quantity: itemGroup.First().menuItem.Quantity,
                AmountSold: totalSold
            );

        return query.ToList();
    }

    public async Task<ErrorOr<List<SalesReport>>> GetSalesReport(DateTime startDate, DateTime endDate, string ItemName)
    {   
        IEnumerable<OrderedMenuItem>? orderedMenuItems = await _orderedMenuItemsRepository.GetOrderedMenuItemsAsync();
        IEnumerable<Order>? orders = await _ordersRepository.GetOrdersAsync();

        if (orders == null || orderedMenuItems == null)
        {
            return Errors.Reports.InvalidRequest;
        }

        var historicalSales = orders
            .Where(o => o.OrderTime.Date >= startDate && o.OrderTime.Date <= endDate)
            .Join(
                orderedMenuItems.Where(omi => omi.MenuItemName == ItemName),
                o => o.Id,
                omi => omi.OrderId,
                (o, omi) => new { Order = o, OrderedMenuItem = omi })
            .GroupBy(o => o.Order.OrderTime.Date)
            .Select(g => new SalesReport(
                g.Sum(om => om.OrderedMenuItem.Quantity * om.Order.Price),
                g.Key
            ))
            .OrderBy(g => g.Date)
            .ToList();

        return historicalSales;

    }   
    
}