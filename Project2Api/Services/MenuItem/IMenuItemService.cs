using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;

namespace Project2Api.Services.MenuItems
{
    public interface IMenuItemService
    {
        ErrorOr<MenuItem> CreateMenuItem(MenuItem menuItem);
    
        ErrorOr<MenuItem> GetMenuItem(Guid id);

        ErrorOr<List<MenuItem>> GetAllMenuItems(int pageNumber, int pageSize);

        ErrorOr<MenuItem> UpdateMenuItem(Guid id, Order order);

        ErrorOr<IActionResult> DeleteMenuItem(Guid id);
    }
}