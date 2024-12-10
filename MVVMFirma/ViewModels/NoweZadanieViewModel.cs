using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NoweZadanieViewModel : JedenViewModel<Tasks>
    {
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
            }
        }

        public DateTime? TaskEndDate
        {
            get => item.TaskEndDate;
            set
            {
                item.TaskEndDate = value;
                OnPropertyChanged(() => TaskEndDate);
            }
        }

        public decimal? EstimatedHours
        {
            get => item.EstimatedHours;
            set
            {
                item.EstimatedHours = value;
                OnPropertyChanged(() => EstimatedHours);
            }
        }

        public string TaskName
        {
            get => item.TaskName;
            set
            {
                item.TaskName = value;
                OnPropertyChanged(() => TaskName);
            }
        }

        public string TaskDescription
        {
            get => item.TaskDescription;
            set
            {
                item.TaskDescription = value;
                OnPropertyChanged(() => TaskDescription);
            }
        }

        public string TaskStatus
        {
            get => item.TaskStatus;
            set
            {
                item.TaskStatus = value;
                OnPropertyChanged(() => TaskStatus);
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
            designOfficeEntities.Tasks.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
