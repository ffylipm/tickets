using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class Rol
{
    public string RolId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Active { get; set; }

    public bool Virtual { get; set; }

    public virtual ICollection<RolMenu> RolMenus { get; set; } = new List<RolMenu>();

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
