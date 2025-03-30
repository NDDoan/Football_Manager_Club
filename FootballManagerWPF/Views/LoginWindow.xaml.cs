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
using FootballManagerWPF.ViewModels;

namespace FootballManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Cập nhật giá trị Password trong ViewModel
            if (this.DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        }
        private void LoginMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Reset lại vị trí và phát lại
            MediaElement media = sender as MediaElement;
            media.Position = TimeSpan.Zero;
            media.Play();
        }

    }
}
