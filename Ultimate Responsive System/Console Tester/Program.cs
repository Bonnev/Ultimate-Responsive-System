using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Console_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //string[] commands = File.ReadAllLines("commands.txt");
            string command = Console.ReadLine();

            while (true)
            {
                switch (command.Split(' ').First().ToLower())
                {
                    case "exit":
                    case "quit":
                        return;
                    case "wake":
                        string[] commandParts = command.Split(' ').Select(p => p.ToLower()).ToArray();
                        for (int i = 0; i < commandParts.Length; i++)
                        {
                            if (commandParts[i] == "at")
                            {
                                string[] dateTimeParts = commandParts[i + 1].Split(':');
                                int hours = Int32.Parse(dateTimeParts[0]);
                                int minutes = Int32.Parse(dateTimeParts[1]);
                                DateTime now = DateTime.Now;
                                DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, hours, minutes, 0);

                                var alarm = Assembly.LoadFile(CommandResources.GetCommandDLL(Commands.Alarm));
                                foreach (Type type in alarm.GetExportedTypes())
                                {
                                    //var c = Activator.CreateInstance(type);
                                    if(type.IsClass)
                                    type.InvokeMember("WakeAt", BindingFlags.InvokeMethod, null, null, new object[] { dateTime, @"KRISKO ft DIM4OU - Zlatnite Momcheta.mp3" });
                                }
                            }
                        }
                        break;
                }
                command = Console.ReadLine();
            }
        }
    }
}
