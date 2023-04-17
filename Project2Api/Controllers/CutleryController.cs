using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.Cutlery;
using Project2Api.Models;
using Project2Api.Services.CutleryItems;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("cutlery")]
    public class CutleryController : ApiController 
    {
        private readonly ICutleryService _cutleryService;
        
        public CutleryController(ICutleryService cutleryService)
        {
            _cutleryService = cutleryService;
        }

        /// <summary>
        /// Creates cutlery item
        /// </summary>
        /// <param name="cutleryRequest"></param>
        /// <returns>Cutlery item or error</returns>
        [HttpPost()]
        public async Task<IActionResult> CreateCutleryItem(CutleryRequest cutleryRequest)
        {
            ErrorOr<Cutlery> cutleryItem = Cutlery.From(cutleryRequest);

            if (cutleryItem.IsError)
            {
                return Problem(cutleryItem.Errors);
            }

            ErrorOr<Cutlery> cutleryItemErrorOr = await _cutleryService.CreateCutleryAsync(cutleryItem.Value);

            return cutleryItemErrorOr.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Gets all cutlery items
        /// </summary>
        /// <returns>List of cutlery items</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllCutleryItems()
        {
            // get all cutlery items
            ErrorOr<List<Cutlery>> cutleryItemsErrorOr = await _cutleryService.GetAllCutleryAsync();

            // return Ok(cutleryItems) if succcessful, otherwise return error
            return cutleryItemsErrorOr.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Gets cutlery item by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Cutlery item or error</returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCutleryItemByName(string name)
        {
            ErrorOr<Cutlery> cutleryItemErrorOr = await _cutleryService.GetCutleryAsync(name);

            return cutleryItemErrorOr.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Updates cutlery item
        /// </summary>
        /// <param name="cutleryRequest"></param>
        /// <returns>Cutlery item or error</returns>
        [HttpPut()]
        public async Task<IActionResult> UpdateCutleryItem(CutleryRequest cutleryRequest)
        {
            ErrorOr<Cutlery> cutleryItem = Cutlery.From(cutleryRequest);

            if (cutleryItem.IsError)
            {
                return Problem(cutleryItem.Errors);
            }

            ErrorOr<Cutlery> updatedCutleryItem = await _cutleryService.UpdateCutleryAsync(cutleryItem.Value);

            return updatedCutleryItem.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Deletes cutlery item
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Success or error</returns>
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCutleryItem(string name)
        {
            ErrorOr<IActionResult> deletedCutleryItem = await _cutleryService.DeleteCutleryAsync(name);

            return deletedCutleryItem.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}