using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus;
using DSharpPlus.CommandsNext.Converters;
using System.Runtime.InteropServices;
using System.Threading.Channels;

namespace PerditionGuardBot.commands
{
    public class Moderation : BaseCommandModule // for the lower level commands such as kick mute timeout etc (don;t forget to add the asylum
    {
        // Moderation Command List
        //
        // 1: kick (working) (embedded)
        // 2: warn (not started)
        // 3: checkwarn (not started) (void : part of the profile system now)
        // 4: nick (not started)
        // 5: purge (working) (slash commands are taking over this one since it's hard to make it outside of a slash command.)
        // 6: cleanup (part of the purge slash commands.)
        // 7: mute (asylum) (working) (dm the user - make toggleable) (embedded)
        // 8: unmute (asylum) (not started)
        //
        // End

        [Command("modtest")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Testing mod functions\nping: {ctx.Client.Ping}"); // displays testing messgae and sends current ping for each response etc
            if (ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                await ctx.Channel.SendMessageAsync("true");
            }
            else
                await ctx.Channel.SendMessageAsync("false");
        }


        // kick commands


        [Command("kick")]
        public async Task Kick(CommandContext ctx, DiscordMember targetUser, [RemainingText] string reason)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                string OriginalReason = reason;
                if (reason == null)
                    reason = "Not specified by " + ctx.User.Username;
                else
                    reason = "**Reason:** " + reason + "\n **Kicked by:** " + ctx.User.Username;
                await targetUser.RemoveAsync(reason);
                var KickedEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Yellow,
                    Title = $"{targetUser.Username} Kicked",
                    Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}",
                };
                await ctx.Channel.SendMessageAsync(KickedEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Kick Members"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("kick")]
        public async Task KickCatch(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                var MissingInfoEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "Missing Information",
                    Description = "You haven't provided a user to kick"
                };
                await ctx.Channel.SendMessageAsync(MissingInfoEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Kick Members"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }


        // purge commands


        [Command("purge")]
        public async Task Purge(CommandContext ctx, int amount)
        {
            await ctx.Message.DeleteAsync();
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageMessages))
            {
                IEnumerable<DiscordMessage> messages = await ctx.Channel.GetMessagesAsync(amount);
                var FilteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14);
                await ctx.Channel.DeleteMessagesAsync(FilteredMessages);
                DiscordMessage Message = await ctx.Channel.SendMessageAsync($"{amount} messages deleted");
                await Task.Delay(3000);
                await ctx.Channel.DeleteMessageAsync(Message);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageMessages))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Messages"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("purgeuser")] // this for somereason does not have a method for getting the user to purge or the message id of the sernder for each message, tried for hours to get it to work
        public async Task PurgeUser(CommandContext ctx)
        {
            await ctx.Message.DeleteAsync();
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageMessages))
            {
                await ctx.Channel.SendMessageAsync("Please use the slash command as this does not work (yet?)");
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageMessages))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Messages"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("purgeguard")] // same issue and will be made one command for interaction commands (slash commands)
        public async Task PurgeGuard(CommandContext ctx, [Optional] int amount)
        {
            await ctx.Message.DeleteAsync();
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageMessages))
            {
                await ctx.Channel.SendMessageAsync("Please use the slash command as this does not work (yet?)");
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageMessages))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Messages"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }


        // mute commands
        [Command("setmuterole")]
        public async Task SetMuteRole(CommandContext ctx, DiscordRole roleToSet)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                Settings.SetMutedRole(roleToSet.Id);
                var MuteRoleSetEmbed = new DiscordEmbedBuilder()
                {
                    Color = Settings.GetPrimaryColor(),
                    Title = "Muted Role Set",
                    Description = $"The muted role has been set to {roleToSet.Name}"
                };
                await ctx.Channel.SendMessageAsync(MuteRoleSetEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Roles"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }

        [Command("mute")]
        public async Task Mute(CommandContext ctx, DiscordMember targetUser, string duration, [RemainingText] string reason)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                string OriginalReason = reason;
                DiscordRole MutedRole = ctx.Guild.GetRole(Settings.GetMutedRole(ctx.Guild.Id));
                if (reason == null)
                    reason = "Not specified by " + ctx.User.Username;
                else
                    reason = "**Reason:** " + reason + "\n **Muted by:** " + ctx.User.Username;
                duration = duration.ToLower();
                if (duration.Contains("s"))
                {
                    duration = duration.Replace("s", "");
                    int seconds = int.Parse(duration);
                    await targetUser.GrantRoleAsync(MutedRole);
                    var MutedEmbed = new DiscordEmbedBuilder()
                    {
                        Color = Settings.GetPrimaryColor(),
                        Title = $"{targetUser.Username} Muted",
                        Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}, For: {seconds} second(s)",
                    };
                    await ctx.Channel.SendMessageAsync(MutedEmbed);
                    await Task.Delay(seconds * 1000);
                    await targetUser.RevokeRoleAsync(MutedRole);
                }
                if (duration.Contains("m"))
                {
                    duration = duration.Replace("m", "");
                    int minutes = int.Parse(duration);
                    await targetUser.GrantRoleAsync(MutedRole);
                    var MutedEmbed = new DiscordEmbedBuilder()
                    {
                        Color = Settings.GetPrimaryColor(),
                        Title = $"{targetUser.Username} Muted",
                        Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}, For: {minutes} minute(s)",
                    };
                    await ctx.Channel.SendMessageAsync(MutedEmbed);
                    await Task.Delay(minutes * 60000);
                    await targetUser.RevokeRoleAsync(MutedRole);
                }
                if (duration.Contains("h"))
                {
                    duration = duration.Replace("h", "");
                    int hours = int.Parse(duration);
                    await targetUser.GrantRoleAsync(MutedRole);
                    var MutedEmbed = new DiscordEmbedBuilder()
                    {
                        Color = Settings.GetPrimaryColor(),
                        Title = $"{targetUser.Username} Muted",
                        Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}, For: {hours} hour(s)",
                    };
                    await ctx.Channel.SendMessageAsync(MutedEmbed);
                    await Task.Delay(hours * 3600000);
                    await targetUser.RevokeRoleAsync(MutedRole);
                }
                if (duration.Contains("d"))
                {
                    duration = duration.Replace("d", "");
                    int days = int.Parse(duration);
                    await targetUser.GrantRoleAsync(MutedRole);
                    var MutedEmbed = new DiscordEmbedBuilder()
                    {
                        Color = Settings.GetPrimaryColor(),
                        Title = $"{targetUser.Username} Muted",
                        Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}, For: {days} day(s)",
                    };
                    await ctx.Channel.SendMessageAsync(MutedEmbed);
                    await Task.Delay(days * 86400000);
                    await targetUser.RevokeRoleAsync(MutedRole);
                }
                if (duration.Contains("m"))
                {
                    duration = duration.Replace("m", "");
                    int months = int.Parse(duration);
                    await targetUser.GrantRoleAsync(MutedRole);
                    var MutedEmbed = new DiscordEmbedBuilder()
                    {
                        Color = Settings.GetPrimaryColor(),
                        Title = $"{targetUser.Username} Muted",
                        Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}, For: {months} month(s)",
                    };
                    await ctx.Channel.SendMessageAsync(MutedEmbed);
                    await Task.Delay((int)(months * 2592000000));
                    await targetUser.RevokeRoleAsync(MutedRole);
                }
                if (duration.Contains("y"))
                {
                    var TooLongEmbed = new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Red,
                        Title = $"Too long",
                        Description = $"The most likely scenario will be that this bot is not up for longer than a month at a time unless hosted somewhere 24/7 but even then very unlikely and due to the way that the bot is set up the bot does not store the mute information after the bot deactivates and therefore does not continue the task.",
                    };
                    await ctx.Channel.SendMessageAsync(TooLongEmbed);
                }   
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Roles"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("mute")]
        public async Task MuteCatch(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                var NoMutedRoleEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "No Muted Role or no duration set",
                    Description = "If there is no role set please use the setmuterole command or use the duration format (s,m,h,d,m)"
                };
                await ctx.Channel.SendMessageAsync(NoMutedRoleEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Roles"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
    }
}