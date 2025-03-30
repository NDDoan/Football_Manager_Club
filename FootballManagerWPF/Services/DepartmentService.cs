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
    public class DepartmentService : IDepartmentService
    {
        private readonly string _connectionString;

        public DepartmentService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }

        public List<Department> GetAllDepartments()
        {
            var departments = new List<Department>();
            string query = "SELECT department_id, name, description, created_at FROM dbo.Department";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["department_id"]),
                            Name = reader["name"].ToString(),
                            Description = reader["description"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["created_at"])
                        });
                    }
                }
            }
            return departments;
        }

        public void AddDepartment(Department department)
        {
            string query = "INSERT INTO dbo.Department (name, description, created_at) VALUES (@Name, @Description, GETDATE())";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", department.Name);
                    cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(department.Description) ? (object)DBNull.Value : department.Description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDepartment(Department department)
        {
            string query = "UPDATE dbo.Department SET name = @Name, description = @Description WHERE department_id = @DepartmentId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", department.Name);
                    cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(department.Description) ? (object)DBNull.Value : department.Description);
                    cmd.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            string query = "DELETE FROM dbo.Department WHERE department_id = @DepartmentId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}