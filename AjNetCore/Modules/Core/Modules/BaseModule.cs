using System;
using Microsoft.AspNetCore.Routing;

namespace AjNetCore.Modules.Core.Modules
{
    public abstract class BaseModule : IComparable<BaseModule>
    {
        public string ModuleName { get; set; }
        public int OrderId { get; set; }

        public bool HasViews { get; set; } = false;

        public abstract void RegisterRoutes(IEndpointRouteBuilder endpoint);

        public virtual void RegisterBackgroundJobs()
        {

        }

        public int CompareTo(BaseModule other)
        {
            return OrderId.CompareTo(other.OrderId);
        }
    }
}