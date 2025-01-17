using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System.Collections;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class NowyProjectAssignmentsViewModel : JedenViewModel<ProjectAssignments>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor
        public NowyProjectAssignmentsViewModel() : base("Nowy Przydział Projektu")
        {
            item = new ProjectAssignments();
            LoadData();
            AssignmentDate = DateTime.Now; // Domyślna data przydziału
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
            List<string> errors = new List<string>();

            switch (propertyName)
            {
                case nameof(ProjectID):
                    errors.AddRange(StringValidator.ValidateRequired(ProjectID?.ToString(), "Projekt"));
                    break;
                case nameof(EmployeeID):
                    errors.AddRange(StringValidator.ValidateRequired(EmployeeID?.ToString(), "Pracownik"));
                    break;
                case nameof(Role):
                    errors.AddRange(StringValidator.ValidateRequired(Role, "Rola"));
                    errors.AddRange(StringValidator.ValidateMaxLength(Role, 100, "Rola"));
                    break;
                case nameof(AssignmentDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(AssignmentDate, "Data Przydziału"));
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

        public int? EmployeeID
        {
            get => item.EmployeeID;
            set
            {
                item.EmployeeID = value;
                OnPropertyChanged(() => EmployeeID);
                OnPropertyChanged(() => SelectedEmployee);
                ValidateProperty(nameof(EmployeeID));
            }
        }

        public KeyAndValue SelectedEmployee
        {
            get => EmployeesItems.FirstOrDefault(e => e.Key == EmployeeID);
            set
            {
                EmployeeID = value?.Key;
                OnPropertyChanged(() => SelectedEmployee);
            }
        }

        public string Role
        {
            get => item.Role;
            set
            {
                item.Role = value;
                OnPropertyChanged(() => Role);
                ValidateProperty(nameof(Role));
            }
        }

        public DateTime? AssignmentDate
        {
            get => item.AssignmentDate;
            set
            {
                item.AssignmentDate = value;
                OnPropertyChanged(() => AssignmentDate);
                ValidateProperty(nameof(AssignmentDate));
            }
        }

        public ObservableCollection<KeyAndValue> ProjectsItems { get; set; }
        public ObservableCollection<KeyAndValue> EmployeesItems { get; set; }
        #endregion

        #region Helpers
        private void LoadData()
        {
            var rawProjects = new ProjectsAdd(designOfficeEntities).GetProjectsKeyAndValueItems().ToList();
            var rawEmployees = new EmployeesAdd(designOfficeEntities).GetEmployeesKeyAndValueItems().ToList();

            ProjectsItems = new ObservableCollection<KeyAndValue>(rawProjects);
            EmployeesItems = new ObservableCollection<KeyAndValue>(rawEmployees);
        }

        public override void Save()
        {
            if (HasErrors)
            {
                // Wyświetl komunikat w interfejsie użytkownika
                MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.ProjectAssignments.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
