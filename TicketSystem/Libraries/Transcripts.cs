using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.TicketsSystem
{
    public class Transcripts
    {
        public string Username { get; set; }
        public ulong ChannelId { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
    }
}