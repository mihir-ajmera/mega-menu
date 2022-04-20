using AjNetCore.Modules.Core.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AjNetCore.Modules.Menus
{
    public class MenusModule : BaseModule
    {
        public MenusModule()
        {
            ModuleName = "Menus";

            HasViews = true;

            OrderId = 5987;
        }

        public override void RegisterRoutes(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapControllerRoute(
                name: "Menu",
                pattern: "menu",
                defaults: new { controller = "Menus", action = "Index" }
            );
        }
    }
}