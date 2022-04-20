using AjNetCore.Modules.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AjNetCore.Modules.Frontend.Controllers
{
    public class ErrorsController : BaseFrontController
    {
        public ErrorsController(
            IFrontCommonService frontCommonService) : base(frontCommonService)
        {
        }

        public IActionResult PageNotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            ViewBag.HomePage = "Homepage";

            return View();
        }

        //public IActionResult Unauthorized()
        //{
        //    //ViewBag.MetaTags = FrontCommonService.GetMetaTag(SeoMetaTagType.Unauthorized);

        //    Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        //    return View();
        //}
    }
}
