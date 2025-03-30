using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Models;

namespace FootballManagerWPF.Services
{
    public interface IPayrollService
    {
        List<Payroll> GetPayrollForEmployeeManagement(int employeeId);
        List<PayrollRecord> GetPayrollForEmployeeDisplay(int employeeId);
        public List<PayrollRecord> GetAllPayrollRecords();
    }
}
