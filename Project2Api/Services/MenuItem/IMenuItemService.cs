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
        ErrorOr<MenuItem> CreateMenuItem(MenuItem menuItem);

        /// <summary>
        /// Gets menu item by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Menu item</returns>
        ErrorOr<MenuItem> GetMenuItem(string name);

        /// <summary>
        /// Gets all menu items
        /// </summary>
        /// <returns>Dictionary of menu items</returns>
        ErrorOr<Dictionary<string, List<MenuItem>>> GetAllMenuItems();

        /// <summary>
        /// Updates menu item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="menuItem"></param>
        /// <returns>Updated menu item</returns>
        ErrorOr<MenuItem> UpdateMenuItem(string name, MenuItem menuItem);

        /// <summary>
        /// Deletes menu item
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Deleted menu item</returns>
        ErrorOr<IActionResult> DeleteMenuItem(string name);
    }
}