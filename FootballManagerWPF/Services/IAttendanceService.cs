using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;

namespace FootballManagerWPF.Services
{
    public interface IAttendanceService
    {
        public List<AttendanceRecord> GetAttendanceForEmployee(int employeeId);
        void InsertAttendance(Attendance attendance);
        public bool HasClockedInToday(int employeeId);
        public void ClockIn(int employeeId);
        public void ClockOut(int employeeId);
        public List<AttendanceRecord> GetAllAttendanceRecords();
    }
}
