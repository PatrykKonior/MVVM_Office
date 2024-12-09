using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowyCzasPracyViewModel : JedenViewModel<TimeLogs>
    {
        #region Constructor

        public NowyCzasPracyViewModel() : base("Nowy czas pracy")
        {
            item = new TimeLogs();
            LoadData();
            LogDate = DateTime.Now;
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
            }
        }

        public decimal? HoursWorked
        {
            get => item.HoursWorked;
            set
            {
                item.HoursWorked = value;
                OnPropertyChanged(() => HoursWorked);
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
            item.TotalAmount = HoursWorked * HourlyRate;

            designOfficeEntities.TimeLogs.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
