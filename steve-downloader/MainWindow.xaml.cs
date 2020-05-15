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
using steve_downloader;

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

        public void open_bool(bool bool_value)
        {
            open_window = bool_value;
            return;
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


        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }


        private void test_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {

        }

        private void Install_Start_Click(object sender, RoutedEventArgs e)
        {
            if (open_window == true)
            {
                second install_page = new second();
                install_page.Owner = Application.Current.MainWindow;
                install_page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                install_page.Show();
                //open_window = false;
            }
            else
            {
                return;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SideMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            
            this.shadowEffect = new DropShadowEffect
            {
                ShadowDepth = 1,
                BlurRadius = 5
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

        private void text2_Click(object sender, RoutedEventArgs e)
        {
            second sesds = new second();
            Test_textblock.Text = sesds.select_path;
        }
    }
}
