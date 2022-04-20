using AjNetCore.Modules.CurrentProject.Helpers;

namespace AjNetCore.Modules.Frontend.Services
{
    public interface IFrontCommonService
    {
        SiteSetting SiteSetting { get; set; }
    }

    public class FrontCommonService : IFrontCommonService
    {
        public SiteSetting SiteSetting { get; set; }

        public FrontCommonService(
            AjConfiguration configuration)
        {
            SiteSetting = configuration.SiteSetting;
        }
    }
}