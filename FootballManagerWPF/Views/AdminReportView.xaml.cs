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
using FootballManagerWPF.ViewModels;

namespace FootballManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for AdminReportView.xaml
    /// </summary>
    public partial class AdminReportView : Window
    {
        public AdminReportView()
        {
            InitializeComponent();
            DataContext = new AdminReportViewModel();
        }
    }
}
