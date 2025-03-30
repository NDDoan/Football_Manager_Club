using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;

namespace FootballManagerWPF.Services
{
    public interface IEmployeeService
    {
        EmployeeInfo GetEmployeeInfo(int userId);
        List<EmployeeInfo> GetAllEmployeeInfos();
        void AddEmployee(Employee employee, string username, string password, string role);
        void UpdateEmployee(Employee employee, string username, string password, string role);
        void DeleteEmployee(int employeeId);
        bool UserExists(string username);
        void UpdateEmployeeByUsername(EmployeeInfo backup);
    }
}
