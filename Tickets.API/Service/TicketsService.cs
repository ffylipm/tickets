using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class TicketsService
    {
        private readonly TicketsContext context;
        public TicketsService(TicketsContext context)
        {
            this.context = context;
        }


    }
}
