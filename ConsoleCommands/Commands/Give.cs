using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;
using UnityEngine;

namespace ConsoleCommands.Commands
{
    public class Give : Command
    {
        public override string command { get; } = "give";

        public override string Name { get; } = "Give";

        public override string Description { get; } = "Gives an item to a player.";

        public override void Run(string[] args)
        {
            if (args.Length > 1)
            {
                if (Core.Instance.SvManager.connectedPlayers.Values.Any(x => x.username == args[0]))
                {
                    if (args.Length == 3)
                    {
                        GiveItem(Core.Instance.SvManager.connectedPlayers.Values.First(x => x.username == args[0]), args[1], int.Parse(args[2]));
                    }
                    else
                    {
                        GiveItem(Core.Instance.SvManager.connectedPlayers.Values.First(x => x.username == args[0]), args[1]);
                    }
                }
                else
                {
                    Debug.Log("Player not found");
                }
            }
            else
            {
                Debug.Log("Use the format \"Give {player} {item} {quantity}\".");
            }
        }
        private void GiveItem(ShPlayer player, string itemName, int quantity = 1)
        {

            if (Core.Instance.items.Any(x => x.name == itemName))
            {
                Debug.Log("Found Item");
                player.TransferItem(DeltaInv.AddToMe, Core.Instance.items.First(x => x.name == itemName), quantity);
                Debug.Log($"Gave {player.username} {quantity} {itemName}'s");
            }
            else
            {
                Debug.Log("Object not found");
            }
        }
    }
}
