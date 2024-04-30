using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tickets.API.Service;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly MenuService service;

        public MenuController(MenuService service)
        {
            this.service = service;
        }

        [HttpGet(Name = "")]
        public async Task<IEnumerable<MenuDTO>> GetMenus(
            [FromQuery] string? menuId,
            [FromQuery] string? name,
            [FromQuery] string? description,
            [FromQuery] string? path)
        {
            return await service.GetMenus(menuId, name, description, path);
        }

        [HttpPost(Name = "")]
        public async Task<MenuDTO> AddMenu(MenuDTO add)
        {
            return await service.AddMenu(add);
        }

        [HttpPut(Name = "")]
        public async Task<MenuDTO> UpdMenu(MenuDTO add)
        {
            return await service.UpdMenu(add);
        }

        [HttpDelete(Name = "")]
        public async Task<MenuDTO> DelMenu(MenuDTO add)
        {
            return await service.DelMenu(add);
        }
    }
}
