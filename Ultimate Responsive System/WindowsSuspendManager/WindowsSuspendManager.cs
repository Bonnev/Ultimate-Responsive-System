using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsSuspendManager
{
    using UltimateResponsiveSystem.Structure.Base;
    public class WindowsSuspendManager : Module
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr pArgToCompletionRoutine, bool fResume);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CancelWaitableTimer(IntPtr hTimer);

        [DllImport("powrprof.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        private IntPtr _waitableHandle;

        public override string Name
        {
            get { return "WindowsSuspendManager"; }
        }

        public override void Execute(params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string Status()
        {
            throw new NotImplementedException();
        }

        public override bool TryCommandManage(string command)
        {
            string[] words = command.Split(' ');

            if (words[0] == "wake")
            {
                int indexOfMeUs = command.IndexOf("me", StringComparison.Ordinal);
                if (indexOfMeUs == -1) indexOfMeUs = command.IndexOf("us", StringComparison.Ordinal);
                if (indexOfMeUs > 4 && command.Substring(indexOfMeUs - 5, 4) != "with") return false;

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
                            if (DateTime.Now.Hour >= 12) time += 12;
                            DateTime now = DateTime.Now;
                            DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, time, 00, 00);
                            if (dateTime < now) dateTime = dateTime.AddDays(1);
                            SetWake(dateTime);
                            return true;
                        }

                        //e.g. at 10:20:00; at 00:30:24
                        int indexOfColumn = timeParts[0].IndexOf(':');
                        if (indexOfColumn != -1)
                        {
                            timeParts = timeParts[0].Split(':');

                            int hours;
                            if (!Int32.TryParse(timeParts[0], out hours)) return false;
                            int minutes;
                            if (!Int32.TryParse(timeParts[1], out minutes)) return false;
                            int seconds = 0;
                            if (timeParts.Length > 2 && !Int32.TryParse(timeParts[2], out seconds)) return false;

                            DateTime now = DateTime.Now;
                            DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds);
                            if (dateTime < now) dateTime = dateTime.AddDays(1);
                            SetWake(dateTime);
                            return true;
                        }
                    }
                }

            }
            else if (words[0] == "sleep")
            {
                Sleep();
                return true;
            }
            else if (words[0] == "hibernate")
            {
                Hibernate();
                return true;
            }
            return false;
        }

        private void SetWake(DateTime dateTime)
        {

            long wakeTime = dateTime.ToFileTime();
            IntPtr handle = CreateWaitableTimer(IntPtr.Zero, true, "WaitableTimer");

            if (!SetWaitableTimer(handle, ref wakeTime, 0, IntPtr.Zero, IntPtr.Zero, true))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            _waitableHandle = handle;

            Console.WriteLine("---- I'll wake up then!");
        }

        private bool Sleep()
        {
            Console.WriteLine("---- Good night!");

            return SetSuspendState(false, false, false);
        }

        private bool Hibernate()
        {
            Console.WriteLine("---- Goodbye!");

            return SetSuspendState(true, false, false);
        }
    }
}
