using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
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
using Apbaze.StaticClasses;
using Apbaze.ViewModels;
using MaterialDesignThemes.Wpf;
using Notification.Wpf;

namespace Apbaze
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window, INotifyPropertyChanged
    {
        #region Properties
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

        public string Wallet { get; set; }

        public string Username { get; set; }

        private string jobType;
        public string JobType
        {
            get { return jobType; }
            set
            {
                if (jobType != value)
                {
                    jobType = value;
                    OnPropertyChanged(nameof(JobType));
                    ApplyFilter();
                }
            }
        }

        public List<string> JobTypes { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Main()
        {
            InitializeComponent();

            SetWallet();

            JobTypes = new List<string>() { "All", "New", "Already proposed" };
            JobType = JobTypes.FirstOrDefault();

            Username = "Welcome, " + UserContext.LoggedInUsername;

            LoadJobs();

            DataContext = this;
        }

        private void SetWallet()
        {
            var transactions = _context.Transactions
                    .Where(t => t.SenderId == UserContext.LoggedInUserId || t.ReceiverId == UserContext.LoggedInUserId)
                    .ToList();

            var walletPlus = transactions.Where(t => t.ReceiverId == UserContext.LoggedInUserId).Sum(t => t.Amount);
            var walletMinus = transactions.Where(t => t.SenderId == UserContext.LoggedInUserId).Sum(t => t.Amount * (-1));

            Wallet = "Wallet: $" + (walletPlus + walletMinus).ToString();
        }

        private void LoadJobs()
        {
            _context.Refresh(RefreshMode.OverwriteCurrentValues, _context.Bids);
            _context.Refresh(RefreshMode.OverwriteCurrentValues, _context.Deliverables);
            _context.Refresh(RefreshMode.OverwriteCurrentValues, _context.Jobs);
            
            var jobs = _context.Jobs
                .Where(j => !j.IsDeleted && j.JobStatus == 1)
                .Select(j => new JobViewModel(j))
                .ToList();

            if (Jobs != null)
            {
                Jobs.Clear();

                foreach (var item in jobs)
                    Jobs.Add(item);
            }
            else
            {
                Jobs = new ObservableCollection<JobViewModel>(jobs);
            }
        }

        private void ApplyFilter()
        {
            var jobTypeId = JobTypes.IndexOf(JobType);

            if (string.IsNullOrEmpty(SearchText))
            {
                var jobs = new List<JobViewModel>();

                switch (jobTypeId)
                {
                    case 0:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1)
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                    case 1:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1 && !j.Bids.Any(b => b.FreelancerId == UserContext.LoggedInUserId))
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                    case 2:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1 && j.Bids.Any(b => b.FreelancerId == UserContext.LoggedInUserId))
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                    default:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1)
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                }

                if (Jobs != null)
                {
                    Jobs.Clear();

                    foreach (var item in jobs)
                        Jobs.Add(item);
                }
                else
                    Jobs = new ObservableCollection<JobViewModel>(jobs);
            }
            else
            {
                var jobs = new List<JobViewModel>();

                switch (jobTypeId)
                {
                    case 0:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1 && (j.Title.ToLower().Contains(SearchText) || j.Description.ToLower().Contains(SearchText)))
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                    case 1:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1 && !j.Bids.Any(b => b.FreelancerId == UserContext.LoggedInUserId) && (j.Title.ToLower().Contains(SearchText) || j.Description.ToLower().Contains(SearchText)))
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                    case 2:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1 && j.Bids.Any(b => b.FreelancerId == UserContext.LoggedInUserId) && (j.Title.ToLower().Contains(SearchText) || j.Description.ToLower().Contains(SearchText)))
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                    default:
                        jobs = _context.Jobs
                            .Where(j => !j.IsDeleted && j.JobStatus == 1 && (j.Title.ToLower().Contains(SearchText) || j.Description.ToLower().Contains(SearchText)))
                            .Select(j => new JobViewModel(j))
                            .ToList();
                        break;
                }

                if (Jobs != null)
                {
                    Jobs.Clear();

                    foreach (var item in jobs)
                        Jobs.Add(item);
                }
                else
                    Jobs = new ObservableCollection<JobViewModel>(jobs);
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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

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

        #region Redirects

        private void FindWorkRedirect(object sender, MouseButtonEventArgs e)
        {
            var findWorkView = new Main();
            findWorkView.Show();
            Close();
        }

        private void MyJobsRedirect(object sender, MouseButtonEventArgs e)
        {
            var myJobsView = new MyJobs();
            myJobsView.Show();
            Close();
        }

        private void MessagesRedirect(object sender, MouseButtonEventArgs e)
        {
            var messagesView = new Messages();
            messagesView.Show();
            Close();
        }

        #endregion

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border clickedButton = (Border)sender;

            if (clickedButton.Tag is int jobId)
            {
                var jobDetailsWindow = new JobDetails(jobId);
                jobDetailsWindow.Show();
                Close();
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var walletWindow = new Wallet();
            walletWindow.Show();
            Close();
        }
    }
}
