using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Task1.DataService;
using Task1.Models;

namespace Task1.ModleView
{
    public class HomeViewModel : ObservableObject
    {
        private readonly AppDbContext? _context;

        public HomeViewModel(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();

            Students = new ObservableCollection<Student>(_context.Students.ToList());
            AddCommand = new RelayCommand(AddStudent);
            UpdateCommand = new RelayCommand<Student>(UpdateStudent, CanExecuteStudentCommand);
            DeleteCommand = new RelayCommand<Student>(DeleteStudent, CanExecuteStudentCommand);
            NewStudentCommand = new RelayCommand(ClearForm);


            ButtonContent = "Add";
            ButtonCommand = AddCommand;
        }

        private Student? _selectedStudent;
        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                SetProperty(ref _selectedStudent, value as Student);

                _selectedStudent = (value as Student);

                ButtonContent = value == null ? "Add" : "Update";
                ButtonCommand = value == null ? AddCommand : UpdateCommand;

                if (value != null)
                {
                    NewStudentName = value.Name;
                    NewStudentEmail = value.Email;
                    NewStudentAddress = value.Address;
                }
            }
        }

        private string? _newStudentName;
        public string? NewStudentName
        {
            get => _newStudentName;
            set => SetProperty(ref _newStudentName, value);
        }

        private string? _newStudentEmail;
        public string? NewStudentEmail
        {
            get => _newStudentEmail;
            set => SetProperty(ref _newStudentEmail, value);
        }

        private string? _newStudentAddress;
        public string? NewStudentAddress
        {
            get => _newStudentAddress;
            set => SetProperty(ref _newStudentAddress, value);
        }

        public ObservableCollection<Student> Students { get; private set; }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        private string _buttonContent;
        public string ButtonContent
        {
            get => _buttonContent;
            set => SetProperty(ref _buttonContent, value);
        }

        private ICommand _buttonCommand;
        public ICommand ButtonCommand
        {
            get => _buttonCommand;
            set => SetProperty(ref _buttonCommand, value);
        }
        public ICommand NewStudentCommand { get; }


        private void AddStudent()
        {
            if (!string.IsNullOrWhiteSpace(NewStudentName) && !string.IsNullOrWhiteSpace(NewStudentEmail) && !string.IsNullOrWhiteSpace(NewStudentAddress))
            {
                var newStudent = new Student { Name = NewStudentName, Email = NewStudentEmail, Address = NewStudentAddress };
                _context?.Students.Add(newStudent);
                _context?.SaveChanges();
                RefreshStudents();

                ClearForm();
            }
        }

        private void UpdateStudent(Student student)
        {
            if (_selectedStudent != null)
            {
                _selectedStudent.Name = NewStudentName;
                _selectedStudent.Email = NewStudentEmail;
                _selectedStudent.Address = NewStudentAddress;

                _context?.Students.Update(_selectedStudent);
                _context?.SaveChanges();
                RefreshStudents();

                ClearForm();
                SelectedStudent = null;
            }
        }

        private void DeleteStudent(Student student)
        {
            if (student != null)
            {
                _context?.Students.Remove(student);
                _context?.SaveChanges();
                RefreshStudents();
                ClearForm();
            }
        }

        private bool CanExecuteStudentCommand(Student student)
        {
            return student != null || _selectedStudent != null;
        }


        private void ClearForm()
        {
            NewStudentName = string.Empty;
            NewStudentEmail = string.Empty;
            NewStudentAddress = string.Empty;


            ButtonContent = "Add";
            ButtonCommand = AddCommand;

            SelectedStudent = null;
        }

        private void RefreshStudents()
        {
            Students = new ObservableCollection<Student>(_context?.Students.ToList());
            OnPropertyChanged(nameof(Students));
        }

    }
}
