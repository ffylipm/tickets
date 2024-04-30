using Microsoft.EntityFrameworkCore;
using Tickets.API.Common;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class RolService
    {
        private readonly TicketsContext context;
        public RolService(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RolDTO>> GetRols(string? rolId, string? name, string? description)
        {
            var query = context.Rols
                .Include(r => r.RolMenus)
                .ThenInclude(r => r.Menu)
                .Select(m => m);

            if (!string.IsNullOrEmpty(rolId))
            {
                query = query.Where(m => m.RolId.Contains(rolId));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(m => m.Description.Contains(description));
            }

            return await query.Select(r => new RolDTO
            {
                RolId = r.RolId,
                Description = r.Description,
                Active = r.Active,
                Virtual = r.Virtual,
                Name = r.Name,
                Menus = r.RolMenus.Select(m => new MenuDTO() 
                {
                    Active = m.Active,
                    Description = m.Menu.Description,
                    MenuId = m.Menu.MenuId,
                    Name = m.Menu.Name,
                    Path = m.Menu.Path
                })
            }).ToListAsync();
        }

        public async Task<Rol> GetRol(string rolId, bool? active = true)
        {
            Rol? rol = await context.Rols
                .Include(r => r.RolMenus)
                .ThenInclude(r => r.Menu)
                .FirstOrDefaultAsync(m => m.RolId == rolId);
            if (rol == null)
            {
                throw new CustomException("No existe el rol.");
            }

            if (active.HasValue && active.Value && !rol.Active)
            {
                throw new CustomException("El rol está inactivo.");
            }

            return rol;
        }


        public async Task<RolDTO> AddRol(RolDTO add)
        {
            Rol rol = new ()
            {
                Active = true,
                Name = add.Name,
                Description = add.Description,
                Virtual = add.Virtual
            };

            context.Rols.Add(rol);
            await context.SaveChangesAsync();

            return add;
        }

        public async Task<RolDTO> UpdRol(RolDTO upd)
        {
            Rol rol = await GetRol(upd.RolId);
            rol.Name = upd.Name;
            rol.Description = upd.Description;

            context.Rols.Update(rol);
            await context.SaveChangesAsync();

            return upd;
        }

        public async Task<RolDTO> DelRol(RolDTO del)
        {
            Rol rol = await GetRol(del.RolId, active: false);
            rol.Active = false;

            context.Rols.Update(rol);
            await context.SaveChangesAsync();

            return del;
        }
    }
}
