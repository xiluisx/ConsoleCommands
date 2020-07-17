using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConsoleCommands.Commands.Moderation
{
    class Unban : Command
    {
        public override string command => "unban";

        public override string Name => "UnBan";

        public override string Description => "Removes the ban from a player.";

        public override void Run(string[] args)
        {
            if (args.Length >0)
            {
                if (!Core.Instance.SvManager.TryGetUserData(args[0], out var user))
                {
                    Debug.Log("User not found");
                    return;
                }

                if (Core.Instance.SvManager.database.Bans.Delete(user.IP))
                {
                    Core.Instance.SvManager.database.Users.Upsert(user);
                    Debug.Log($"{args[0]} has been unbanned.");
                }
                else
                {
                    Debug.Log($"{args[0]} is not banned");
                }



            }
            else
            {
                Debug.Log("Use the format \"unban {player}\".");
            }
        }
    }
}
