using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class UserRolDTO
{
    public string UserId { get; set; } = null!;

    public string RolId { get; set; } = null!;

    public bool Active { get; set; }

    public virtual RolDTO Rol { get; set; } = null!;
}
