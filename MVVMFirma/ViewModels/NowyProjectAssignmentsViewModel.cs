using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowyProjectAssignmentsViewModel : JedenViewModel<ProjectAssignments>
    {
        #region Constructor
        public NowyProjectAssignmentsViewModel() : base("Nowy Przydział Projektu")
        {
            item = new ProjectAssignments();
            LoadData();
            AssignmentDate = DateTime.Now; // Domyślna data przydziału
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

        public string Role
        {
            get => item.Role;
            set
            {
                item.Role = value;
                OnPropertyChanged(() => Role);
            }
        }

        public DateTime? AssignmentDate
        {
            get => item.AssignmentDate;
            set
            {
                item.AssignmentDate = value;
                OnPropertyChanged(() => AssignmentDate);
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
            designOfficeEntities.ProjectAssignments.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
