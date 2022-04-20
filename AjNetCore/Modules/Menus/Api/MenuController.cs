using AjNetCore.Modules.Core.Api;
using AjNetCore.Modules.Menus.Models.DTOs;
using AjNetCore.Modules.Menus.Services;
using Microsoft.AspNetCore.Mvc;

namespace AjNetCore.Modules.Menus.Api
{
    [Route("api/menus")]
    public class MenuController : BaseApiController
    {
        private readonly IMenuItemService _menuItemService;

        public MenuController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet("get-all-menus-items/{menuId:int}")]
        public IActionResult GetAllMenuItems(int menuId)
        {
            return Result(_menuItemService.GetAllMenuItems(menuId));
        }

        [HttpGet("get-all-root-menus-items/{menuId:int}")]
        public IActionResult GetAllRootMenuItems(int menuId)
        {
            return Result(_menuItemService.GetAllRootMenuItems(menuId));
        }

        [HttpGet("get-all-leaf-menus-items/{menuId:int}")]
        public IActionResult GetAllLeafMenuItems(int menuId)
        {
            return Result(_menuItemService.GetAllLeafMenuItems(menuId));
        }

        [HttpGet("get-menu-items-by-name/{menuId:int}/{menuItemName}")]
        public IActionResult GetMenuItemsByName(int menuId, string menuItemName)
        {
            return Result(_menuItemService.GetMenuItemsByName(menuId, menuItemName));
        }

        [HttpPost("create")]
        public IActionResult Post([FromBody] MenuItemCreateDto dto)
        {
            return Result(_menuItemService.Create(dto));
        }

        [HttpPost("delete/{menuId:int}/{menuItemId:int}")]
        public IActionResult Delete(int menuId, int menuItemId)
        {
            return Result(_menuItemService.Delete(menuId, menuItemId));
        }

    }

}