using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.Windows.Media.Effects;
using System.Management;
using steve_downloader.second_window;
using steve_downloader.download;
using System.Windows.Threading;
using System.Diagnostics;

namespace steve_downloader
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DropShadowEffect shadowEffect;
        public static bool open_window = true;
        public static bool open_window_visiable = true;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void open_bool()
        {
            open_window_visiable = true;
            return;
        }      
        //  byte -> GB
        static readonly string[] SizeSuffixes = { "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }       
        static string SizeSuffixMb(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffixMb(-value); }
            if (value == 0) { return "0.0 bytes"; }
            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));
            return string.Format("{0}", Math.Round(adjustedSize * 1024));
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
            second install_page = new second();
            install_page.Owner = Application.Current.MainWindow;
            install_page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (open_window == false)
            {
                if (open_window_visiable == true)
                {
                    open_window_visiable = false;
                    install_page.Visibility = Visibility.Visible;
                }
            }
            else
            {
                open_window = false;
                install_page.Show();
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
            Dispatcher.Invoke(DispatcherPriority.SystemIdle, new Action(delegate
            {
                //CPU
                ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject obj in myVideoObject.Get())
                {
                    text_vga.Text = ": " + obj["Name"];
                }
                //VGA
                ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject obj in myProcessorObject.Get())
                {
                    text_cpu.Text = ": " + obj["Name"];
                }
                //Ram
                ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
                ManagementObjectCollection results = searcher.Get();
                foreach (ManagementObject result in results)
                {
                    text_ram.Text = ": " + SizeSuffix((long)Convert.ToDouble(result["TotalVisibleMemorySize"]));
                    ram_rate.Text = "400000 / " + SizeSuffixMb((long)Convert.ToDouble(result["TotalVisibleMemorySize"]));
                }
                //java information
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "java.exe";
                psi.Arguments = " -version";
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
               
                Process pr = Process.Start(psi);
                string strOutput = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");
                java_version.Text = strOutput;

            }));
        }
 

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(get_system_information));
            t.Start();
        }
        //스티브 갤러기 열기
        private void Direct_visit_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://gall.dcinside.com/mgallery/board/lists/?id=steve&page=1");
        }
    }
}
