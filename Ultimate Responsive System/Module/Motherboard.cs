using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace UltimateResponsiveSystem.Module
{
    using DomainSpecific;

    public class Motherboard
    {
        public List<Module> Modules { get; private set; }

        public Motherboard()
        {
            Modules = new List<Module>();
        }

        public void AttachModule(string absoluteDllPath)
        {
            Assembly moduleAssembly = Assembly.LoadFile(absoluteDllPath);

            Type moduleType = GetModuleType(moduleAssembly.GetTypes());

            if(moduleType == null) throw new InvalidDataException("The file provided does not contain a Module (no classes in the assembly inherit the Module class).");

            Module moduleObject = (Module)Activator.CreateInstance(moduleType, null, null);
            Modules.Add(moduleObject);
        }

        public void Detach()
        {
            
        }

        public void FindMatchingModule(string command)
        {
            Modules[0].TryCommandManage(command);
        }

        public string GenerateKeywordReport(string dllFolderPath)
        {
            var dlls = Directory.GetFiles(dllFolderPath).Where(f => Path.GetExtension(f) == ".dll").ToArray();

            AppDomain domain = AppDomain.CreateDomain("dllLoader");
            var mediator = (DomainMediator)
                domain
                .CreateInstanceFromAndUnwrap(
                    typeof(DomainMediator).Assembly.Location,
                    typeof(DomainMediator).FullName);
            
            mediator.LoadDlls(dlls);
            string keywordsReport = mediator.GenerateKeywordsReport();

            AppDomain.Unload(domain);
            return keywordsReport;
        }

        private static Type GetModuleType(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsAbstract && (typeof(Module)).IsAssignableFrom(type))
                {
                    return type;
                }
            }
            return null;
        }
    }
}
