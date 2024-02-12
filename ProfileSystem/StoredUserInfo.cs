﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerditionGuardBot.commands
{
    public class StoredUserInfo
    {
        public string UserName { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public int Bans { get; set; }
        public int Kicks { get; set; }
        public int Warnings { get; set; }
        public bool McbeCheater { get; set; }
        public bool IsBooster { get; set; } // for making an automated booster perk system
        public int Roles { get; set; }
    }
}