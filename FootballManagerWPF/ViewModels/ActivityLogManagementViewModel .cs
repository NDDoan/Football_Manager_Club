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
    public class ActivityLogManagementViewModel : BaseViewModel
    {
        private readonly IActivityLogService _activityLogService;
        public ObservableCollection<ActivityLog> ActivityLogs { get; set; }
        public ICommand RefreshActivityLogCommand { get; }

        public ActivityLogManagementViewModel() : this(new ActivityLogService()) { }

        public ActivityLogManagementViewModel(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
            LoadActivityLogs();
            RefreshActivityLogCommand = new RelayCommand(o => LoadActivityLogs());
        }
        private void LoadActivityLogs()
        {
            var logs = _activityLogService.GetAllActivityLogs();
            ActivityLogs = new ObservableCollection<ActivityLog>(logs);
            OnPropertyChanged(nameof(ActivityLogs));
        }
    }
}
