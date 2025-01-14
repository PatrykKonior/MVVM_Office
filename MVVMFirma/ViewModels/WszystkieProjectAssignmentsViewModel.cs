using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class WszystkieProjectAssignmentsViewModel : WszystkieViewModel<ProjectAssignmentsForAllView>
    {

        #region Constructor
        public WszystkieProjectAssignmentsViewModel()
            :base("Przydział projektów")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return null;
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        { }
        // tu decydujemy po czym filtrowac
        public override List<string> GetComboboxFindList()
        {
            return null;
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        { }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ProjectAssignmentsForAllView>
            (
                List = new ObservableCollection<ProjectAssignmentsForAllView>
                (
                    from pa in designOfficeEntities.ProjectAssignments
                    select new ProjectAssignmentsForAllView
                    {
                        ProjectAssignmentID = pa.ProjectAssignmentID,
                        ProjectsName = pa.Projects != null ? pa.Projects.ProjectName : "Brak projektu",
                        EmployeesFirstName = pa.Employees.FirstName,
                        EmployeesLastName = pa.Employees.LastName,
                        EmployeesPosition = pa.Employees.Position,
                        Role = pa.Role,
                        AssignmentDate = pa.AssignmentDate
                    }
                )
            );
        }
        #endregion
    }


}