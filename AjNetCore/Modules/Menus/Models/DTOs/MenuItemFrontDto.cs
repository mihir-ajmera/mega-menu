using System.Collections.Generic;

namespace AjNetCore.Modules.Menus.Models.DTOs
{
    public class MenuItemFrontDto
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int? ParentId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }

        public bool IsOpenNewTab { get; set; }
        public int? Left { get; set; }
        public int? Right { get; set; }

        public List<MenuItemFrontDto> Childrens { get; set; }
    }
}