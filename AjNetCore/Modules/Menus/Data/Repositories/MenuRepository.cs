using AjNetCore.Modules.Core.Data;
using AjNetCore.Modules.Menus.Models;

namespace AjNetCore.Modules.Menus.Data.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
    }

    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
}