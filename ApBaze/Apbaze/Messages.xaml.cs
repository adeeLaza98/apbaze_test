using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;

namespace Apbaze
{
    /// <summary>
    /// Interaction logic for Messages.xaml
    /// </summary>
    public partial class Messages : Window
    {
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();

        public Messages()
        {
            InitializeComponent();
        }

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);

        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                Close();
            }
        }

        private void FindWorkRedirect(object sender, MouseButtonEventArgs e)
        {
            findWork.FontWeight = FontWeights.Bold;
            findWork.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14a800"));

            myJobs.FontWeight = FontWeights.Normal;
            myJobs.Foreground = Brushes.Black;
            messages.FontWeight = FontWeights.Normal;
            messages.Foreground = Brushes.Black;

            var findWorkView = new Main();
            findWorkView.Show();
            Close();
        }

        private void MyJobsRedirect(object sender, MouseButtonEventArgs e)
        {
            myJobs.FontWeight = FontWeights.Bold;
            myJobs.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14a800"));

            findWork.FontWeight = FontWeights.Normal;
            findWork.Foreground = Brushes.Black;
            messages.FontWeight = FontWeights.Normal;
            messages.Foreground = Brushes.Black;
        }

        private void MessagesRedirect(object sender, MouseButtonEventArgs e)
        {
            messages.FontWeight = FontWeights.Bold;
            messages.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14a800"));

            myJobs.FontWeight = FontWeights.Normal;
            myJobs.Foreground = Brushes.Black;
            findWork.FontWeight = FontWeights.Normal;
            findWork.Foreground = Brushes.Black;

            var messagesView = new Messages();
            messagesView.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            message.Text = string.Empty;
            message.Focus();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                message.Text = string.Empty;
                message.Focus();
            }
        }
    }
}
