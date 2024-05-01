using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tickets.API.Common;
using Tickets.API.Models;
using Tickets.Models;
using Tickets.Persistence;
using EService = Tickets.Persistence.Service;

namespace Tickets.API.Service
{
    public class ServicesSvc
    {
        private readonly TicketsContext context;
        public ServicesSvc(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ServiceDTO>> GetServices(int? serviceId, string? name, string? description, bool? active)
        {
            var query = context.Services.Select(r => r);

            if (serviceId.HasValue)
            {
                query = query.Where(m => m.ServiceId == serviceId.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(m => m.Description.Contains(description));
            }

            if (active.HasValue)
            {
                query = query.Where(s => s.Active == active.Value);
            }

            return await query.Select(r => new ServiceDTO
            {
                ServiceId = r.ServiceId,
                Name = r.Name,
                Description = r.Description,
                Active = r.Active
            }).ToListAsync();
        }

        public async Task<EService> GetService(int serviceId, bool? active = true)
        {
            EService? rol = await context.Services.FirstOrDefaultAsync(m => m.ServiceId == serviceId);
            if (rol == null)
            {
                throw new CustomException("No existe el servicio.");
            }

            if (active.HasValue && active.Value && !rol.Active)
            {
                throw new CustomException("El servicio está inactivo.");
            }

            return rol;
        }


        public async Task<ServiceDTO> AddService(ServiceDTO add)
        {
            EService service = new()
            {
                Active = true,
                Name = add.Name,
                Description = add.Description
            };

            context.Services.Add(service);
            await context.SaveChangesAsync();

            add.ServiceId = service.ServiceId;

            return add;
        }

        public async Task<ServiceDTO> UpdService(ServiceDTO upd)
        {
            EService service = await GetService(upd.ServiceId, active: false);
            service.Name = upd.Name;
            service.Description = upd.Description;

            if (upd.Active && !service.Active)
            {
                service.Active = upd.Active;
            }

            context.Services.Update(service);
            await context.SaveChangesAsync();

            return upd;
        }

        public async Task<ServiceDTO> DelService(ServiceDTO del)
        {
            EService service = await GetService(del.ServiceId, active: false);
            service.Active = false;

            context.Services.Update(service);
            await context.SaveChangesAsync();

            return del;
        }
    }
}
