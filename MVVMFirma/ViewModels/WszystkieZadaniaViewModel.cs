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
    public class WszystkieZadaniaViewModel : WszystkieViewModel<ZadaniaForAllView>
    {

        #region Constructor
        public WszystkieZadaniaViewModel()
            :base("Zadania")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ZadaniaForAllView>
            (
                List = new ObservableCollection<ZadaniaForAllView>
                (
                    from task in designOfficeEntities.Tasks
                    select new ZadaniaForAllView
                    {
                        TaskID = task.TaskID,
                        ProjectName = task.Projects != null ? task.Projects.ProjectName : "Brak projektu",
                        ProjectType = task.Projects != null ? task.Projects.ProjectType : "Brak typu projektu",
                        TaskName = task.TaskName,
                        TaskDescription = task.TaskDescription,
                        EmployeesFirstName = task.Employees != null ? task.Employees.FirstName : "Nieprzypisany",
                        EmployeesLastName = task.Employees != null ? task.Employees.LastName : " ",
                        TaskStartDate = task.TaskStartDate,
                        TaskEndDate = task.TaskEndDate,
                        EstimatedHours = task.EstimatedHours,
                        TaskStatus = task.TaskStatus
                    }
                )
            );
        }
        #endregion
    }


}