using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.commands.Basic
{
    public class Everyone : BaseCommandModule
    {
        // Everyone Command List
        //
        // 1: Ping (working)
        // 2: whois (not started)
        // 3: avatar (not started) (Alias: av)
        // 4: serverinfo (not started) (Alias: si)
        //
        // End

        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            var ping = ctx.Client.Ping;
            await ctx.RespondAsync("Pong! : " + ping);
        }
    }
}
