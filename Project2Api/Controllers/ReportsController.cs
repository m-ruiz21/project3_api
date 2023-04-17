using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.Cutlery;
using Project2Api.Models;
using Project2Api.Models.Reports;
using Project2Api.Services.CutleryItems;
using Project2Api.Services.Reports;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ApiController 
    {
        private IReportsService _reportsService;
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        /// <summary>
        // get X Report
        /// </summary>
        /// <returns>Sales History</returns>
        [HttpGet("xreport")]
        public async Task<IActionResult> GetXReport()
        {
            ErrorOr<XReport> result = await _reportsService.GetXReport();

            return result.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}