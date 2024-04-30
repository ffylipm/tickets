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

        [HttpGet(Name = "")]
        public async Task<IEnumerable<RolDTO>> GetRols(
            [FromQuery] string? rolId,
            [FromQuery] string? name,
            [FromQuery] string? description
            )
        {
            return await service.GetRols(rolId, name, description);
        }

        [HttpPost(Name = "")]
        public async Task<UserDTO> AddUser(UserDTO add)
        {
            return await service.AddUser(add);
        }

        [HttpPut(Name = "")]
        public async Task<UserDTO> UpdUser(UserDTO add)
        {
            return await service.UpdUser(add);
        }

        [HttpDelete(Name = "")]
        public async Task<UserDTO> DelUser(UserDTO add)
        {
            return await service.DelUser(add);
        }
    }
}
