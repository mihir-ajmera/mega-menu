using Microsoft.EntityFrameworkCore;

namespace AjNetCore.Modules.Core.Data
{
    public interface ISeedConfiguration
    {
        void Map(ModelBuilder builder);
    }
}