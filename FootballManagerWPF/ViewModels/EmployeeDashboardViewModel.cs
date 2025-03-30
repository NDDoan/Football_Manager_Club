using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using System.Windows.Input;
using FootballManagerWPF.Models;
using FootballManagerWPF.Services;
using FootballManagerWPF.Views;
using System.Windows;

namespace FootballManagerWPF.ViewModels
{
    public class EmployeeDashboardViewModel : BaseViewModel
    {
        private string _employeeUsername;
        public string EmployeeUsername
        {
            get => _employeeUsername;
            set { _employeeUsername = value; OnPropertyChanged(); }
        }
        private string _employeeFullName;
        public string EmployeeFullName
        {
            get => _employeeFullName;
            set { _employeeFullName = value; OnPropertyChanged(); }
        }
        private string _employeeAvatar;
        public string EmployeeAvatar
        {
            get => _employeeAvatar;
            set { _employeeAvatar = value; OnPropertyChanged(); }
        }

        private Notification _latestAdminNotification;
        public Notification LatestAdminNotification
        {
            get => _latestAdminNotification;
            set { _latestAdminNotification = value; OnPropertyChanged(); }
        }

        public ICommand OpenAttendanceCommand { get; }
        public ICommand OpenPayrollCommand { get; }
        public ICommand OpenLeaveRequestCommand { get; }
        public ICommand OpenNotificationsCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand RefreshNotificationCommand { get; }

        public EmployeeDashboardViewModel()
        {

            RefreshNotificationCommand = new RelayCommand(o => LoadLatestNotification());
            LoadLatestNotification();

            // Lấy UserId từ Application.Current.Properties sau khi đăng nhập thành công
            if (Application.Current.Properties.Contains("LoggedInUserId"))
            {
                int userId = (int)Application.Current.Properties["LoggedInUserId"];
                var empService = new EmployeeService();
                EmployeeInfo info = empService.GetEmployeeInfo(userId);
                if (info != null)
                {
                    EmployeeUsername = info.Username;
                    EmployeeFullName = info.FullName;
                    EmployeeAvatar = info.Avatar;
                }
                else
                {
                    EmployeeUsername = "default";
                    EmployeeFullName = "John Doe";
                    EmployeeAvatar = "Images/employee_avatar.png";
                }
            }
            else
            {
                EmployeeUsername = "default";
                EmployeeFullName = "John Doe";
                EmployeeAvatar = "Images/employee_avatar.png";
            }

            OpenAttendanceCommand = new RelayCommand(o =>
            {
                int employeeId = (int)Application.Current.Properties["LoggedInUserId"];
                var attendanceView = new EmployeeAttendanceView(employeeId);
                attendanceView.Show();
            });
            OpenPayrollCommand = new RelayCommand(o =>
            {
                int employeeId = (int)Application.Current.Properties["LoggedInUserId"];
                var payrollView = new EmployeePayrollView(employeeId);
                payrollView.Show();
            });
            OpenLeaveRequestCommand = new RelayCommand(o =>
            {
                int employeeId = (int)Application.Current.Properties["LoggedInUserId"];
                var leaveRequestView = new EmployeeLeaveRequestView(employeeId);
                leaveRequestView.Show();
            });
            OpenNotificationsCommand = new RelayCommand(o =>
            {
                var notificationView = new EmployeeNotificationView();
                notificationView.Show();
            });

            LogoutCommand = new RelayCommand(o =>
            {
                // Mở lại cửa sổ Login
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Xóa thông tin đăng nhập nếu cần
                if (Application.Current.Properties.Contains("LoggedInUserId"))
                    Application.Current.Properties.Remove("LoggedInUserId");

                // Đóng cửa sổ EmployeeDashboard
                var dashboard = Application.Current.Windows.OfType<EmployeeDashboardView>().FirstOrDefault();
                if (dashboard != null)
                {
                    dashboard.Close();
                }
            });
        }

        private void LoadLatestNotification()
        {
            // Giả sử NotificationService có phương thức GetLatestAdminNotification
            var notificationService = new NotificationService();
            LatestAdminNotification = notificationService.GetLatestAdminNotification();
        }
    }
}