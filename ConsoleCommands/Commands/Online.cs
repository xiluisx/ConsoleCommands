using BrokeProtocol.Collections;
using System.Linq;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    public class Online : Command
    {
        public override string command { get; } = "online";

        public override string Name { get; } = "Online";

        public override string Description { get; } = "Gives a list of all online players.";

        public override void Run(string[] args)
        {
            if (Core.Instance.SvManager.connectedPlayers.Count > 0)
            {
                Debug.Log("Players Online:");
                foreach (var player in Core.Instance.SvManager.connectedPlayers.Values)
                {
                    Debug.Log($"- {player.username}");
                }
            }
            else
            {
                Debug.Log("No players online.");
            }
        }
    }
}
