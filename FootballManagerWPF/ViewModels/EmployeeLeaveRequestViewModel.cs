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
    public class EmployeeLeaveRequestViewModel : BaseViewModel
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private int _employeeId;

        public ObservableCollection<LeaveRequest> LeaveRequests { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand AddLeaveRequestCommand { get; }

        // Số ngày nghỉ còn lại trong tháng (giới hạn 7 ngày)
        private int _remainingLeaveDays;
        public int RemainingLeaveDays
        {
            get => _remainingLeaveDays;
            set { _remainingLeaveDays = value; OnPropertyChanged(); }
        }

        public EmployeeLeaveRequestViewModel(ILeaveRequestService leaveRequestService, int employeeId)
        {
            _leaveRequestService = leaveRequestService;
            _employeeId = employeeId;
            RefreshCommand = new RelayCommand(o => LoadLeaveRequests());
            AddLeaveRequestCommand = new RelayCommand(o => AddLeaveRequest());
            LoadLeaveRequests();
            CalculateRemainingLeaveDays();
        }

        // Parameterless constructor cho design-time
        public EmployeeLeaveRequestViewModel() : this(new LeaveRequestService(), 0) { }

        private void LoadLeaveRequests()
        {
            var list = _leaveRequestService.GetAllLeaveRequestsByEmployee(_employeeId);
            LeaveRequests = new ObservableCollection<LeaveRequest>(list);
            OnPropertyChanged(nameof(LeaveRequests));
            CalculateRemainingLeaveDays();
        }

        private void CalculateRemainingLeaveDays()
        {
            // Giới hạn nghỉ mỗi tháng là 7 ngày
            int limit = 7;
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            int approvedLeaveDays = _leaveRequestService.GetTotalApprovedLeaveDaysInMonth(_employeeId, currentYear, currentMonth);
            RemainingLeaveDays = limit - approvedLeaveDays;
        }

        private void AddLeaveRequest()
        {
            string leaveType = Microsoft.VisualBasic.Interaction.InputBox("Nhập loại nghỉ (Annual, Sick, Unpaid):", "Yêu cầu nghỉ", "");
            if (string.IsNullOrWhiteSpace(leaveType))
                return;
            string startDateStr = Microsoft.VisualBasic.Interaction.InputBox("Nhập ngày bắt đầu (yyyy-MM-dd):", "Yêu cầu nghỉ", "");
            string endDateStr = Microsoft.VisualBasic.Interaction.InputBox("Nhập ngày kết thúc (yyyy-MM-dd):", "Yêu cầu nghỉ", "");
            if (!DateTime.TryParse(startDateStr, out DateTime startDt) || !DateTime.TryParse(endDateStr, out DateTime endDt))
            {
                MessageBox.Show("Ngày không hợp lệ!");
                return;
            }
            var request = new LeaveRequest
            {
                EmployeeId = _employeeId,
                LeaveType = leaveType,
                StartDate = DateOnly.FromDateTime(startDt),
                EndDate = DateOnly.FromDateTime(endDt),
                RequestedAt = DateTime.Now
            };

            string result = _leaveRequestService.AddLeaveRequest(request);
            if (result == "Success")
            {
                MessageBox.Show("Yêu cầu nghỉ đã được gửi.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(result, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            LoadLeaveRequests();
        }

    }
}
