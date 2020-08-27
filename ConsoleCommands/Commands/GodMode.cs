using System;
using System.Linq;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    class GodMode : Command
    {
        public override string command => "godmode";

        public override string Name => "GodMode";

        public override string Description => "Changes the player godmode status.";

        public override void Run(string[] args)
        {
            if (Core.Instance.SvManager.connectedPlayers.Values.Any(x => x.username == args[0]))
            {
                var player = Core.Instance.SvManager.connectedPlayers.Values.First(x => x.username == args[0]);
                player.svPlayer.godMode = !player.svPlayer.godMode;
                Debug.Log($"Set {player.username} godmode to {player.svPlayer.godMode}");
                player.svPlayer.SendGameMessage($"GodMode set to {player.svPlayer.godMode}");
            }
            else
            {
                Debug.Log("Player not found");
            }
        }
    }
}
