using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using steve_downloader;
using steve_downloader.download;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Net;

namespace steve_downloader.second_window
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class second : Window
    {
        public static string select_path;
        public bool set_download_start;
        public static Uri korean_link = new Uri("http://222.234.190.69/WordPress/wp-content/uploads/2020/03/koreanchat-creo-1.12-1.9.jar");
        public static string donwloadpath = second.select_path + "\\koreanchat-creo-1.12-1.9.jar";
        MainWindow askdl = new MainWindow();
        public second()
        {
            InitializeComponent();
            
        }

        public void find_path_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            select_path = dialog.SelectedPath;
            paste_path.Text = select_path;
            return;
        }

        private void CLosePopup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            askdl.open_bool();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                var F_th = new Thread(F_Thread);
            }
            catch
            {
                System.Windows.MessageBox.Show("성공");
            }
        }

        private void download_ok_Click(object sender, RoutedEventArgs e)
        {
                this.Close();
                askdl.open_bool();
                set_download_start = true;
        }
        private void F_Thread()
        {
            while (true)
            {
                if (set_download_start == true)
                {
                    using (WebClient client = new WebClient())
                    {
                     client.DownloadDataAsync(korean_link, donwloadpath);
                    }
                }
            }
        }
    }
}
