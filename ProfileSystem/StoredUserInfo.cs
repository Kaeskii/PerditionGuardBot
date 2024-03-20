namespace PerditionGuardBot.Commands
{
    public class StoredUserInfo
    {
        public string UserName { get; set; }
        public ulong UserID { get; set; }
        public string AvatarURL { get; set; }
        public int Level { get; set; }
        public double XP { get; set; }
        public int Bans { get; set; }
        public int Kicks { get; set; }
        public int Warnings { get; set; }
        public bool McbeCheater { get; set; }
        public string McbeCheaterCaughtBy { get; set; }
        public bool IsBooster { get; set; } // for making an automated booster perk system
    }
}