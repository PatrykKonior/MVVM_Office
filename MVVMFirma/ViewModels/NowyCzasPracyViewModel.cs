using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;

namespace MVVMFirma.ViewModels
{
    public class NowyCzasPracyViewModel : JedenViewModel<TimeLogs>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowyCzasPracyViewModel() : base("Nowy czas pracy")
        {
            item = new TimeLogs();
            LoadData();
            LogDate = DateTime.Now;
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
                case nameof(EmployeeID):
                    if (!EmployeeID.HasValue)
                        errors.Add("Pracownik jest wymagany.");
                    break;
                case nameof(ProjectID):
                    if (!ProjectID.HasValue)
                        errors.Add("Projekt jest wymagany.");
                    break;
                case nameof(LogDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(LogDate, "Data"));
                    break;
                case nameof(HoursWorked):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(HoursWorked, "Godziny Pracy"));
                    break;
                case nameof(HourlyRate):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(HourlyRate, "Stawka godzinowa"));
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

        public DateTime? LogDate
        {
            get => item.LogDate;
            set
            {
                item.LogDate = value;
                OnPropertyChanged(() => LogDate);
                ValidateProperty(nameof(LogDate));
            }
        }

        public decimal? HoursWorked
        {
            get => item.HoursWorked;
            set
            {
                item.HoursWorked = value;
                OnPropertyChanged(() => HoursWorked);
                ValidateProperty(nameof(HoursWorked));
            }
        }

        public decimal? HourlyRate
        {
            get => item.HourlyRate;
            set
            {
                item.HourlyRate = value;
                OnPropertyChanged(() => HourlyRate);
                OnPropertyChanged(() => TotalAmount);
                ValidateProperty(nameof(HourlyRate));
            }
        }

        public decimal? TotalAmount
        {
            get => item.TotalAmount ?? (HoursWorked * HourlyRate);
            set
            {
                item.TotalAmount = value;
                OnPropertyChanged(() => TotalAmount);
            }
        }

        public ObservableCollection<KeyAndValue> EmployeesItems { get; set; }
        public ObservableCollection<KeyAndValue> ProjectsItems { get; set; }

        #endregion

        #region Helpers

        private void LoadData()
        {
            // Pobieramy dane z bazy jako lista
            var employees = new TimeLogsAdd(designOfficeEntities).GetEmployeesKeyAndValueItems().ToList();
            var projects = new TimeLogsAdd(designOfficeEntities).GetProjectsKeyAndValueItems().ToList();

            // Formatowanie odbywa się w pamięci
            EmployeesItems = new ObservableCollection<KeyAndValue>(
                employees.Select(e => new KeyAndValue
                {
                    Key = e.Key,
                    Value = $"{e.Value}" // Formatowanie w pamięci
                })
            );

            ProjectsItems = new ObservableCollection<KeyAndValue>(
                projects.Select(p => new KeyAndValue
                {
                    Key = p.Key,
                    Value = $"{p.Value}" // Formatowanie w pamięci
                })
            );
        }

        public override void Save()
        {
            if (HasErrors)
            {
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            item.TotalAmount = HoursWorked * HourlyRate;

            designOfficeEntities.TimeLogs.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
