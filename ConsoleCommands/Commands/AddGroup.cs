using BrokeProtocol.API;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ConsoleCommands
{
    class AddGroup : Command
    {
        public override string command => "addgroup";

        public override string Name => "AddGroup";

        public override string Description => "Adds a player to a group from groups.json.";

        public override void Run(string[] args)
        {
            if (args.Length >1)
            {
                if (Core.Instance.SvManager.connectedPlayers.Values.Any(i => i.username == args[0]))
                {
                    if (GroupManager.Groups.Values.Any(i => i.Name == args[1]))
                    {
                        GroupManager.Groups.Values.First(i => i.Name == args[1]).AddMember(Core.Instance.SvManager.connectedPlayers.Values.First(x => x.username == args[0]));
                        Debug.LogWarning($"Added role \"{args[1]}\" to player \"{args[0]}\".");
                        if (File.Exists("groups.json"))
                        {
                            string json = File.ReadAllText("groups.json");
                            List<RoleGroup.Group> groups = JsonConvert.DeserializeObject<List<RoleGroup.Group>>(json);
                            groups.First(i => i.Name == args[1]).Members.Add(args[0]);
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
    }
}
