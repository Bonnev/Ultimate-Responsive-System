﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateResponsiveSystem.Module
{
    public class Emergency : Module
    {
        public override string Name
        {
            get { return "Emergency"; }
        }

        public override void Execute(params object[] parameters)
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            Object shellObject = Activator.CreateInstance(shellAppType);
            shellAppType.InvokeMember("ToggleDesktop",BindingFlags.InvokeMethod, null, shellObject, null);
            /*Shell32.Shell objShel = new Shell32.Shell();

            // Show the desktop
            ((Shell32.IShellDispatch4)objShel).ToggleDesktop();*/

            // Do some operations here

            // Restore the desktop
            //((Shell32.IShellDispatch4)objShel).ToggleDesktop();
        }

        public override string Status()
        {
            throw new NotImplementedException();
        }

        public override bool TryCommandManage(string command)
        {
            this.Execute();
            return true;
        }
    }
}