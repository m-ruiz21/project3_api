using ErrorOr;
using Project2Api.Contracts.Reports;
using Project2Api.Models;
using Project2Api.Models.Reports;

namespace Project2Api.Services.Reports
{
    public interface IReportsService
    {
        /// <summary>
        /// Gets X Report
        /// </summary>
        /// <returns>X Report</returns>
        Task<ErrorOr<XReport>> GetXReport();

        /// <summary>
        /// Gets Y Report
        /// </summary>
        /// <returns>Y Report</returns>
        Task<ErrorOr<List<ZReportDataPoint>>> GetZReport(int pageNumber, int pageSize);

        /// <summary>
        /// Gets Excess Report
        /// </summary>
        /// <returns>Excess Report</returns>
        Task<ErrorOr<List<ExcessMenuItem>>> GetExcessReport(DateTime fromDate);

        /// <summary>
        /// Gets Sales Report
        /// </summary>
        /// <returns>Sales Report</returns>
        Task<ErrorOr<List<SalesReport>>> GetSalesReport(DateTime startDate, DateTime endDate, string ItemName);
    }
}