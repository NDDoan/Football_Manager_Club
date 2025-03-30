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
using System.Windows;
using FootballManagerWPF.Views;

namespace FootballManagerWPF.ViewModels
{
    public class EmployeeManagementViewModel : BaseViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        // Danh sách nhân viên ban đầu và sau khi lọc
        public ObservableCollection<EmployeeInfo> Employees { get; set; }
        private List<EmployeeInfo> _allEmployees; // lưu trữ danh sách đầy đủ từ service
        public EmployeeInfo SelectedEmployee { get; set; }

        // Các tiêu chí tìm kiếm
        private string _searchFullName;
        public string SearchFullName
        {
            get => _searchFullName;
            set { _searchFullName = value; OnPropertyChanged(); }
        }

        private int? _searchDepartmentId;
        public int? SearchDepartmentId
        {
            get => _searchDepartmentId;
            set { _searchDepartmentId = value; OnPropertyChanged(); }
        }

        private string _searchPosition;
        public string SearchPosition
        {
            get => _searchPosition;
            set { _searchPosition = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Department> Departments { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand RefreshCommand { get; }

        public EmployeeManagementViewModel() : this(new EmployeeService(), new DepartmentService()) { }

        public EmployeeManagementViewModel(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            LoadDepartments(departmentService);
            LoadEmployees();
            RefreshCommand = new RelayCommand(o => LoadEmployees());
            SearchCommand = new RelayCommand(o => ExecuteSearch());
            // Bạn cũng có thể khôi phục danh sách nhân viên lọc từ SessionData nếu có
            RestoreSessionState();

            AddEmployeeCommand = new RelayCommand(o => AddEmployee());
            EditEmployeeCommand = new RelayCommand(o => EditEmployee(), o => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(o => DeleteEmployee(), o => SelectedEmployee != null);
        }

        private void LoadDepartments(IDepartmentService departmentService)
        {
            var deptList = departmentService.GetAllDepartments();
            Departments = new ObservableCollection<Department>(deptList);
            OnPropertyChanged(nameof(Departments));
        }

        private void LoadEmployees()
        {
            _allEmployees = _employeeService.GetAllEmployeeInfos(); // lấy danh sách đầy đủ
            Employees = new ObservableCollection<EmployeeInfo>(_allEmployees);
            OnPropertyChanged(nameof(Employees));
            // Cập nhật session state
            SaveSessionState();
        }

        private void AddEmployee()
        {
            // Mở cửa sổ EmployeeEditView với chế độ thêm mới
            var editView = new EmployeeEditView();
            // Giả sử EmployeeEditViewModel có constructor cho thêm mới không cần EmployeeInfo
            var vm = new EmployeeEditViewModel();
            editView.DataContext = vm;
            if (editView.ShowDialog() == true)
            {
                // Sau khi thêm thành công, reload danh sách
                LoadEmployees();
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null)
                return;

            // Mở cửa sổ EmployeeEditView với dữ liệu của SelectedEmployee
            var editView = new EmployeeEditView();
            // Giả sử EmployeeEditViewModel có constructor nhận EmployeeInfo để sửa
            var vm = new EmployeeEditViewModel(SelectedEmployee);
            editView.DataContext = vm;
            if (editView.ShowDialog() == true)
            {
                LoadEmployees();
            }
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null)
                return;

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên {SelectedEmployee.FullName}?",
                "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _employeeService.DeleteEmployee(SelectedEmployee.EmployeeId);
                LoadEmployees();
            }
        }

        private void SaveSessionState()
        {
            // Giả sử bạn lưu lại danh sách ID nhân viên hiện đang hiển thị
            var session = Application.Current.Properties["SessionData"] as SessionData ?? new SessionData();
            session.FilteredEmployeeIds = new List<int>();
            foreach (var emp in Employees)
            {
                session.FilteredEmployeeIds.Add(emp.EmployeeId);
            }
            Application.Current.Properties["SessionData"] = session;
        }

        private void RestoreSessionState()
        {
            // Nếu có trạng thái phiên lưu trữ, bạn có thể khôi phục bộ lọc
            var session = Application.Current.Properties["SessionData"] as SessionData;
            if (session != null && session.FilteredEmployeeIds != null && session.FilteredEmployeeIds.Count > 0)
            {
                // Ví dụ: lọc lại danh sách Employees theo ID đã lưu
                var allEmployees = _employeeService.GetAllEmployeeInfos();
                var filtered = allEmployees.FindAll(e => session.FilteredEmployeeIds.Contains(e.EmployeeId));
                if (filtered.Count > 0)
                {
                    Employees = new ObservableCollection<EmployeeInfo>(filtered);
                    OnPropertyChanged(nameof(Employees));
                }
            }
        }

        private void ExecuteSearch()
        {
            // Lọc danh sách dựa trên tiêu chí (không phân biệt chữ hoa/thường)
            var filtered = _allEmployees.Where(e =>
                (string.IsNullOrWhiteSpace(SearchFullName) || e.FullName.ToLower().Contains(SearchFullName.ToLower())) &&
                (!SearchDepartmentId.HasValue || (e.DepartmentId.HasValue && e.DepartmentId.Value == SearchDepartmentId.Value)) &&
                (string.IsNullOrWhiteSpace(SearchPosition) || e.Position.ToLower().Contains(SearchPosition.ToLower()))
            ).ToList();

            Employees = new ObservableCollection<EmployeeInfo>(filtered);
            OnPropertyChanged(nameof(Employees));
        }
    }
}
