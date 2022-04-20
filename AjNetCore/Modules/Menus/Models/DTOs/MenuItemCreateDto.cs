namespace AjNetCore.Modules.Menus.Models.DTOs
{
    public class MenuItemCreateDto
    {
        public int MenuId { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsOpenNewTab { get; set; }
    }
}