namespace ConsoleCommands.Commands
{
    public class Say : Command
    {
        public override string command => "say";

        public override string Name => "Say";

        public override string Description => "Sends a message to the whole server.";

        public override void Run(string[] args)
        {
            foreach (var player in Core.Instance.SvManager.connectedPlayers.Values)
            {
                player.svPlayer.SendGameMessage(StringBuilder.WithArray(args,args.Length));
            }
        }
    }
}
