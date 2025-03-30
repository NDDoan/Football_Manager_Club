using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Reporting;
using FootballManagerWPF.Services;
using System.Windows.Input;
using System.Windows;

namespace FootballManagerWPF.ViewModels
{
    public class AdminReportViewModel : BaseViewModel
    {
        private readonly ReportService _reportService;

        public ICommand ExportToExcelCommand { get; }
        public ICommand ExportToPdfCommand { get; }

        public AdminReportViewModel() : this(new ReportService(new EmployeeService()))
        {
        }

        public AdminReportViewModel(ReportService reportService)
        {
            _reportService = reportService;
            ExportToExcelCommand = new RelayCommand(ExportReport);
            ExportToPdfCommand = new RelayCommand(ExportReportToPdf);
        }
        // Xuất báo cáo ra file Excel
        private void ExportReport(object parameter)
        {
            // Sử dụng SaveFileDialog để chọn nơi lưu file Excel
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "EmployeeReport",
                DefaultExt = ".xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    _reportService.ExportEmployeeReportToExcel(dlg.FileName);
                    MessageBox.Show("Báo cáo đã được xuất thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Xuất báo cáo ra file PDF
        private void ExportReportToPdf(object parameter)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "EmployeeReport",
                DefaultExt = ".pdf",
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    _reportService.ExportEmployeeReportToPdf(dlg.FileName);
                    MessageBox.Show("Báo cáo PDF đã được xuất thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất báo cáo PDF: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
