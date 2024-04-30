using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class RolMenu
{
    public string MenuId { get; set; } = null!;

    public string RolId { get; set; } = null!;

    public bool Active { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual Rol Rol { get; set; } = null!;
}
