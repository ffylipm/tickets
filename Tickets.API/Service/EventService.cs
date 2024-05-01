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

        public async Task<IEnumerable<EventDTO>> GetEvents(int? eventId, string? name, string? description, bool? active)
        {
            var query = context.Events.Include(e => e.Place).Select(r => r);

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

            if (active.HasValue)
            {
                query = query.Where(e => e.Active == active.Value);
            }

            return await query.Select(r => new EventDTO
            {
                Active = r.Active,
                Description = r.Description,
                Name = r.Name,
                EventId = r.EventId,
                MaxTicketQty = r.MaxTicketQty,
                MinTicketQty = r.MinTicketQty,
                PlaceId = r.PlaceId,
                Place = r.Place == null ? null : new PlaceDTO() 
                {
                    Active = r.Place.Active,
                    PlaceId = r.Place.PlaceId,
                    Address = r.Place.Address,
                    NameShort = r.Place.NameShort,
                    NameFull = r.Place.NameFull,
                    Capacity = r.Place.Capacity
                }
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
                Active = true,
                Description = add.Description,
                Name = add.Name,
                EventId = add.EventId,
                MaxTicketQty = add.MaxTicketQty,
                MinTicketQty = add.MinTicketQty,
                PlaceId = add.PlaceId
            };

            context.Events.Add(e);
            await context.SaveChangesAsync();

            add.EventId = e.EventId;

            return add;
        }

        public async Task<EventDTO> UpdEvent(EventDTO upd)
        {
            Event e = await GetEvent(upd.EventId, active: false);
            e.Name = upd.Name;
            e.Description = upd.Description;
            e.MinTicketQty = upd.MinTicketQty;
            e.MaxTicketQty = upd.MaxTicketQty;
            e.PlaceId = upd.PlaceId;

            if (upd.Active && !e.Active)
            {
                e.Active = upd.Active;
            }

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
