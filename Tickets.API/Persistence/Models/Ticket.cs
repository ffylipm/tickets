﻿using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int EventId { get; set; }

    public string UserId { get; set; } = null!;

    public bool Used { get; set; }

    public DateTime IssueOn { get; set; }

    public bool Active { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<TicketService> TicketServices { get; set; } = new List<TicketService>();

    public virtual User User { get; set; } = null!;
}
