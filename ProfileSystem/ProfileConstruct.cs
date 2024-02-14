using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.commands.ProfileSystem
{
    public class ProfileConstruct
    {
        public bool StoreUserInfo(StoredUserInfo user)
        {
            var Path = @"C:\Users\Kayla\source\repos\PerditionGuardBot\ProfileSystem\UserInfo.json";
            var json = File.ReadAllText(Path);
            var JsonObj = JObject.Parse(json);
            var members = JsonObj["members"];
        }
    }
}