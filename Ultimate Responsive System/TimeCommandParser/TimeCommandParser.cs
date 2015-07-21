using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateResponsiveSystem.Structure.Modules
{
    using Structure.Base;

    [Dependency(10)]
    public class TimeCommandParser : Module
    {
        public override string Name
        {
            get { return "TimeCommandParser"; }
        }

        public override void Execute(params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string Status()
        {
            throw new NotImplementedException();
        }

        public override bool TryCommandManage(string command)
        {
            return false;
        }
    }
}
