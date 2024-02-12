using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using DSharpPlus.CommandsNext.Converters;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace PerditionGuardBot.commands
{
    public class Administration : BaseCommandModule // ban commands and whatnot, add embeds once the commands are somewhat done
    {
        // Administration Command List
        //
        // 1: ban Commands (working) (not implimented a tracking method)
        // 1.5: tempban (not started) (could bug out with other commands but I think it's probably fixable) (start after other commands) (before profiles) - had a shot but i'm going to need to store the time length elsewhere and also store multiple users for temp bans or something etc
        // 2: unban Commands (working)
        // 3: lock (working)
        // 4: unlock (working)
        // 5: role (working)
        //
        // End

        [Command("admintest")]
        public async Task Test(CommandContext ctx)
        {

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                await ctx.Channel.SendMessageAsync($"Testing admin functions\nping: {ctx.Client.Ping}\ntrue : admin permissions accessible"); // displays testing messgae and sends current ping for each response etc
                var adminEmbedTest = new DiscordEmbedBuilder()
                {
                    Color = Settings.GetPrimaryColor(),
                    Title = "Embeds are white",
                    Description = $"The color name is displayed as: {Settings.GetNameOfColor()}"
                };
                await ctx.Channel.SendMessageAsync(adminEmbedTest);
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"Testing admin functions\nping: {ctx.Client.Ping}");
                await ctx.Channel.SendMessageAsync("false : lacking admin permission");
            }
        }
        [Command("admintesttu")] // target user
        public async Task Test2(CommandContext ctx, DiscordMember targetUser) // discord user and discord member are different user: user in general ; member: member of guild
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                await ctx.Channel.SendMessageAsync($"Target User data: {targetUser}");
            }
        }


        // ban commands


        [Command("ban")] // using the parameter discord member causes the command to be a totally different function requiring that to even run.
        public async Task Ban(CommandContext ctx, DiscordMember targetUser, [RemainingText] string reason)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                string OriginalReason = reason;
                if (reason == null)
                    reason = "Not specified by " + ctx.User.Username;
                else
                    reason = "**Reason:** " + reason + "\n **Banned by:** " + ctx.User.Username;
                await ctx.Guild.BanMemberAsync(targetUser, 0, reason);
                var BannedEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = $"{targetUser.Username} Banned",
                    Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}",
                };
                await ctx.Channel.SendMessageAsync(BannedEmbed); // ban message but sends a long string of information instead of just the user ID or mention.
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Ban Members"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("ban")] // using the parameter discord member causes the command to be a totally different function requiring that to even run.
        public async Task BanID(CommandContext ctx, ulong targetUser, [RemainingText] string reason) // target user is the problem (fixed with seperate command), to fix the id it's a ulong
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                string OriginalReason = reason;
                if (reason == null)
                    reason = "Not specified by " + ctx.User.Username;
                else
                    reason = "**Reason:** " + reason + "\n **Banned by:** " + ctx.User.Username;
                await ctx.Guild.BanMemberAsync(targetUser, 0, reason);
                var BannedEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = $"{targetUser} Banned",
                    Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}",
                };
                await ctx.Channel.SendMessageAsync(BannedEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Ban Members"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("ban")] // this command catches the original commands but provides a reason for the command not running
        public async Task BanCatch(CommandContext ctx) // this makes it so the command runs even if they add anything after the command
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                var MissingInfoEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "Missing Information",
                    Description = "You haven't provided a user to ban"
                };
                await ctx.Channel.SendMessageAsync(MissingInfoEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Ban Members"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }


        // unban commands


        [Command("unban")] // very similar to the ban command
        public async Task Unban(CommandContext ctx, ulong targetUser, [RemainingText] string reason)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                string OriginalReason = reason;
                if (reason == null)
                    reason = "Not specified by " + ctx.User.Username;
                else
                    reason = "**Reason:** " + reason + "\n **Unbanned by:** " + ctx.User.Username;
                await ctx.Guild.UnbanMemberAsync(targetUser, reason);
                var UnbannedEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Yellow,
                    Title = $"{targetUser} Unbanned",
                    Description = $"By: {ctx.User.Username}, With reason: {OriginalReason}",
                };
                await ctx.Channel.SendMessageAsync(UnbannedEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Ban Members"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("unban")]
        public async Task UnbanCatch(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                var MissingInfoEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "Missing Information",
                    Description = "You haven't provided a user ID to unban"
                };
                await ctx.Channel.SendMessageAsync(MissingInfoEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Ban Members"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }


        // lock commands


        [Command("lock")]
        public async Task LockChannel(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                var Everyonerole = ctx.Guild.GetRole(ctx.Guild.Id);
                await ctx.Channel.AddOverwriteAsync(Everyonerole, Permissions.None, Permissions.SendMessages);
                var LockEmbed = new DiscordEmbedBuilder()
                {
                    Color = Settings.GetPrimaryColor(),
                    Title = "Channel Locked",
                };
                await ctx.Channel.SendMessageAsync(LockEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Channels"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }
        [Command("unlock")]
        public async Task UnlockChannel(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                var Everyonerole = ctx.Guild.GetRole(ctx.Guild.Id);
                await ctx.Channel.AddOverwriteAsync(Everyonerole, Permissions.SendMessages, Permissions.None);
                var UnlockEmbed = new DiscordEmbedBuilder()
                {
                    Color = Settings.GetPrimaryColor(),
                    Title = "Channel Unlocked",
                };
                await ctx.Channel.SendMessageAsync(UnlockEmbed);
            }
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                var LackPermsEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "You are missing the following permission:",
                    Description = "Manage Channels"
                };
                var LackPermsMessage = await ctx.Channel.SendMessageAsync(LackPermsEmbed);
                await Task.Delay(5000);
                await LackPermsMessage.DeleteAsync();
            }
        }


        // role


        [Command("role")]
        public async Task Role(CommandContext ctx, DiscordMember targetUser, DiscordRole role)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                if (targetUser != null && role != null)
                {
                    if (targetUser.Roles.Contains(role))
                    {
                        await targetUser.RevokeRoleAsync(role);
                        var RoleRevokedEmbed = new DiscordEmbedBuilder()
                        {
                            Color = Settings.GetPrimaryColor(),
                            Title = $"Removed {role.Name} from {targetUser.Username}",
                        };
                        await ctx.Channel.SendMessageAsync(RoleRevokedEmbed);
                    }
                    else
                    {
                        await targetUser.GrantRoleAsync(role);
                        var RoleGrantedEmbed = new DiscordEmbedBuilder()
                        {
                            Color = Settings.GetPrimaryColor(),
                            Title = $"Gave {role.Name} to {targetUser.Username}",
                        };
                        await ctx.Channel.SendMessageAsync(RoleGrantedEmbed);
                    }
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
        [Command("role")]
        public async Task RoleCatch(CommandContext ctx, DiscordMember targetUser)
        {
            var Ignore = targetUser;
            Ignore.ToString(); // the target User is used so that if one is provided but no role is provided this command will run and not the role ID catch instead
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                var MissingInfoEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "Missing Information",
                    Description = "You haven't provided a role ID"
                };
                await ctx.Channel.SendMessageAsync(MissingInfoEmbed);
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
        [Command("role")]
        public async Task RoleMemberCatch(CommandContext ctx)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                var MissingInfoEmbed = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Red,
                    Title = "Missing Information",
                    Description = "You haven't provided a user or user ID\nMay be an invalid user."
                };
                await ctx.Channel.SendMessageAsync(MissingInfoEmbed);
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