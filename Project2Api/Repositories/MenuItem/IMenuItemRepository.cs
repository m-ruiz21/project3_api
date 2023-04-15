using Project2Api.Models;

namespace Project2Api.Repositories;

public interface IMenuItemRepository
{
    Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
    Task<MenuItem> GetMenuItemByNameAsync(string name);
    Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);
    Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
    Task<MenuItem> DeleteMenuItemAsync(string name);
}