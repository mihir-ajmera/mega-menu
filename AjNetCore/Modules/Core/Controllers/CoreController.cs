using AjNetCore.Modules.Core.Data;
using AjNetCore.Modules.Core.Data.Seed;
using AjNetCore.Modules.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AjNetCore.Modules.Core.Controllers
{
    public class CoreController : Controller
    {
        private SqlContext _dataContext;
        protected IDatabaseFactory DatabaseFactory { get; }
        protected SqlContext DataContext => _dataContext ??= DatabaseFactory.Get();

        public CoreController(IDatabaseFactory databaseFactory
            )
        {
            DatabaseFactory = databaseFactory;
        }

        public ActionResult Seed()
        {
            ObjectHelper.GetEnumerableOfType<BaseSeed>(DataContext)
                .ForEach(seedClass => seedClass.Seed());

            return Content("Done");
        }
    }
}
