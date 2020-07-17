using System;
using UnityEngine;
namespace ConsoleCommands
{
    class Clear : Command
    {
        public override string command => "clear";

        public override string Name => "Clear";

        public override string Description => "Clears the console messages.";

        public override void Run(string[] args)
        {
            Console.Clear();
            Debug.LogWarning("Cleared console.");
        }
    }
}
