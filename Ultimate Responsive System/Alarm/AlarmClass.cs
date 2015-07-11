using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace Alarm
{
    public static class AlarmClass
    {
        public static void PlaySound(string file)
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer {URL = file};
            player.controls.play();
        }

        public static async void WakeAt(DateTime dateTime, string wakeSound)
        {
            TimeSpan timeRemaining = dateTime - DateTime.Now;
            await Task.Delay(timeRemaining);
            Console.WriteLine("Elapsed!");
            PlaySound(wakeSound);
        }
    }
}
