namespace AjNetCore.Modules.Core.Data
{
    public interface IMetaTag
    {
        string MetaTitle { get; set; }
        string MetaDescription { get; set; }
        string MetaExtraTags { get; set; }
    }
}