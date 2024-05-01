using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class Service
{
    public int ServiceId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<TicketService> TicketServices { get; set; } = new List<TicketService>();
}
