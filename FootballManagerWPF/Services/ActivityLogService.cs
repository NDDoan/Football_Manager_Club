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
    public class ActivityLogService : IActivityLogService
    {
        private readonly string _connectionString;

        public ActivityLogService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }
        // Ghi lại một log hoạt động.
        public void LogActivity(ActivityLog log)
        {
            // Kiểm tra xem user có tồn tại không; nếu không, ta có thể chuyển sang dùng user id mặc định (ví dụ: 1) hoặc bỏ qua ghi log.
            if (!DoesUserExist(log.UserId))
            {
                // Ở đây, ta sử dụng user id mặc định (admin) để ghi log.
                log.UserId = 1;
                // Hoặc: return; // để bỏ qua ghi log nếu user không tồn tại.
            }

            string query = @"
        INSERT INTO dbo.ActivityLog (user_id, activity, table_name, record_id, details, activity_timestamp)
        VALUES (@UserId, @Activity, @TableName, @RecordId, @Details, GETDATE())";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", log.UserId);
                    cmd.Parameters.AddWithValue("@Activity", log.Activity);
                    cmd.Parameters.AddWithValue("@TableName", (object)log.TableName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RecordId", (object)log.RecordId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Details", (object)log.Details ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Helper method để kiểm tra sự tồn tại của một user
        private bool DoesUserExist(int userId)
        {
            string query = "SELECT COUNT(*) FROM dbo.Users WHERE user_id = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        // Lấy danh sách tất cả các log hoạt động.
        public List<ActivityLog> GetAllActivityLogs()
        {
            var logs = new List<ActivityLog>();
            string query = @"SELECT log_id, user_id, activity, table_name, record_id, details, activity_timestamp
                             FROM dbo.ActivityLog";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(new ActivityLog
                        {
                            LogId = Convert.ToInt32(reader["log_id"]),
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Activity = reader["activity"].ToString(),
                            TableName = reader["table_name"] == DBNull.Value ? null : reader["table_name"].ToString(),
                            RecordId = reader["record_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["record_id"]),
                            Details = reader["details"] == DBNull.Value ? null : reader["details"].ToString(),
                            ActivityTimestamp = Convert.ToDateTime(reader["activity_timestamp"])
                        });
                    }
                }
            }
            return logs;
        }
    }
}
