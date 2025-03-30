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
using FootballManagerWPF.ViewModels;

namespace FootballManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for EmployeeEditView.xaml
    /// </summary>
    public partial class EmployeeEditView : Window
    {
        public EmployeeEditView()
        {
            InitializeComponent();
            if (DataContext is EmployeeEditViewModel vm)
            {
                vm.CloseAction = new Action<bool?>(this.CloseDialog);
            }
        }

        private void CloseDialog(bool? result)
        {
            this.DialogResult = result;
            this.Close();
        }

        // Thêm xử lý sự kiện cho PasswordBox
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Giả sử PasswordBox có tên là PasswordBox
            if (DataContext is EmployeeEditViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        }
    }
}