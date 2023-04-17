using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.Cutlery;
using Project2Api.Models;
using Project2Api.Services.CutleryItems;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ApiController 
    {
        public ReportsController()
        {

        }

        /// <summary>
        // get X Report
        /// </summary>
        /// <returns>Sales History</returns>
        [HttpGet("xreport")]
        public IActionResult GetXReport()
        {
            return Ok();
        }
    }
}