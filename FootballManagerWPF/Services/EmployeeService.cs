using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Models;
using Microsoft.Data.SqlClient;

namespace FootballManagerWPF.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString;

        public EmployeeService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }

        public EmployeeInfo GetEmployeeInfo(int userId)
        {
            string query = @"
                SELECT e.employee_id, e.full_name, e.gender, e.address, e.phone, e.position, e.avatar, 
                       e.department_id, e.start_date, e.date_of_birth,
                       u.username, u.password, u.role
                FROM dbo.Employee e
                INNER JOIN dbo.Users u ON e.user_id = u.user_id
                WHERE u.user_id = @UserId AND u.role = 'Employee'";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new EmployeeInfo
                            {
                                EmployeeId = Convert.ToInt32(reader["employee_id"]),
                                FullName = reader["full_name"].ToString(),
                                Gender = reader["gender"].ToString(),
                                Address = reader["address"].ToString(),
                                Phone = reader["phone"].ToString(),
                                Position = reader["position"].ToString(),
                                Avatar = reader["avatar"].ToString(),
                                DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                                StartDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["start_date"])),
                                DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(reader["date_of_birth"])),
                                Username = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Role = reader["role"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<EmployeeInfo> GetAllEmployeeInfos()
        {
            var list = new List<EmployeeInfo>();
            string query = @"
                SELECT e.employee_id, e.full_name, e.gender, e.address, e.phone, e.position, e.avatar, 
                       e.department_id, e.start_date, e.date_of_birth,
                       u.username, u.password, u.role
                FROM dbo.Employee e
                INNER JOIN dbo.Users u ON e.user_id = u.user_id
                WHERE u.role = 'Employee'";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new EmployeeInfo
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            FullName = reader["full_name"].ToString(),
                            Gender = reader["gender"].ToString(),
                            Address = reader["address"].ToString(),
                            Phone = reader["phone"].ToString(),
                            Position = reader["position"].ToString(),
                            Avatar = reader["avatar"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            StartDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["start_date"])),
                            DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(reader["date_of_birth"])),
                            Username = reader["username"].ToString(),
                            Password = reader["password"].ToString(),
                            Role = reader["role"].ToString()
                        };
                        list.Add(info);
                    }
                }
            }
            return list;
        }

        public bool UserExists(string username)
        {
            string query = "SELECT COUNT(*) FROM dbo.Users WHERE username = @username";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void AddEmployee(Employee employee, string username, string password, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Thêm mới vào bảng Users
                        string queryUser = @"
                            INSERT INTO dbo.Users (username, password, role, created_at, updated_at)
                            VALUES (@Username, @Password, @Role, GETDATE(), GETDATE());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int newUserId;
                        using (SqlCommand cmdUser = new SqlCommand(queryUser, connection, transaction))
                        {
                            cmdUser.Parameters.AddWithValue("@Username", username);
                            cmdUser.Parameters.AddWithValue("@Password", password);
                            cmdUser.Parameters.AddWithValue("@Role", role);
                            newUserId = (int)cmdUser.ExecuteScalar();
                        }

                        // Thêm mới vào bảng Employee với UserId vừa tạo
                        string queryEmployee = @"
                            INSERT INTO dbo.Employee (user_id, full_name, gender, address, phone, position, avatar, department_id, start_date, date_of_birth, created_at, updated_at)
                            VALUES (@UserId, @FullName, @Gender, @Address, @Phone, @Position, @Avatar, @DepartmentId, @StartDate, @DateOfBirth, GETDATE(), GETDATE());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        int newEmployeeId;
                        using (SqlCommand cmdEmployee = new SqlCommand(queryEmployee, connection, transaction))
                        {
                            cmdEmployee.Parameters.AddWithValue("@UserId", newUserId);
                            cmdEmployee.Parameters.AddWithValue("@FullName", employee.FullName);
                            cmdEmployee.Parameters.AddWithValue("@Gender", employee.Gender);
                            cmdEmployee.Parameters.AddWithValue("@Address", employee.Address);
                            cmdEmployee.Parameters.AddWithValue("@Phone", employee.Phone);
                            cmdEmployee.Parameters.AddWithValue("@Position", employee.Position);
                            cmdEmployee.Parameters.AddWithValue("@Avatar", employee.Avatar);
                            cmdEmployee.Parameters.AddWithValue("@DepartmentId", (object)employee.DepartmentId ?? DBNull.Value);
                            // Chuyển đổi DateOnly sang DateTime
                            cmdEmployee.Parameters.AddWithValue("@StartDate",
                                employee.StartDate.HasValue
                                ? employee.StartDate.Value.ToDateTime(TimeOnly.MinValue)
                                : (object)DBNull.Value);
                            cmdEmployee.Parameters.AddWithValue("@DateOfBirth", new DateTime(employee.DateOfBirth.Year, employee.DateOfBirth.Month, employee.DateOfBirth.Day));
                            newEmployeeId = (int)cmdEmployee.ExecuteScalar();
                        }
                        transaction.Commit();
                        transaction.Commit();

                        // Log hành động thêm nhân viên
                        ActivityLogService logService = new ActivityLogService();
                        logService.LogActivity(new ActivityLog
                        {
                            UserId = newUserId, // Hoặc ID của admin hiện tại (có thể lấy từ session)
                            Activity = "INSERT",
                            TableName = "Employee",
                            RecordId = newEmployeeId,
                            Details = $"Thêm 1 Nhân viên mới: {employee.FullName}"
                        });
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateEmployee(Employee employee, string username, string password, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Cập nhật bảng Users
                        string queryUser = @"
                            UPDATE dbo.Users 
                            SET username = @Username,
                                password = @Password,
                                role = @Role,
                                updated_at = GETDATE()
                            WHERE user_id = (SELECT user_id FROM dbo.Employee WHERE employee_id = @EmployeeId)";
                        using (SqlCommand cmdUser = new SqlCommand(queryUser, connection, transaction))
                        {
                            cmdUser.Parameters.AddWithValue("@Username", username);
                            cmdUser.Parameters.AddWithValue("@Password", password);
                            cmdUser.Parameters.AddWithValue("@Role", role);
                            cmdUser.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                            cmdUser.ExecuteNonQuery();
                        }

                        // Cập nhật bảng Employee
                        string queryEmployee = @"
                            UPDATE dbo.Employee 
                            SET full_name = @FullName,
                                gender = @Gender,
                                address = @Address,
                                phone = @Phone,
                                position = @Position,
                                avatar = @Avatar,
                                department_id = @DepartmentId,
                                start_date = @StartDate,
                                date_of_birth = @DateOfBirth,
                                updated_at = GETDATE()
                            WHERE employee_id = @EmployeeId";
                        using (SqlCommand cmdEmployee = new SqlCommand(queryEmployee, connection, transaction))
                        {
                            cmdEmployee.Parameters.AddWithValue("@FullName", employee.FullName);
                            cmdEmployee.Parameters.AddWithValue("@Gender", employee.Gender);
                            cmdEmployee.Parameters.AddWithValue("@Address", employee.Address);
                            cmdEmployee.Parameters.AddWithValue("@Phone", employee.Phone);
                            cmdEmployee.Parameters.AddWithValue("@Position", employee.Position);
                            cmdEmployee.Parameters.AddWithValue("@Avatar", employee.Avatar);
                            cmdEmployee.Parameters.AddWithValue("@DepartmentId", (object)employee.DepartmentId ?? DBNull.Value);
                            cmdEmployee.Parameters.AddWithValue("@StartDate",
                                employee.StartDate.HasValue
                                ? employee.StartDate.Value.ToDateTime(TimeOnly.MinValue)
                                : (object)DBNull.Value);
                            cmdEmployee.Parameters.AddWithValue("@DateOfBirth", new DateTime(employee.DateOfBirth.Year, employee.DateOfBirth.Month, employee.DateOfBirth.Day));
                            cmdEmployee.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                            cmdEmployee.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        // Log hành động sửa nhân viên
                        ActivityLogService logService = new ActivityLogService();
                        logService.LogActivity(new ActivityLog
                        {
                            Activity = "UPDATE",
                            TableName = "Employee",
                            Details = $"Cập nhật thông tin 1 nhân viên: {employee.FullName}"
                        });
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM dbo.Employee WHERE employee_id = @EmployeeId";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();
                }
                ActivityLogService logService = new ActivityLogService();
                logService.LogActivity(new ActivityLog
                {
                    Activity = "DELETE",
                    TableName = "Employee",
                    Details = $"Gỡ bỏ thông tin nhân viên: {employeeId}"
                });

            }
        }
        public void UpdateEmployeeByUsername(EmployeeInfo backup)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryUser = @"
                    UPDATE dbo.Users 
                    SET password = @Password, role = @Role, updated_at = GETDATE()
                    WHERE username = @Username";
                        using (var cmdUser = new SqlCommand(queryUser, connection, transaction))
                        {
                            cmdUser.Parameters.AddWithValue("@Password", backup.Password);
                            cmdUser.Parameters.AddWithValue("@Role", backup.Role);
                            cmdUser.Parameters.AddWithValue("@Username", backup.Username);
                            cmdUser.ExecuteNonQuery();
                        }

                        string queryEmployee = @"
                    UPDATE dbo.Employee
                    SET full_name = @FullName,
                        gender = @Gender,
                        address = @Address,
                        phone = @Phone,
                        position = @Position,
                        avatar = @Avatar,
                        department_id = @DepartmentId,
                        start_date = @StartDate,
                        date_of_birth = @DateOfBirth,
                        updated_at = GETDATE()
                    WHERE user_id = (SELECT user_id FROM dbo.Users WHERE username = @Username)";
                        using (var cmdEmployee = new SqlCommand(queryEmployee, connection, transaction))
                        {
                            cmdEmployee.Parameters.AddWithValue("@FullName", backup.FullName);
                            cmdEmployee.Parameters.AddWithValue("@Gender", backup.Gender);
                            cmdEmployee.Parameters.AddWithValue("@Address", backup.Address);
                            cmdEmployee.Parameters.AddWithValue("@Phone", backup.Phone);
                            cmdEmployee.Parameters.AddWithValue("@Position", backup.Position);
                            cmdEmployee.Parameters.AddWithValue("@Avatar", backup.Avatar);
                            cmdEmployee.Parameters.AddWithValue("@DepartmentId", (object)backup.DepartmentId ?? DBNull.Value);
                            cmdEmployee.Parameters.AddWithValue("@StartDate", new DateTime(backup.StartDate.Year, backup.StartDate.Month, backup.StartDate.Day));
                            cmdEmployee.Parameters.AddWithValue("@DateOfBirth", new DateTime(backup.DateOfBirth.Year, backup.DateOfBirth.Month, backup.DateOfBirth.Day));
                            cmdEmployee.Parameters.AddWithValue("@Username", backup.Username);
                            cmdEmployee.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            // Sau khi giao dịch đã hoàn thành, ghi lại log trong một kết nối riêng biệt
            ActivityLogService logService = new ActivityLogService();
            logService.LogActivity(new ActivityLog
            {
                Activity = "UPDATE",
                TableName = "Employee",
                Details = $"Cập nhật thông tin nhân viên: {backup.FullName}"
                // Có thể bổ sung UserId nếu có thông tin về người thực hiện
            });
        }

        public List<EmployeeBackup> GetAllEmployeeBackups()
        {
            var list = new List<EmployeeBackup>();
            string query = @"
        SELECT e.employee_id, e.full_name, e.gender, e.address, e.phone, e.position, e.avatar, 
               e.department_id, e.start_date, e.date_of_birth,
               u.username, u.password AS UserPassword, u.role
        FROM dbo.Employee e
        INNER JOIN dbo.Users u ON e.user_id = u.user_id
        WHERE u.role = 'Employee'";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var backup = new FootballManagerWPF.Models.EmployeeBackup
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            FullName = reader["full_name"].ToString(),
                            Gender = reader["gender"].ToString(),
                            Address = reader["address"].ToString(),
                            Phone = reader["phone"].ToString(),
                            Position = reader["position"].ToString(),
                            Avatar = reader["avatar"].ToString(),
                            DepartmentId = reader["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["department_id"]),
                            StartDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["start_date"])),
                            DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(reader["date_of_birth"])),
                            Username = reader["username"].ToString(),
                            Password = reader["UserPassword"].ToString(),  // Sử dụng alias "UserPassword"
                            Role = reader["role"].ToString()
                        };
                        list.Add(backup);
                    }
                }
            }
            return list;
        }

    }
}