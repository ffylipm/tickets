using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tickets.API.Common;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class EventService
    {
        private readonly TicketsContext context;
        public EventService(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<EventDTO>> GetEvents(int? eventId, string? name, string? description)
        {
            var query = context.Events.Where(r => r.Active);

            if (eventId.HasValue)
            {
                query = query.Where(m => m.EventId == eventId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(m => m.Description.Contains(description));
            }

            return await query.Select(r => new EventDTO
            {
                Active = r.Active,
                Description = r.Description,
                Name = r.Name,
                EventId = r.EventId,
                MaxTicketQty = r.MaxTicketQty,
                MinTicketQty = r.MinTicketQty,
                PlaceId = r.PlaceId
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

            context.Events.Add(e);
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
