using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot
{
    public class Blacklist // obviously this is a long list of words not to say in a chat. += when sending the bot as a zip remove all words for professionalism.
    {
        public List<string> blacklist = new List<string>();
        public Blacklist()
        { 
            blacklist.Add("fag");
            blacklist.Add("nigg");
        }
    }
}