namespace AjNetCore.Modules.CurrentProject.Helpers
{
    public class AjConfiguration
    {
        public SiteSetting SiteSetting { get; set; }
    }

    public class SiteSetting
    {
        public string SiteTitle { get; set; }
        public string WebsiteUrl { get; set; }
        public string Environment { get; set; }
    }
}