using Apbaze.StaticClasses;
using Apbaze.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Wallet.xaml
    /// </summary>
    public partial class Wallet : Window, INotifyPropertyChanged
    {
        #region Properties
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();
        public string Username { get; set; }

        private string walletValue;

        public string WalletValue
    {
            get { return walletValue; }
            set
            {
                if (walletValue != value)
                {
                    walletValue = value;
                    OnPropertyChanged(nameof(WalletValue));
                }
            }
        }

        public int ChatRoomId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isFreelancer;

        public bool IsFreelancer
        {
            get { return isFreelancer; }
            set
            {
                if (isFreelancer != value)
                {
                    isFreelancer = value;
                    OnPropertyChanged(nameof(IsFreelancer));
                }
            }
        }

        private bool isClient;

        public bool IsClient
        {
            get { return isClient; }
            set
            {
                if (isClient != value)
                {
                    isClient = value;
                    OnPropertyChanged(nameof(IsClient));
                }
            }
        }

        public ObservableCollection<TransactionViewModel> WalletTransactions { get; set; }

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Wallet()
        {
            InitializeComponent();

            SetWallet();

            LoadTransactions();

            Username = "Welcome, " + UserContext.LoggedInUsername;

            var user = _context.Users.FirstOrDefault(u => u.Id == UserContext.LoggedInUserId);

            IsClient = user.Role.Name == "Client";
            IsFreelancer = user.Role.Name == "Freelancer";

            DataContext = this;
        }

        private void LoadTransactions()
        {
            var transactions = _context.Transactions
                .Where(c => c.SenderId == UserContext.LoggedInUserId || c.ReceiverId == UserContext.LoggedInUserId)
                .OrderByDescending(cr => cr.CreatedAt)
                .Select(c => new TransactionViewModel
                {
                    Id = c.Id,
                    Amount = c.SenderId == UserContext.LoggedInUserId ? "- $" + c.Amount.ToString() : "+ $" + c.Amount.ToString(),
                    Color = c.SenderId == UserContext.LoggedInUserId ? "Red" : "#14a800",
                    From = c.User1 != null ? c.User1.Username : "-",
                    To = c.User != null ? c.User.Username : "-",
                    CreatedAt = c.CreatedAt.Value
                })
                .ToList();

            if (WalletTransactions != null)
            {
                WalletTransactions.Clear();

                foreach (var transaction in transactions)
                {
                    WalletTransactions.Add(transaction);
                }
            }
            else
            {
                WalletTransactions = new ObservableCollection<TransactionViewModel>(transactions);
            }
        }

        private void SetWallet()
        {
            var transactions = _context.Transactions
                    .Where(t => t.SenderId == UserContext.LoggedInUserId || t.ReceiverId == UserContext.LoggedInUserId)
                    .ToList();

            var walletPlus = transactions.Where(t => t.ReceiverId == UserContext.LoggedInUserId).Sum(t => t.Amount);
            var walletMinus = transactions.Where(t => t.SenderId == UserContext.LoggedInUserId).Sum(t => t.Amount * (-1));

            WalletValue = "Wallet: $" + (walletPlus + walletMinus).ToString();
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

        private void PostedJobsRedirect(object sender, MouseButtonEventArgs e)
        {
            var mainClientView = new MainClient();
            mainClientView.Show();
            Close();
        }

        private void ClientMessagesRedirect(object sender, MouseButtonEventArgs e)
        {
            var messagesView = new Messages();
            messagesView.Show();
            Close();
        }

        #endregion

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var walletWindow = new Wallet();
            walletWindow.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _context.Transactions.InsertOnSubmit(new Transaction
            {
                Amount = 500,
                SenderId = null,
                ReceiverId = UserContext.LoggedInUserId,
                CreatedAt = DateTime.Now
            });

            _context.SubmitChanges();

            SetWallet();

            LoadTransactions();
        }
    }
}
