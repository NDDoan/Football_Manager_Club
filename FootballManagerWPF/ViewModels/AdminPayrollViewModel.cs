using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Services;
using FootballManagerWPF.Models;
using System.Windows.Input;

namespace FootballManagerWPF.ViewModels
{
    public class AdminPayrollViewModel : BaseViewModel
    {
        private readonly IPayrollService _payrollService;

        public ObservableCollection<PayrollRecord> PayrollRecords { get; set; }
        public ICommand RefreshCommand { get; }

        public AdminPayrollViewModel() : this(new PayrollService())
        {
        }

        public AdminPayrollViewModel(IPayrollService payrollService)
        {
            _payrollService = payrollService;
            LoadPayrollRecords();
            RefreshCommand = new RelayCommand(o => LoadPayrollRecords());
        }

        private void LoadPayrollRecords()
        {
            var records = _payrollService.GetAllPayrollRecords();
            PayrollRecords = new ObservableCollection<PayrollRecord>(records);
            OnPropertyChanged(nameof(PayrollRecords));
        }
    }
}
