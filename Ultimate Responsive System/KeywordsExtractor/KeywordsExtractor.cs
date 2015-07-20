using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace UltimateResponsiveSystem.Functionality.DomainSpecific
{
    using Structure.Base;

    public static class KeywordsExtractor
    {
        public static string GenerateKeywordsReport()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var moduleObjects =
                assemblies
                .Select(a => a.GetExportedTypes()) // get all types from assembly
                .Select(GetModuleType) // get Module type from types
                .Where(mt => mt != null) // filter out the null values
                .Select(mt => Activator.CreateInstance(mt, null, null)) // create an instanse of the module type
                .Cast<Module>()
                .ToArray(); 

            var moduleDataList =
                ""
                .Select(c => new
                {
                    IsDependency = false,
                    DependencyPriority = 0,
                    CommandString = string.Empty
                }).ToList();

            foreach (Module module in moduleObjects)
            {
                var attributes =
                    module
                    .GetType()
                    .GetCustomAttributes(false);

                bool isDependency = false;
                int dependencyPriority = -1;
                foreach (object attribute in attributes)
                {
                    var dependencyAttribute = attribute as DependencyAttribute;
                    if (dependencyAttribute != null)
                    {
                        isDependency = true;
                        dependencyPriority = dependencyAttribute.Priority;
                    }
                }

                moduleDataList.Add(new
                {
                    IsDependency = isDependency,
                    DependencyPriority = dependencyPriority,
                    CommandString = module.Name + ": " + LoadCommandKeywords(module.GetType())
                });
            }

            return String.Join("\r\n",
                moduleDataList
                .OrderBy(d => d.IsDependency)
                .ThenByDescending(d => d.DependencyPriority)
                .Select(d => d.CommandString));
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

        private static string LoadCommandKeywords(Type moduleType)
        {
            var assembly = moduleType.Assembly;

            foreach (Type type in assembly.GetExportedTypes())
            {
                if (type.Name != "Resources") continue;

                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.Name != "ResourceManager") continue;

                    ResourceManager resourceManager = (ResourceManager)
                        propertyInfo.GetMethod.Invoke(
                            null, BindingFlags.InvokeMethod, null, new object[] { }, null);

                    return resourceManager.GetString("CommandKeywords");
                }
            }
            return null;
        }
    }
}
