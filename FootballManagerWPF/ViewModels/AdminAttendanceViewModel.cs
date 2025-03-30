using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Services;
using FootballManagerWPF.Models;
using System.Windows.Input;

namespace FootballManagerWPF.ViewModels
{
    public class AdminAttendanceViewModel : BaseViewModel
    {
        private readonly IAttendanceService _attendanceService;

        public ObservableCollection<AttendanceRecord> AttendanceRecords { get; set; }
        public ICommand RefreshCommand { get; }

        public AdminAttendanceViewModel() : this(new AttendanceService())
        {
        }

        public AdminAttendanceViewModel(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
            LoadAttendanceRecords();
            RefreshCommand = new RelayCommand(o => LoadAttendanceRecords());
        }

        private void LoadAttendanceRecords()
        {
            var records = _attendanceService.GetAllAttendanceRecords();
            AttendanceRecords = new ObservableCollection<AttendanceRecord>(records);
            OnPropertyChanged(nameof(AttendanceRecords));
        }
    }
}
