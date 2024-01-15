using Apbaze.StaticClasses;
using Apbaze.ViewModels;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;

namespace Apbaze
{
    /// <summary>
    /// Interaction logic for ClientJob.xaml
    /// </summary>
    public partial class ClientJob : Window
    {
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();
        public ObservableCollection<JobProposal> JobProposals { get; set; }
        public string Username { get; set; }
        public int JobId { get; set; }
        public string Wallet { get; set; }
        public ClientJob(int jobId)
        {
            InitializeComponent();

            SetWallet();

            Username = "Welcome, " + UserContext.LoggedInUsername;
            JobId = jobId;

            LoadProposals();

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

        private void LoadProposals()
        {
            var proposals = _context.Bids
                .Where(b => b.JobId == JobId)
                .Select(b => new JobProposal()
                {
                    CompletedJobs = "Completed jobs: " + b.User.Bids.Count(bd => bd.BidStatus == 2),
                    Description = b.Message,
                    Experience = "Experience level: " + (b.User.ExperienceLevel == 1 ? "Beginner" : (b.User.ExperienceLevel == 2 ? "Intermediate" : "Advanced")),
                    FullName = b.User.FirstName + " " + b.User.LastName,
                    Price = "Price: $" + b.Amount,
                    Id = b.Id
                })
                .ToList();

            if (JobProposals != null)
            {
                JobProposals.Clear();

                foreach (var proposal in proposals)
                {
                    JobProposals.Add(proposal);
                }
            }
            else
            {
                JobProposals = new ObservableCollection<JobProposal>(proposals);
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

        private void BackRedirect(object sender, MouseButtonEventArgs e)
        {
            var mainClientWindow = new MainClient();
            mainClientWindow.Show();
            Close();
        }

        #endregion

        private void AcceptProposal(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to accept this proposal ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Button clickedButton = (Button)sender;

                if (clickedButton.Tag is int bidId)
                {
                    var bid = _context.Bids.FirstOrDefault(b => b.Id == bidId);
                    bid.BidStatus = 2;

                    var job = _context.Jobs.FirstOrDefault(j => j.Id == bid.JobId);
                    job.JobStatus = 2;

                    _context.ChatRooms.InsertOnSubmit(new ChatRoom()
                    {
                        CreatedAt = DateTime.Now,
                        Title = bid.User.Username + ", " + UserContext.LoggedInUsername,
                        ClientId = UserContext.LoggedInUserId,
                        FreelancerId = bid.FreelancerId
                    });

                    _context.SubmitChanges();

                    var mainClientWindow = new MainClient();
                    mainClientWindow.Show();
                    Close();
                }
            }
        }

        private void RejectProposal(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (clickedButton.Tag is int bidId)
            {
                var bid = _context.Bids.FirstOrDefault(b => b.Id == bidId);

                bid.BidStatus = 3;

                _context.SubmitChanges();
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
