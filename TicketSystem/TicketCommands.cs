using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PerditionGuardBot.TicketsSystem;

namespace PerditionGuardBot.Commands
{
    public class TicketCommands : BaseCommandModule
    {
        // Ticket Command List
        //
        // 1: supportmessage (embed needs working on) (working) (Needs permission checks to manage channels or something!)
        // info: (cheater tickets needed) (partnership tickets needed) (improve normal tickets)
        // 2: manage (embed needs working on) (working) (Needs permission checks to manage channels or something!)
        // 3: transcript (starting)
        //
        // End

        [Command("supportmessage")] // sends the message for the ticket options ina  designated ticket channel
        public async Task SupportMessage(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                var helpButton = new DiscordButtonComponent(ButtonStyle.Primary, "helpButton", "help");
                var supportmessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithColor(Settings.GetPrimaryColor())
                        .WithTitle("Tickets")
                        .WithDescription("Select Which ticket you would like to open below"))
                    .AddComponents(helpButton);
                await ctx.Channel.SendMessageAsync(supportmessage);
            }
            else
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Channels"
                };
                var LackPermsMessage = await ctx.RespondAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("manage")] // optional dashboard for options outside of the original ticket etc - also stores extra information
        public async Task ManageSystem(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                var viewButton = new DiscordButtonComponent(ButtonStyle.Primary, "viewButton", "Ticket list");
                var deleteButton = new DiscordButtonComponent(ButtonStyle.Danger, "deleteIdButton", "Delete Ticket");
                var dashboard = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithColor(Settings.GetPrimaryColor())
                        .WithTitle("Ticket Dashboard")
                        .WithDescription("Select an option:"))
                    .AddComponents(viewButton, deleteButton);
                await ctx.RespondAsync(dashboard);
            }
            else
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Channels"
                };
                var LackPermsMessage = await ctx.RespondAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("transcript")]
        public async Task Transcript(CommandContext ctx, DiscordChannel ChannelIdInput)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                ulong ChannelId = ulong.Parse(ChannelIdInput.ToString());
                List<Transcripts> messages = ProgramMain.logger.messages.Where(message => message.ChannelId == ChannelId).ToList();
                using (StreamWriter wr = new StreamWriter("output.txt"))
                {
                    foreach (var message in messages)
                    {
                        string messageToWrite = $"[User: {message.Username}] [Date: {message.SendDate}] : {message.Content}";
                        wr.WriteLine(messageToWrite);
                    }
                }
                FileStream file = new FileStream(@"C:\Users\Kayla\source\repos\PerditionGuardCode\bin\Debug\output.txt", FileMode.Open, FileAccess.Read);
                var fileMessage = new DiscordWebhookBuilder().AddFile("output.txt", file);
                await ctx.RespondAsync(fileMessage.ToString());
            }
            else
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Channels"
                };
                var LackPermsMessage = await ctx.RespondAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
    }
}