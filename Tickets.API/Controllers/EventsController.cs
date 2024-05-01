using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tickets.API.Service;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventService service;

        public EventsController(EventService service)
        {
            this.service = service;
        }

        [HttpGet("", Name = nameof(GetEvents))]
        public async Task<IEnumerable<EventDTO>> GetEvents(
            [FromQuery] int? eventId,
            [FromQuery] string? name,
            [FromQuery] string? description,
            [FromQuery] bool? active
            )
        {
            return await service.GetEvents(eventId, name, description, active);
        }

        [HttpPost("", Name = nameof(AddEvent))]
        public async Task<EventDTO> AddEvent(EventDTO add)
        {
            return await service.AddEvent(add);
        }

        [HttpPut("", Name = nameof(UpdEvent))]
        public async Task<EventDTO> UpdEvent(EventDTO upd)
        {
            return await service.UpdEvent(upd);
        }

        [HttpDelete("", Name = nameof(DelEvent))]
        public async Task<EventDTO> DelEvent(EventDTO del)
        {
            return await service.DelEvent(del);
        }
    }
}
