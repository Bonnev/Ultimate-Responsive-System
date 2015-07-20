using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UltimateResponsiveSystem.Functionality.Testers
{
    class LabelTextWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

        Label label = null;

        public LabelTextWriter(Label output)
        {
            label = output;
        }

        public override void Write(string value)
        {
            base.Write(value);
            label.Dispatcher.BeginInvoke(new Action(() =>
            {
                label.Content = "> " + value;
            }));
        }
    }
}
