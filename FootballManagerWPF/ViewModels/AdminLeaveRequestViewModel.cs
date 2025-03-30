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
    public class AdminLeaveRequestViewModel : BaseViewModel
    {
        private readonly ILeaveRequestService _leaveRequestService;
        // Giả sử AdminId được cung cấp (có thể lấy từ session)
        private int _adminId = 1;

        public ObservableCollection<LeaveRequest> LeaveRequests { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        private LeaveRequest _selectedLeaveRequest;
        public LeaveRequest SelectedLeaveRequest
        {
            get => _selectedLeaveRequest;
            set
            {
                _selectedLeaveRequest = value;
                OnPropertyChanged();
                ((RelayCommand)ApproveCommand).RaiseCanExecuteChanged();
                ((RelayCommand)RejectCommand).RaiseCanExecuteChanged();
            }
        }

        public AdminLeaveRequestViewModel() : this(new LeaveRequestService())
        {
        }

        public AdminLeaveRequestViewModel(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
            LoadLeaveRequests();
            RefreshCommand = new RelayCommand(o => LoadLeaveRequests());
            ApproveCommand = new RelayCommand(o => ApproveRequest(), o => SelectedLeaveRequest != null && SelectedLeaveRequest.Status == "Pending");
            RejectCommand = new RelayCommand(o => RejectRequest(), o => SelectedLeaveRequest != null && SelectedLeaveRequest.Status == "Pending");
        }

        private void LoadLeaveRequests()
        {
            var list = _leaveRequestService.GetAllLeaveRequests();
            LeaveRequests = new ObservableCollection<LeaveRequest>(list);
            OnPropertyChanged(nameof(LeaveRequests));
        }

        private void ApproveRequest()
        {
            if (SelectedLeaveRequest == null)
                return;
            try
            {
                _leaveRequestService.ApproveLeaveRequest(SelectedLeaveRequest.LeaveId, _adminId);
                MessageBox.Show("Yêu cầu nghỉ đã được phê duyệt.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadLeaveRequests();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi phê duyệt: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectRequest()
        {
            if (SelectedLeaveRequest == null)
                return;
            try
            {
                _leaveRequestService.RejectLeaveRequest(SelectedLeaveRequest.LeaveId, _adminId);
                MessageBox.Show("Yêu cầu nghỉ đã bị từ chối.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadLeaveRequests();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi từ chối: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}