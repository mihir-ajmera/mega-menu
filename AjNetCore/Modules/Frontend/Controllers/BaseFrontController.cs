using AjNetCore.Modules.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AjNetCore.Modules.Frontend.Controllers
{
    public class BaseFrontController : Controller
    {
        protected IFrontCommonService FrontCommonService;

        public BaseFrontController(
            IFrontCommonService frontCommonService)
        {
            FrontCommonService = frontCommonService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}