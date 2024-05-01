namespace Tickets.API.Models
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }

        public string? Name { get; set; } = null!;

        public string? Description { get; set; } = null!;

        public bool Active { get; set; }
    }
}
