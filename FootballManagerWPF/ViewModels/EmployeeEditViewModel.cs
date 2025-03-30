using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManagerWPF.Helpers;
using FootballManagerWPF.Models;
using System.Windows.Input;
using System.Windows;
using FootballManagerWPF.Services;
using System.Collections.ObjectModel;

namespace FootballManagerWPF.ViewModels
{
    public class EmployeeEditViewModel : BaseViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        // Employee chứa thông tin từ bảng Employee
        public Employee Employee { get; set; }

        // Thông tin đăng nhập (Users)
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _role;
        public string Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(); }
        }

        // Danh sách lựa chọn
        public ObservableCollection<string> GenderOptions { get; set; } =
            new ObservableCollection<string> { "Male", "Female", "Other" };
        public ObservableCollection<string> RoleOptions { get; set; } =
            new ObservableCollection<string> { "Admin", "Employee" };

        // Phòng ban
        public ObservableCollection<Department> Departments { get; set; }
        private int _selectedDepartmentId;
        public int SelectedDepartmentId
        {
            get => _selectedDepartmentId;
            set { _selectedDepartmentId = value; OnPropertyChanged(); }
        }

        // Thuộc tính trung gian cho binding DatePicker (WPF dùng DateTime)
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set { _dateOfBirth = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        // Constructor cho trường hợp thêm mới
        public EmployeeEditViewModel() : this(new EmployeeService(), new DepartmentService())
        {
            Employee = new Employee();
            Employee.FullName = "";
            Employee.Gender = "Male";
            Employee.Address = "";
            Employee.Phone = "";
            Employee.Position = "";
            Employee.Avatar = "";
            Employee.DepartmentId = null;

            Username = "";
            Password = "";
            Role = "Employee";

            StartDate = DateTime.Now;
            DateOfBirth = DateTime.Now.AddYears(-20);

            LoadDepartments();

            SaveCommand = new RelayCommand(o => Save());
            CancelCommand = new RelayCommand(o => Cancel());
        }

        // Constructor cho trường hợp sửa: nhận EmployeeInfo từ service
        public EmployeeEditViewModel(EmployeeInfo info, IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;

            // Gán dữ liệu từ info
            Employee = new Employee
            {
                EmployeeId = info.EmployeeId,
                FullName = info.FullName,
                Gender = info.Gender,
                Address = info.Address,
                Phone = info.Phone,
                Position = info.Position,
                Avatar = info.Avatar,
                DepartmentId = info.DepartmentId,
                StartDate = info.StartDate,
                DateOfBirth = info.DateOfBirth
            };

            Username = info.Username;
            Password = info.Password;
            Role = info.Role;

            // Chuyển từ DateOnly sang DateTime cho binding (nếu không nullable)
            StartDate = Employee.StartDate.HasValue
               ? new DateTime(Employee.StartDate.Value.Year, Employee.StartDate.Value.Month, Employee.StartDate.Value.Day)
               :DateTime.Now; // hoặc một giá trị mặc định khác 
            DateOfBirth = new DateTime(Employee.DateOfBirth.Year, Employee.DateOfBirth.Month, Employee.DateOfBirth.Day);
            SelectedDepartmentId = Employee.DepartmentId ?? 0;
            LoadDepartments();

            SaveCommand = new RelayCommand(o => Save());
            CancelCommand = new RelayCommand(o => Cancel());
        }

        // Overload cho trường hợp sửa nếu không truyền service
        public EmployeeEditViewModel(EmployeeInfo info)
            : this(info, new EmployeeService(), new DepartmentService())
        {
        }

        // Overload cho thêm mới với service
        public EmployeeEditViewModel(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            SaveCommand = new RelayCommand(o => Save());
            CancelCommand = new RelayCommand(o => Cancel());
        }

        private void LoadDepartments()
        {
            var deptList = _departmentService.GetAllDepartments();
            Departments = new ObservableCollection<Department>(deptList);
            OnPropertyChanged(nameof(Departments));
        }

        private void Save()
        {
            // Kiểm tra các trường bắt buộc
            var errors = new ObservableCollection<string>();
            if (string.IsNullOrWhiteSpace(Username))
                errors.Add("Username không được để trống.");
            if (string.IsNullOrWhiteSpace(Employee.FullName))
                errors.Add("Họ tên không được để trống.");
            if (string.IsNullOrWhiteSpace(Password))
                errors.Add("Mật khẩu không được để trống.");
            // Có thể thêm kiểm tra định dạng khác nếu cần

            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Nếu đang thêm mới, kiểm tra username tồn tại
            if (Employee.EmployeeId == 0 && _employeeService.UserExists(Username))
            {
                MessageBox.Show("Tài khoản này đã có chủ rồi.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Employee.DepartmentId = SelectedDepartmentId;
            Employee.StartDate = DateOnly.FromDateTime(StartDate);
            Employee.DateOfBirth = DateOnly.FromDateTime(DateOfBirth);

            try
            {
                if (Employee.EmployeeId == 0)
                {
                    _employeeService.AddEmployee(Employee, Username, Password, Role);
                    MessageBox.Show("Thêm nhân viên thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _employeeService.UpdateEmployee(Employee, Username, Password, Role);
                    MessageBox.Show("Cập nhật nhân viên thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                CloseAction?.Invoke(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            // Hiển thị hộp thoại xác nhận hủy
            var result = MessageBox.Show("Bạn có chắc chắn muốn hủy?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CloseAction?.Invoke(false);
            }
        }
    }
}