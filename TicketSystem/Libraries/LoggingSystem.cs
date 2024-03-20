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