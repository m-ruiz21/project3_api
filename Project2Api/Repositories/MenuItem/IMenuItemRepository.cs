using Project2Api.Models;

namespace Project2Api.Repositories;

public interface IMenuItemRepository
{
    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    /// <param name="menuItem">The menu item to create.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int?> CreateMenuItemAsync(MenuItem menuItem);

    /// <summary>
    /// Gets all menu items.
    /// </summary>
    /// <returns>A list of menu items.</returns>
    Task<IEnumerable<MenuItem>?> GetMenuItemsAsync();

    /// <summary>
    /// Gets a menu item by name.
    /// </summary>
    /// <param name="name">The name of the menu item.</param>
    /// <returns>A menu item.</returns>
    Task<MenuItem?> GetMenuItemByNameAsync(string name);

    /// <summary>
    /// Updates a menu item.
    /// </summary>
    /// <param name="menuItem">The menu item to update.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int?> UpdateMenuItemAsync(MenuItem menuItem);

    /// <summary>
    /// Deletes a menu item.
    /// </summary>
    /// <param name="name">The name of the menu item to delete.</param>
    /// <returns>The number of rows affected.</returns>
    Task<bool> DeleteMenuItemAsync(string name);
}