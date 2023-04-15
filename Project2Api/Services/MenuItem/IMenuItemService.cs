using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;

namespace Project2Api.Services.MenuItems
{
    public interface IMenuItemService
    {
        /// <summary>
        /// Creates menu item
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns>Created menu item</returns>
        Task<ErrorOr<MenuItem>> CreateMenuItemAsync(MenuItem menuItem);

        /// <summary>
        /// Gets menu item by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Menu item</returns>
        Task<ErrorOr<MenuItem>> GetMenuItem(string name);

        /// <summary>
        /// Gets all menu items
        /// </summary>
        /// <returns>Dictionary of menu items</returns>
        Task<ErrorOr<Dictionary<string, List<MenuItem>>>> GetAllMenuItemsAsync();

        /// <summary>
        /// Updates menu item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="menuItem"></param>
        /// <returns>Updated menu item</returns>
        Task<ErrorOr<MenuItem>> UpdateMenuItem(string name, MenuItem menuItem);

        /// <summary>
        /// Deletes menu item
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Deleted menu item</returns>
        Task<ErrorOr<IActionResult>> DeleteMenuItem(string name);
    }
}