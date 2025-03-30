using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerWPF.Models
{
    public class SessionData
    {
        // Ví dụ: danh sách ID nhân viên được lọc trong EmployeeManagementView
        public List<int> FilteredEmployeeIds { get; set; } = new List<int>();

        // Ví dụ: danh sách tên các tab đang mở (có thể là các view key)
        public List<string> OpenTabs { get; set; } = new List<string>();
    }
}
