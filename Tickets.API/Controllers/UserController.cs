using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tickets.API.Service;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService service;

        public UserController(UserService service)
        {
            this.service = service;
        }

        [HttpGet(Name = "")]
        public async Task<IEnumerable<UserDTO>> GetUsers(
            [FromQuery][Required] int skip,
            [FromQuery][Required] int take,
            [FromQuery] string? name,
            [FromQuery] string? lastname,
            [FromQuery] string? phone,
            [FromQuery] string? document,
            [FromQuery] string? documentType,
            [FromQuery] string? userId,
            [FromQuery] bool? active
            )
        {
            return await service.GetUsers(skip, take, name, lastname, phone, document, documentType, userId, active);
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
