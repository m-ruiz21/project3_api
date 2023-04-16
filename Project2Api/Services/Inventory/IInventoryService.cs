using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;

namespace Project2Api.Services.Inventory
{
    public interface IInventoryService
    {
        /// <summary>
        // Gets all inventory items
        /// </summary>
        /// <returns>List of inventory items</returns>
        Task<ErrorOr<List<InventoryItem>>> GetAllInventoryItems();

        /// <summary>
        /// Gets inventory item by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>No Content Result or Error</returns>
        Task<ErrorOr<IActionResult>> DeleteInventoryItem(InventoryItem inventoryItem);
    }
}