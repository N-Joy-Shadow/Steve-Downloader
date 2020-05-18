using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes;
using MaterialDesignColors;
using System.Windows.Media.Animation;
using Syncfusion.UI.Xaml.ProgressBar;
using MetroFramework;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Effects;
using MaterialDesignThemes.Wpf;
using System.Management;
using steve_downloader.second_window;
using steve_downloader.download;
using System.Management;
using System.Windows.Threading;

namespace steve_downloader
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DropShadowEffect shadowEffect;
        public static bool open_window = true;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void open_bool()
        {
            open_window = true;
            return;
        }

        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        private void TopGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
        }

        private void Quiet_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void minize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Install_Start_Click(object sender, RoutedEventArgs e)
        {
            if (open_window == true)
            {
                second install_page = new second();
                install_page.Owner = Application.Current.MainWindow;
                install_page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                open_window = false;
                install_page.Show();
            }
            else
            {
                return;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Download asg = new Download();
        }

        private void SideMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            
            this.shadowEffect = new DropShadowEffect
            {
                ShadowDepth = 2,
                BlurRadius = 6
            };
            Grid.SetColumnSpan(SideMenu, 2);
            SideMenu.Effect = this.shadowEffect;
        }

        private void SideMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            this.shadowEffect = new DropShadowEffect
            {
                ShadowDepth = 0,
                BlurRadius = 0
            };
            Grid.SetColumnSpan(SideMenu, 1);
            SideMenu.Effect = this.shadowEffect;
        }
        private void get_system_information()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject obj in myVideoObject.Get())
                {
                    text_vga.Text = ": " + obj["Name"];
                }

                ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject obj in myProcessorObject.Get())
                {
                    text_cpu.Text = ": " + obj["Name"];
                }

                ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
                ManagementObjectCollection results = searcher.Get();
                foreach (ManagementObject result in results)
                {
                    text_rem.Text = ": " + SizeSuffix((long)Convert.ToDouble(result["TotalVisibleMemorySize"]));
                }

            }));
        }
 

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(get_system_information));
            t.Start();
        }

        static readonly string[] SizeSuffixes = {"KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };



    }
}
