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
using System.Windows.Shapes;
using FootballManagerWPF.Services;
using Microsoft.Win32;

namespace FootballManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for BackupManagementView.xaml
    /// </summary>
    public partial class BackupManagementView : Window
    {
        private readonly IBackupService _backupService;

        public BackupManagementView()
        {
            InitializeComponent();
            _backupService = new BackupService();
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = "EmployeeBackup.json"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _backupService.BackupEmployees(saveFileDialog.FileName);
                MessageBox.Show("Sao lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _backupService.RestoreEmployees(openFileDialog.FileName);
                MessageBox.Show("Phục hồi thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}