using Microsoft.EntityFrameworkCore;
using Tickets.API.Common;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class MenuService
    {
        private readonly TicketsContext context;
        public MenuService(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MenuDTO>> GetMenus(string? menuId, string? name, string? description, string? path)
        {
            var query = context.Menus.Select(m => m);

            if (!string.IsNullOrEmpty(menuId))
            {
                query = query.Where(m => m.MenuId.Contains(menuId));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(m => m.Description.Contains(description));
            }

            if (!string.IsNullOrEmpty(path))
            {
                query = query.Where(m => m.Path.Contains(path));
            }

            return await query.Select(m => new MenuDTO 
            {
                Path = m.Path,
                Description = m.Description,
                Active = m.Active,
                MenuId = m.MenuId,
                Name = m.Name
            }).ToListAsync();
        }

        public async Task<Menu> GetMenu(string menuId, bool? active = true)
        {
            Menu? menu = await context.Menus.FirstOrDefaultAsync(m => m.MenuId == menuId);
            if (menu == null)
            {
                throw new CustomException("No existe el menu.");
            }

            if (active.HasValue && active.Value && !menu.Active)
            {
                throw new CustomException("El menu está inactivo.");
            }

            return menu;
        }


        public async Task<MenuDTO> AddMenu(MenuDTO add)
        {
            Menu menu  = new Menu() 
            {
                Active = true,
                Path = add.Path,
                Description = add.Description,
                Name = add.Name
            };

            context.Menus.Add(menu);
            await context.SaveChangesAsync();

            return add;
        }

        public async Task<MenuDTO> UpdMenu(MenuDTO upd)
        {
            Menu menu = await GetMenu(upd.MenuId);
            menu.Name = upd.Name;
            menu.Description = upd.Description;
            menu.Path = upd.Path;

            context.Menus.Update(menu);
            await context.SaveChangesAsync();

            return upd;
        }

        public async Task<MenuDTO> DelMenu(MenuDTO del)
        {
            Menu menu = await GetMenu(del.MenuId, active: false);
            menu.Active = false;

            context.Menus.Update(menu);
            await context.SaveChangesAsync();

            return del;
        }
    }
}
