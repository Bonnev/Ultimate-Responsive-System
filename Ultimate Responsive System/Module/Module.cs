using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UltimateResponsiveSystem.Module
{
    public abstract class Module
    {
        public abstract string Name { get;}

        public abstract void Execute(params object[] parameters);

        public abstract string Status();

        public abstract bool TryCommandManage(string command);

    }
}