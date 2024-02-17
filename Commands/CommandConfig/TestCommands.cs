using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System.Threading.Tasks;

namespace PerditionGuardBot.Commands
{
    public class TestCommands : BaseCommandModule
    {
        // all commands are built in classes for their purpose and whatnot, do not lump all the commands in the testing file.
        // making more classes is thorugh the commands folder
        // all commands are async and require an await func to work otherwise nono
        // consult jesus if need help with any of the commands :D

        // The first command (working)
        [RequireOwner] // means only I can run these commands in a given server (external verification)

        [Command("test")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Testing\n{ctx.Client.Ping}"); // displays testing messgae and sends current ping for each response etc
        }
        // for embedded messages within commands or something
        [Command("embed")]
        public async Task EmbedTemplate(CommandContext ctx)
        {
            var message = new DiscordMessageBuilder() 
                .AddEmbed(new DiscordEmbedBuilder() // here has embed builder inside of a message builder, very ineficient but this is for embeds with interactable buttons
                .WithTitle("testing embed functions")
                .WithDescription($"Execution by {ctx.User.Username}"));
            await ctx.RespondAsync(message);
        }
        // a better way to do the above that just looks more readable
        [Command("embedneat")]
        public async Task EmbedTemplatNeat(CommandContext ctx)
        {
            var message = new DiscordEmbedBuilder // note the difference in the code for the new discord builder
            {
                Title = "testing neat embed function", // always finish with a comma too...
                Description = $"Execution by {ctx.User.Username}",
                Color = DiscordColor.HotPink
            };
            await ctx.RespondAsync(embed: message);
        }
        [Command("interactivitytest")]
        public async Task InteractivityTest(CommandContext ctx)
        { // this calls the bots client not the user client
            var interactivity = ProgramMain.Client.GetInteractivity();
            var messageToRestrieve = await interactivity.WaitForMessageAsync(message => message.Content == "Hello");
            if (messageToRestrieve.Result.Content == "Hello")
            {
                await ctx.RespondAsync($"testing autoresponse with {ctx.User.Username}");
            }
        }
    }
}