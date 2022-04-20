using AjNetCore.Modules.Core.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AjNetCore.Modules.Core
{
    public class CoreModule : BaseModule
    {
        public CoreModule()
        {
            // Module
            ModuleName = "Core";

            HasViews = true;

            // Class order for register
            OrderId = 0;
        }

        public override void RegisterRoutes(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapControllerRoute(
                name: "CoreSeed",
                pattern: "core/seed",
                defaults: new { controller = "Core", action = "Seed" }
            );
        }
    }
}