using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class RolDTO
{
    public string? RolId { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public bool Active { get; set; }

    public bool Virtual { get; set; }

    public virtual IEnumerable<MenuDTO> Menus { get; set; } = new List<MenuDTO>();
}
