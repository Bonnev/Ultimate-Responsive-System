using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Tester
{
    static class CommandResources
    {
        public const string DllFolderName = "DLLs";
        public static string DllFolderPath = DllFolderName;

        public static readonly string[] DllNames =
        {
            "Alarm.dll",
            "Emergency.dll"
        };

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

        public static string GetCommandDll(Commands command)
        {
            int commandIndex = Convert.ToInt32(command);
            return String.Format("{0}\\{1}", DllFolderPath, DllNames[commandIndex]);
        }
    }
}
