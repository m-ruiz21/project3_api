using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Models;

namespace Project2Api.Services.MenuItems
{
    public interface IMenuItemService
    {
        ErrorOr<MenuItem> CreateMenuItem(MenuItem menuItem);
    
        ErrorOr<MenuItem> GetMenuItem(string name);

        ErrorOr<Dictionary<string, List<MenuItem>>> GetAllMenuItems();

        ErrorOr<MenuItem> UpdateMenuItem(string name, MenuItem menuItem);

        ErrorOr<IActionResult> DeleteMenuItem(string name);
    }
}