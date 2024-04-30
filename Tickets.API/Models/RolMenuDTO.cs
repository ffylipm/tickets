using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class RolMenuDTO
{
    public string MenuId { get; set; } = null!;

    public string RolId { get; set; } = null!;

    public bool Active { get; set; }

    public MenuDTO? Menu { get; set; }
}
