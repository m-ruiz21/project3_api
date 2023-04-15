using Project2Api.Models;

namespace Project2Api.Repositories;

public interface IMenuItemRepository
{
    Task<int?> CreateMenuItemAsync(MenuItem menuItem);
    Task<IEnumerable<MenuItem>?> GetMenuItemsAsync();
    Task<MenuItem?> GetMenuItemByNameAsync(string name);
    Task<int?> UpdateMenuItemAsync(MenuItem menuItem);
    Task<bool> DeleteMenuItemAsync(string name);
}