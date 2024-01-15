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
    /// Interaction logic for Messages.xaml
    /// </summary>
    public partial class Messages : Window, INotifyPropertyChanged
    {
        #region Properties

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();
        public string Username { get; set; }
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

        public string Wallet { get; set; }

        public ObservableCollection<MessageViewModel> MessageItems { get; set; }

        public ObservableCollection<ChatRoomViewModel> ChatRooms { get; set; }

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Messages()
        {
            InitializeComponent();

            SetWallet();

            LoadInitialData();

            LoadChatRoomsAndMessages();

            DataContext = this;
        }

        private void LoadInitialData()
        {
            Username = "Welcome, " + UserContext.LoggedInUsername;

            var user = _context.Users.FirstOrDefault(u => u.Id == UserContext.LoggedInUserId);

            IsClient = user.Role.Name == "Client";
            IsFreelancer = user.Role.Name == "Freelancer";
        }

        private void LoadChatRoomsAndMessages()
        {
            var chatRooms = _context.ChatRooms
                .Where(c => c.ClientId == UserContext.LoggedInUserId || c.FreelancerId == UserContext.LoggedInUserId)
                .Distinct()
                .OrderBy(cr => cr.CreatedAt)
                .Select(c => new ChatRoomViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                })
                .ToList();

            if (ChatRooms != null)
            {
                ChatRooms.Clear();

                foreach (var chatRoomViewModel in ChatRooms)
                    ChatRooms.Add(chatRoomViewModel);
            }
            else
                ChatRooms = new ObservableCollection<ChatRoomViewModel>(chatRooms);

            if (ChatRooms.Count > 0)
            {
                ChatRoomId = ChatRooms.First().Id;

                var messagesDb = _context.Conversations
                    .Where(m => m.ChatRoomId == ChatRooms.First().Id)
                    .OrderBy(m => m.CreatedAt)
                    .Select(m => new MessageViewModel
                    {
                        Text = "   " + m.Message + "   ",
                        Sent = m.SenderId == UserContext.LoggedInUserId,
                        Received = m.ReceiverId == UserContext.LoggedInUserId,
                        Column = m.SenderId == UserContext.LoggedInUserId ? 1 : 2,
                        Alignment = m.SenderId == UserContext.LoggedInUserId ? "Left" : "Right",
                        Background = m.SenderId == UserContext.LoggedInUserId ? "White" : "LightGreen"
                    })
                    .ToList();

                if (MessageItems != null)
                {
                    MessageItems.Clear();

                    foreach (var messageItemViewModel in MessageItems)
                        MessageItems.Add(messageItemViewModel);
                }
                else
                    MessageItems = new ObservableCollection<MessageViewModel>(messagesDb);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _context.Conversations.InsertOnSubmit(new Conversation
            {
                SenderId = UserContext.LoggedInUserId,
                ReceiverId = 5,
                Message = message.Text,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                MessageStatus = 1,
                ChatRoomId = ChatRoomId
            });

            _context.SubmitChanges();

            MessageItems.Add(new MessageViewModel()
            {
                Text = "   " + message.Text + "   ",
                Sent = true,
                Received = false,
                Column = 1,
                Alignment = "Left",
                Background = "White"
            });

            message.Text = string.Empty;
            message.Focus();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _context.Conversations.InsertOnSubmit(new Conversation
                {
                    SenderId = UserContext.LoggedInUserId,
                    ReceiverId = 5,
                    Message = message.Text,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    MessageStatus = 1,
                    ChatRoomId = ChatRoomId
                });

                _context.SubmitChanges();

                MessageItems.Add(new MessageViewModel()
                {
                    Text = "   " + message.Text + "   ",
                    Sent = true,
                    Received = false,
                    Column = 1,
                    Alignment = "Left",
                    Background = "White"
                });

                message.Text = string.Empty;
                message.Focus();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border clickedButton = (Border)sender;

            if (clickedButton.Tag is int chatRoomId)
            {
                ChatRoomId = chatRoomId;

                MessageItems.Clear();

                var messages = _context.Conversations
                .Where(m => m.ChatRoomId == chatRoomId)
                .OrderBy(m => m.CreatedAt)
                .Select(m => new MessageViewModel
                {
                    Text = "   " + m.Message + "   ",
                    Sent = m.SenderId == UserContext.LoggedInUserId,
                    Received = m.ReceiverId == UserContext.LoggedInUserId,
                    Column = m.SenderId == UserContext.LoggedInUserId ? 1 : 2,
                    Alignment = m.SenderId == UserContext.LoggedInUserId ? "Left" : "Right",
                    Background = m.SenderId == UserContext.LoggedInUserId ? "White" : "LightGreen"
                });

                foreach (var message in messages)
                    MessageItems.Add(message);
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
