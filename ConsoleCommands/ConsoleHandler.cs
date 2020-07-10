using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Server.LiteDB.Models;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
namespace ConsoleCommands
{
    class ConsoleHandler : IScript
    {

        List<ShItem> items = new List<ShItem>();
        

        public ConsoleHandler()
        {
            BrokeProtocol.Managers.SceneManager.Instance.entityCollection.ToList().ForEach(x => Debug.LogWarning(x.Value + "-" + x.Value.GetType().Name));

            foreach (var obj in BrokeProtocol.Managers.SceneManager.Instance.entityCollection)
            {
                if (obj.Value is ShItem)
                {
                    items.Add(obj.Value as ShItem);
                }
            }
        }

        [Target(GameSourceEvent.ManagerConsoleInput, ExecutionMode.Event)]
        public void OnConsoleMessage(string command)
        {
            string[] args = command.Split('"')
                     .Select((element, index) => index % 2 == 0  // If even index
                                           ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                           : new string[] { element })  // Keep the entire item
                     .SelectMany(element => element).ToArray();

            string first = args[0].ToLower();
            if (first == "groups")
                GroupManager.Groups.Values.ToList().ForEach(i => Debug.Log(i.Name));
            else if (first == "give")
            {
                if (args.Length >= 2)
                {
                    if (Core.Instance.SvManager.connectedPlayers.Values.Any(x => x.username == args[1]))
                    {
                        if (args.Length == 4)
                        {
                            GiveItem(Core.Instance.SvManager.connectedPlayers.Values.First(x => x.username == args[1]), args[2], int.Parse(args[3]));
                        }
                        else
                        {
                            GiveItem(Core.Instance.SvManager.connectedPlayers.Values.First(x => x.username == args[1]), args[2]);
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
            else if (first == "addgroup")
            {

                if (args.Length == 3)
                {
                    if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[1]))
                    {
                        if (GroupManager.Groups.Values.Any(i => i.Name == args[2]))
                        {
                            GroupManager.Groups.Values.First(i => i.Name == args[2]).AddMember(Core.Instance.SvManager.connectedPlayers.Values.First(x => x.username == args[1]));
                            Debug.Log($"Added role \"{args[2]}\" to player \"{args[1]}\".");
                            if (File.Exists("groups.json"))
                            {
                                string json = File.ReadAllText("groups.json");
                                List<RoleGroup.Group> groups = JsonConvert.DeserializeObject<List<RoleGroup.Group>>(json);
                                groups.First(i => i.Name == args[2]).Members.Add(args[1]);
                                File.WriteAllText("groups.json", JsonConvert.SerializeObject(groups, Newtonsoft.Json.Formatting.Indented));
                            }
                            else
                            {
                                Debug.Log("Could not find groups.json, add manually or try again");
                            }
                        }
                        else
                        {
                            Debug.Log("Role not found, be sure to create it first.");
                        }
                    }
                    else
                    {
                        Debug.Log("Player not found online.");
                    }
                }
                else
                {
                    Debug.Log("You need to use the format \"AddGroup {player} {role}\".");
                }

            }
            else if (first == "say")
            {
                if (args.Length >= 2)
                {
                    foreach (var player in Core.Instance.SvManager.connectedPlayers.Values)
                    {
                        player.svPlayer.SendGameMessage(StringBuilder.WithArray(args, args.Length, 1));
                    }
                    Debug.Log("You said: " + StringBuilder.WithArray(args, args.Length, 1));
                }
                else
                {
                    Debug.Log("Use the format \"say {message}\".");
                }
            }
            else if (first == "online")
            {
                if (Core.Instance.SvManager.connectedPlayers.Count > 0)
                {
                    Debug.Log("Players Online:");
                    foreach (var player in Core.Instance.SvManager.connectedPlayers.Values)
                    {
                        Debug.Log("-" + player.username);
                    }
                }
                else
                {
                    Debug.Log("No players online.");
                }
            }
            else if (first == "ban")
            {
                if (args.Length >= 2)
                {
                    if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[1]))
                    {
                        Debug.Log("Player is online");
                        string reason = null;
                        if (args.Length >= 3)
                        {
                            reason = StringBuilder.WithArray(args, args.Length, 2);
                        }
                        var player = Core.Instance.SvManager.connectedPlayers.Values.First(i => i.username == args[1]);
                        Core.Instance.SvManager.Disconnect(player.svPlayer.connection, DisconnectTypes.Banned);
                        var user = Core.Instance.SvManager.database.Users.FindById(args[1]);
                        Core.Instance.SvManager.database.Bans.Insert(user.IP, new Ban { Reason = reason ?? "No reason.", Username = player.username });
                        Core.Instance.SvManager.database.Users.Upsert(user);
                        Debug.Log($"{args[1]} has been banned.");
                    }
                    else
                    {
                        var user = Core.Instance.SvManager.database.Users.FindById(args[1]);
                        if (user != null)
                        {
                            string reason = null;
                            if (args.Length >= 3)
                            {
                                reason = StringBuilder.WithArray(args, args.Length, 2);
                            }
                            Core.Instance.SvManager.database.Bans.Insert(user.IP, new Ban { Reason = reason ?? "No reason.", Username = user.ID });
                            Core.Instance.SvManager.database.Users.Upsert(user);
                            Debug.Log($"{args[1]} has been banned.");
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
            else if (first == "unban")
            {
                if (args.Length >= 2)
                {
                    if (!Core.Instance.SvManager.TryGetUserData(args[1], out var user))
                    {
                        Debug.Log("User not found");
                        return;
                    }

                    if (Core.Instance.SvManager.database.Bans.Delete(user.IP))
                    {
                        Core.Instance.SvManager.database.Users.Upsert(user);
                        Debug.Log($"{args[1]} has been unbanned.");

                    }
                    else
                    {
                        Debug.Log($"{args[1]} was not banned");
                    }



                }
                else
                {
                    Debug.Log("Use the format \"unban {player}\".");
                }
            }
            else if (first == "baninfo")
            {

                if (args.Length >= 2)
                {
                    if (Core.Instance.SvManager.TryGetUserData(args[1], out var user))
                    {
                        var userInfo = Core.Instance.SvManager.database.Bans.FindById(user.IP);
                        Debug.Log("-User: " + userInfo.Username + "\n-Reason: " + userInfo.Reason);
                    }
                    else
                    {
                        Debug.Log("User was not found");
                    }
                }
                else
                {
                    Debug.Log("Use the format \"banInfo {player}\".");
                }
            }
            else if (first == "help")
            {
                Debug.Log("Commands Help:" +
                    "\n" + "-Help: Show this menu.\n\tHelp" +
                    "\n" + "-Give: Gives a player x items.\n\tGive {player} {item} {quantity-optional}" +
                    "\n" + "-AddGroup: Adds a player to a group.\n\tAddGroup {player} {group}" +
                    "\n" + "-Groups: Shows all groups the server has.\n\tGroups" +
                    "\n" + "-Say: Sends a message to all users on the server.\n\tSay {message}" +
                    "\n" + "-Online: Shows all players that are online.\n\tOnline" +
                    "\n" + "-Ban: Bans a player with a reason.\n\tBan {user} {reason-optional}" +
                    "\n" + "-Unban: Unbans a player.\n\tUnban {user}" +
                    "\n" + "-Baninfo: Shows the info of a player ban.\n\tBaninfo {user}" +
                    "\n" + "-Tp/Teleport: Teleports a player to another player.\n\tTp {user} {target}" +
                    "\n" + "-Banlist: Lists all players that are banned.\n\tBanlist" +
                    "\n" + "You can use uppercase or lowercase with the command you want to run"
                    );
            }
            else if (first == "quote")
            {
                foreach (var a in args)
                {
                    Debug.Log(a);
                }
            }
            else if (first == "tp" || first == "teleport")
            {
                if (args.Length >= 2)
                {
                    if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[1]))
                    {
                        var player = Core.Instance.SvManager.connectedPlayers.Values.First(i => i.username == args[1]);
                        if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[2]))
                        {
                            var target = Core.Instance.SvManager.connectedPlayers.Values.First(i => i.username == args[2]);
                            player.SetPositionSafe(target.GetPosition);
                        }
                        else
                        {
                            Debug.Log("Target not found");
                        }
                    }
                    else
                    {
                        Debug.Log("Player not found");
                    }
                }
                else
                {
                    Debug.LogError("Use the format \"tp {user} {target}\".");
                }
            }else if(first == "banlist")
            {
                foreach(var banned in Core.Instance.SvManager.database.Bans.FindAll())
                {
                    Debug.Log("{\n" + "\t" + "Username: "+banned.Username + "\n" + "\t" +"Reason: "+ banned.Reason + "\n}");
                }
            }

            void GiveItem(ShPlayer player, string itemName, int quantity = 1)
            {

                if (items.Any(x => x.name == itemName))
                {
                    Debug.Log("Found Item");
                    player.TransferItem(DeltaInv.AddToMe, items.First(x => x.name == itemName), quantity);
                    Debug.Log($"Gave {player.username} {quantity} {itemName}'s");
                }
                else
                {
                    Debug.Log("Object not found");
                }
            }
        }
    }
}
