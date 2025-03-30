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

namespace FootballManagerWPF.ViewModels
{
    public class DepartmentManagementViewModel : BaseViewModel
    {
        private readonly IDepartmentService _departmentService;
        public ObservableCollection<Department> Departments { get; set; }

        public ICommand RefreshCommand { get; }
        public ICommand AddDepartmentCommand { get; }
        public ICommand EditDepartmentCommand { get; }
        public ICommand DeleteDepartmentCommand { get; }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set { _selectedDepartment = value; OnPropertyChanged(); }
        }

        public DepartmentManagementViewModel() : this(new DepartmentService()) { }

        public DepartmentManagementViewModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            LoadDepartments();
            RefreshCommand = new RelayCommand(o => LoadDepartments());
            AddDepartmentCommand = new RelayCommand(o => AddDepartment());
            EditDepartmentCommand = new RelayCommand(o => EditDepartment(), o => SelectedDepartment != null);
            DeleteDepartmentCommand = new RelayCommand(o => DeleteDepartment(), o => SelectedDepartment != null);
        }

        private void LoadDepartments()
        {
            var list = _departmentService.GetAllDepartments();
            Departments = new ObservableCollection<Department>(list);
            OnPropertyChanged(nameof(Departments));
        }

        private void AddDepartment()
        {
            // Mở hộp thoại nhập thông tin phòng ban mới (ví dụ: InputBox hoặc cửa sổ riêng)
            string name = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên phòng ban:", "Thêm phòng ban", "");
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên phòng ban không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string description = Microsoft.VisualBasic.Interaction.InputBox("Nhập mô tả phòng ban:", "Thêm phòng ban", "");
            var dept = new Department { Name = name, Description = description };
            _departmentService.AddDepartment(dept);
            MessageBox.Show("Thêm phòng ban thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadDepartments();
        }

        private void EditDepartment()
        {
            if (SelectedDepartment == null) return;
            // Mở hộp thoại để sửa thông tin phòng ban
            string name = Microsoft.VisualBasic.Interaction.InputBox("Sửa tên phòng ban:", "Sửa phòng ban", SelectedDepartment.Name);
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên phòng ban không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string description = Microsoft.VisualBasic.Interaction.InputBox("Sửa mô tả phòng ban:", "Sửa phòng ban", SelectedDepartment.Description);
            SelectedDepartment.Name = name;
            SelectedDepartment.Description = description;
            _departmentService.UpdateDepartment(SelectedDepartment);
            MessageBox.Show("Cập nhật phòng ban thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadDepartments();
        }

        private bool CanEditOrDeleteDepartment(object parameter)
        {
            return SelectedDepartment != null;
        }

        private void DeleteDepartment()
        {
            if (SelectedDepartment == null) return;
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng ban '{SelectedDepartment.Name}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _departmentService.DeleteDepartment(SelectedDepartment.DepartmentId);
                MessageBox.Show("Xóa phòng ban thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDepartments();
            }
        }

        private void RefreshDepartments()
        {
            Departments.Clear();
            foreach (var dept in _departmentService.GetAllDepartments())
            {
                Departments.Add(dept);
            }
        }
    }
}
