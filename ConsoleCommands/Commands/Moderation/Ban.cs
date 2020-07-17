using BrokeProtocol.API;
using BrokeProtocol.Utility.Networking;
using System.Linq;
using UnityEngine;

namespace ConsoleCommands.Commands.Moderation
{
    class Ban : Command
    {
        public override string command => "ban";

        public override string Name => "Ban";

        public override string Description => "Bans a player ip from the server.";

        public override void Run(string[] args)
        {
            /*
             * args[1] = username
             * args[2-lenght] = reason 
             */

            if (args.Length >0)
            {
                if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[0]))
                {
                    Debug.Log("Player is online");
                    string reason = null;
                    if (args.Length > 1)
                    {
                        reason = StringBuilder.WithArray(args, args.Length, 1);
                    }
                    var player = Core.Instance.SvManager.connectedPlayers.Values.First(i => i.username == args[0]);
                    Core.Instance.SvManager.Disconnect(player.svPlayer.connection, DisconnectTypes.Banned);
                    var user = Core.Instance.SvManager.database.Users.FindById(args[0]);
                    Core.Instance.SvManager.database.Bans.Insert(user.IP, new BrokeProtocol.Server.LiteDB.Models.Ban { Reason = reason ?? "No reason.", Username = player.username });
                    Core.Instance.SvManager.database.Users.Upsert(user);
                    Debug.Log($"{args[0]} has been banned for {reason??"No reason"}.");
                }
                else
                {
                    var user = Core.Instance.SvManager.database.Users.FindById(args[0]);
                    if (user != null)
                    {
                        string reason = null;
                        if (args.Length > 1)
                        {
                            reason = StringBuilder.WithArray(args, args.Length, 1);
                        }
                        Core.Instance.SvManager.database.Bans.Insert(user.IP, new BrokeProtocol.Server.LiteDB.Models.Ban { Reason = reason ?? "No reason.", Username = user.ID });
                        Core.Instance.SvManager.database.Users.Upsert(user);
                        Debug.Log($"{args[0]} has been banned for {reason ?? "No reason"}.");
                    }
                    else
                    {
                        Debug.Log("Player was not found");
                    }
                }
            }
            else
            {
                Debug.Log("Use the format \"ban {player} {reason}\".");
            }
        }
    }
}
