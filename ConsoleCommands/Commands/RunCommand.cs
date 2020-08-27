using BrokeProtocol.API;
using BrokeProtocol.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    class RunCommand : Command
    {
        public override string command => "run";

        public override string Name => "Run";

        public override string Description => "Runs any command from other plugins. (Not fully functional)";

        public override void Run(string[] args)
        {
            var tempNpc = EntityCollections.NPCs.FirstOrDefault();
            Debug.LogWarning($"{tempNpc} npc is running the command.");
            if (!GroupManager.Groups.ContainsKey("console_perms"))
            {
                Group tempGroup = new Group();
                tempGroup.Permissions.Add("*");
                GroupManager.Groups.Add("console_perms", tempGroup);
            }
            GroupManager.Groups["console_perms"].AddMember(tempNpc);
            if (CommandHandler.Commands.TryGetValue((args[0].ToLowerInvariant(), args.Length).GetHashCode(), out BrokeProtocol.API.Types.Command value))
            {
                value.Invoke(tempNpc, args, args.Length);
                return;
            }
            GroupManager.Groups["console_perms"].RemoveMember(tempNpc);
        }
    }
}
