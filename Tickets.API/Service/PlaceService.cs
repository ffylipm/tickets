using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tickets.API.Common;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class PlaceService
    {
        private readonly TicketsContext context;
        public PlaceService(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PlaceDTO>> GetPlaces(int? placeId, string? nameFull, string? nameShort, string? address)
        {
            var query = context.Places.Where(r => r.Active);

            if (placeId.HasValue)
            {
                query = query.Where(m => m.PlaceId == placeId.Value);
            }

            if (!string.IsNullOrEmpty(nameFull))
            {
                query = query.Where(m => m.NameFull.Contains(nameFull));
            }

            if (!string.IsNullOrEmpty(nameShort))
            {
                query = query.Where(m => m.NameShort.Contains(nameShort));
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(m => m.Address !=null && m.Address.Contains(address));
            }

            return await query.Select(r => new PlaceDTO
            {
                Active = r.Active,
                PlaceId = r.PlaceId,
                Address = r.Address,
                NameShort = r.NameShort,
                NameFull = r.NameFull,
                Capacity = r.Capacity
            }).ToListAsync();
        }

        public async Task<Place> GetPlace(int placeId, bool? active = true)
        {
            Place? place = await context.Places.FirstOrDefaultAsync(m => m.PlaceId == placeId);
            if (place == null)
            {
                throw new CustomException("No existe el lugar.");
            }

            if (active.HasValue && active.Value && !place.Active)
            {
                throw new CustomException("El lugar está inactivo.");
            }

            return place;
        }


        public async Task<PlaceDTO> AddPlace(PlaceDTO add)
        {
            Place place = new()
            {
                Active = true,
                Address = add.Address,
                Capacity = add.Capacity,
                NameFull = add.NameFull,
                NameShort = add.NameShort
            };

            context.Places.Add(place);
            await context.SaveChangesAsync();
            add.PlaceId = place.PlaceId;

            return add;
        }

        public async Task<PlaceDTO> UpdPlace(PlaceDTO upd)
        {
            Place place = await GetPlace(upd.PlaceId);
            place.Capacity = upd.Capacity;
            place.Address = upd.Address;
            place.NameShort = upd.NameShort;
            place.NameFull = upd.NameFull;

            context.Places.Update(place);
            await context.SaveChangesAsync();

            return upd;
        }

        public async Task<PlaceDTO> DelPlace(PlaceDTO del)
        {
            Place place = await GetPlace(del.PlaceId, active: false);
            place.Active = false;

            context.Places.Update(place);
            await context.SaveChangesAsync();

            return del;
        }
    }
}
