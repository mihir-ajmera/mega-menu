using System.Collections.Generic;
using System.Linq;
using AjNetCore.Modules.Core.Helpers;
using AjNetCore.Modules.Core.Modules;
using Microsoft.AspNetCore.Mvc.Razor;

namespace AjNetCore.Modules.Core.ViewEngine
{
    public class AjViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        { }

        public virtual IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return LoadModuleViews();
        }

        private static IEnumerable<string> LoadModuleViews()
        {
            var modules = ObjectHelper.GetEnumerableOfType<BaseModule>(null)
                .Where(w => w.HasViews)
                .ToArray();

            var locationFormats = new List<string>();

            locationFormats.AddRange(
                modules.Select(module => $"~/Modules/{module.ModuleName}/Views/{{1}}/{{0}}.cshtml").ToList());

            locationFormats.AddRange(
                modules.Select(module => $"~/Modules/{module.ModuleName}/Views/Shared/{{0}}.cshtml").ToList());

            return locationFormats;
        }
    }
}