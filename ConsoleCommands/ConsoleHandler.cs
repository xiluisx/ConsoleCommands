using BrokeProtocol.API;
using System;
using System.Linq;
using UnityEngine;

namespace ConsoleCommands
{
    class ConsoleHandler : IScript
    {
        [Target(GameSourceEvent.ManagerConsoleInput, ExecutionMode.Event)]
        public void OnConsoleMessage(string text)
        {

            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            string[] args = text.Split('"')
                     .Select((element, index) => index % 2 == 0
                                           ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                           : new string[] { element })
                     .SelectMany(element => element).ToArray();

            args[0] = args[0].ToLowerInvariant();

            if (!Core.Instance.Commands.TryGetValue(args[0], out var command))
            {
                Debug.LogError($"Command not found by name '{args[0]}'!");
                return;
            }

            Debug.LogWarning($"Executing command handler for '{args[0]}'");
            command.Run(args.Skip(1).ToArray());
        }
    }
}
