using Apbaze.StaticClasses;
using Apbaze.ViewModels;
using MaterialDesignThemes.Wpf;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Interaction logic for MainClient.xaml
    /// </summary>
    public partial class MainClient : Window, INotifyPropertyChanged
    {
        #region Properties
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();
        public ObservableCollection<JobViewModel> Jobs { get; set; }

        public string Wallet { get; set; }

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

        public string Username { get; set; }

        private string jobStatus;
        public string JobStatus
        {
            get { return jobStatus; }
            set
            {
                if (jobStatus != value)
                {
                    jobStatus = value;
                    OnPropertyChanged(nameof(JobStatus));
                    ApplyFilter();
                }
            }
        }

        public List<string> JobStatuses { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainClient()
        {
            InitializeComponent();

            SetWallet();

            JobStatuses = new List<string> { "All", "Open", "Hired", "Completed" };

            JobStatus = JobStatuses.FirstOrDefault();

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
            _context.Refresh(RefreshMode.OverwriteCurrentValues, _context.Jobs);
            _context.Refresh(RefreshMode.OverwriteCurrentValues, _context.Bids);
            _context.Refresh(RefreshMode.OverwriteCurrentValues, _context.Deliverables);

            var jobs = _context.Jobs
                .Where(j => !j.IsDeleted && j.ClientId == UserContext.LoggedInUserId)
                .Select(j => new JobViewModel(j))
                .ToList();

            if (Jobs != null)
            {
                Jobs.Clear();

                foreach (var item in jobs)
                {
                    Jobs.Add(item);
                }
            }
            else
            {
                Jobs = new ObservableCollection<JobViewModel>(jobs);
            }
        }

        private void ApplyFilter()
        {
            var jobStatusId = JobStatuses.IndexOf(JobStatus);

            if (string.IsNullOrEmpty(SearchText))
            {
                var jobs = _context.Jobs
                    .Where(j => !j.IsDeleted && j.ClientId == UserContext.LoggedInUserId && (j.JobStatus == jobStatusId || jobStatusId == 0))
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
            else
            {
                var jobs = _context.Jobs
                    .Where(j => !j.IsDeleted && j.ClientId == UserContext.LoggedInUserId && (j.JobStatus == jobStatusId || jobStatusId == 0) && (j.Title.ToLower().Contains(SearchText) || j.Description.ToLower().Contains(SearchText)))
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
            var mainClientWindow = new MainClient();
            mainClientWindow.Show();
            Close();
        }

        private void MessagesRedirect(object sender, MouseButtonEventArgs e)
        {
            var messagesView = new Messages();
            messagesView.Show();
            Close();
        }

        #endregion

        private void ViewAction(object sender, MouseButtonEventArgs e)
        {
            StackPanel clickedButton = (StackPanel)sender;

            if (clickedButton.Tag is int jobId)
            {
                var job = _context.Jobs.FirstOrDefault(j => j.Id == jobId);
                var hasDeliverable = _context.Deliverables.Count(d => d.JobId == jobId) > 0;

                if (job.JobStatus == 2 && hasDeliverable)
                {
                    var reviewDeliverableWindow = new DeliverableReview(jobId);
                    reviewDeliverableWindow.Show();
                    Close();
                }
                else
                {
                    var clientJobWindow = new ClientJob(jobId);
                    clientJobWindow.Show();
                    Close();
                }
            }
        }

        private void DeleteAction(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this job?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                StackPanel clickedButton = (StackPanel)sender;

                if (clickedButton.Tag is int jobId)
                {
                    var job = _context.Jobs.FirstOrDefault(j => j.Id == jobId);

                    if (job.JobStatus == 3)
                    {
                        MessageBoxResult error = MessageBox.Show("You cannot delete a completed job!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    _context.Jobs.DeleteOnSubmit(job);
                    _context.SubmitChanges();

                    var item = Jobs.SingleOrDefault(j => j.Id == jobId);
                    Jobs.Remove(item);

                    var notificationManager = new NotificationManager();

                    var content = new NotificationContent
                    {
                        Title = "Success",
                        Message = "Job deleted successfully!",
                        Type = NotificationType.Success,
                    };

                    notificationManager.Show(content, "WindowArea");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var createJobWindow = new CreateJob();
            createJobWindow.Show();
            Close();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var walletWindow = new Wallet();
            walletWindow.Show();
            Close();
        }
    }
}
