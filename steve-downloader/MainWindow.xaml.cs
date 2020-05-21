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
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace steve_downloader
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DropShadowEffect shadowEffect;
        PerformanceCounter cpuCounter;
        public static bool open_window = true;
        public static bool open_window_visiable = true;
        public static string ram_slide_value = "0";
        public static string ram_capable;
        public static string slider_value;
        public string json_test; 






        public MainWindow()
        {
            InitializeComponent();
        }
        public string Raqm(string value)
        {
            ram_slide_value = value;
            return ram_slide_value;

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
                    ram_capable = SizeSuffixMb((long)Convert.ToDouble(result["TotalVisibleMemorySize"]));

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
                java_version.Text = ": " + strOutput;

                //slider set
                double ram_limit;
                ram_limit = Convert.ToDouble(ram_capable);
                ram_slider.Maximum = ram_limit;
                if (ram_limit <= 4100)
                {
                    ram_slider.TickFrequency = 1024;
                }
                else if (ram_limit <= 8100 && ram_limit > 4100)
                {
                    ram_slider.TickFrequency = 2048;
                }
                else
                {
                    ram_slider.TickFrequency = 4096;
                }
                ram_rate.Text = ram_capable + " / ";

                //for (int i = 0; i < 10; i++)
                //{
                //    ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
                //    double a = cpuCounter.NextValue();
                //    MessageBox.Show(Convert.ToString(a));
                //}


            }));
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\data.json")) 
            {
               string a =  File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\data.json");
                //var json4 = JObject.FromObject(new { id = "J01", name = "June", age = 23 });

                //string json = JsonConvert.SerializeObject(json4);
                //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\data.json", json);
            }
            else
            {
                string json_path = AppDomain.CurrentDomain.BaseDirectory + @"\data.json";
                FileStream stream = File.Create(json_path);
                stream.Close();

            }



            //컴퓨터 정보
            Thread t = new Thread(new ThreadStart(get_system_information));
            t.Start();
            //프로그래스바
            BackgroundWorker worker_ram = new BackgroundWorker();
            BackgroundWorker worker_cpu = new BackgroundWorker();
            worker_cpu.WorkerReportsProgress = true;
            worker_ram.WorkerReportsProgress = true;
            worker_cpu.DoWork += worker_cpu_DoWork;
            worker_ram.DoWork += worker_ram_DoWork;
            worker_cpu.ProgressChanged += worker_Cpu_ProgressChanged;
            worker_ram.ProgressChanged += worker_ram_ProgressChanged;
            worker_cpu.RunWorkerAsync(1000);
            worker_ram.RunWorkerAsync(1000);
        }
        //스티브 갤러기 열기
        private void Direct_visit_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://gall.dcinside.com/mgallery/board/lists/?id=steve&page=1");
        }

        private void ram_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Raqm(Convert.ToString(Math.Round(ram_slider.Value)));

        }

        void worker_cpu_DoWork(object sender, DoWorkEventArgs e)
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            for (; ; )
            {
                double a = cpuCounter.NextValue();
                (sender as BackgroundWorker).ReportProgress(Convert.ToInt32(a));
                System.Threading.Thread.Sleep(1000);
            }
        }
        void worker_ram_DoWork(object sender, DoWorkEventArgs e)
        {
            for (; ; )
            {
                Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
                decimal percentFree = ((decimal)phav / (decimal)tot) * 100;
                decimal percentOccupied = 100 - percentFree;
                (sender as BackgroundWorker).ReportProgress(Convert.ToInt32(percentOccupied));
                System.Threading.Thread.Sleep(1200);
            }
        }
        void worker_Cpu_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            cpu_progressbar.Value = e.ProgressPercentage;
            cpu_using.Text = " " + Convert.ToString(cpu_progressbar.Value) + "%";
        }
        void worker_ram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ram_progressbar.Value = e.ProgressPercentage;
            ram_using.Text = " " + Convert.ToString(ram_progressbar.Value) + "%";
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
        }

        private void luanch_mincraft_Click(object sender, RoutedEventArgs e)
        {
            string jsonUpdateFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\data.json");
            JObject jsonO = JObject.Parse(jsonUpdateFile);
            //json 파일을 이용하여 경로 재설정 예정
            if (File.Exists(Convert.ToString(jsonO["basic_path"])))
            {
                System.Diagnostics.Process.Start(Convert.ToString(jsonO["basic_path"]));
            }
            else if (File.Exists(Convert.ToString(jsonO["selected_path"])))
            {
                System.Diagnostics.Process.Start(Convert.ToString(jsonO["selected_path"]));
            }
            else
            {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Filter = "Exe files(*.exe) | *.exe;",
                    Multiselect = false,
                    Title = "마크 선택해라"
                };
                dialog.ShowDialog();
                jsonO.Add("selected_path", dialog.FileName);
                string convert_json = Convert.ToString(jsonO);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\data.json", convert_json);

            }
            

        }
    }
    //램 설정
    public static class PerformanceInfo
        {
            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

            [StructLayout(LayoutKind.Sequential)]
            public struct PerformanceInformation
            {
                public int Size;
                public IntPtr CommitTotal;
                public IntPtr CommitLimit;
                public IntPtr CommitPeak;
                public IntPtr PhysicalTotal;
                public IntPtr PhysicalAvailable;
                public IntPtr SystemCache;
                public IntPtr KernelTotal;
                public IntPtr KernelPaged;
                public IntPtr KernelNonPaged;
                public IntPtr PageSize;
                public int HandlesCount;
                public int ProcessCount;
                public int ThreadCount;
            }

            public static Int64 GetPhysicalAvailableMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }

            public static Int64 GetTotalMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }
        }
    
}
