using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using FootballManagerWPF.Models;
using FootballManagerWPF.Services;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;


namespace FootballManagerWPF.Reporting
{
    public class ReportService
    {
        private readonly IEmployeeService _employeeService;

        public ReportService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Xuất báo cáo nhân viên ra file Excel.
        /// </summary>
        /// <param name="filePath">Đường dẫn file Excel sẽ được lưu (ví dụ: "C:\Reports\EmployeeReport.xlsx")</param>
        public void ExportEmployeeReportToExcel(string filePath)
        {
            // Lấy danh sách nhân viên (EmployeeInfo)
            List<EmployeeInfo> employees = _employeeService.GetAllEmployeeInfos();

            // Tạo workbook mới
            using (var workbook = new XLWorkbook())
            {
                // Tạo worksheet với tên "Employee Report"
                var worksheet = workbook.Worksheets.Add("Employee Report");

                // Tạo header cho bảng
                worksheet.Cell("A1").Value = "Employee ID";
                worksheet.Cell("B1").Value = "Full Name";
                worksheet.Cell("C1").Value = "Gender";
                worksheet.Cell("D1").Value = "Address";
                worksheet.Cell("E1").Value = "Phone";
                worksheet.Cell("F1").Value = "Position";
                worksheet.Cell("G1").Value = "Department ID";
                worksheet.Cell("H1").Value = "Start Date";
                worksheet.Cell("I1").Value = "Date of Birth";

                // Định dạng header (có thể tuỳ chỉnh theo ý)
                var headerRange = worksheet.Range("A1:I1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Ghi dữ liệu nhân viên bắt đầu từ dòng 2
                int row = 2;
                foreach (var emp in employees)
                {
                    worksheet.Cell(row, 1).Value = emp.EmployeeId;
                    worksheet.Cell(row, 2).Value = emp.FullName;
                    worksheet.Cell(row, 3).Value = emp.Gender;
                    worksheet.Cell(row, 4).Value = emp.Address;
                    worksheet.Cell(row, 5).Value = emp.Phone;
                    worksheet.Cell(row, 6).Value = emp.Position;
                    worksheet.Cell(row, 7).Value = emp.DepartmentId.HasValue ? emp.DepartmentId.Value.ToString() : "";
                    worksheet.Cell(row, 8).Value = emp.StartDate.ToString();
                    worksheet.Cell(row, 9).Value = emp.DateOfBirth.ToString();
                    row++;
                }

                // Tự động điều chỉnh chiều rộng của các cột
                worksheet.Columns().AdjustToContents();

                // Lưu file Excel tại đường dẫn đã chỉ định
                workbook.SaveAs(filePath);
            }
        }
        /// <summary>
        /// Xuất báo cáo nhân viên ra file PDF với chế độ Landscape và bảng hiển thị dữ liệu.
        /// </summary>
        /// <param name="filePath">Đường dẫn lưu file PDF (ví dụ: "C:\Reports\EmployeeReport.pdf")</param>
        public void ExportEmployeeReportToPdf(string filePath)
        {
            List<EmployeeInfo> employees = _employeeService.GetAllEmployeeInfos();

            // Tạo tài liệu PDF mới
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Employee Report";

            // Tạo trang mới với chế độ Landscape
            PdfPage page = document.AddPage();
            page.Orientation = PageOrientation.Landscape;
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Định nghĩa font sử dụng (đảm bảo PdfSharp.XFontStyle được import đầy đủ)
            XFont headerFont = new XFont("Verdana", 14);
            XFont font = new XFont("Verdana", 10);

            double margin = 40;
            double yPoint = margin;

            // Vẽ tiêu đề báo cáo
            gfx.DrawString("Employee Report", headerFont, XBrushes.Black,
                new XRect(0, yPoint, page.Width, headerFont.Height), XStringFormats.TopCenter);
            yPoint += headerFont.Height + 20;

            // Định nghĩa độ rộng cho các cột (có thể điều chỉnh cho phù hợp)
            double[] colWidths = { 20, 90, 50, 125, 80, 80, 30, 80, 80, 80, 80 };
            // Danh sách header cột
            string[] headers = { "ID", "Full Name", "Gender", "Address", "Phone", "Position", "Dept", "Start Date", "Birth Date", "Username", "Role" };

            double xStart = margin;
            double x = xStart;

            // Vẽ header bảng
            for (int i = 0; i < headers.Length; i++)
            {
                gfx.DrawString(headers[i], font, XBrushes.Black,
                    new XRect(x, yPoint, colWidths[i], font.Height), XStringFormats.Center);
                x += colWidths[i];
            }
            yPoint += font.Height + 5;

            // Vẽ đường phân cách header
            gfx.DrawLine(XPens.Black, xStart, yPoint, page.Width - margin, yPoint);
            yPoint += 5;

            // Vẽ dữ liệu từng dòng
            foreach (var emp in employees)
            {
                x = xStart;
                // Tạo mảng chứa dữ liệu của một dòng
                string[] rowData = {
                    emp.EmployeeId.ToString(),
                    emp.FullName,
                    emp.Gender,
                    emp.Address,
                    emp.Phone,
                    emp.Position,
                    emp.DepartmentId.HasValue ? emp.DepartmentId.Value.ToString() : "",
                    emp.StartDate.ToString(),
                    emp.DateOfBirth.ToString(),
                    emp.Username,
                    emp.Role
                };

                for (int i = 0; i < rowData.Length; i++)
                {
                    gfx.DrawString(rowData[i], font, XBrushes.Black,
                        new XRect(x, yPoint, colWidths[i], font.Height), XStringFormats.CenterLeft);
                    x += colWidths[i];
                }

                yPoint += font.Height + 5;

                // Kiểm tra nếu hết trang, thêm trang mới
                if (yPoint > page.Height - margin)
                {
                    page = document.AddPage();
                    page.Orientation = PageOrientation.Landscape;
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin;
                }
            }

            // Lưu file PDF và đóng tài liệu
            document.Save(filePath);
            document.Close();
        }
    }
}
