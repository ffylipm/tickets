using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class Event
{
    public int EventId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Active { get; set; }

    public string? PlaceId { get; set; }

    public int MinTicketQty { get; set; }

    public int MaxTicketQty { get; set; }

    public virtual Place? Place { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
