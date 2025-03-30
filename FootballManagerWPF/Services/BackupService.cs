using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;
using FootballManagerWPF.Helpers;
using Newtonsoft.Json;
using System.IO;

namespace FootballManagerWPF.Services
{
    public class BackupService : IBackupService
    {
        private readonly EmployeeService _employeeService;

        public BackupService()
        {
            _employeeService = new EmployeeService();
        }

        public void BackupEmployees(string filePath)
        {
            // Lấy danh sách backup nhân viên (bao gồm thông tin đăng nhập)
            List<EmployeeBackup> backups = _employeeService.GetAllEmployeeBackups();
            string json = JsonConvert.SerializeObject(backups, Formatting.Indented);
            File.WriteAllText(filePath, json);

            // Log sao lưu
            ActivityLogService logService = new ActivityLogService();
            // Giả sử admin đang thực hiện sao lưu, có thể lấy userId từ session
            int adminUserId = 1; // ví dụ
            logService.LogActivity(new ActivityLog
            {
                UserId = adminUserId,
                Activity = "BACKUP",
                TableName = "Employee, Users",
                RecordId = null,
                Details = $"Backup employees to file: {filePath}"
            });
        }

        public void RestoreEmployees(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<EmployeeBackup> backups = JsonConvert.DeserializeObject<List<EmployeeBackup>>(json);
            if (backups != null)
            {
                foreach (var backup in backups)
                {
                    if (_employeeService.UserExists(backup.Username))
                    {
                        // Nếu username đã tồn tại, chuyển đổi EmployeeBackup sang EmployeeInfo và cập nhật
                        EmployeeInfo info = EmployeeInfo.FromBackup(backup);
                        _employeeService.UpdateEmployeeByUsername(info);

                        // Log phục hồi cập nhật
                        ActivityLogService logService = new ActivityLogService();
                        logService.LogActivity(new ActivityLog
                        {
                            UserId = 1, // admin ID từ session
                            Activity = "RESTORE",
                            TableName = "Employee, Users",
                            RecordId = info.EmployeeId,
                            Details = $"Restored employee (updated) with username: {backup.Username}"
                        });
                    }
                    else
                    {
                        // Nếu không tồn tại, thêm mới nhân viên
                        Employee newEmployee = new Employee
                        {
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
                        _employeeService.AddEmployee(newEmployee, backup.Username, backup.Password, backup.Role);

                        // Log phục hồi thêm mới
                        ActivityLogService logService = new ActivityLogService();
                        logService.LogActivity(new ActivityLog
                        {
                            UserId = 1, // admin ID từ session
                            Activity = "RESTORE",
                            TableName = "Employee, Users",
                            RecordId = newEmployee.EmployeeId,
                            Details = $"Restored employee (inserted) with username: {backup.Username}"
                        });
                    }
                }
            }
        }
    }
}