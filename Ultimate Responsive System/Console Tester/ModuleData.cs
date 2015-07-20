using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateResponsiveSystem.Functionality.Testers
{
    public class ModuleData
    {
        public string Name { get; set; }
        public string[] Keywords { get; set; }
        public string Path { get; set; }

        public ModuleData(string name, string[] keywords)
        {
            Name = name;
            Keywords = keywords;
        }
    }
}
