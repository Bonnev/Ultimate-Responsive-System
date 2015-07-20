using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UltimateResponsiveSystem.Functionality.Testers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private string output;

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            StackPanel pn2 = new StackPanel();
            Grid gr2 = new Grid();

            Label lbl = new Label();

            TextWriter writer = new LabelTextWriter(lbl);
            Console.SetOut(writer);
            lbl.Content = "Load is very high! Please wait...";
            Console.Write("dasda");
            lbl.Margin = new Thickness(200,
                System.Windows.SystemParameters.PrimaryScreenHeight / 2, 0, 0);
            lbl.FontSize = 35;
            lbl.FontFamily = new FontFamily("Consolas");
            lbl.Foreground = Brushes.White;
            gr2.Children.Add(lbl);
            pn2.Children.Add(gr2);

            Window window2 = new Window();
            LabelTextReader textReader = new LabelTextReader(lbl, window2);
            textReader.StartReadKey();
            textReader.OutputReady += output1 =>
            {
                //window2.Close();
                output = output1;
                Console.Write("");
                //tb.Text = textReader.StandardOutput;
            };

            Console.Write("");
            window2.Topmost = true;
            window2.WindowState = WindowState.Maximized;
            window2.WindowStyle = WindowStyle.None;
            window2.AllowsTransparency = true;
            window2.Background = Brushes.Gray;
            window2.Opacity = 0.8;
            window2.IsHitTestVisible = true;
            window2.Content = pn2;
            window2.ShowInTaskbar = false;
            window2.Show();
            window2.Activate();
        }

        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        private string customHis;

        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            NavigationWindow window = new NavigationWindow();
            SetWindowExTransparent(new WindowInteropHelper(window).Handle);
            window.Title = "Soon!";
            Uri source = new Uri("http://www.google.com", UriKind.Absolute);

            window.Navigating += (o, args) => { customHis = customHis + ((NavigationWindow)o).CurrentSource; };
            window.Navigated += (o, args) => { customHis = customHis + ((NavigationWindow)o).CurrentSource; MessageBox.Show(customHis.ToString()); };
            window.Source = source;
            window.ShowsNavigationUI = false;
            window.Closing += (o, args) =>
            {
                args.Cancel = true;
                ((NavigationWindow)o).Navigate(new Uri("http://www.softuni.bg", UriKind.Absolute));

                IEnumerable back = window.BackStack;
                foreach (var vara in back)
                {
                    MessageBox.Show(vara.ToString());
                }
            };
            //window.AllowsTransparency = true;
            //window.Opacity = 1;
            window.WindowStyle = WindowStyle.None;
            window.Show();
        }
    }
}
