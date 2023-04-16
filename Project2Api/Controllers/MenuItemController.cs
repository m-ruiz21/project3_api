using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Contracts.MenuItem;
using Project2Api.Models;
using Project2Api.Services.MenuItems;

namespace Project2Api.Controllers
{
    [ApiController]
    [Route("menu-item")]
    public class MenuItemController : ApiController 
    {
        private readonly IMenuItemService _menuItemService;
        
        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        } 

        /// <summary>
        /// Creates menu item
        /// </summary>
        /// <param name="menuItemRequest"></param>
        /// <returns>Added Object</returns>
        [HttpPost()]
        public async Task<IActionResult> CreateMenuItem(MenuItemRequest menuItemRequest)
        {
            ErrorOr<MenuItem> menuItem = MenuItem.From(menuItemRequest);

            if (menuItem.IsError)
            {
                return Problem(menuItem.Errors);
            }

            ErrorOr<MenuItem> menuItemErrorOr = await _menuItemService.CreateMenuItemAsync(menuItem.Value);

            return menuItemErrorOr.Match(
                value => Ok(MapMenuItemToMenuItemResponse(value)),
                errors => Problem(errors)
            );
        }
        
        /// <summary>
        /// Gets menu item with specific name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Requested object</returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetMenuItem(string name)
        {
            ErrorOr<MenuItem> menuItemErrorOr = await _menuItemService.GetMenuItemAsync(name);

            return menuItemErrorOr.Match(
                value => Ok(MapMenuItemToMenuItemResponse(value)),
                errors => Problem(errors)
            );
        }
        
        /// <summary>
        /// Gets all menu items, grouped by category
        /// </summary>
        /// <returns>Requested object</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllMenuItems()
        {
            ErrorOr<Dictionary<string, List<MenuItem>>> menuItemsErrorOr = await _menuItemService.GetAllMenuItemsAsync();

            return menuItemsErrorOr.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Updates menu item with specific name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="menuItemRequest"></param>
        /// <returns>Updated object</returns>
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateMenuItem(string name, MenuItemRequest menuItemRequest)
        {
            ErrorOr<MenuItem> menuItem = MenuItem.From(menuItemRequest);

            if (menuItem.IsError)
            {
                return Problem(menuItem.Errors);
            }

            ErrorOr<MenuItem> menuItemErrorOr = await _menuItemService.UpdateMenuItemAsync(name, menuItem.Value);

            return menuItemErrorOr.Match(
                value => Ok(MapMenuItemToMenuItemResponse(value)),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Deletes menu item with specific name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Deleted object</returns>
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteMenuItem(string name)
        {
            ErrorOr<IActionResult> menuItemErrorOr = await _menuItemService.DeleteMenuItemAsync(name);

            return menuItemErrorOr.Match(
                value => NoContent(), 
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// Maps MenuItem to MenuItemResponse
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns>MenuItemResponse</returns>
        private static MenuItemResponse MapMenuItemToMenuItemResponse(MenuItem menuItem)
        {
            return new MenuItemResponse
            (
                menuItem.Name,
                menuItem.Price,
                menuItem.Category,
                menuItem.Quantity,
                menuItem.Cutlery
            );
        }    
    }
}