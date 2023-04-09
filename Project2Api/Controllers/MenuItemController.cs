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
        public IActionResult CreateMenuItem(MenuItemRequest menuItemRequest)
        {
            // convert request to menu item 
            ErrorOr<MenuItem> menuItem = MenuItem.From(menuItemRequest);

            if (menuItem.IsError)
            {
                return Problem(menuItem.Errors);
            }

            // save menu item to database
            ErrorOr<MenuItem> menuItemErrorOr = _menuItemService.CreateMenuItem(menuItem.Value);

            // return Ok(menuItemResponse) if succcessful, otherwise return error
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
        public IActionResult GetMenuItem(string name)
        {
            // get menu item with name=name
            ErrorOr<MenuItem> menuItemErrorOr = _menuItemService.GetMenuItem(name);

            // return Ok(menuItemResponse) if succcessful, otherwise return error
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
        public IActionResult GetAllMenuItems()
        {
            // get all menu items
            ErrorOr<Dictionary<string, List<MenuItem>>> menuItemsErrorOr = _menuItemService.GetAllMenuItems();

            // return Ok(menuItemResponse) if succcessful, otherwise return error
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
        public IActionResult UpdateMenuItem(string name, MenuItemRequest menuItemRequest)
        {
            // convert request to menu item
            ErrorOr<MenuItem> menuItem = MenuItem.From(menuItemRequest);

            if (menuItem.IsError)
            {
                return Problem(menuItem.Errors);
            }

            // update menu item
            ErrorOr<MenuItem> menuItemErrorOr = _menuItemService.UpdateMenuItem(name, menuItem.Value);

            // return Ok(menuItemResponse) if succcessful, otherwise return error
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
        public IActionResult DeleteMenuItem(string name)
        {
            // delete menu item
            ErrorOr<IActionResult> menuItemErrorOr = _menuItemService.DeleteMenuItem(name);

            // return NoContent() if succcessful, otherwise return error
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