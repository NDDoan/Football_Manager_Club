using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Models;
using FootballManagerWPF.Services;
using System.Windows.Input;

namespace FootballManagerWPF.ViewModels
{
    public class EmployeePayrollViewModel : BaseViewModel
    {
        private readonly IPayrollService _payrollService;
        private int _employeeId;

        public ObservableCollection<PayrollRecord> PayrollRecords { get; set; }
        public ICommand RefreshCommand { get; }

        public EmployeePayrollViewModel(IPayrollService payrollService, int employeeId)
        {
            _payrollService = payrollService;
            _employeeId = employeeId;
            LoadPayrollRecords();
            RefreshCommand = new RelayCommand(o => LoadPayrollRecords());
        }

        public EmployeePayrollViewModel() : this(new PayrollService(), 0) { }

        private void LoadPayrollRecords()
        {
            List<PayrollRecord> records = _payrollService.GetPayrollForEmployeeDisplay(_employeeId);
            PayrollRecords = new ObservableCollection<PayrollRecord>(records);
            OnPropertyChanged(nameof(PayrollRecords));
        }
    }
}