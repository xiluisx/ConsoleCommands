using System;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    public class ItemList : Command
    {
        public override string command { get; } = "itemlist";

        public override string Name { get; } = "ItemList";

        public override string Description { get; } = "List all the items of type ShItem.";

        public override void Run(string[] args)
        {
           foreach(var item in Core.Instance.items)
            {
                Debug.Log($" - {item.name}");
            }
        }
    }
}
