using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PerditionGuardBot.commands.ProfileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.commands
{
    public class Profile : BaseCommandModule
    {
        // Profile Command List
        //
        // 1: profile
        // 2: profile (show admins more information than normal users.)
        // 3: resetxp (user or guild)
        // 4: resetlevel (user or guild)
        // note: there will be no "setxp" or "setlevel" so that there's no missuse of the leveling system
        //
        // End

        [Command("profile")]
        public async Task ProfileCommand(CommandContext ctx)
        {
            string username = ctx.User.Username;
            int xp = 0;
            int level = 0;
            var UserInfo = new StoredUserInfo()
            {
                UserName = username,
                Level = 1,
                XP = 0,
            };
            var ProfileConstruct = new ProfileConstruct();
            var Process = ProfileConstruct.StoreUserInfo(UserInfo);
            var ProfileEmbed = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(Settings.GetPrimaryColor())
                .WithTitle(username + "'s Profile")
                .WithThumbnail(ctx.User.AvatarUrl)
                .AddField("Level", level.ToString())
                .AddField("XP", xp.ToString(), true)
                );
            await ctx.Channel.SendMessageAsync(ProfileEmbed);
        }
    }
}