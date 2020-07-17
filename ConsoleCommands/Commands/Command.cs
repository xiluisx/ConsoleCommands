namespace ConsoleCommands
{
    public abstract class Command
    {
        public abstract string command { get; }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract void Run(string[] args);
    }
}
