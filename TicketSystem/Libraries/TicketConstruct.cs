using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PerditionGuardBot.commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.TicketsSystem
{
    public class TicketConstruct
    {
        public bool Transcript(NormalTickets ticket) // the paths will need to be reassigned on a different device.
        {
            try
            {
                string Path = @"C:\Users\Kayla\source\repos\PerditionGuardBot\bin\Debug\net8.0\transcripts.json";
                var Json = File.ReadAllText(Path);
                var JsonObj = JObject.Parse(Json);
                var tickets = JsonObj["tickets"].ToObject<List<NormalTickets>>();
                tickets.Add(ticket);
                JsonObj["tickets"] = JArray.FromObject(tickets);
                File.WriteAllText(Path, JsonObj.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool DeleteTicket(ulong ticketID)
        {
            try
            {
                string Path = @"C:\Users\Kayla\source\repos\PerditionGuardBot\bin\Debug\net8.0\transcripts.json";
                var Json = File.ReadAllText(Path);
                var JsonObj = JObject.Parse(Json);
                var Tickets = JsonObj["tickets"].ToObject<List<NormalTickets>>();
                bool isDeleted = false;
                while (!isDeleted)
                {
                    for (int i = 0; i < Tickets.Count; i++)
                    {
                        if (Tickets[i].TicketId == ticketID)
                        {
                            Tickets.Remove(Tickets[i]);
                            isDeleted = true;
                        }
                    }
                }
                JsonObj["tickets"] = JArray.FromObject(Tickets);
                File.WriteAllText(Path, JsonObj.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public List<NormalTickets> GetTickets() 
        {
            List<NormalTickets> Tickets = new List<NormalTickets>();
            using (StreamReader sr = new StreamReader("transcripts.json"))
            {
                string Json = sr.ReadToEnd();
                TicketJSON? TicketObj = JsonConvert.DeserializeObject<TicketJSON>(Json);
                foreach (var ticket in TicketObj.Tickets)
                {
                    Tickets.Add(ticket);
                }
            }
            return Tickets;
        }

        public int TotTickets()
        {
            int Tickets = 0;
            using (StreamReader sr = new StreamReader("transcripts.json"))
            {
                string Json = sr.ReadToEnd();
                TicketJSON? ticketObj = JsonConvert.DeserializeObject<TicketJSON>(Json);
                foreach (var Ticket in ticketObj.Tickets) 
                {
                     Tickets++;
                }
            }
            return Tickets;
        }
    }

    internal sealed class TicketJSON
    {
        public NormalTickets[] Tickets { get; set;}
    }
}