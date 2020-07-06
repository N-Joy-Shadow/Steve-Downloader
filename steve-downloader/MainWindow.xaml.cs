using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.Windows.Media.Effects;
using System.Management;
using steve_downloader.second_window;
using System.Windows.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Win32;
using System.Net;
using System.IO.Compression;
using System.Windows.Media;
using log4net;

namespace steve_downloader
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        
        DropShadowEffect shadowEffect;
        PerformanceCounter cpuCounter;
        public string modpack_title = "mollalehu";
        public static bool open_window_path = true;
        public static bool open_window_path_visiable = true;
        public static bool open_window_modlist = true;
        public static bool open_window_modlist_visiable = true;
        public static string ram_slide_value = "4096";
        public static string ram_capable;
        public static string slider_value;
        public string json_test;
        public int cmp_counted = 0;

        public string mc_forced_folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\.minecraft";

        public static bool downloading_file = true;

        public static string mc_zip_path;
        public static string mc_folder;
        public static string processed_context;
        public static int pro_total;
        public static int process;

        public MainWindow()
        {
            InitializeComponent();
        }
        public string Ram_slider_change()
        {
            return ram_slide_value;

        }

        public bool return_downloading_file(bool value)
        {
            downloading_file = value;
            return downloading_file;
        }


        public int completed_counted()
        {
            cmp_counted++;
            return cmp_counted;
        }

        public static void ExtractToDirectory(ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                string directory = Path.GetDirectoryName(completeFileName);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                if (file.Name != "")
                    file.ExtractToFile(completeFileName, true);
            }
        }


        public void donwload_function(string Uri, string donwload_path)
        {
            progressbar_text_text.Text = processed_context + " 설치중...";
            WebClient dl_mc = new WebClient();
            Uri uri_forge = new Uri(Uri);
            dl_mc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
            dl_mc.DownloadFileCompleted += new AsyncCompletedEventHandler(download_complete);
            dl_mc.DownloadFileAsync(uri_forge, donwload_path);

        }


        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            download_progress_function(e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive, "MB");

        }

        public void download_complete(object sender, AsyncCompletedEventArgs e)
        {
            download_progressbar.Value = 0;
            completed_counted();
        }
        public void download_progress_function(double ProgressPercentage, double BytesReceived, double TotalBytesToReceive, string context)
        {
            download_progressbar.Value = ProgressPercentage;
            progressbar_text.Text = Convert.ToString(ProgressPercentage) + " %  " + SizeSuffixMb_Ram((long)Convert.ToDouble(BytesReceived)) + "MB / " + SizeSuffixMb_Ram((long)Convert.ToDouble(TotalBytesToReceive)) + context;
        }


        public void open_path_bool()
        {
            open_window_path_visiable = true;
            return;
        }
        public void open_modlist_bool()
        {
            open_window_modlist_visiable = true;
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
        static string SizeSuffixMb_Ram(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffixMb_Ram(-value); }
            if (value == 0) { return "0.0 bytes"; }
            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));
            return string.Format("{0}", Math.Round(adjustedSize));
        }
        static string SizeSuffixMb_Donwload(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffixMb_Donwload(-value); }
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
            if (downloading_file == false)
            {
                if (MessageBox.Show("아직 다운로드중입니다. 종료하시겠습니까?", "Real?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                Application.Current.Shutdown();
            }

        }

        private void minize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Install_Setting_Click(object sender, RoutedEventArgs e)
        {
            second install_page = new second();
            install_page.Owner = Application.Current.MainWindow;
            install_page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (open_window_path == false)
            {
                if (open_window_path_visiable == true)
                {
                    open_window_path_visiable = false;
                    install_page.Visibility = Visibility.Visible;
                }
            }
            else
            {
                open_window_path = false;
                install_page.Show();
            }
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
                    ram_capable = SizeSuffixMb_Donwload((long)Convert.ToDouble(result["TotalVisibleMemorySize"]));

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
                ram_rate.Text = " / " + ram_capable;
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
                string a = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\data.json");
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
            ram_slide_value = Convert.ToString(Math.Round(ram_slider.Value));
            Ram_slider_change();
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
        //업데이트 필요
        private void luanch_mincraft_Click(object sender, RoutedEventArgs e)
        {
            string jsonUpdateFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\data.json");
            JObject jsonO = JObject.Parse(jsonUpdateFile);
            //json 파일을 이용하여 경로 재설정 예정
            if (File.Exists(Convert.ToString(jsonO["basic_path"])))
            {
                try
                {
                    Process.Start(Convert.ToString(jsonO["basic_path"]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
            else if (File.Exists(Convert.ToString(jsonO["selected_path"])))
            {
                try
                {
                    Process.Start(Convert.ToString(jsonO["selected_path"]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
            else
            {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Filter = "Exe files(*.exe) | *.exe;",
                    Multiselect = false,
                    Title = "마크를 선택해라"
                };
                dialog.ShowDialog();
                jsonO.Add("selected_path", dialog.FileName);
                string convert_json = Convert.ToString(jsonO);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\data.json", convert_json);

            }


        }

        private void modlist_Click(object sender, RoutedEventArgs e)
        {
            modlist.modlist skla = new modlist.modlist();
            skla.Owner = Application.Current.MainWindow;
            skla.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (open_window_modlist == false)
            {
                if (open_window_modlist_visiable == true)
                {
                    open_window_modlist_visiable = false;
                    skla.Visibility = Visibility.Visible;
                }
            }
            else
            {
                open_window_modlist = false;
                skla.Show();
            }
        }

        private void checkbox_check()
        {
            if (modlist.modlist.optifine_check == false)
            {
                pro_total--;
            }
            if (modlist.modlist.koreanchat_check == false)
            {
                pro_total--;
            }
        }


        private void Install_Start_Click(object sender, RoutedEventArgs e)
        {
            if (second.set_download_start == true)
            {
                if (downloading_file == true)
                {
                    return_downloading_file(false);
                    total_download_text.Text = "0 / 0";
                    total_download_progressbar.Value = 0;
                    download_progressbar.Value = 0;
                    progressbar_text.Text = "";
                    progressbar_text_text.Text = "";
                    pro_total = 2;
                    cmp_counted = 0;

                    //checkbox_check();
                    total_download_progressbar.Maximum = pro_total;
                    total_download_text.Text = "0 / " + pro_total;


                    BackgroundWorker Download_progress = new BackgroundWorker();
                    Download_progress.DoWork += report_progress;
                    Download_progress.ProgressChanged += Download_progress_progress_change;
                    Download_progress.WorkerReportsProgress = true;
                    Download_progress.RunWorkerCompleted += Download_progress_complete;
                    Download_progress.RunWorkerAsync(1000);




                    //폴더 만들기
                    try
                    {
                        Directory.CreateDirectory(second.korean_check_path + @"\" + modpack_title);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }
                    // 포지, 모드팩 다운
                    donwload_function(@"http://222.234.190.69/WordPress/wp-content/uploads/2020/06/minecraft_forge.zip", @".\minecraft_forge.zip");
                    donwload_function(@"http://222.234.190.69/WordPress/wp-content/uploads/2020/06/1st_alphatest.zip", @".\1st_alphatest.zip");

                    if (modlist.modlist.optifine_check == true)
                    {
                        donwload_function(@"http://222.234.190.69/WordPress/wp-content/uploads/2020/03/OptiFine_1.12.2_HD_U_F5.jar", second.korean_check_path + @"\" + modpack_title + @"\mods\OptiFine_1.12.2_HD_U_F5.jar");
                    }
                    if (modlist.modlist.koreanchat_check == true)
                    {
                        donwload_function(@"http://222.234.190.69/WordPress/wp-content/uploads/2020/03/koreanchat-creo-1.12-1.9.jar", second.korean_check_path + @"\" + modpack_title + @"\mods\koreanchat-creo-1.12-1.9.jar");
                    }
                }
                else
                {
                    MessageBox.Show("이미 다운로드가 진행중입니다.", "Information");
                }
            }
            else
            {
                MessageBox.Show("Setting에서 경로를 먼저 지정해 주세요", "Information");
            }
        }
        void report_progress(object sender, DoWorkEventArgs e)
        {
            for (; ; )
            {
                (sender as BackgroundWorker).ReportProgress(cmp_counted);
                Thread.Sleep(100);
                (sender as BackgroundWorker).ReportProgress(cmp_counted);
                if (cmp_counted == 2)
                {
                    break;
                }
                Thread.Sleep(100);
                /**
                
                
    **/
            }
        }
        void Download_progress_progress_change(object sender, ProgressChangedEventArgs e)
        {
            total_download_progressbar.Value = e.ProgressPercentage;
            total_download_text.Text = e.ProgressPercentage + " / " + pro_total;
        }


        void Download_progress_complete(object sender, AsyncCompletedEventArgs e)
        {

            progressbar_text_text.Text = "Json 설정중..";
            string jsonUpdateFile1 = File.ReadAllText(mc_forced_folder + @"\launcher_profiles.json");
            JObject profiles_json = JObject.Parse(jsonUpdateFile1);
            try
            {
                var pjob = (JObject)profiles_json["profiles"];
                pjob.Add(modpack_title, new JObject());
            }
            catch
            {
            }
            profiles_json["profiles"][modpack_title]["created"] = "2020-01-10T17:47:57.637Z";
            profiles_json["profiles"][modpack_title]["gameDir"] = second.korean_check_path + @"\" + modpack_title;
            profiles_json["profiles"][modpack_title]["icon"] = "Furnace";
            profiles_json["profiles"][modpack_title]["javaArgs"] = "-Xmx" + Ram_slider_change() + "m";
            profiles_json["profiles"][modpack_title]["lastUsed"] = "2020-01-10T17:47:57.637Z";
            profiles_json["profiles"][modpack_title]["lastVersionId"] = "1.12.2-forge-14.23.5.2854";
            profiles_json["profiles"][modpack_title]["name"] = modpack_title;
            profiles_json["profiles"][modpack_title]["type"] = "custom";
            string convert_json = Convert.ToString(profiles_json);
            File.WriteAllText(mc_forced_folder + @"\launcher_profiles.json", convert_json);
            BackgroundWorker zip_extract_file = new BackgroundWorker();
            zip_extract_file.DoWork += Do_extract_zip_File;
            zip_extract_file.RunWorkerAsync(1000);

            progressbar_text_text.Text = "Json 설정완료!";

        }

        void Do_extract_zip_File(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.SystemIdle, new Action(delegate
            {
                progressbar_text_text.Text = "압축푸는중..";
                if (File.Exists(@".\minecraft_forge.zip"))
                {
                    using (ZipArchive archive = ZipFile.Open(@".\minecraft_forge.zip", ZipArchiveMode.Update))
                    {
                        ExtractToDirectory(archive, mc_forced_folder, true);
                    }
                    File.Delete(@".\minecraft_forge.zip");
                }
                if (File.Exists(@".\1st_alphatest.zip"))
                {
                    using (ZipArchive archive = ZipFile.Open(@".\1st_alphatest.zip", ZipArchiveMode.Update))
                    {
                        ExtractToDirectory(archive, second.select_path + @"\" + modpack_title, true);
                    }
                    File.Delete(@".\1st_alphatest.zip");
                }
                progressbar_text_text.Text = "설치끝!";
                return_downloading_file(true);
            }));

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


    //압축 오버라이드
    public static class ZipArchiveExtensions
    {
           
    }
}
