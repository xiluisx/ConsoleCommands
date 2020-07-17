using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ConsoleCommands
{
    public class Core : Plugin
    {
        public static Core Instance { get; internal set; }

        public Dictionary<string, Command> Commands { get; set; } = new Dictionary<string, Command>();

        public SvManager SvManager { get; set; }

        public List<ShItem> items = new List<ShItem>();
        public Core()
        {
            Instance = this;
            Info = new PluginInfo("ConsoleCommands", "ConsCmd")
            {
                Description = "This plugin adds commands to the console"
            };
            Commands = typeof(Command).Assembly.GetTypes()
                             .Where(t => t.IsSubclassOf(typeof(Command)) && !t.IsAbstract)
                             .Select(x => (Command)Activator.CreateInstance(x)).ToDictionary(x => x.command.ToLowerInvariant(), y => y);
            foreach (var obj in SceneManager.Instance.entityCollection)
            {
                if (obj.Value is ShItem)
                {
                    items.Add(obj.Value as ShItem);
                }
            }
        }
    }

    public class OnStarted : IScript
    {
        [Target(GameSourceEvent.ManagerStart, ExecutionMode.Event)]
        public void OnStart(SvManager svManager)
        {
            Core.Instance.SvManager = svManager;
        }
    }

}
