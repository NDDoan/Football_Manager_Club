using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerWPF.Models
{
    public class EmployeeInfo
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }    // Từ bảng Users
        public string Password { get; set; }    // Từ bảng Users (plain text)
        public string Role { get; set; }          // Từ bảng Users
        public string FullName { get; set; }      // Từ bảng Employee
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string Avatar { get; set; }
        public int? DepartmentId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public static EmployeeInfo FromBackup(EmployeeBackup backup)
        {
            return new EmployeeInfo
            {
                EmployeeId = backup.EmployeeId,
                Username = backup.Username,
                Password = backup.Password,
                Role = backup.Role,
                FullName = backup.FullName,
                Gender = backup.Gender,
                Address = backup.Address,
                Phone = backup.Phone,
                Position = backup.Position,
                Avatar = backup.Avatar,
                DepartmentId = backup.DepartmentId,
                StartDate = backup.StartDate,
                DateOfBirth = backup.DateOfBirth
            };
        }
    }
}