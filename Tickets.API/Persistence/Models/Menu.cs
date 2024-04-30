using System;
using System.Collections.Generic;

namespace Tickets.Persistence;

public partial class Menu
{
    public string MenuId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Active { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<RolMenu> RolMenus { get; set; } = new List<RolMenu>();
}
