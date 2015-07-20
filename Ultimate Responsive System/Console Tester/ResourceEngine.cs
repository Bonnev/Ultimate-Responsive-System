using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateResponsiveSystem.Structure.Base;

namespace UltimateResponsiveSystem.Functionality.Testers
{
    static class ResourceEngine
    {
        public const string KeywordsFilePath = "keywords.txt";
        public const string DllFolderName = "DLLs";
        public static string DllFolderPath = DllFolderName;

        private static List<ModuleData> ModuleData = new List<ModuleData>();
        private static Motherboard motherboard = new Motherboard();

        public static void UpdateDllFolderPath()
        {
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string folderPath = exePath;
            while (!Directory.GetDirectories(folderPath).Select(path => path.Split('\\').Last()).Contains(DllFolderName))
            {
                folderPath = Directory.GetParent(folderPath).FullName;
            }
            DllFolderPath = folderPath + "\\" + DllFolderName;
        }

        /*public static string GetCommandDll(Commands command)
        {
            int commandIndex = Convert.ToInt32(command);
            return String.Format("{0}\\{1}", DllFolderPath, ModuleFilePaths[commandIndex]);
        }*/

        public static void UpdateKeywords()
        {
            string report = motherboard.GenerateKeywordReport(DllFolderPath);
            File.WriteAllText(KeywordsFilePath, report);
            Console.WriteLine("\r----Keywords updated!");
        }

        public static bool TryFindMatchingModule(string command)
        {
            bool moduleFound = false;

            foreach (string word in command.Split(' '))
            {
                foreach (ModuleData data in ModuleData)
                {
                    foreach (string keyword in data.Keywords)
                    {
                        if (word == keyword)
                        {
                            motherboard.AttachModule(data.Path);
                            moduleFound = motherboard.TryModuleExecute(command, data.Name);
                        }
                    }
                }
            }

            return moduleFound;
        }

        public static void LoadModules()
        {
            LoadModuleInfo(KeywordsFilePath);
            PopulateDllData();
            Console.WriteLine("\r----Modules reloaded!");
        }

        private static void LoadModuleInfo(string infoFileName)
        {
            string[] data = File.ReadAllLines(infoFileName);
            ModuleData = new List<ModuleData>();

            foreach (string s in data)
            {
                string[] sParts = s.Split(':').Select(part => part.Trim()).ToArray();
                string moduleName = sParts[0];
                string[] keywords = sParts[1].Split(';');
                ModuleData info = new ModuleData(moduleName, keywords);
                
                ModuleData.Add(info);
            }
        }

        private static void PopulateDllData()
        {
            foreach (ModuleData data in ModuleData)
            {
                data.Path = FindDllForModule(data.Name);
            }
        }

        private static string FindDllForModule(string moduleName)
        {
            return
                Directory
                    .GetFiles(DllFolderPath)
                    .Where(f => Path.GetExtension(f) == ".dll")
                    .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == moduleName);
        }
    }
}
