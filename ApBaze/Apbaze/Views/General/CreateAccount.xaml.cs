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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Apbaze.StaticClasses;
using MaterialDesignThemes.Wpf;

namespace Apbaze
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window, INotifyPropertyChanged
    {
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private static AppBazeDataContext _context = new AppBazeDataContext();

        #region Properties

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string userType;
        public string UserType
        {
            get { return userType; }
            set
            {
                if (userType != value)
                {
                    userType = value;
                    OnPropertyChanged(nameof(UserType));
                }
            }
        }

        public List<string> UserTypes { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateAccount()
        {
            InitializeComponent();

            UserTypes = new List<string> { "Client", "Freelancer" };

            DataContext = this;
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

        private void GoToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var roles = _context.Roles.ToList();
            var timestamp = DateTime.Now;

            if (Utils.IsValidPhoneNumber(txtPhone.Text) == false)
            {
                MessageBoxResult error = MessageBox.Show("Invalid phone number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                txtPhone.Focus();

                return;
            }

            if (Utils.IsValidEmail(txtEmail.Text) == false)
            {
                MessageBoxResult error = MessageBox.Show("Invalid email!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                txtEmail.Focus();

                return;
            }

            var users = _context.Users
                .Where(u => u.Username == txtUsername.Text)
                .ToList();

            if(users.Count > 0) 
            {
                MessageBoxResult error = MessageBox.Show("There is already an user with this username!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                txtEmail.Focus();

                return;
            }

            var usersEmails = _context.Users
               .Where(u => u.Email == txtEmail.Text)
               .ToList();

            if (usersEmails.Count > 0)
            {
                MessageBoxResult error = MessageBox.Show("There is already an user with this email!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                txtEmail.Focus();

                return;
            }

            _context.Users.InsertOnSubmit(new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Password = Utils.HashPassword(txtPassword.Password, timestamp.ToString("yyyyMMddHHmmss")),
                CreatedAt = timestamp,
                IsDeleted = false,
                ExperienceLevel = 1,
                RoleId = roles.FirstOrDefault(r => r.Name == UserType).Id
            });

            _context.SubmitChanges();

            CreateAccountSuccess successWindow = new CreateAccountSuccess();
            successWindow.Show();
            this.Close();
        }
    }
}
