using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateResponsiveSystem.Module
{
    public class Motherboard
    {
        public List<Type> Modules { get; private set; }

        public Motherboard()
        {
            Modules = new List<Type>();
        }

        public void AttachModule(string absoluteDllPath)
        {
            Assembly moduleAssembly = Assembly.LoadFile(absoluteDllPath);

            Type moduleType = GetModuleType(moduleAssembly.GetTypes());

            if(moduleType == null) throw new InvalidDataException("The file provided does not contain a Module (no classes in the assembly inherit the Module class.");

            Modules.Add(moduleType);
        }

        public void Detach()
        {
            
        }

        public void FindMatchinModule(string command)
        {
            object obj = Activator.CreateInstance(Modules[0], null, null);
            Modules[0].GetMethod("TryCommandManage").Invoke(obj, new object[] { command });
        }

        private static Type GetModuleType(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (type.IsClass && (typeof(Module)).IsAssignableFrom(type))
                {
                    return type;
                }
            }
            return null;
        }

        private static bool FilterInterface(Type typeObj, Object criteriaObj)
        {
            if (typeObj.FullName == ((Type)criteriaObj).FullName)
                return true;
            else
                return false;
        }
    }
}
