using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConsoleCommands.Commands.Moderation
{
    class Teleport : Command
    {
        public override string command => "teleport";

        public override string Name => "Teleport";

        public override string Description => "Teleports a player to another player(target).";

        public override void Run(string[] args)
        {
            if (args.Length > 1)
            {
                if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[0]))
                {
                    var user = Core.Instance.SvManager.connectedPlayers.Values.First(i => i.username == args[0]);
                    if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[1]))
                    {
                        var target = Core.Instance.SvManager.connectedPlayers.Values.First(i => i.username == args[1]);

                        user.SetPositionSafe(target.GetPosition);
                    }
                    else
                    {
                        Debug.Log("Target not found");
                    }
                }
                else
                {
                    Debug.Log("Player not found");
                }
            }
            else
            {
                Debug.LogError("Use the format \"tp {user} {target}\".");
            }
        }
    }
}
