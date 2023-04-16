using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.Inventory;
using Project2Api.Models;
using Project2Api.Services.Inventory;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryController : ApiController 
    {
        private readonly IInventoryService _inventoryService;
        
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Gets all inventory items
        /// </summary>
        /// <returns>List of inventory items</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllInventoryItems()
        {
            // get all inventory items
            ErrorOr<List<InventoryItem>> inventoryItemsErrorOr = await _inventoryService.GetAllInventoryItems();

            // return Ok(inventoryItems) if succcessful, otherwise return error
            return inventoryItemsErrorOr.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Deletes inventory item
        /// </summary>
        /// <param name="inventoryRequest"></param>
        /// <returns>No Content Result or Error</returns>
        [HttpDelete()]
        public async Task<IActionResult> DeleteInventoryItem(InventoryRequest inventoryRequest)
        {
            // convert request to inventory item 
            ErrorOr<InventoryItem> inventoryItem = InventoryItem.From(inventoryRequest);

            if (inventoryItem.IsError)
            {
                return Problem(inventoryItem.Errors);
            }

            // delete inventory item from database
            ErrorOr<IActionResult> inventoryItemErrorOr = await _inventoryService.DeleteInventoryItem(inventoryItem.Value);

            // return NoContent() if succcessful, otherwise return error
            return inventoryItemErrorOr.Match(
                value => value,
                errors => Problem(errors)
            );
        }
    }
} 