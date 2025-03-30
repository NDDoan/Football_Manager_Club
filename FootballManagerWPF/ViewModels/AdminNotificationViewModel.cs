using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Models;
using FootballManagerWPF.Services;
using System.Windows.Input;

namespace FootballManagerWPF.ViewModels
{
    public class AdminNotificationViewModel : BaseViewModel
    {
        private readonly INotificationService _notificationService;

        public ObservableCollection<Notification> Notifications { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand SendNotificationCommand { get; }

        // Thuộc tính cho thông báo mới
        private string _newTitle;
        public string NewTitle
        {
            get => _newTitle;
            set { _newTitle = value; OnPropertyChanged(); }
        }
        private string _newContent;
        public string NewContent
        {
            get => _newContent;
            set { _newContent = value; OnPropertyChanged(); }
        }
        private int? _targetDepartmentId; // Nếu null: gửi cho tất cả
        public int? TargetDepartmentId
        {
            get => _targetDepartmentId;
            set { _targetDepartmentId = value; OnPropertyChanged(); }
        }

        public AdminNotificationViewModel() : this(new NotificationService())
        {
        }

        public AdminNotificationViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            LoadNotifications();
            RefreshCommand = new RelayCommand(o => LoadNotifications());
            SendNotificationCommand = new RelayCommand(o => SendNotification());
        }

        private void LoadNotifications()
        {
            // Không cần lọc theo phòng ban nếu Admin gửi cho tất cả
            var list = _notificationService.GetNotificationsForEmployee(null);
            Notifications = new ObservableCollection<Notification>(list);
            OnPropertyChanged(nameof(Notifications));
        }

        private void SendNotification()
        {
            // Giả sử SenderUserId là Admin (bạn có thể lấy từ session)
            var notification = new Notification
            {
                Title = NewTitle,
                Content = NewContent,
                DepartmentId = TargetDepartmentId,
                SenderUserId = 1 // ví dụ
            };
            _notificationService.SendNotification(notification);
            LoadNotifications();
        }
    }
}