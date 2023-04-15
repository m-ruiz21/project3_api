using Project2Api.Models;

namespace Project2Api.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    public Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
    {
        throw new NotImplementedException();
    }

    public Task<MenuItem> DeleteMenuItemAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<MenuItem> GetMenuItemByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
    {
        throw new NotImplementedException();
    }
}