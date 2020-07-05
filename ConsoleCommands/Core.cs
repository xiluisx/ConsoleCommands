using BrokeProtocol.API;
using BrokeProtocol.Managers;

namespace ConsoleCommands
{
    public class Core : Plugin
    {
        public static Core Instance { get; internal set; }

        public SvManager SvManager { get; set; }

        public Core()
        {
            Instance = this;
            Info = new PluginInfo("ConsoleCommands", "ConsCmd")
            {
                Description = "This plugin adds commands to the console"
            };
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
