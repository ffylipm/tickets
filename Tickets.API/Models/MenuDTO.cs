using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class MenuDTO
{
    public string MenuId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Active { get; set; }

    public string Path { get; set; } = null!;
}
