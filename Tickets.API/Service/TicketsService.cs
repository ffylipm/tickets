using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Tickets.API.Common;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class TicketsService
    {
        private readonly TicketsContext context;
        public TicketsService(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TicketDTO>> GetTickets(int? ticketId, int? eventId, bool? active)
        {
            var query = context.Tickets
                .Include(t => t.Event)
                .ThenInclude(e => e.Place)
                .Select(t => t);

            if (ticketId.HasValue)
            {
                query = query.Where(t => t.TicketId == ticketId);
            }

            if (eventId.HasValue)
            {
                query = query.Where(t => t.EventId == eventId);
            }

            if (active.HasValue)
            {
                query = query.Where(t => t.Active == active.Value);
            }

            return await query.Select(t => new TicketDTO
            {
                EventId = t.EventId,
                Active = t.Active,
                IssueOn = t.IssueOn,
                TicketId = t.TicketId,
                Used = t.Used,
                UserId = t.UserId,
                Event = t.Event == null ? null : new EventDTO()
                {
                    Active = t.Event.Active,
                    Description = t.Event.Description,
                    Name = t.Event.Name,
                    EventId = t.Event.EventId,
                    MaxTicketQty = t.Event.MaxTicketQty,
                    MinTicketQty = t.Event.MinTicketQty,
                    PlaceId = t.Event.PlaceId,
                    Place = t.Event.Place == null ? null : new PlaceDTO()
                    {
                        Active = t.Event.Place.Active,
                        PlaceId = t.Event.Place.PlaceId,
                        Address = t.Event.Place.Address,
                        NameShort = t.Event.Place.NameShort,
                        NameFull = t.Event.Place.NameFull,
                        Capacity = t.Event.Place.Capacity
                    }
                }
            }).ToListAsync();
        }

        public async Task<Ticket> GetTicket(int ticketId, bool? active = true)
        {
            Ticket? ticket = await context.Tickets
                .Include(t => t.Event)
                .ThenInclude(e => e.Place)
                .FirstOrDefaultAsync(m => m.TicketId == ticketId);

            if (ticket == null)
            {
                throw new CustomException("No existe el ticket.");
            }

            if (active.HasValue && active.Value && !ticket.Active)
            {
                throw new CustomException("El ticket está inactivo.");
            }

            return ticket;
        }

        public async Task<TicketDTO> AddTicket(TicketDTO add)
        {
            using (var tx = await context.Database.BeginTransactionAsync())
            {
                Ticket ticket = new Ticket()
                {
                    Active = true,
                    IssueOn = DateTime.Now,
                    Used = false,
                    UserId = add.UserId,
                    EventId = add.EventId
                };

                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();
                add.TicketId = ticket.TicketId;
                add.Active = true;

                await tx.CommitAsync();
            }

            return add;
        }

        public async Task<TicketDTO> UpdTicket(TicketDTO upd)
        {
            Ticket ticket = await GetTicket(upd.TicketId, active: false);
            ticket.Used = upd.Used;

            if (upd.Active == !ticket.Active)
            {
                ticket.Active = upd.Active;
            }

            context.Tickets.Update(ticket);
            await context.SaveChangesAsync();

            return upd;
        }

        public async Task<TicketDTO> DelTicket(TicketDTO del)
        {
            Ticket ticket = await GetTicket(del.TicketId, active: false);
            ticket.Active = false;

            context.Tickets.Update(ticket);
            await context.SaveChangesAsync();

            return del;
        }
    }
}
