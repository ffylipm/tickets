using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tickets.API.Models;
using Tickets.API.Service;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ServicesSvc service;

        public ServiceController(ServicesSvc service)
        {
            this.service = service;
        }

        [HttpGet("", Name = nameof(GetServices))]
        public async Task<IEnumerable<ServiceDTO>> GetServices(
            [FromQuery] int? serviceId,
            [FromQuery] string? name,
            [FromQuery] string? description
            )
        {
            return await service.GetServices(serviceId, name, description);
        }

        [HttpPost("", Name = nameof(AddService))]
        public async Task<ServiceDTO> AddService(ServiceDTO add)
        {
            return await service.AddService(add);
        }

        [HttpPut("", Name = nameof(UpdService))]
        public async Task<ServiceDTO> UpdService(ServiceDTO upd)
        {
            return await service.UpdService(upd);
        }

        [HttpDelete("", Name = nameof(DelService))]
        public async Task<ServiceDTO> DelService(ServiceDTO del)
        {
            return await service.DelService(del);
        }
    }
}
