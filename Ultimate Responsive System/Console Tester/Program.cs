using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateResponsiveSystem.Functionality.Testers
{
    using Structure;

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ResourceEngine.UpdateDllFolderPath();

            string command = Console.ReadLine();

            while (true)
            {
                switch (command.Split(' ').First().ToLower())
                {
                    case "exit":
                    case "quit":
                        return;
                    case "update":
                        Console.Write("Loading...");
                        ResourceEngine.UpdateKeywords();
                        ResourceEngine.LoadModules();
                        break;
                    default:
                        ResourceEngine.TryFindMatchingModule(command);
                        break;
                }
                command = Console.ReadLine();
                /*if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.V && key.Modifiers == ConsoleModifiers.Control)
                    {
                        Console.WriteLine(Clipboard.GetText());
                    }
                }*/
            }
        }
    }
}
