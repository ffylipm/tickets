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

        public async Task<IEnumerable<TicketDTO>> GetTickets(int? ticketId, int? eventId)
        {
            var query = context.Tickets
                .Include(t => t.Event)
                .ThenInclude(e => e.Place)
                .Where(t => t.Active);

            if (ticketId.HasValue)
            {
                query = query.Where(t => t.TicketId == ticketId);
            }

            if (eventId.HasValue)
            {
                query = query.Where(t => t.EventId == eventId);
            }

            return await query.Select(t => new TicketDTO
            {
                EventId = t.EventId,
                TicketId = t.TicketId,
                Used = t.Used,
                IssueOn = t.IssueOn,
                UserId = t.UserId,
                Active = t.Active
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
            Ticket ticket = await GetTicket(upd.TicketId);
            ticket.Used = upd.Used;

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
