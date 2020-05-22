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
using steve_downloader;

namespace steve_downloader.modlist
{
    /// <summary>
    /// modlist.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class modlist : Window
    {
        public modlist()
        {
            InitializeComponent();
        }

        private void down_exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow atrq = new MainWindow();
            atrq.open_modlist_bool();
            this.Visibility = Visibility.Collapsed;
        }

        private void top_grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
