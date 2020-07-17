using UnityEngine;
namespace ConsoleCommands.Commands.Moderation
{
    class GetUserInfo : Command
    {
        public override string command => "getuser";

        public override string Name => "GetUserInfo";

        public override string Description => "Gets the information of an user.";

        public override void Run(string[] args)
        {
            if(Core.Instance.SvManager.database.Users.Exists(args[0]))
            {
                var user = Core.Instance.SvManager.database.Users.FindById(args[0]);
                Debug.Log(user.ID);
                Debug.Log(user.JoinDate);
                Debug.Log(user.Character);
            }
        }
    }
}
