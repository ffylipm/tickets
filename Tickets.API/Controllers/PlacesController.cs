using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tickets.API.Service;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly PlaceService service;

        public PlacesController(PlaceService service)
        {
            this.service = service;
        }

        [HttpGet("", Name = nameof(GetPlaces))]
        public async Task<IEnumerable<PlaceDTO>> GetPlaces(
            [FromQuery] int? placeId,
            [FromQuery] string? nameFull,
            [FromQuery] string? nameShort,
            [FromQuery] string? address
            )
        {
            return await service.GetPlaces(placeId, nameFull, nameShort, address);
        }

        [HttpPost("", Name = nameof(AddPlace))]
        public async Task<PlaceDTO> AddPlace(PlaceDTO add)
        {
            return await service.AddPlace(add);
        }

        [HttpPut("", Name = nameof(UpdPlace))]
        public async Task<PlaceDTO> UpdPlace(PlaceDTO upd)
        {
            return await service.UpdPlace(upd);
        }

        [HttpDelete("", Name = nameof(DelPlace))]
        public async Task<PlaceDTO> DelPlace(PlaceDTO del)
        {
            return await service.DelPlace(del);
        }
    }
}
