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
using log4net;
using System.Text.RegularExpressions;
using NotifyVariable;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
        public int Download_Complete_Count = 0;
        public int check_forge = 0;

        public string mc_forced_folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\.minecraft";
        public string LocalTempPath = Path.GetTempPath() + @"\steve-installer";

        public static bool downloading_file = true;

        public static string mc_zip_path;
        public static string mc_folder;
        public static string processed_context;
        public static int pro_total = 4;
        public static int process; 


        char[] Ram_Trnas_Char;
        List<string> Filtered_Char = new List<string>();

        private BackgroundWorker main_work;
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


        //CPU,RAM 사용량 얻기
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
            }));
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (Directory.Exists(LocalTempPath))
            {
                if (File.Exists(LocalTempPath + @"\data.json"))
                {
                    string a = File.ReadAllText(LocalTempPath + @"\data.json");
                }
                else
                {
                    string json_path = LocalTempPath + @"\data.json";
                    FileStream stream = File.Create(json_path);
                    stream.Close();

                }
            }
            else
            {
                Directory.CreateDirectory(LocalTempPath);
                string json_path = LocalTempPath + @"\data.json";
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
            if (ram_slider.Value != 0) { 
                if (second.set_download_start == true)
                {
                    if (downloading_file == true)
                    {
                        return_downloading_file(false);
                        total_download_text.Text = "0 / 0";
                        download_progressbar.Value = 0;
                        progressbar_text.Text = "";
                        progressbar_text_text.Text = "";
                        
                        string download_ptha = second.korean_check_path + @"\" + modpack_title;
                        //checkbox_check();
                        total_download_progressbar.Maximum = pro_total;
                        total_download_text.Text = "0 / " + pro_total;
                        total_download_progressbar.Value = 0;

                        //폴더 만들기
                        try
                        {
                            Directory.CreateDirectory(download_ptha);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Convert.ToString(ex));
                        }
                        // 포지, 모드팩 다운
                        main_work = new BackgroundWorker();
                        main_work.WorkerReportsProgress = true;
                        main_work.DoWork += new DoWorkEventHandler(Main_Do_work);
                        main_work.RunWorkerAsync(1000);
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
            else
            {
                MessageBox.Show("오른쪽 위에서 램 용량을 먼저 지정해 주세요", "Information");
            }
        }
        async void Main_Do_work(object sender, DoWorkEventArgs e)
        {   
            WebClient Download_forge = new WebClient();
            Uri url_forge = new Uri("http://222.234.190.69/WordPress/wp-content/uploads/2020/06/minecraft_forge.zip");
            Download_forge.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChagedForge);
            Download_forge.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedForge);
            await Download_forge.DownloadFileTaskAsync(url_forge, @".\zip\minecraft_forge.zip");

            WebClient Download_modpack = new WebClient();
            Uri url_modpack = new Uri(@"http://222.234.190.69/WordPress/wp-content/uploads/2020/06/1st_alphatest.zip");
            Download_modpack.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChagedModpack);
            await Download_modpack.DownloadFileTaskAsync(url_modpack, @".\zip\modpack.zip");
            //다운로드 컴플리트 대신해 작용
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {

            Download_Complete_Count++;
            total_download_text.Text = Download_Complete_Count + " / " + pro_total;
            total_download_progressbar.Value = Download_Complete_Count;
            if (File.Exists(@".\minecraft_forge.zip"))
                {
                    using (ZipArchive archive = ZipFile.Open(@".\minecraft_forge.zip", ZipArchiveMode.Update))
                    {
                        ExtractToDirectory(archive, mc_forced_folder, true);
                    }
                    File.Delete(@".\minecraft_forge.zip");
                    Download_Complete_Count++;
                    total_download_text.Text = Download_Complete_Count + " / " + pro_total;
                    total_download_progressbar.Value = Download_Complete_Count;
                }
                else
                {
                    System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
                    notifyIcon.Icon = new System.Drawing.Icon(@"icon.ico");
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(2000, "Steve Downloader", "다운로드 실패", System.Windows.Forms.ToolTipIcon.Error);
                    return_downloading_file(true);
                }
            
                if (File.Exists(@".\1st_alphatest.zip"))
                {
                    using (ZipArchive archive = ZipFile.Open(@".\1st_alphatest.zip", ZipArchiveMode.Update))
                    {
                        ExtractToDirectory(archive, second.select_path + @"\" + modpack_title, true);
                    }
                    File.Delete(@".\1st_alphatest.zip");
                    Download_Complete_Count++;
                    total_download_text.Text = Download_Complete_Count + " / " + pro_total;
                    total_download_progressbar.Value = Download_Complete_Count;
                }
                else
                {
                    System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
                    notifyIcon.Icon = new System.Drawing.Icon(@"icon.ico");
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(2000, "Steve Downloader", "다운로드 실패", System.Windows.Forms.ToolTipIcon.Error);
                    return_downloading_file(true);
                }

                
                


                    if (total_download_progressbar.Value == 4)
                {
                    progressbar_text.Text = "json 설정중..";
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
                    //run_threading();

                    progressbar_text.Text = "json 설정완료!";

                    progressbar_text.Text = "설치끝!";
                    System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
                    notifyIcon.Icon = new System.Drawing.Icon(@"icon.ico");
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(2000, "Steve Downloader", "설치 끝", System.Windows.Forms.ToolTipIcon.Info);
                    return_downloading_file(true);
                }
            }));
           
        }
        void DownloadProgressChagedForge(object sender,DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                download_progressbar.Value = e.ProgressPercentage;
                progressbar_text.Text = String.Format("{0}% {1}MB / {2}MB  포지 다운로드중..",
                    Convert.ToString(e.ProgressPercentage),
                    SizeSuffixMb_Ram((long)Convert.ToDouble(e.BytesReceived)),
                    SizeSuffixMb_Ram((long)Convert.ToDouble(e.TotalBytesToReceive)));
            }));
        }        
        void DownloadProgressChagedModpack(object sender,DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                download_progressbar.Value = e.ProgressPercentage;
                progressbar_text.Text = String.Format("{0}% {1}MB / {2}MB  모드팩 다운로드중..",
                    Convert.ToString(e.ProgressPercentage), 
                    SizeSuffixMb_Ram((long)Convert.ToDouble(e.BytesReceived)), 
                    SizeSuffixMb_Ram((long)Convert.ToDouble(e.TotalBytesToReceive)));
            }));
        }
        void DownloadCompletedForge(object sender, AsyncCompletedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                Download_Complete_Count++;
                total_download_text.Text = Download_Complete_Count + " / " + pro_total;
                total_download_progressbar.Value = Download_Complete_Count;
            }));
        }      
        /*
        void run_threading()
        {
            BackgroundWorker zip_extract_file = new BackgroundWorker();
            zip_extract_file.DoWork += Do_extract_zip_File;
            zip_extract_file.RunWorkerAsync(1000);
        }
        */
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
            }));

        }
        public static bool CheckNumber(string letter)
        {
            bool IsCheck = true;
            Regex numRegex = new Regex(@"[0-9]");
            Boolean ismatch = numRegex.IsMatch(letter);
            if (!ismatch)
            {
                IsCheck = false;
            }
            return IsCheck;
        }
        public static bool CheckEnglish(string letter)
        {
            bool IsCheck = true;
            Regex engRegex = new Regex(@"[a-zA-Z]");
            Boolean ismatch = engRegex.IsMatch(letter);
            if (!ismatch)
            {
                IsCheck = false;
            }
            return IsCheck;
        }
        
        private void ram_trans_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            Ram_Trnas_Char = ram_trans.Text.ToCharArray(0, 1);
            foreach (var s in Ram_Trnas_Char)
            {
                if (CheckNumber(Convert.ToString(s)) == true)
                {
                    Filtered_Char.Add(Convert.ToString(s));
                }
                else if (char.GetUnicodeCategory(s) == UnicodeCategory.OtherLetter)
                {
                    Filtered_Char.Add(Convert.ToString(s));
                }
                else if (CheckEnglish(Convert.ToString(s)) == false)
                {
                    Filtered_Char.Add(Convert.ToString(s));
                }
                string Final_char = string.Join("",Filtered_Char);
            }

            /*
            if (ram_trans.Text != "")
            {
                if (ram_trans.Text == "")
                {
                    ram_slider.Value = 0;
                }
                else
                {
                    if (Convert.ToInt32(ram_trans.Text) < Convert.ToInt32(ram_capable))
                    {
                        ram_slider.Value = Convert.ToDouble(ram_trans.Text);
                        ram_slide_value = ram_trans.Text;
                        Ram_slider_change();
                    }
                    else
                    {
                        ram_trans.Text = ram_capable;
                        ram_slider.Value = Convert.ToDouble(ram_capable);
                        ram_slide_value = ram_capable;
                        Ram_slider_change();
                    }
                }
            }
            else
            {
                ram_slider.Value = 0;
                ram_trans.Text = "";
            }
            */
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









