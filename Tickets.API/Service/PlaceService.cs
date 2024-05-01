using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tickets.API.Common;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class PlaceService
    {
        private readonly TicketsContext context;
        public PlaceService(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PlaceDTO>> GetPlaces(int? placeId, string? nameFull, string? nameShort)
        {
            var query = context.Places.Where(r => r.Active);

            if (placeId.HasValue)
            {
                //query = query.Where(m => m.PlaceId == placeId.Value);
            }

            if (!string.IsNullOrEmpty(nameFull))
            {
                query = query.Where(m => m.NameFull.Contains(nameFull));
            }

            if (!string.IsNullOrEmpty(nameShort))
            {
                query = query.Where(m => m.NameShort.Contains(nameShort));
            }

            return await query.Select(r => new PlaceDTO
            {
                Active = r.Active,
                PlaceId = r.PlaceId,
                Address = r.Address,
                NameShort = r.NameShort,
                NameFull = r.NameFull,
                Capacity = r.Capacity
            }).ToListAsync();
        }

        public async Task<Event> GetEvent(int eventId, bool? active = true)
        {
            Event? e = await context.Events.FirstOrDefaultAsync(m => m.EventId == eventId);
            if (e == null)
            {
                throw new CustomException("No existe el evento.");
            }

            if (active.HasValue && active.Value && !e.Active)
            {
                throw new CustomException("El evento está inactivo.");
            }

            return e;
        }


        public async Task<EventDTO> AddEvent(EventDTO add)
        {
            Event e = new()
            {
                Active = add.Active,
                Description = add.Description,
                Name = add.Name,
                EventId = add.EventId,
                MaxTicketQty = add.MaxTicketQty,
                MinTicketQty = add.MinTicketQty,
                PlaceId = add.PlaceId
            };

            //context.Events.(e);
            await context.SaveChangesAsync();

            return add;
        }

        public async Task<EventDTO> UpdEvent(EventDTO upd)
        {
            Event e = await GetEvent(upd.EventId);
            e.Name = upd.Name;
            e.Description = upd.Description;
            e.MinTicketQty = upd.MinTicketQty;
            e.MaxTicketQty = upd.MaxTicketQty;
            e.PlaceId = upd.PlaceId;

            context.Events.Update(e);
            await context.SaveChangesAsync();

            return upd;
        }

        public async Task<EventDTO> DelEvent(EventDTO del)
        {
            Event e = await GetEvent(del.EventId, active: false);
            e.Active = false;

            context.Events.Update(e);
            await context.SaveChangesAsync();

            return del;
        }
    }
}
