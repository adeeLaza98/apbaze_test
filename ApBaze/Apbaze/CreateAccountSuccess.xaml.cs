﻿using System;
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
using System.Windows.Shapes;

namespace Apbaze
{
    /// <summary>
    /// Interaction logic for CreateAccountSuccess.xaml
    /// </summary>
    public partial class CreateAccountSuccess : Window
    {
        public CreateAccountSuccess()
        {
            InitializeComponent();
        }

        private void GoToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }
    }
}
