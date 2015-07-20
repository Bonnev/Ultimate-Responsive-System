using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace UltimateResponsiveSystem.Structure.Base
{
    using Functionality.DomainSpecific;

    public class Motherboard
    {
        public Dictionary<string, Module> Modules { get; private set; }

        public Motherboard()
        {
            Modules = new Dictionary<string, Module>();
        }

        public void AttachModule(string absoluteDllPath, bool errorIfAlreadyAttached = false)
        {
            Assembly moduleAssembly = Assembly.LoadFile(absoluteDllPath);

            Type moduleType = GetModuleType(moduleAssembly.GetTypes());

            if (moduleType == null) throw new InvalidDataException("The file provided does not contain a Module (no classes in the assembly inherit the Module class).");

            if(Modules.ContainsKey(moduleType.Name))
            {
                if (errorIfAlreadyAttached)
                {
                    throw new InvalidOperationException("Module already attached!");
                }
                return;
            }

            Module moduleObject = (Module)Activator.CreateInstance(moduleType, null, null);
            Modules.Add(moduleType.Name, moduleObject);
        }

        public void Detach()
        {

        }

        public bool TryModuleExecute(string command, string moduleName)
        {
            return Modules[moduleName].TryCommandManage(command);
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
