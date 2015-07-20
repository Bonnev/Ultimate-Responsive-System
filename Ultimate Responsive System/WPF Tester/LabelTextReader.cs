using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup.Localizer;

namespace UltimateResponsiveSystem.Functionality.Testers
{
    class LabelTextReader : TextReader
    {
        private Label StatusLabel;
        public bool KeyPressed = false;
        public Window ActiveWindow;
        public string StandardOutput;

        public LabelTextReader(Label label, Window window)
        {
            this.ActiveWindow = window;
            this.StatusLabel = label;
        }

        public void StartReadKey()
        {
            StatusLabel.Content = "> ";
            ActiveWindow.KeyUp += KeyUp;
        }

        private void KeyUp(object obj, KeyEventArgs args)
        {
            if (args.Key == Key.Return)
            {
                StandardOutput = StatusLabel.Content.ToString();
                ActiveWindow.KeyUp -= KeyUp;
                OutputReady(StatusLabel.Content.ToString());
                return;
            }
            string stringToAppend = "\0" ;
            string input = args.Key.ToString();
            if (input.Length == 1 &&
                input[0] >= 'A' && input[0] <= 'Z') stringToAppend = input;
            else if(input.Length == 2 && input[1] >= '0' && input[1] <= '9') stringToAppend = input[1].ToString();
            else if(input.Length == 7 && input.Last() >= '0' && input.Last() <= '9') stringToAppend = input.Last().ToString();
            switch (args.Key)
            {
                case Key.Space:
                    stringToAppend = " ";
                    break;
                case Key.Back:
                    stringToAppend = "\b";
                    break;
            }
            StatusLabel.Dispatcher.BeginInvoke(new Action(() =>
            {
                if(stringToAppend == "\b")
                StatusLabel.Content = StatusLabel.Content.ToString().Substring(0, StatusLabel.Content.ToString().Length-1);

                else
                    StatusLabel.Content = StatusLabel.Content + stringToAppend;
            }));
        }
        public delegate void OutputReadyHandler(string output);

        public event OutputReadyHandler OutputReady;
    }
}
