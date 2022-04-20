using Z.EntityFramework.Plus;

namespace AjNetCore.Modules.Menus.CacheManagers
{
	public static class MenuCacheManager
	{
		public static void ClearCache()
		{
			QueryCacheManager.ExpireTag(Name);
		}

		public static string Name { get; set; } = "Menu";
	}
}