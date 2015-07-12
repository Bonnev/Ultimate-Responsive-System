using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace UltimateResponsiveSystem.Module
{
    public class Alarm : Module
    {
        public static void PlaySound(string file)
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer {URL = file};
            player.controls.play();
        }

        public override string Name { get { return "Alarm"; } }

        public override async void Execute(params object[] parameters)
        {
            if (!(parameters[0] is DateTime) || !(parameters[1] is string)) throw new ArgumentException("Alarm Execute method has invalid parameters - first should be DateTime, second string.");

            DateTime dateTime = (DateTime) parameters[0];
            string wakeSound = (string) parameters[1];

            TimeSpan timeRemaining = dateTime - DateTime.Now;
            await Task.Delay(timeRemaining);
            Console.WriteLine("Elapsed!");
            PlaySound(wakeSound);
        }

        public override string Status()
        {
            throw new NotImplementedException();
        }

        public override bool TryCommandManage(string command)
        {
            string[] commandParts = command.Split(' ').Select(p => p.ToLower()).ToArray();
            for (int i = 0; i < commandParts.Length; i++)
            {
                if (commandParts[i] == "at")
                {
                    string[] dateTimeParts = commandParts[i + 1].Split(':');
                    int hours = Int32.Parse(dateTimeParts[0]);
                    int minutes = Int32.Parse(dateTimeParts[1]);
                    int seconds = 0;
                    if (dateTimeParts.Length > 2) seconds = Int32.Parse(dateTimeParts[2]);
                    DateTime now = DateTime.Now;
                    DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds);

                    Execute(dateTime, @"H:\C#\Ultimate-Responsive-System\Ultimate Responsive System\Console Tester\bin\Debug\KRISKO ft DIM4OU - Zlatnite Momcheta.mp3");
                    return true;
                }
            }
            return false;
        }
    }
}
