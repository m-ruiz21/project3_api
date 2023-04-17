using ErrorOr;
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
    }
}