using System.Collections.Generic;

namespace ConsoleCommands
{
    class RoleGroup
    {
        public class Group
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Tag { get; set; }
            public List<string> Permissions { get; set; }
            public CustomData CustomData { get; set; }
            public List<string> Members { get; set; }
            public List<string> Inherits { get; set; }
        }

        public partial class CustomData
        {
            public Data Data { get; set; }
        }
        public partial class Data
        {
        }
    }
}
