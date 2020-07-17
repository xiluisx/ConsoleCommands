using BrokeProtocol.API;
using System.Linq;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    public class Groups : Command
    {
        public override string command { get; } = "groups";

        public override string Name { get; } = "Groups";

        public override string Description { get; } = "Shows all the groups from the server.";

        public override void Run(string[] args)
        {
            GroupManager.Groups.Values.ToList().ForEach(i => Debug.Log($"-{i.Name}."));
        }
    }
}
