using Apbaze.StaticClasses;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CreateJob.xaml
    /// </summary>
    public partial class CreateJob : Window, INotifyPropertyChanged
    {
        #region Properties
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();
        public string Wallet { get; set; }

        private string jobTitle;

        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                if (jobTitle != value)
                {
                    jobTitle = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string jobDescription;

        public string JobDescription
        {
            get { return jobDescription; }
            set
            {
                if (jobDescription != value)
                {
                    jobDescription = value;
                    OnPropertyChanged(nameof(JobDescription));
                }
            }
        }

        private string paymentType;

        public string PaymentType
        {
            get { return paymentType; }
            set
            {
                if (paymentType != value)
                {
                    paymentType = value;
                    OnPropertyChanged(nameof(PaymentType));
                }
            }
        }

        private string jobPrice;

        public string JobPrice
        {
            get { return jobPrice; }
            set
            {
                if (jobPrice != value)
                {
                    jobPrice = value;
                    OnPropertyChanged(nameof(JobPrice));
                }
            }
        }

        private string jobExperience;

        public string JobExperience
        {
            get { return jobExperience; }
            set
            {
                if (jobExperience != value)
                {
                    jobExperience = value;
                    OnPropertyChanged(nameof(JobExperience));
                }
            }
        }

        private string projectLength;

        public string ProjectLength
        {
            get { return projectLength; }
            set
            {
                if (projectLength != value)
                {
                    projectLength = value;
                    OnPropertyChanged(nameof(ProjectLength));
                }
            }
        }

        private string jobCategory;
        public string JobCategory
        {
            get { return jobCategory; }
            set
            {
                if (jobCategory != value)
                {
                    jobCategory = value;
                    OnPropertyChanged(nameof(JobCategory));
                }
            }
        }

        public List<string> Categories { get; set; }
        public List<string> PaymentTypes { get; set; }
        public List<string> ExperienceLevels { get; set; }
        public string Username { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateJob()
        {
            InitializeComponent();

            SetWallet();

            Username = "Welcome, " + UserContext.LoggedInUsername;

            Categories = _context.Categories.Select(c => c.Title).ToList();
            PaymentTypes = new List<string>() { "Hourly rate", "Fixed price" };
            ExperienceLevels = new List<string>() { "Beginner", "Intermediate", "Advanced" };

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
            var postedJobsWindow = new MainClient();
            postedJobsWindow.Show();
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
            var postedJobsWindow = new MainClient();
            postedJobsWindow.Show();
            Close();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Title == JobCategory);

            _context.Jobs.InsertOnSubmit(new Job()
            {
                Title = JobTitle,
                Description = JobDescription,
                IsPaymentHourly = PaymentType == "Hourly rate",
                Price = decimal.Parse(JobPrice),
                ExperienceLevel = JobExperience == "Beginner" ? 1 : (JobExperience == "Intermediate" ? 2 : 3),
                ProjectLength = int.Parse(ProjectLength),
                CategoryId = category.Id,
                ClientId = UserContext.LoggedInUserId,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                JobStatus = 1
            });

            _context.SubmitChanges();

            var mainClientWindows = new MainClient();
            mainClientWindows.Show();
            Close();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var walletWindow = new Wallet();
            walletWindow.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var postedJobsWindow = new MainClient();
                postedJobsWindow.Show();
                Close();
            }
        }
    }
}
