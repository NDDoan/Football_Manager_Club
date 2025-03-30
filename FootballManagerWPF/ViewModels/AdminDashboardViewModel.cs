using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using System.Windows.Input;
using FootballManagerWPF.Views;
using System.Windows;
using FootballManagerWPF.Services;

namespace FootballManagerWPF.ViewModels
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private string _adminName;
        public string AdminName
        {
            get => _adminName;
            set { _adminName = value; OnPropertyChanged(); }
        }

        private string _adminEmail;
        public string AdminEmail
        {
            get => _adminEmail;
            set { _adminEmail = value; OnPropertyChanged(); }
        }

        private string _adminAvatar;
        public string AdminAvatar
        {
            get => _adminAvatar;
            set { _adminAvatar = value; OnPropertyChanged(); }
        }

        public ICommand OpenEmployeeManagementCommand { get; }
        public ICommand OpenDepartmentManagementCommand { get; }
        public ICommand OpenPayrollManagementCommand { get; }
        public ICommand OpenAttendanceManagementCommand { get; }
        public ICommand OpenNotificationManagementCommand { get; }
        public ICommand OpenActivityLogManagementCommand { get; }
        public ICommand OpenLeaveRequestManagementCommand { get; }
        public ICommand OpenBackupManagementCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand OpenReportManagementCommand { get; }

        public AdminDashboardViewModel()
        {
            LoadAdminInfo();

            OpenEmployeeManagementCommand = new RelayCommand(o => new EmployeeManagementView().Show());
            OpenDepartmentManagementCommand = new RelayCommand(o => new DepartmentManagementView().Show());
            OpenPayrollManagementCommand = new RelayCommand(o => new AdminPayrollView().Show());
            OpenAttendanceManagementCommand = new RelayCommand(o => new AdminAttendanceView().Show());
            OpenNotificationManagementCommand = new RelayCommand(o => new AdminNotificationView().Show());
            OpenActivityLogManagementCommand = new RelayCommand(o => new ActivityLogManagementView().Show());
            OpenLeaveRequestManagementCommand = new RelayCommand(o => new AdminLeaveRequestView().Show());
            OpenBackupManagementCommand = new RelayCommand(o => new BackupManagementView().Show());
            OpenReportManagementCommand = new RelayCommand(o => new AdminReportView().Show());
            LogoutCommand = new RelayCommand(o =>
            {
                // Tạo và hiển thị cửa sổ Login trước khi đóng Dashboard
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Đóng cửa sổ Admin Dashboard hiện tại
                var dashboard = Application.Current.Windows.OfType<AdminDashboardView>().FirstOrDefault();
                if (dashboard != null)
                {
                    dashboard.Close();
                }
            });
        }

        private void LoadAdminInfo()
        {
            var adminService = new AdminService();
            var adminInfo = adminService.GetAdminInfo();
            if (!string.IsNullOrEmpty(adminInfo.FullName))
            {
                AdminName = adminInfo.FullName;
                AdminEmail = adminInfo.Email;
                AdminAvatar = adminInfo.Avatar;
            }
            else
            {
                // Nếu không tìm thấy, sử dụng giá trị mặc định
                AdminName = "Admin User";
                AdminEmail = "admin@example.com";
                AdminAvatar = "Images/admin_avatar.png";
            }
        }
    }
}