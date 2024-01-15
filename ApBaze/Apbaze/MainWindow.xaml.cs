using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public bool IsDarkTheme { get; set; }
        public static AppBazeDataContext _context = new AppBazeDataContext();
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        public MainWindow()
        {
            InitializeComponent();
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccountWindow = new CreateAccount();
            createAccountWindow.Show();
            this.Close();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = _context.Users
                .Where(u => u.Username == txtUsername.Text)
                .ToList()
                .Where(u => u.Password == Utils.HashPassword(txtPassword.Password, u.CreatedAt.ToString("yyyyMMddHHmmss")))
                .FirstOrDefault();

            if(user == null)
            {

                MessageBoxResult result = MessageBox.Show("Username or password incorrect!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPassword.Clear();
                txtUsername.Clear();
                txtUsername.Focus();
            }
            else
            {
                UserContext.LoggedInUserId = user.Id;
                UserContext.LoggedInUsername = user.Username;


                if (user.Role.Name == "Freelancer")
                {
                    Main main = new Main();
                    main.Show();
                }
                else
                {
                    MainClient main = new MainClient();
                    main.Show();
                }
                
                this.Close();
            }
        }
    }
}
