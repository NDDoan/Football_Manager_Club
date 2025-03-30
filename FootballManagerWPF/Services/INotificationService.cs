using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;

namespace FootballManagerWPF.Services
{
    public interface INotificationService
    {
        List<Notification> GetNotifications();
        public void SendNotification(Notification notification);
        public List<Notification> GetNotificationsForEmployee(int? departmentId = null);
    }
}
