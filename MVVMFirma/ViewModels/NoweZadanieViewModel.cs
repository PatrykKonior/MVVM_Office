using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;

namespace MVVMFirma.ViewModels
{
    public class NoweZadanieViewModel : JedenViewModel<Tasks>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Modal

        public string SelectedProjectFullName
        {
            get
            {
                var project = ProjectsItems.FirstOrDefault(p => p.Key == ProjectID);
                return project != null ? project.Value : "Nie wybrano projektu";
            }
        }

        public string SelectedEmployeeFullName
        {
            get
            {
                var employee = EmployeesItems.FirstOrDefault(e => e.Key == AssignedEmployeeID);
                return employee != null ? employee.Value : "Nie wybrano pracownika";
            }
        }

        public ICommand ShowAllProjectsCommand { get; }
        public ICommand ShowAllEmployeesCommand { get; }


        #endregion

        #region Constructor
        public NoweZadanieViewModel() : base("Nowe Zadanie")
        {
            item = new Tasks();
            LoadData();
            TaskStartDate = DateTime.Now; // Domyślna data rozpoczęcia
            TaskStatusList = new ObservableCollection<string>
            {
                "Zakończone",
                "W trakcie",
                "Anulowane"
            };
            ShowAllProjectsCommand = new RelayCommand(ShowAllProjects);
            ShowAllEmployeesCommand = new RelayCommand(ShowAllEmployees);

            Messenger.Default.Register<KeyAndValue>(this, "SelectedProject", GetSelectedProject);
            Messenger.Default.Register<KeyAndValue>(this, "SelectedEmployee", GetSelectedEmployee);
        }
        #endregion

        #region Validators
        public bool HasErrors => _validationErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationErrors.ContainsKey(propertyName) ? _validationErrors[propertyName] : null;
        }

        private void ValidateProperty(string propertyName)
        {
            var errors = new List<string>();

            switch (propertyName)
            {
                case nameof(TaskName):
                    errors.AddRange(StringValidator.ValidateRequired(TaskName, "Nazwa Zadania"));
                    errors.AddRange(StringValidator.ValidateMaxLength(TaskName, 255, "Nazwa Zadania"));
                    break;

                case nameof(TaskDescription):
                    errors.AddRange(StringValidator.ValidateRequired(TaskDescription, "Opis Zadania"));
                    errors.AddRange(StringValidator.ValidateMaxLength(TaskDescription, 1000, "Opis Zadania"));
                    break;

                case nameof(ProjectID):
                    errors.AddRange(StringValidator.ValidateRequired(ProjectID?.ToString(), "Projekt"));
                    break;

                case nameof(AssignedEmployeeID):
                    errors.AddRange(StringValidator.ValidateRequired(AssignedEmployeeID?.ToString(), "Pracownik"));
                    break;

                case nameof(TaskStartDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(TaskStartDate, "Data Rozpoczęcia"));
                    break;

                case nameof(TaskEndDate):
                    if (TaskStartDate.HasValue && TaskEndDate < TaskStartDate)
                        errors.Add("Data Zakończenia nie może być wcześniejsza niż Data Rozpoczęcia.");
                    errors.AddRange(DateValidator.ValidateNotInFuture(TaskEndDate, "Data Zakończenia"));
                    break;

                case nameof(EstimatedHours):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(EstimatedHours, "Szacowane Godziny"));
                    break;

                case nameof(TaskStatus):
                    errors.AddRange(StringValidator.ValidateRequired(TaskStatus, "Status Zadania"));
                    break;
            }

            if (errors.Any())
                _validationErrors[propertyName] = errors;
            else
                _validationErrors.Remove(propertyName);

            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(() => HasErrors);
        }
        #endregion

        #region Properties
        private void ShowAllProjects()
        {
            Messenger.Default.Send("ProjektyAll");
        }

        private void ShowAllEmployees()
        {
            Messenger.Default.Send("PracownicyAll");
        }

        private void GetSelectedProject(KeyAndValue selectedProject)
        {
            if (selectedProject != null)
            {
                ProjectID = selectedProject.Key;
                OnPropertyChanged(() => SelectedProjectFullName);
            }
        }

        private void GetSelectedEmployee(KeyAndValue selectedEmployee)
        {
            if (selectedEmployee != null)
            {
                AssignedEmployeeID = selectedEmployee.Key;
                OnPropertyChanged(() => SelectedEmployeeFullName);
            }
        }
        
        public int? ProjectID
        {
            get => item.ProjectID;
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
                OnPropertyChanged(() => SelectedProject);
                ValidateProperty(nameof(ProjectID));
            }
        }

        public KeyAndValue SelectedProject
        {
            get => ProjectsItems.FirstOrDefault(p => p.Key == ProjectID);
            set
            {
                ProjectID = value?.Key;
                OnPropertyChanged(() => SelectedProject);
            }
        }

        public int? AssignedEmployeeID
        {
            get => item.AssignedEmployeeID;
            set
            {
                item.AssignedEmployeeID = value;
                OnPropertyChanged(() => AssignedEmployeeID);
                OnPropertyChanged(() => SelectedEmployee);
                ValidateProperty(nameof(AssignedEmployeeID));
            }
        }

        public KeyAndValue SelectedEmployee
        {
            get => EmployeesItems.FirstOrDefault(e => e.Key == AssignedEmployeeID);
            set
            {
                AssignedEmployeeID = value?.Key;
                OnPropertyChanged(() => SelectedEmployee);
            }
        }

        public DateTime? TaskStartDate
        {
            get => item.TaskStartDate;
            set
            {
                item.TaskStartDate = value;
                OnPropertyChanged(() => TaskStartDate);
                ValidateProperty(nameof(TaskStartDate));
            }
        }

        public DateTime? TaskEndDate
        {
            get => item.TaskEndDate;
            set
            {
                item.TaskEndDate = value;
                OnPropertyChanged(() => TaskEndDate);
                ValidateProperty(nameof(TaskEndDate));
            }
        }

        public decimal? EstimatedHours
        {
            get => item.EstimatedHours;
            set
            {
                item.EstimatedHours = value;
                OnPropertyChanged(() => EstimatedHours);
                ValidateProperty(nameof(EstimatedHours));
            }
        }

        public string TaskName
        {
            get => item.TaskName;
            set
            {
                item.TaskName = value;
                OnPropertyChanged(() => TaskName);
                ValidateProperty(nameof(TaskName));
            }
        }

        public string TaskDescription
        {
            get => item.TaskDescription;
            set
            {
                item.TaskDescription = value;
                OnPropertyChanged(() => TaskDescription);
                ValidateProperty(nameof(TaskDescription));
            }
        }

        public string TaskStatus
        {
            get => item.TaskStatus;
            set
            {
                item.TaskStatus = value;
                OnPropertyChanged(() => TaskStatus);
                ValidateProperty(nameof(TaskStatus));
            }
        }

        public ObservableCollection<string> TaskStatusList { get; set; }
        public ObservableCollection<KeyAndValue> ProjectsItems { get; set; }
        public ObservableCollection<KeyAndValue> EmployeesItems { get; set; }
        #endregion

        #region Helpers
        private void LoadData()
        {
            ProjectsItems = new ObservableCollection<KeyAndValue>(
                new ProjectsAdd(designOfficeEntities).GetProjectsKeyAndValueItems());

            EmployeesItems = new ObservableCollection<KeyAndValue>(
                new EmployeesAdd(designOfficeEntities).GetEmployeesKeyAndValueItems());
        }

        public override void Save()
        {
            if (HasErrors)
            {
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.Tasks.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
