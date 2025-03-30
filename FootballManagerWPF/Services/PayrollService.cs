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
    public class PayrollService : IPayrollService
    {
        private readonly string _connectionString;

        public PayrollService()
        {
            _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }
        public List<Payroll> GetPayrollForEmployeeManagement(int employeeId)
        {
            var payrolls = new List<Payroll>();
            string query = @"SELECT payroll_id, employee_id, base_salary, allowance, bonus, penalty, payroll_date, created_at 
                             FROM dbo.Payroll WHERE employee_id = @EmployeeId";
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
                            payrolls.Add(new Payroll
                            {
                                PayrollId = Convert.ToInt32(reader["payroll_id"]),
                                EmployeeId = Convert.ToInt32(reader["employee_id"]),
                                BaseSalary = Convert.ToDecimal(reader["base_salary"]),
                                Allowance = Convert.ToDecimal(reader["allowance"]),
                                Bonus = Convert.ToDecimal(reader["bonus"]),
                                Penalty = Convert.ToDecimal(reader["penalty"]),
                                PayrollDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["payroll_date"])),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                                // total_income được tính sẵn (PERSISTED) nên không cần truy vấn riêng
                            });
                        }
                    }
                }
            }
            return payrolls;
        }

        public List<PayrollRecord> GetPayrollForEmployeeDisplay(int employeeId)
        {
            var payrollRecords = new List<PayrollRecord>();
            string query = @"SELECT payroll_id, employee_id, base_salary, allowance, bonus, penalty, payroll_date, created_at 
                     FROM dbo.Payroll WHERE employee_id = @EmployeeId";
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
                            payrollRecords.Add(new PayrollRecord
                            {
                                PayrollId = Convert.ToInt32(reader["payroll_id"]),
                                EmployeeId = Convert.ToInt32(reader["employee_id"]),
                                BaseSalary = Convert.ToDecimal(reader["base_salary"]),
                                Allowance = Convert.ToDecimal(reader["allowance"]),
                                Bonus = Convert.ToDecimal(reader["bonus"]),
                                Penalty = Convert.ToDecimal(reader["penalty"]),
                                // Giả sử TotalIncome được tính từ SQL hoặc tính toán trong code
                                TotalIncome = 0, // Nếu chưa có, bạn có thể tính sau
                                PayrollDate = Convert.ToDateTime(reader["payroll_date"]).Date,
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            });
                        }
                    }
                }
            }
            return payrollRecords;
        }

        public List<PayrollRecord> GetAllPayrollRecords()
        {
            var list = new List<PayrollRecord>();
            string query = @"
                SELECT payroll_id, employee_id, base_salary, allowance, bonus, penalty, total_income, payroll_date, created_at
                FROM dbo.Payroll
                ORDER BY payroll_date DESC";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PayrollRecord
                        {
                            PayrollId = Convert.ToInt32(reader["payroll_id"]),
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            BaseSalary = Convert.ToDecimal(reader["base_salary"]),
                            Allowance = Convert.ToDecimal(reader["allowance"]),
                            Bonus = Convert.ToDecimal(reader["bonus"]),
                            Penalty = Convert.ToDecimal(reader["penalty"]),
                            TotalIncome = Convert.ToDecimal(reader["total_income"]),
                            PayrollDate = Convert.ToDateTime(reader["payroll_date"]).Date,
                            CreatedAt = Convert.ToDateTime(reader["created_at"])
                        });
                    }
                }
            }
            return list;
        }

    }
}
