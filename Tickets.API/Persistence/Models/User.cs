using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Lastname { get; set; }

    public string? Phone { get; set; }

    public string Document { get; set; } = null!;

    public string DocumentType { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
