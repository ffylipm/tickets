using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class EventDTO
{
    public int EventId { get; set; }

    public string? Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public bool Active { get; set; }

    public int PlaceId { get; set; }

    public int MinTicketQty { get; set; }

    public int MaxTicketQty { get; set; }

    public virtual PlaceDTO? Place { get; set; }

    public virtual ICollection<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
}
