using System.Linq;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    public class Help : Command
    {
        public override string command { get; } = "help";
        public override string Name { get; } = "Help";

        public override string Description { get; } = "Shows all commands with it's description.";
        public override void Run(string[] args)
        {
            Debug.LogError("Commands Help:");
            foreach (var cmd in Core.Instance.Commands.Values.OrderBy(x => x.Name == "Help"?0:1).ThenBy(x => x.Name))
            {
                Debug.Log($"\t-Name: {cmd.Name}" +
                        $"\n\t\t-Command: \"{cmd.command}\"" +
                        $"\n\t\t-Description: {cmd.Description}");
            }
        }
    }
}