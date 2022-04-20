using AjNetCore.Modules.Core;
using AjNetCore.Modules.Core.Content;
using AjNetCore.Modules.Core.Data;
using AjNetCore.Modules.Menus.CacheManagers;
using AjNetCore.Modules.Menus.Data.Repositories;
using AjNetCore.Modules.Menus.Models;
using AjNetCore.Modules.Menus.Models.DTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace AjNetCore.Modules.Menus.Services
{
    public interface IMenuItemService
    {
        List<MenuItemFrontDto> GetAllMenuItems(int menuId);
        List<MenuListDto> GetAllRootMenuItems(int menuId);
        List<MenuListDto> GetAllLeafMenuItems(int menuId);
        List<MenuItemFrontDto> GetMenuItemsByName(int menuId, string menuItemName);

        Result Create(MenuItemCreateDto dto);
        Result Delete(int menuId, int menuItemId);
    }

    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuItemService(
            IMenuItemRepository menuItemRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<MenuItemFrontDto> GetAllMenuItems(int menuId)
        {
            var items = _menuItemRepository.AsNoTracking
                .Where(w => w.MenuId == menuId)
                .OrderBy(o => o.Left)
                .Select(s => new MenuItemFrontDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    ParentId = s.ParentId,
                    Url = s.Url,
                    IsOpenNewTab = s.IsOpenNewTab
                })
                .ToList();

            var menuItemFrontDtos = items.Any() ? BuildMenuItemTree(items) : new List<MenuItemFrontDto>();

            return menuItemFrontDtos;
        }

        private List<MenuItemFrontDto> BuildMenuItemTree(List<MenuItemFrontDto> source)
        {
            var groups = source.GroupBy(i => i.ParentId);

            var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

            if (roots.Count <= 0) return roots;
            {
                var dict = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                foreach (var t in roots)
                    AddChildren(t, dict);
            }

            return roots;
        }

        private static void AddChildren(MenuItemFrontDto node, IDictionary<int, List<MenuItemFrontDto>> source)
        {
            if (source.ContainsKey(node.Id))
            {
                node.Childrens = source[node.Id];

                foreach (var t in node.Childrens)
                    AddChildren(t, source);
            }
            else
            {
                node.Childrens = new List<MenuItemFrontDto>();
            }
        }

        public List<MenuListDto> GetAllRootMenuItems(int menuId)
        {
            var items = _menuItemRepository.AsNoTracking
                .Where(w => w.MenuId == menuId && w.ParentId == null)
                .OrderBy(o => o.Left)
                .Select(s => new MenuListDto()
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return items;
        }

        public List<MenuListDto> GetAllLeafMenuItems(int menuId)
        {
            var items = _menuItemRepository.AsNoTracking
                .Where(w => w.MenuId == menuId && w.Children.Count() == 0)
                .OrderBy(o => o.Left)
                .Select(s => new MenuListDto()
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return items;
        }

        public List<MenuItemFrontDto> GetMenuItemsByName(int menuId, string menuItemName)
        {
            var items = _menuItemRepository.AsNoTracking
                .Where(w => w.MenuId == menuId && w.Name.Contains(menuItemName) && w.ParentId != null)
                .OrderBy(o => o.Left)
                .Select(s => new MenuItemFrontDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    ParentId = s.ParentId,
                    Url = s.Url,
                    IsOpenNewTab = s.IsOpenNewTab
                })
                .ToList();

            var menuItemFrontDtos = items.Any() ? BuildMenuItemTree(items) : new List<MenuItemFrontDto>();

            return menuItemFrontDtos;
        }

        public Result Create(MenuItemCreateDto dto)
        {
            //dto.MenuId = menuId;

            var entity = _mapper.Map<MenuItem>(dto);
            _menuItemRepository.Insert(entity);

            _unitOfWork.Commit();
            ClearCache();

            var result = new Result
            {
                Id = entity.Id
            };

            return result.SetSuccess(Messages.RecordSaved);
        }

        public Result Delete(int menuId, int menuItemId)
        {
            var result = new Result();

            var query = _menuItemRepository.AsNoTracking.Where(q => (q.MenuId == menuId) && (q.Id == menuItemId));
            result = new Result().SetSuccess(Messages.RecordDelete, query.Count());

            var list = query.ToList();

            foreach (var item in list)
                _menuItemRepository.Delete(item);

            _unitOfWork.Commit();
            ClearCache();

            return result;
        }

        private static void ClearCache()
        {
            MenuCacheManager.ClearCache();
        }

    }
}