using Apbaze.StaticClasses;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for JobDetails.xaml
    /// </summary>
    public partial class JobDetails : Window, INotifyPropertyChanged
    {
        #region Properties
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();
        public int JobId { get; set; }
        public string Username { get; set; }
        public string Wallet { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public JobDetails(int jobId)
        {
            InitializeComponent();

            SetWallet();

            LoadInitialData(jobId);

            DataContext = this;
        }

        private void LoadInitialData(int jobId)
        {
            Username = "Welcome, " + UserContext.LoggedInUsername;

            var job = _context.Jobs
                .Where(j => j.Id == jobId)
                .FirstOrDefault();

            JobId = jobId;

            title.Content = job.Title;
            description.Text = job.Description;
            payment.Content = job.IsPaymentHourly ? "Payment: Hourly" : "Payment: Fixed Price";
            value.Content = "Est. Value: $" + job.Price;
            experience.Content = "Experience: " + (job.ExperienceLevel == 1 ? "Beginner" : (job.ExperienceLevel == 2 ? "Intermediate" : "Advanced"));
            time.Content = "Est. Time: " + job.ProjectLength + (job.ProjectLength > 1 ? " months" : "month");

            var myBid = _context.Bids.FirstOrDefault(b => b.JobId == jobId && b.FreelancerId == UserContext.LoggedInUserId);

            if (myBid != null)
            {
                if (myBid.BidStatus == 1)
                {
                    message.Text = myBid.Message;
                    bid.Text = myBid.Amount.ToString();
                }
            }
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

        #region Redirects

        private void BackRedirect(object sender, MouseButtonEventArgs e)
        {
            var myJobsWindow = new MyJobs();
            myJobsWindow.Show();
            Close();
        }

        private void FindWorkRedirect(object sender, MouseButtonEventArgs e)
        {
            var findWorkWindow = new Main();
            findWorkWindow.Show();
            Close();
        }

        private void MyJobsRedirect(object sender, MouseButtonEventArgs e)
        {
            var myJobsWindow = new MyJobs();
            myJobsWindow.Show();
            Close();
        }

        private void MessagesRedirect(object sender, MouseButtonEventArgs e)
        {
            var messagesView = new Messages();
            messagesView.Show();
            Close();
        }

        #endregion

        private void SubmitProposal(object sender, RoutedEventArgs e)
        {
            var myBid = _context.Bids.FirstOrDefault(b => b.JobId == JobId && b.FreelancerId == UserContext.LoggedInUserId);

            if (myBid != null)
            {
                MessageBoxResult error = MessageBox.Show("You already submitted a proposal for this job!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to submit this proposal?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _context.Bids.InsertOnSubmit(new Bid
                {
                    Amount = int.Parse(bid.Text),
                    BidStatus = 1,
                    CreatedAt = DateTime.Now,
                    FreelancerId = UserContext.LoggedInUserId,
                    IsDeleted = false,
                    JobId = JobId,
                    Message = message.Text
                });

                _context.SubmitChanges();

                var mainWindow = new Main();
                mainWindow.Show();

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
