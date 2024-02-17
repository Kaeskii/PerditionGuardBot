using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.Commands.ProfileSystem
{
    public class ProfileConstruct
    {
        public bool levelup = false;
        public bool StoreUserInfo(StoredUserInfo user)
        {
            try
            {
                var Path = @"C:\Users\Kayla\source\repos\PerditionGuardBot\ProfileSystem\UserInfo.json";
                var Json = File.ReadAllText(Path);
                var JsonObj = JObject.Parse(Json);
                var members = JsonObj["members"].ToObject<List<StoredUserInfo>>();
                members.Add(user);
                JsonObj["members"] = JArray.FromObject(members);
                File.WriteAllText(Path, JsonObj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckIfUserAlreadyExists(string username, ulong userID)
        {
            using (StreamReader sr = new StreamReader("UserInfo.json"))
            {
                string json = sr.ReadToEnd();
                ProfileJSONFile? userProfile = JsonConvert.DeserializeObject<ProfileJSONFile>(json);
                foreach (var user in userProfile.Members)
                {
                    if (user.UserName == username && user.UserID == userID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public StoredUserInfo? GetUserInfo(string username, ulong userID)
        {
            using (StreamReader sr = new StreamReader("UserInfo.json"))
            {
                string json = sr.ReadToEnd();
                ProfileJSONFile? userProfile = JsonConvert.DeserializeObject<ProfileJSONFile>(json);
                foreach (var user in userProfile.Members)
                {
                    if (user.UserName == username && user.UserID == userID)
                    {
                        return new StoredUserInfo()
                        {
                            UserName = user.UserName,
                            UserID = user.UserID,
                            AvatarURL = user.AvatarURL,
                            Level = user.Level,
                            XP = user.XP,
                            Bans = user.Bans,
                            Kicks = user.Kicks,
                            Warnings = user.Warnings,
                            McbeCheater = user.McbeCheater,
                            McbeCheaterCaughtBy = user.McbeCheaterCaughtBy,
                            IsBooster = user.IsBooster
                        };
                    }
                }
            }
            return null;
        }
        public bool AddXP(string username, ulong userID, double xp)
        {
            levelup = false;
            try
            {
                var Path = @"C:\Users\Kayla\source\repos\PerditionGuardBot\ProfileSystem\UserInfo.json";
                var Json = File.ReadAllText(Path);
                var JsonObj = JObject.Parse(Json);
                var members = JsonObj["members"].ToObject<List<StoredUserInfo>>();
                foreach (var user in members)
                {
                    if (user.UserName == username && user.UserID == userID)
                    {
                        user.XP += xp;
                        return true;
                    }
                    if (user.XP >= 100 + user.Level * user.Level * 1.5)
                    {
                        user.Level += 1;
                        user.XP = 0;
                        levelup = true;
                        return true;
                    }
                }
                JsonObj["members"] = JArray.FromObject(members);
                File.WriteAllText(Path, JsonObj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    class ProfileJSONFile
    {
        public string UserInfo { get; set; }
        public StoredUserInfo[] Members { get; set; }
    }
}