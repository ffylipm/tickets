using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tickets.API.Service;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolController : ControllerBase
    {
        private readonly RolService service;

        public RolController(RolService service)
        {
            this.service = service;
        }

        [HttpGet("", Name = nameof(GetRols))]
        public async Task<IEnumerable<RolDTO>> GetRols(
            [FromQuery] string? rolId,
            [FromQuery] string? name,
            [FromQuery] string? description
            )
        {
            return await service.GetRols(rolId, name, description);
        }

        [HttpPost("", Name = nameof(AddRol))]
        public async Task<RolDTO> AddRol(RolDTO add)
        {
            return await service.AddRol(add);
        }

        [HttpPut("", Name = nameof(UpdRol))]
        public async Task<RolDTO> UpdRol(RolDTO upd)
        {
            return await service.UpdRol(upd);
        }

        [HttpDelete("", Name = nameof(DelRol))]
        public async Task<RolDTO> DelRol(RolDTO del)
        {
            return await service.DelRol(del);
        }
    }
}
