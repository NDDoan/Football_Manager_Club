using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerWPF.Models
{
    public class AttendanceRecord
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal OvertimeHours { get; set; }
        public string? LeaveType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
