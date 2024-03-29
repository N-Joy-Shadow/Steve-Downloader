﻿using System;
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
using System.Threading;
using System.Net;

namespace steve_downloader.second_window
{
    public partial class second : Window
    {
        public static string korean_check_path;
        public static string select_path;
        public static bool set_download_start = false;
        MainWindow askdl = new MainWindow();
        public second()
        {
            InitializeComponent();

        }

        public bool setdownload()
        {
            set_download_start = true;
            return set_download_start;
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
            this.Visibility = Visibility.Hidden;
            askdl.open_path_bool();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            paste_path.Text = select_path;
        }

        private void download_ok_Click(object sender, RoutedEventArgs e)
        {
            int path_int = 0;
            char[] asd = select_path.ToCharArray();
            if (paste_path.Text != "")
            {
                for (int i = 0; i < select_path.Length; i++)
                { 
                    if (char.GetUnicodeCategory(asd[i]) == System.Globalization.UnicodeCategory.OtherLetter)
                    {
                        path_int++;
                    }
                }
                if (path_int == 0)
                {
                    korean_check_path = select_path;
                    this.Visibility = Visibility.Hidden;
                    askdl.open_path_bool();
                    setdownload();
                }
                else 
                {
                    path_korean.Foreground = Brushes.Red;
                }

            }
        }
    }
}
