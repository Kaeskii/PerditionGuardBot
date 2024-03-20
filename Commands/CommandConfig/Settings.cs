using DSharpPlus.Entities;

namespace PerditionGuardBot.Commands
{
    public class Settings // for all the frequently used settings that may want to be changed for customization.
    {
        public static ulong MutedRole { get; set; }
        public static DiscordColor PrimaryColor { get; set; }
        public static string NameOfColor { get; set; }
        public static DiscordColor GetPrimaryColor()
        {
            PrimaryColor = DiscordColor.White;
            return PrimaryColor;
        }
        public static ulong GetMutedRole(ulong serverID)
        {
            if (serverID == 964579124985352192)
            {
                MutedRole = 965343669366423612;
            }
            return MutedRole;
        }
        public static void SetMutedRole(ulong input)
        {
            MutedRole = input;
        }
        public static void SetPrimaryColor(string input)
        {
            switch (input) // this was the best way I could think about going this without triying to input a discord color or something
            { // for now these are the main themes that are used, more can be added later.
                case "white":
                    PrimaryColor = DiscordColor.White;
                    break;
                case "gray":
                    PrimaryColor = DiscordColor.Gray;
                    break;
                case "black":
                    PrimaryColor = DiscordColor.Black;
                    break;
            }
        }
        public static string GetNameOfColor()
        {
            NameOfColor = PrimaryColor.ToString(); // this returns it as a hex code which is great
            return NameOfColor;
        }
    }
}