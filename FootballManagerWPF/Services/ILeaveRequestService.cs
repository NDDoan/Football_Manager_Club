using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;

namespace FootballManagerWPF.Services
{
    public interface ILeaveRequestService
    {
        public List<LeaveRequest> GetAllLeaveRequestsByEmployee(int employeeId);
        public int GetTotalApprovedLeaveDaysInMonth(int employeeId, int year, int month);
        public string AddLeaveRequest(LeaveRequest leaveRequest);

        // Dành cho Admin:
        List<LeaveRequest> GetAllLeaveRequests();
        void ApproveLeaveRequest(int leaveId, int approverId);
        void RejectLeaveRequest(int leaveId, int approverId);
    }
}
