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
    /// Interaction logic for EmployeeDashboardView.xaml
    /// </summary>
    public partial class EmployeeDashboardView : Window
    {
        public EmployeeDashboardView()
        {
            InitializeComponent();
            DataContext = new EmployeeDashboardViewModel();
        }
        private void EmployeeMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement media = sender as MediaElement;
            media.Position = TimeSpan.Zero;
            media.Play();
        }
    }
}
