using System;
using System.IO;
using System.Reflection;

namespace UltimateResponsiveSystem.Functionality.DomainSpecific
{
    public static class AssemblyLoader
    {

        public static void LoadAssemblies(string[] assembliesFilePaths)
        {
            foreach (string filePath in assembliesFilePaths)
            {
                ValidatePath(filePath);
                try
                {
                    Assembly.LoadFile(filePath);
                }
                catch (BadImageFormatException) { }
            }
        }

        private static void ValidatePath(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (!File.Exists(path))
                throw new ArgumentException(String.Format("path \"{0}\" does not exist", path));
        }
    }
}
