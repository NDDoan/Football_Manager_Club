using System.Configuration;
using System.Data;
using System.Windows;
using FootballManagerWPF.Models;
using FootballManagerWPF.Services;

namespace FootballManagerWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SessionStorageService _sessionStorage = new SessionStorageService();
        public SessionData CurrentSession { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Khôi phục phiên làm việc nếu có
            try
            {
                CurrentSession = _sessionStorage.LoadSession();
                // Bạn có thể đặt thông tin này vào Application.Current.Properties nếu cần
                Application.Current.Properties["SessionData"] = CurrentSession;
            }
            catch
            {
                CurrentSession = new SessionData();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            // Trước khi thoát, lưu lại phiên làm việc hiện tại.
            // Ví dụ: lấy thông tin từ Application.Current.Properties nếu bạn lưu ở đó.
            if (Application.Current.Properties.Contains("SessionData"))
            {
                CurrentSession = Application.Current.Properties["SessionData"] as SessionData;
            }
            _sessionStorage.SaveSession(CurrentSession);
        }
    }

}
