using AjNetCore.Modules.Menus.Models;
using AjNetCore.Modules.Menus.Models.DTOs;
using AutoMapper;

namespace AjNetore.Modules.Menus.Models.Mappers
{
    public class MenuItemMappingProfile : Profile
    {
        public MenuItemMappingProfile()
        {
            // Create
            CreateMap<MenuItemCreateDto, MenuItem>();
        }
    }
}