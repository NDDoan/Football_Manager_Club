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
using System.Windows;

namespace FootballManagerWPF.ViewModels
{
    public class EmployeeAttendanceViewModel : BaseViewModel
    {
        private readonly IAttendanceService _attendanceService;
        private readonly int _employeeId;

        public EmployeeAttendanceViewModel(int employeeId)
            : this(new AttendanceService(), employeeId)
        {
        }

        // Constructor chính với employeeId
        public EmployeeAttendanceViewModel(IAttendanceService attendanceService, int employeeId)
        {
            _attendanceService = attendanceService;
            _employeeId = employeeId;
            LoadAttendanceRecords();
            ClockInCommand = new RelayCommand(o => ClockIn());
            RefreshAttendanceCommand = new RelayCommand(o => LoadAttendanceRecords());
            ClockOutCommand = new RelayCommand(o => ClockOut());
        }

        private void LoadAttendanceRecords()
        {
            List<AttendanceRecord> records = _attendanceService.GetAttendanceForEmployee(_employeeId);
            AttendanceRecords = new ObservableCollection<AttendanceRecord>(records);
            OnPropertyChanged(nameof(AttendanceRecords));
        }

        public ObservableCollection<AttendanceRecord> AttendanceRecords { get; set; }
        public ICommand ClockInCommand { get; }
        public ICommand RefreshAttendanceCommand { get; }
        public ICommand ClockOutCommand { get; }

        private void ClockIn()
        {
            try
            {
                if (_attendanceService.HasClockedInToday(_employeeId))
                {
                    MessageBox.Show("Bạn đã chấm công trong ngày hôm nay.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                _attendanceService.ClockIn(_employeeId);
                MessageBox.Show("Chấm công thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi chấm công: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClockOut()
        {
            try
            {
                _attendanceService.ClockOut(_employeeId);
                MessageBox.Show("Check out thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadAttendanceRecords();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi check out: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
