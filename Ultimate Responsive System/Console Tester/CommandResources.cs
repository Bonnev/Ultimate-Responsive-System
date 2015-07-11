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
        public const string DllDirectory = "DLLs";

        public static readonly string[] DllNames =
        {
            "Alarm.dll"
        };

        public static string GetCommandDLL(Commands command)
        {
            int commandIndex = Convert.ToInt32(command);
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            StringBuilder upPath = new StringBuilder(".\\");
            while (!Directory.GetDirectories(upPath.ToString()).Select(path => path.Split('\\').Last()).Contains(DllDirectory))
            {
                upPath.Append("..\\");
            }
            return String.Format("{0}\\{1}{2}\\{3}", exePath, upPath, DllDirectory, DllNames[commandIndex]);
        }
    }
}
