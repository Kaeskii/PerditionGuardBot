using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace PerditionGuardBot.Commands
{
    public class Configuration : BaseCommandModule // idk if i'll need to change this for theses commands yet
    {
        // Configuration Command List
        //
        // 1: welcome & leave (needs testing again) (no altering command yet | no need)
        // 2: help (not started) (basically just one large embedded message) (add buttons for pages) (categorise the commands)
        // 3: blacklist (not started)
        // 4: autodelete (not started)
        // 5: info and rule commands (perdition only) (not started)
        // 6: kill (working) spent 2 hours trying to make a reconnect command but it's not possible.
        // 7: sticky roles (not started)
        // 8: prefix (not started) spent a few hours trying to make this too but couldn't find a way to do it. It would work but not without a restart of the bot.
        // 9: reaction roles (not started)
        // -1: profiles (starting)
        //
        // End

        // use leaveasync to leave the server if I get banned or kicked

        // help

        // prefix

        [Command("configurationtest")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Testing Configurations\n{ctx.Client.Ping}"); // displays testing messgae and sends current ping for each response etc
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageGuild))
            {
                await ctx.Message.DeleteAsync();
                await ctx.RespondAsync("true");
            }
            else
                await ctx.RespondAsync("false");
        }


        // Bot control commands


        [Command("Kill")]
        public async Task Kill(CommandContext ctx)
        {
            if (ctx.Member.Id == 514141397960359970 || ctx.Member.Id == 596553233024155679)
            {
                await ctx.Message.DeleteAsync();
                var message = await ctx.RespondAsync("Shutting down in 3 seconds");
                await Task.Delay(2500);
                await message.DeleteAsync();
                await ctx.Client.DisconnectAsync();
                Environment.Exit(0);
            }
            else
            {
                var RestrictedEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "Permission Denied",
                    Description = "Must be Kay or Koro to use this command"
                };
                var LackPermsMessage = await ctx.RespondAsync(RestrictedEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
    }
}