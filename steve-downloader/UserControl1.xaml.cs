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
using Xamarin.Forms.Xaml;

namespace steve_downloader.second_window
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class second : Window
    {
        public string select_path;
        public bool set_download_start;
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
        }

        private void CLosePopup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            askdl.open_bool();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            
        }

        private void download_ok_Click(object sender, RoutedEventArgs e)
        {
            if (select_path == paste_path.Text)
            {
                this.Close();
                askdl.open_bool();
                set_download_start = true;
            }
            else
            {
                this.Close();
                askdl.open_bool();
            }
        }
    }
}
