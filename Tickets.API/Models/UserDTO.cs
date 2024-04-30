using System;
using System.Collections.Generic;

namespace Tickets.Models;

public partial class UserDTO
{
    public string? UserId { get; set; } = null!;

    public string? Password { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public string? Lastname { get; set; }

    public string? Phone { get; set; }

    public string? Document { get; set; } = null!;

    public string? DocumentType { get; set; } = null!;

    public bool Active { get; set; }

    public virtual IEnumerable<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();

    public virtual IEnumerable<RolDTO> Rols { get; set; } = new List<RolDTO>();
}