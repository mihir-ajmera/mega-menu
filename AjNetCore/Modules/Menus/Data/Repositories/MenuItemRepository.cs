using AjNetCore.Modules.Core.Data;
using AjNetCore.Modules.Menus.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace AjNetCore.Modules.Menus.Data.Repositories
{
    public interface IMenuItemRepository : IRepository<MenuItem>, INestedSet
    {
    }

    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly DbSet<MenuItem> _dbSet;
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemRepository(IDatabaseFactory databaseFactory, IUnitOfWork unitOfWork) : base(databaseFactory)
        {
            _dbSet = databaseFactory.Get().Set<MenuItem>();
            _unitOfWork = unitOfWork;
        }

        public new void Insert(MenuItem entity)
        {
            var query = AsNoTracking.Where(w => w.MenuId == entity.MenuId);

            // When no parent selected
            if (entity.ParentId == null)
            {
                var maxNestedSet = query.Max(c => c.Right) ?? 0;
                entity.Left = maxNestedSet + 1;
                entity.Right = maxNestedSet + 2;
                entity.Depth = 0;
            }
            else
            {
                var parentNode = query.First(w => w.Id == entity.ParentId);
                var valNode = (parentNode.Left + 1 == parentNode.Right)
                    ? parentNode.Left
                    : parentNode.Right - 1;

                var rightNodes = _dbSet.Where(f => f.MenuId == entity.MenuId && (f.Right > valNode)).ToList();
                rightNodes.ForEach(c => c.Right = c.Right + 2);

                var leftNodes = _dbSet.Where(f => f.MenuId == entity.MenuId && (f.Left > valNode)).ToList();
                leftNodes.ForEach(c => c.Left = c.Left + 2);

                entity.Left = valNode + 1;
                entity.Right = valNode + 2;
                entity.Depth = parentNode.Depth + 1;
            }

            _dbSet.Add(entity);
            _unitOfWork.Commit();
        }

        public new void Delete(MenuItem entity)
        {
            this.DeleteNode("MenuItems", entity.Id, $"MenuId = {entity.MenuId}");
        }
    }
}