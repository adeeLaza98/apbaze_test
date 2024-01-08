using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Apbaze.ViewModels;
using MaterialDesignThemes.Wpf;
using Notification.Wpf;

namespace Apbaze
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();
        public ObservableCollection<JobViewModel> Jobs { get; set; }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                ApplyFilter();
            }
        }

        public Main()
        {
            InitializeComponent();
            LoadJobs();

            DataContext = this;
        }

        private void LoadJobs()
        {
            var jobs = _context.Jobs
                .Where(j => !j.IsDeleted)
                .Select(j => new JobViewModel(j))
                .ToList();

            Jobs = new ObservableCollection<JobViewModel>(jobs);
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                Jobs.Clear();

                var jobs = _context.Jobs
                    .Where(j => !j.IsDeleted)
                    .Select(j => new JobViewModel(j))
                    .ToList();

                foreach (var item in jobs)
                    Jobs.Add(item);
            }
            else
            {
                Jobs.Clear();

                var jobs = _context.Jobs
                    .Where(j => !j.IsDeleted && (j.Title.ToLower().Contains(SearchText) || j.Description.ToLower().Contains(SearchText)))
                    .Select(j => new JobViewModel(j))
                    .ToList();

                foreach (var item in jobs)
                    Jobs.Add(item);
            }
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
            //var notificationManager = new NotificationManager();

            //var content = new NotificationContent
            //{
            //    Title = "Logout pressed",
            //    Message = "This is a notification message.",
            //    Type = NotificationType.Information,
            //};

            //notificationManager.Show(content, "WindowArea");

            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                Close();
            }
        }

        private void PackIcon_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            var notificationManager = new NotificationManager();

            var content = new NotificationContent
            {
                Title = "Success",
                Message = "Successfully added job to favourites",
                Type = NotificationType.Success,
            };

            notificationManager.Show(content, "WindowArea");
        }

        private void FindWorkRedirect(object sender, MouseButtonEventArgs e)
        {
            findWork.FontWeight = FontWeights.Bold;
            findWork.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14a800"));

            myJobs.FontWeight = FontWeights.Normal;
            myJobs.Foreground = Brushes.Black;
            messages.FontWeight = FontWeights.Normal;
            messages.Foreground = Brushes.Black;
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
        }
    }
}
