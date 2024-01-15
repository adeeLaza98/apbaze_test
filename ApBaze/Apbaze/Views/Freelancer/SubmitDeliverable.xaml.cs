using Apbaze.StaticClasses;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
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
    /// Interaction logic for SubmitDeliverable.xaml
    /// </summary>
    public partial class SubmitDeliverable : Window, INotifyPropertyChanged
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

        public SubmitDeliverable(int jobId)
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

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Set the file path to the TextBox
                FilePathTextBox.Text = openFileDialog.FileName;
                // You can also use the openFileDialog.FileName to handle the file as needed
                // For example, you might want to read the file content or perform other operations.
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

        private void BackRedirect(object sender, MouseButtonEventArgs e)
        {
            var myJobsWindow = new MyJobs();
            myJobsWindow.Show();
            Close();
        }

        #endregion

        private void Submit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to submit this deliverable?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _context.Deliverables.InsertOnSubmit(new Deliverable
                {
                    JobId = JobId,
                    FilePath = FilePathTextBox.Text,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    FreelancerId = UserContext.LoggedInUserId,
                    DeliverableStatus = 1
                });

                _context.SubmitChanges();

                var myJobsWindow = new MyJobs();
                myJobsWindow.Show();

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
