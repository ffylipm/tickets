using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class TicketDTO
{
    public int TicketId { get; set; }

    public int EventId { get; set; }

    public string? UserId { get; set; }

    public bool Used { get; set; }
    public bool Active { get; set; }
    public DateTime IssueOn { get; set; }
    public EventDTO? Event { get; set; }
}
