using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.Commands.Basic
{
    public class Everyone : BaseCommandModule
    {
        // Everyone Command List
        //
        // 1: Ping (working)
        // 2: whois (started)
        // 3: avatar (working) (embedded) (Alias: av)
        // 4: serverinfo (not started) (Alias: si)
        //
        // End

        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            var ping = ctx.Client.Ping;
            await ctx.RespondAsync("Pong! : " + ping);
        }
        [Command("whois")]
        public async Task Whois(CommandContext ctx)
        {
            var WhoisEmbed = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder()
                .WithColor(Settings.GetPrimaryColor())
                .WithThumbnail(ctx.User.AvatarUrl)
                .WithTitle(ctx.User.Username)
                .AddField("Join Date:", ctx.Member.JoinedAt.ToString(), true)
                .AddField("Account Created:", ctx.User.CreationTimestamp.ToString(), true)
                .AddField("Permissions:", ctx.Member.Permissions.ToString(), false)
                );
            await ctx.RespondAsync(WhoisEmbed);
        }


        // avatar Commands


        [Command("avatar")] // nice simple command
        [Aliases("av")]
        public async Task Avatar(CommandContext ctx, DiscordMember targetUser)
        {
            var avatar = targetUser.AvatarUrl;
            var AvatarEmbed = new DiscordEmbedBuilder
            {
                Color = Settings.GetPrimaryColor(),
                Title = "Avatar",
                ImageUrl = avatar
            };
            await ctx.RespondAsync(AvatarEmbed);
        }
        [Command("avatar")]
        public async Task Avatar(CommandContext ctx)
        {
            var AvatarEmbed = new DiscordEmbedBuilder
            {
                Color = Settings.GetPrimaryColor(),
                Title = "Avatar",
                ImageUrl = ctx.User.AvatarUrl
            };
            await ctx.RespondAsync(AvatarEmbed);
        }


        // serverinfo Commands


        [Command("serverinfo")]
        [Aliases("si")]
        public async Task ServerInfo(CommandContext ctx)
        {
            await ctx.RespondAsync("This command is not yet implemented.");
        }
    }
}