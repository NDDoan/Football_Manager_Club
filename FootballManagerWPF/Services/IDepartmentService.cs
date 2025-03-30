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
    public interface IDepartmentService
    {
        List<Department> GetAllDepartments();
        void AddDepartment(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(int departmentId);
    }
}
