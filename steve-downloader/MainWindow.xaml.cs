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

namespace steve_downloader
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }



        //private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    ButtonCloseMenu.Visibility = Visibility.Visible
        //}
        //private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    
        //}
        //private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    UserControl usc = null;
        //    GridMain.Children.Clear();

        //switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
        //{
        //    case "ItemHome":
        //        usc = new UserControlHome();
        //        GridMain.Children.Add(usc);
        //        break;
        //    case "ItemCreate":
        //        usc = new UserControlCreate();
        //        GridMain.Children.Add(usc);
        //        break;
        //    default:
        //        break;
        //

        //private void mouseDrag_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        this.DragMove();
        //    }

        //}

        //private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        //{
        //    ButtonCloseMenu.Visibility = Visibility.Visible;
        //    ButtonOpenMenu.Visibility = Visibility.Collapsed;
        //}

        //private void ButtonClose_Click(object sender, RoutedEventArgs e)
        //{ 
        //    ButtonCloseMenu.Visibility = Visibility.Collapsed;
        //    ButtonOpenMenu.Visibility = Visibility.Visible;

        //}
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

        private void MenuCloseButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuCopenButton_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int a = 1; a < 100; a++)
            {
                (sender as BackgroundWorker).ReportProgress(a);
                Thread.Sleep(1);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CPU_using.Value = e.ProgressPercentage;
        }
        private void test_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();      
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {

        }

        private void Install_Start_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
