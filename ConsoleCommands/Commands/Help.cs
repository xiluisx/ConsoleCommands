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
            foreach(var cmd in Core.Instance.Commands)
            {
                Debug.Log($"\t-Name: {cmd.Value.Name}" +
                        $"\n\t\t-Command: \"{cmd.Value.command}\"" +
                        $"\n\t\t-Description: {cmd.Value.Description}");
            }
        }
    }
}