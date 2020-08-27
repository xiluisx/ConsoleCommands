using BrokeProtocol.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    class GetCommands : Command
    {
        public override string command => "getcommands";

        public override string Name => "GetCommands";

        public override string Description => "Gets all commands from all plugins.";

        public override void Run(string[] args)
        {
            foreach(var comm in CommandHandler.Commands.Values)
            {
                Debug.LogWarning($"{comm.Name} - {string.Join(",",comm.PartialParameters.ToList())}");
            }
        }
    }
}
