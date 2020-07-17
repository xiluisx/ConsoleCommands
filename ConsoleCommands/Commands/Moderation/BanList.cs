using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConsoleCommands.Commands.Moderation
{
    class BanList : Command
    {
        public override string command => "banlist";

        public override string Name => "BanList";

        public override string Description => "Shows a list of all users that are banned.";

        public override void Run(string[] args)
        {
            foreach (var banned in Core.Instance.SvManager.database.Bans.FindAll())
            {
                Debug.Log("{\n" + "\t" + "Username: " + banned.Username + "\n" + "\t" + "Reason: " + banned.Reason + "\n}");
            }
        }
    }
}
