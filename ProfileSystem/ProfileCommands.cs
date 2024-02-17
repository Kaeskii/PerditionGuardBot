using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PerditionGuardBot.Commands.ProfileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.Commands
{
    public class ProfileCommands : BaseCommandModule
    {
        // Profile Command List
        //
        // 1: profile
        // 2: profile (show admins more information than normal users.)
        // 3: resetxp (user or guild)
        // 4: resetlevel (user or guild)
        // note: there will be no "setxp" or "setlevel" so that there's no missuse of the leveling system
        // 5: method for setting a user as a booster
        // 6: method for setting a user as a cheater (and who caught them)
        //
        // End

        [Command("profile")]
        public async Task ProfileCommand(CommandContext ctx)
        {
            var UserInfo = new StoredUserInfo()
            {
                UserName = ctx.User.Username,
                UserID = ctx.User.Id,
                AvatarURL = ctx.User.AvatarUrl,
                Level = 1,
                XP = 0,
                Bans = 0,
                Kicks = 0,
                Warnings = 0,
                McbeCheater = false,
                McbeCheaterCaughtBy = "Null",
                IsBooster = false,
            };
            string Cheater = "No";
            var ProfileConstruct = new ProfileConstruct();
            var DoesExist = ProfileConstruct.CheckIfUserAlreadyExists(ctx.User.Username, ctx.User.Id);
            if (DoesExist == false)
            {
                var Stored = ProfileConstruct.StoreUserInfo(UserInfo);
                if (Stored == true)
                {
                    var UserProfile = ProfileConstruct.GetUserInfo(ctx.User.Username, ctx.User.Id);
                    double xpoutof = 100 + UserProfile.Level * UserProfile.Level * 1.5;
                    string xpoutofstring = $"{UserProfile.XP} / {xpoutof}";
                    if (UserProfile.McbeCheater == true)
                        Cheater = $"Yes | Caught by: {UserProfile.McbeCheaterCaughtBy}";
                    var ProfileEmbed = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder()
                        .WithColor(Settings.GetPrimaryColor())
                        .WithTitle($"{UserProfile.UserName};s Profile")
                        .WithThumbnail(UserProfile.AvatarURL)
                        .AddField("Level", UserProfile.Level.ToString(), true)
                        .AddField("XP", xpoutofstring, true)
                        .AddField("Hive Cheater", Cheater, false)
                        .AddField("Boosting", UserProfile.IsBooster.ToString(), false)
                        );
                    await ctx.RespondAsync(ProfileEmbed);
                }
                else
                {
                    var message = await ctx.RespondAsync("There was an error creating your profile.");
                    await Task.Delay(3000);
                    await message.DeleteAsync();
                }
            }
            if (DoesExist == true)
            {
                var UserProfile = ProfileConstruct.GetUserInfo(ctx.User.Username, ctx.User.Id);
                if (UserProfile.McbeCheater == true)
                    Cheater = $"Yes | Caught by: {UserProfile.McbeCheaterCaughtBy}";
                var ProfileEmbed = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(Settings.GetPrimaryColor())
                    .WithTitle($"{UserProfile.UserName};s Profile")
                    .WithThumbnail(UserProfile.AvatarURL)
                    .AddField("Level", UserProfile.Level.ToString(), true)
                    .AddField("XP", UserProfile.XP.ToString(), true)
                    .AddField("Hive Cheater", Cheater, false)
                    .AddField("Boosting", UserProfile.IsBooster.ToString(), false)
                    );
                await ctx.RespondAsync(ProfileEmbed);
            }
            else
            {
                var message = await ctx.RespondAsync("There was an error loading your profile.");
                await Task.Delay(3000);
                await message.DeleteAsync();
            }
        }
    }
}