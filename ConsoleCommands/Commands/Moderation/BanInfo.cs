using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConsoleCommands.Commands.Moderation
{
    class BanInfo : Command
    {
        public override string command => "baninfo";

        public override string Name => "BanInfo";

        public override string Description => "Gets the info of a user ban.";

        public override void Run(string[] args)
        {
            if (args.Length > 0)
            {
                if (Core.Instance.SvManager.TryGetUserData(args[0], out var user))
                {
                    var userInfo = Core.Instance.SvManager.database.Bans.FindById(user.IP);
                    Debug.Log("-User: " + userInfo.Username + "\n-Reason: " + userInfo.Reason);
                }
                else
                {
                    Debug.Log("User was not found");
                }
            }
            else
            {
                Debug.Log("Use the format \"banInfo {player}\".");
            }
        }
    }
}
