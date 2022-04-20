using System.Collections.Generic;

namespace AjNetCore.Modules.Menus.Models
{
	public class Menu
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public List<MenuItem> MenuItems { get; set; }
	}
}