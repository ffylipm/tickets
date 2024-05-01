using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class TicketService
{
    public int TicketId { get; set; }

    public int ServiceId { get; set; }

    public bool Active { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
