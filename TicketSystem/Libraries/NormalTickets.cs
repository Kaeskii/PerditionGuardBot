using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.TicketsSystem
{
    public class NormalTickets
    {
        public string User { get; set; }
        public string Content { get; set; }
        public int TicketNo { get; set; }
        public ulong TicketId { get; set; }
    }
}