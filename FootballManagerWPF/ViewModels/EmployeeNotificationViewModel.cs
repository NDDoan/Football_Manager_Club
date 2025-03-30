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
    public class EmployeeNotificationViewModel : BaseViewModel
    {
        private readonly INotificationService _notificationService;
        public ObservableCollection<Notification> Notifications { get; set; }
        public ICommand RefreshNotificationsCommand { get; }

        public EmployeeNotificationViewModel() : this(new NotificationService()) { }

        public EmployeeNotificationViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            Notifications = new ObservableCollection<Notification>(_notificationService.GetNotifications());
            RefreshNotificationsCommand = new RelayCommand(o => RefreshNotifications());
        }

        private void RefreshNotifications()
        {
            Notifications.Clear();
            foreach (var n in _notificationService.GetNotifications())
            {
                Notifications.Add(n);
            }
        }
    }
}
