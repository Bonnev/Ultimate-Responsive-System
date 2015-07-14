using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using UltimateResponsiveSystem.Module;

namespace Console_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandResources.UpdateDllFolderPath();
            //string[] commands = File.ReadAllLines("commands.txt");
            Motherboard motherboard = new Motherboard();
            motherboard.AttachModule(CommandResources.GetCommandDll(Commands.Alarm));
            //motherboard.AttachModule(CommandResources.GetCommandDll(Commands.Emergency));
            string command = Console.ReadLine();

            while (true)
            {
                switch (command.Split(' ').First().ToLower())
                {
                    case "exit":
                    case "quit":
                        return;
                    case "wake":
                        motherboard.FindMatchingModule(command);
                        break;
                    case "porn":
                        motherboard.FindMatchingModule(command);
                        break;
                    case "update":
                        string keywordReport =  motherboard.GenerateKeywordReport(CommandResources.DllFolderPath);
                        File.WriteAllText("keywords.txt", keywordReport);
                        break;
                }
                command = Console.ReadLine();
            }
        }
    }
}
