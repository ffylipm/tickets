using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tickets.API.Service;
using Tickets.Models;
using Tickets.Persistence;
using TicketsService = Tickets.API.Service.TicketsService;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketsService service;

        public TicketController(TicketsService service)
        {
            this.service = service;
        }

        [HttpGet("", Name = nameof(GetTickets))]
        public async Task<IEnumerable<TicketDTO>> GetTickets(
            [FromQuery] int? ticketId,
            [FromQuery] int? eventId,
            [FromQuery] bool? active
            )
        {
            return await service.GetTickets(ticketId, eventId, active);
        }

        [HttpPost("", Name = nameof(AddTicket))]
        public async Task<TicketDTO> AddTicket(TicketDTO add)
        {
            return await service.AddTicket(add);
        }

        [HttpPut("", Name = nameof(UpdTicket))]
        public async Task<TicketDTO> UpdTicket(TicketDTO upd)
        {
            return await service.UpdTicket(upd);
        }

        [HttpDelete("", Name = nameof(DelTicket))]
        public async Task<TicketDTO> DelTicket(TicketDTO del)
        {
            return await service.DelTicket(del);
        }
    }
}
