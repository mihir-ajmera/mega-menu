using AjNetCore.Modules.Core.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AjNetCore.Modules.Frontend
{
    public class FrontendModule : BaseModule
    {
        public FrontendModule()
        {
            ModuleName = "Frontend";

            HasViews = true;

            OrderId = 1000;
        }

        public override void RegisterRoutes(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapControllerRoute(
                name: "Home",
                pattern: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            endpoint.MapControllerRoute(
                name: "Error",
                pattern: "error/{statusCode}",
                defaults: new { controller = "Home", action = "Error" }
            );

            endpoint.MapControllerRoute(
                name: "PageNotFound",
                pattern: "page-not-found",
                defaults: new { controller = "Errors", action = "PageNotFound" }
            );

            endpoint.MapControllerRoute(
                name: "Unauthorized",
                pattern: "unauthorized",
                defaults: new { controller = "Errors", action = "Unauthorized" }
            );

            //routes.MapRoute(
            //    name: "AdminLoginAs",
            //    url: "admin-login-as/{memberId}",
            //    defaults: new { controller = "Auth", action = "MemberLoginViaAdminLoginAs" },
            //    namespaces: routeNamespaces
            //);
        }

    }
}