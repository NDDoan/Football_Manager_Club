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
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly string _connectionString;
        public LeaveRequestService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }

        public List<LeaveRequest> GetAllLeaveRequestsByEmployee(int employeeId)
        {
            var list = new List<LeaveRequest>();
            string query = @"
                SELECT leave_id, employee_id, leave_type, start_date, end_date, total_days, status, requested_at, approved_by, approved_at
                FROM dbo.LeaveRequest
                WHERE employee_id = @EmployeeId
                ORDER BY requested_at DESC";
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
                            list.Add(new LeaveRequest
                            {
                                LeaveId = Convert.ToInt32(reader["leave_id"]),
                                EmployeeId = Convert.ToInt32(reader["employee_id"]),
                                LeaveType = reader["leave_type"].ToString(),
                                StartDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["start_date"])),
                                EndDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["end_date"])),
                                TotalDays = reader["total_days"] == DBNull.Value ? null : Convert.ToInt32(reader["total_days"]),
                                Status = reader["status"].ToString(),
                                RequestedAt = Convert.ToDateTime(reader["requested_at"]),
                                ApprovedBy = reader["approved_by"] == DBNull.Value ? null : Convert.ToInt32(reader["approved_by"]),
                                ApprovedAt = reader["approved_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["approved_at"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public int GetTotalApprovedLeaveDaysInMonth(int employeeId, int year, int month)
        {
            string query = @"
                SELECT ISNULL(SUM(total_days), 0)
                FROM dbo.LeaveRequest
                WHERE employee_id = @EmployeeId 
                  AND status = 'Approved'
                  AND YEAR(start_date) = @Year AND MONTH(start_date) = @Month";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public string AddLeaveRequest(LeaveRequest leaveRequest)
        {
            // Tính số ngày nghỉ yêu cầu
            int requestedDays = (new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month, leaveRequest.EndDate.Day)
                               - new DateTime(leaveRequest.StartDate.Year, leaveRequest.StartDate.Month, leaveRequest.StartDate.Day)).Days + 1;

            // Giới hạn nghỉ mỗi tháng là 7 ngày
            int limit = 7;
            int currentYear = leaveRequest.RequestedAt.Year;
            int currentMonth = leaveRequest.RequestedAt.Month;
            int approvedLeaveDays = GetTotalApprovedLeaveDaysInMonth(leaveRequest.EmployeeId, currentYear, currentMonth);

            // Kiểm tra nếu vượt quá hạn mức
            if (approvedLeaveDays + requestedDays > limit)
            {
                // Trả về thông báo cho người dùng thay vì ném ngoại lệ
                return "Yêu cầu nghỉ vượt quá hạn mức cho phép trong tháng. Vui lòng kiểm tra lại số ngày nghỉ.";
            }
            else
            {
                leaveRequest.Status = "Pending";
            }

            // Cột total_days là computed, không chèn vào query
            string query = @"
        INSERT INTO dbo.LeaveRequest (employee_id, leave_type, start_date, end_date, status, requested_at)
        VALUES (@EmployeeId, @LeaveType, @StartDate, @EndDate, @Status, GETDATE())";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", leaveRequest.EmployeeId);
                    cmd.Parameters.AddWithValue("@LeaveType", leaveRequest.LeaveType);
                    cmd.Parameters.AddWithValue("@StartDate", new DateTime(leaveRequest.StartDate.Year, leaveRequest.StartDate.Month, leaveRequest.StartDate.Day));
                    cmd.Parameters.AddWithValue("@EndDate", new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month, leaveRequest.EndDate.Day));
                    cmd.Parameters.AddWithValue("@Status", leaveRequest.Status);
                    cmd.ExecuteNonQuery();
                }
            }
            return "Success";
        }




        // Lấy toàn bộ yêu cầu nghỉ (cho Admin)
        public List<LeaveRequest> GetAllLeaveRequests()
        {
            var list = new List<LeaveRequest>();
            string query = @"
                SELECT leave_id, employee_id, leave_type, start_date, end_date, total_days, status, requested_at, approved_by, approved_at
                FROM dbo.LeaveRequest
                ORDER BY requested_at DESC";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new LeaveRequest
                        {
                            LeaveId = Convert.ToInt32(reader["leave_id"]),
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            LeaveType = reader["leave_type"].ToString(),
                            StartDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["start_date"])),
                            EndDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["end_date"])),
                            TotalDays = reader["total_days"] == DBNull.Value ? null : Convert.ToInt32(reader["total_days"]),
                            Status = reader["status"].ToString(),
                            RequestedAt = Convert.ToDateTime(reader["requested_at"]),
                            ApprovedBy = reader["approved_by"] == DBNull.Value ? null : Convert.ToInt32(reader["approved_by"]),
                            ApprovedAt = reader["approved_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["approved_at"])
                        });
                    }
                }
            }
            return list;
        }

        public void ApproveLeaveRequest(int leaveId, int approverId)
        {
            string query = @"
                UPDATE dbo.LeaveRequest 
                SET status = 'Approved', approved_by = @ApproverId, approved_at = GETDATE()
                WHERE leave_id = @LeaveId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ApproverId", approverId);
                    cmd.Parameters.AddWithValue("@LeaveId", leaveId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RejectLeaveRequest(int leaveId, int approverId)
        {
            string query = @"
                UPDATE dbo.LeaveRequest 
                SET status = 'Rejected', approved_by = @ApproverId, approved_at = GETDATE()
                WHERE leave_id = @LeaveId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ApproverId", approverId);
                    cmd.Parameters.AddWithValue("@LeaveId", leaveId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}