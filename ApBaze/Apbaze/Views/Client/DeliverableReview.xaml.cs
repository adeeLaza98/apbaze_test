using Apbaze.StaticClasses;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for DeliverableReview.xaml
    /// </summary>
    public partial class DeliverableReview : Window, INotifyPropertyChanged
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

        public DeliverableReview(int jobId)
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

            var deliverable = _context.Deliverables.FirstOrDefault(d => d.JobId == jobId && d.DeliverableStatus == 1);

            FilePathTextBox.Text = deliverable.FilePath;
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

        private void FindWorkRedirect(object sender, MouseButtonEventArgs e)
        {
            var findWorkWindow = new MainClient();
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

        private void AcceptDeliverable(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to accept this deliverable?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var transactions = _context.Transactions
                    .Where(t => t.SenderId == UserContext.LoggedInUserId || t.ReceiverId == UserContext.LoggedInUserId)
                    .ToList();

                var acceptedBid = _context.Bids
                    .Where(b => b.JobId == JobId && b.BidStatus == 2)
                    .FirstOrDefault();

                var walletPlus = transactions.Where(t => t.ReceiverId == UserContext.LoggedInUserId).Sum(t => t.Amount);
                var walletMinus = transactions.Where(t => t.SenderId == UserContext.LoggedInUserId).Sum(t => t.Amount * (-1));

                if (walletPlus + walletMinus < acceptedBid.Amount)
                {
                    MessageBox.Show("You don't have enough money to accept this deliverable.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var deliverable = _context.Deliverables
                    .Where(d => d.JobId == JobId && d.DeliverableStatus == 1)
                    .FirstOrDefault();

                deliverable.DeliverableStatus = 2;

                var job = _context.Jobs
                    .Where(j => j.Id == JobId)
                    .FirstOrDefault();

                job.JobStatus = 3;

                _context.Transactions.InsertOnSubmit(new Transaction
                {
                    Amount = (int)acceptedBid.Amount,
                    CreatedAt = DateTime.Now,
                    SenderId = UserContext.LoggedInUserId,
                    ReceiverId = acceptedBid.FreelancerId
                });

                _context.SubmitChanges();

                var mainClientWindow = new MainClient();
                mainClientWindow.Show();

                Close();
            }
        }

        private void RejectDeliverable(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reject this deliverable?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var deliverable = _context.Deliverables
                    .Where(d => d.JobId == JobId && d.DeliverableStatus == 1)
                    .FirstOrDefault();

                deliverable.DeliverableStatus = 3;

                _context.SubmitChanges();

                var mainClientWindow = new MainClient();
                mainClientWindow.Show();

                Close();
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var walletWindow = new Wallet();
            walletWindow.Show();
            Close();
        }

        private void Download_Action(object sender, RoutedEventArgs e)
        {
            var transactions = _context.Transactions
                    .Where(t => t.SenderId == UserContext.LoggedInUserId || t.ReceiverId == UserContext.LoggedInUserId)
                    .ToList();

            var acceptedBid = _context.Bids
                .Where(b => b.JobId == JobId && b.BidStatus == 2)
                .FirstOrDefault();

            var walletPlus = transactions.Where(t => t.ReceiverId == UserContext.LoggedInUserId).Sum(t => t.Amount);
            var walletMinus = transactions.Where(t => t.SenderId == UserContext.LoggedInUserId).Sum(t => t.Amount * (-1));

            if (walletPlus + walletMinus < acceptedBid.Amount)
            {
                MessageBox.Show("You don't have enough money to accept this deliverable.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string sourceFilePath = FilePathTextBox.Text;

            string fileName = System.IO.Path.GetFileName(FilePathTextBox.Text);

            string destinationPath = @"C:\Folder\" + fileName; 

            try
            {
                File.Copy(sourceFilePath, destinationPath, true);
                MessageBox.Show("File downloaded successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading file: {ex.Message}");
            }
        }
    }
}
