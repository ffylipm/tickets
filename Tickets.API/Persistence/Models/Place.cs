using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class Place
{
    public int PlaceId { get; set; }

    public string NameShort { get; set; } = null!;

    public string NameFull { get; set; } = null!;

    public bool Active { get; set; }

    public int Capacity { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
