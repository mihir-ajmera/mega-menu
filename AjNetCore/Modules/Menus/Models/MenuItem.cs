﻿using AjNetCore.Modules.Core.Data;
using System.Collections.Generic;

namespace AjNetCore.Modules.Menus.Models
{
    public class MenuItem : INestedSet
    {
        public int Id { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }

        public bool IsOpenNewTab { get; set; }

        public int? Left { get; set; }
        public int? Right { get; set; }
        public int Depth { get; set; }

        public int? ParentId { get; set; }
        public MenuItem Parent { get; set; }
        public List<MenuItem> Children { get; set; }
    }
}