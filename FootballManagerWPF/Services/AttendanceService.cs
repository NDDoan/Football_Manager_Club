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
    public class AttendanceService : IAttendanceService
    {
        private readonly string _connectionString;

        public AttendanceService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }
        public void InsertAttendance(Attendance attendance)
        {
            string query = @"INSERT INTO dbo.Attendance (employee_id, attendance_date, check_in, check_out, overtime_hours, leave_type, created_at)
                             VALUES (@EmployeeId, @AttendanceDate, @CheckIn, @CheckOut, @OvertimeHours, @LeaveType, GETDATE())";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
                    cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
                    cmd.Parameters.AddWithValue("@CheckIn", (object)attendance.CheckIn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CheckOut", (object)attendance.CheckOut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@OvertimeHours", attendance.OvertimeHours);
                    cmd.Parameters.AddWithValue("@LeaveType", attendance.LeaveType);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool HasClockedInToday(int employeeId)
        {
            string query = "SELECT COUNT(*) FROM dbo.Attendance WHERE employee_id = @EmployeeId AND attendance_date = CAST(GETDATE() AS DATE)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void ClockIn(int employeeId)
        {
            // Giả sử chỉ thực hiện check-in (chưa xử lý check-out)
            string query = @"
                INSERT INTO dbo.Attendance (employee_id, attendance_date, check_in, created_at)
                VALUES (@EmployeeId, CAST(GETDATE() AS DATE), GETDATE(), GETDATE())";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Phương thức ClockOut cập nhật thời điểm check_out và tính toán overtime
        public void ClockOut(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Tìm bản ghi chấm công của ngày hôm nay chưa có check_out
                string querySelect = @"
                    SELECT TOP 1 attendance_id, check_in 
                    FROM dbo.Attendance 
                    WHERE employee_id = @EmployeeId 
                      AND attendance_date = CAST(GETDATE() AS DATE)
                      AND check_out IS NULL
                    ORDER BY created_at DESC";
                int attendanceId = 0;
                DateTime checkInTime = DateTime.MinValue;
                using (var cmdSelect = new SqlCommand(querySelect, connection))
                {
                    cmdSelect.Parameters.AddWithValue("@EmployeeId", employeeId);
                    using (var reader = cmdSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            attendanceId = Convert.ToInt32(reader["attendance_id"]);
                            checkInTime = Convert.ToDateTime(reader["check_in"]);
                        }
                        else
                        {
                            throw new Exception("Không tìm thấy bản ghi chấm công chưa Check Out cho ngày hôm nay.");
                        }
                    }
                }
                DateTime checkOutTime = DateTime.Now;
                // Tính số giờ làm việc
                TimeSpan workDuration = checkOutTime - checkInTime;
                double standardHours = 8.0;
                decimal overtime = workDuration.TotalHours > standardHours ? (decimal)(workDuration.TotalHours - standardHours) : 0;

                string queryUpdate = @"
                    UPDATE dbo.Attendance 
                    SET check_out = @CheckOut, overtime_hours = @OvertimeHours
                    WHERE attendance_id = @AttendanceId";
                using (var cmdUpdate = new SqlCommand(queryUpdate, connection))
                {
                    cmdUpdate.Parameters.AddWithValue("@CheckOut", checkOutTime);
                    cmdUpdate.Parameters.AddWithValue("@OvertimeHours", overtime);
                    cmdUpdate.Parameters.AddWithValue("@AttendanceId", attendanceId);
                    cmdUpdate.ExecuteNonQuery();
                }
            }
        }

        public List<AttendanceRecord> GetAllAttendanceRecords()
        {
            var records = new List<AttendanceRecord>();
            string query = @"
                SELECT attendance_id, employee_id, attendance_date, check_in, check_out, overtime_hours, leave_type, created_at
                FROM dbo.Attendance
                ORDER BY attendance_date DESC";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(new AttendanceRecord
                        {
                            AttendanceId = Convert.ToInt32(reader["attendance_id"]),
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            AttendanceDate = Convert.ToDateTime(reader["attendance_date"]).Date,
                            CheckInTime = Convert.ToDateTime(reader["check_in"]),
                            CheckOutTime = reader["check_out"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["check_out"]),
                            OvertimeHours = reader["overtime_hours"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["overtime_hours"]),
                            LeaveType = reader["leave_type"]?.ToString(),
                            CreatedAt = Convert.ToDateTime(reader["created_at"])
                        });
                    }
                }
            }
            return records;
        }

        public List<AttendanceRecord> GetAttendanceForEmployee(int employeeId)
        {
            var records = new List<AttendanceRecord>();
            string query = @"
                SELECT attendance_id, employee_id, attendance_date, check_in, check_out, overtime_hours, leave_type, created_at
                FROM dbo.Attendance
                WHERE employee_id = @EmployeeId
                ORDER BY attendance_date DESC";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(new AttendanceRecord
                            {
                                AttendanceId = Convert.ToInt32(reader["attendance_id"]),
                                EmployeeId = Convert.ToInt32(reader["employee_id"]),
                                AttendanceDate = Convert.ToDateTime(reader["attendance_date"]).Date,
                                CheckInTime = Convert.ToDateTime(reader["check_in"]),
                                CheckOutTime = reader["check_out"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["check_out"]),
                                OvertimeHours = reader["overtime_hours"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["overtime_hours"]),
                                LeaveType = reader["leave_type"]?.ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            });
                        }
                    }
                }
            }
            return records;
        }
    }
}
