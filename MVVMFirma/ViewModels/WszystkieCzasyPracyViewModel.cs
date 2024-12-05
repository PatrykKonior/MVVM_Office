using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;
using System.Windows.Documents;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class WszystkieCzasyPracyViewModel:WszystkieViewModel<CzasPracyForAllView>
    {
        #region Constructor
        public WszystkieCzasyPracyViewModel()
            : base("Czasy Pracy")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<CzasPracyForAllView>
            (
                List = new ObservableCollection<CzasPracyForAllView>
                (
                    from timeLog in designOfficeEntities.TimeLogs
                    select new CzasPracyForAllView
                    {
                        TimeLogID = timeLog.TimeLogID,
                        EmployeesFirstName = timeLog.Employees != null ? timeLog.Employees.FirstName : "Nieprzypisany",
                        EmployeesLastName = timeLog.Employees.LastName,
                        EmployeesPosition = timeLog.Employees.Position,
                        ProjectsName = timeLog.Projects != null ? timeLog.Projects.ProjectName : "Brak projektu",
                        ProjectsType = timeLog.Projects.ProjectType,
                        LogDate = timeLog.LogDate,
                        HoursWorked = timeLog.HoursWorked,
                        HourlyRate = timeLog.HourlyRate,
                        TotalAmount = timeLog.TotalAmount
                    }
                    )
            );
        }
        #endregion
    }
}
