using System;
using System.Linq;
using System.Threading.Tasks;
using WMPLib;

namespace UltimateResponsiveSystem.Structure.Modules
{
    using Structure.Base;
    public class Alarm : Module
    {
        private string RingTone =
            @"H:\C#\Ultimate-Responsive-System\Ultimate Responsive System\Console Tester\bin\Debug\KRISKO ft DIM4OU - Zlatnite Momcheta.mp3";

        public override string Name { get { return "Alarm"; } }

        public override async void Execute(params object[] parameters)
        {
            if (!(parameters[0] is DateTime)) throw new ArgumentException("Alarm Execute method has invalid parameters - first should be DateTime.");

            DateTime dateTime = (DateTime)parameters[0];

            TimeSpan timeRemaining = dateTime - DateTime.Now;
            await Task.Delay(timeRemaining);

            PlaySound(RingTone);
        }

        public override string Status()
        {
            throw new NotImplementedException();
        }

        public override bool TryCommandManage(string command)
        {
            string[] commandParts = command.Split(' ').Select(p => p.ToLower()).ToArray();

            int indexOfMeUs = command.IndexOf("me", StringComparison.Ordinal);
            if (indexOfMeUs == -1) indexOfMeUs = command.IndexOf("us", StringComparison.Ordinal);
            if (indexOfMeUs == -1 || indexOfMeUs > 4 && command.Substring(indexOfMeUs - 5, 4) == "with") return false;

            int indexOfAt = command.IndexOf("at", StringComparison.Ordinal);
            if (indexOfAt != -1)
            {
                string[] timeParts = command.Substring(indexOfAt + 3).Split(' ');
                if (timeParts.Length == 1)
                {
                    int time = 0;
                    // e.g. at 5; at 10
                    if (Int32.TryParse(timeParts[0], out time) == true)
                    {
                        if (DateTime.Now.Hour == 12) time += 12;
                        DateTime now = DateTime.Now;
                        DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, time, 00, 00);
                        if (dateTime < now) dateTime = dateTime.AddDays(1);
                        Execute(dateTime);
                        return true;
                    }

                    //e.g. at 10:20:00; at 00:30:24
                    int indexOfColumn = timeParts[0].IndexOf(':');
                    if (indexOfColumn != -1)
                    {
                        timeParts = timeParts[0].Split(':');

                        int hours;
                        if (Int32.TryParse(timeParts[0], out hours)) return false;
                        int minutes;
                        if (Int32.TryParse(timeParts[1], out minutes)) return false;
                        int seconds = 0;
                        if (timeParts.Length > 2 && !Int32.TryParse(timeParts[2], out seconds)) return false;

                        DateTime now = DateTime.Now;
                        DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds);
                        if (dateTime < now) dateTime = dateTime.AddDays(1);
                        Execute(dateTime);
                        return true;
                    }
                }
            }


            return false;
        }

        private static void PlaySound(string file)
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer { URL = file };
            player.controls.play();
        }
    }
}
