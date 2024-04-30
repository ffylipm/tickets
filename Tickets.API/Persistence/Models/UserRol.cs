using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class UserRol
{
    public string UserId { get; set; } = null!;

    public string RolId { get; set; } = null!;

    public bool Active { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
