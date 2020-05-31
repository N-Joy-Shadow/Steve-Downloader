using System.Windows;
using System.Windows.Input;

namespace steve_downloader.modlist
{
    /// <summary>
    /// modlist.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class modlist : Window
    {
        public static bool optifine_check = false;
        public static bool koreanchat_check = false;
        public modlist()
        {
            InitializeComponent();
        }

        public bool opti_Return_checked(bool value)
        {
            optifine_check = value;
            return optifine_check;
        }
        public bool chat_Return_checked(bool value)
        {
            koreanchat_check = value;
            return koreanchat_check;
        }

        private void down_exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow atrq = new MainWindow();
            atrq.open_modlist_bool();
            this.Visibility = Visibility.Hidden;
        }

        private void top_grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void cbx_optifine_checked_checked(object sender, RoutedEventArgs e)
        {
            opti_Return_checked(true);
            optifine_txt.Text = " - 옵티파인추가됨";
                
        }

        private void cbx_koreanchat_Checked_checked(object sender, RoutedEventArgs e)
        {
            chat_Return_checked(true);
            korean_txt.Text = " - 한글채팅추가됨";
        }

        private void cbx_koreanchat_checked_Unchecked(object sender, RoutedEventArgs e)
        {
            chat_Return_checked(false);
            korean_txt.Text = "";
        }

        private void cbx_optifine_checked_Unchecked(object sender, RoutedEventArgs e)
        {
            opti_Return_checked(false);
            optifine_txt.Text = "";
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            if (koreanchat_check == true)
            {
                cbx_koreanchat_checked.IsChecked = true;
                korean_txt.Text = " - 한글채팅추가됨";
            }
            if (optifine_check == true)
            {
                cbx_optifine_checked.IsChecked = true;
                optifine_txt.Text = " - 옵티파인추가됨";
            }
        }
    }
}
