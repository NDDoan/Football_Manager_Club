using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Views;
using System.Windows.Input;
using System.Windows;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Services;

namespace FootballManagerWPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private readonly AuthenticationService _authService;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthenticationService();
            LoginCommand = new RelayCommand(ExecuteLogin, _ => true);
        }

        private void ExecuteLogin(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter your Username and Password.";
                return;
            }

            // Giả sử ValidateUser bây giờ trả về bool và out string role, out int userId
            if (_authService.ValidateUser(Username, Password, out string role, out int userId))
            {
                // Lưu userId vào Application.Current.Properties để sử dụng sau này
                Application.Current.Properties["LoggedInUserId"] = userId;

                // Log hoạt động đăng nhập (nếu cần, bạn có thể gọi ActivityLogService ở đây)

                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    var adminDashboard = new AdminDashboardView();
                    adminDashboard.Show();
                }
                else if (role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                {
                    var employeeDashboard = new EmployeeDashboardView();
                    employeeDashboard.Show();
                }

                CloseLoginWindow();
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }
        private void CloseLoginWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
