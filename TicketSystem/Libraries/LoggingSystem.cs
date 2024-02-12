using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.TicketsSystem
{
    public class LoggingSystem
    {
        public List<Transcripts> messages = new List<Transcripts>();
        public static readonly LoggingSystem Instance = new LoggingSystem();
        static LoggingSystem()
        {

        }
    }
}