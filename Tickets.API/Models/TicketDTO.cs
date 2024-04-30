using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class TicketDTO
{
    public int TicketId { get; set; }

    public int EventId { get; set; }

    public string UserId { get; set; } = null!;

    public bool Used { get; set; }

    public DateTime IssueOn { get; set; }

    public virtual EventDTO Event { get; set; } = null!;

    public virtual UserDTO User { get; set; } = null!;
}
