using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FootballManagerWPF.Models;
using FootballManagerWPF.Services;
using FootballManagerWPF.ViewModels;

namespace FootballManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for EmployeePayrollView.xaml
    /// </summary>
    public partial class EmployeePayrollView : Window
    {
        public EmployeePayrollView(int employeeId)
        {
            InitializeComponent();
            DataContext = new EmployeePayrollViewModel(new PayrollService(), employeeId);
        }
    }
}
