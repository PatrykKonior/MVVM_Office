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

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Nazwa zadania",
                "Data rozpoczęcia",
                "Data zakończenia",
                "Status zadania"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa zadania":
                    List = new ObservableCollection<ZadaniaForAllView>(List.OrderBy(z => z.TaskName));
                    break;
                case "Data rozpoczęcia":
                    List = new ObservableCollection<ZadaniaForAllView>(List.OrderBy(z => z.TaskStartDate));
                    break;
                case "Data zakończenia":
                    List = new ObservableCollection<ZadaniaForAllView>(List.OrderBy(z => z.TaskEndDate));
                    break;
                case "Status zadania":
                    List = new ObservableCollection<ZadaniaForAllView>(List.OrderBy(z => z.TaskStatus));
                    break;
                default:
                    break;
            }
        }
        // tu decydujemy po czym filtrowac
        public override List<string> GetComboboxFindList()
        {
            return new List<string>
            {
                "Nazwa zadania",
                "Nazwa projektu",
                "Status zadania",
                "Imię pracownika"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa zadania":
                    List = new ObservableCollection<ZadaniaForAllView>(List.Where(z => z.TaskName != null &&
                        z.TaskName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                case "Nazwa projektu":
                    List = new ObservableCollection<ZadaniaForAllView>(List.Where(z => z.ProjectName != null &&
                        z.ProjectName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                case "Status zadania":
                    List = new ObservableCollection<ZadaniaForAllView>(List.Where(z => z.TaskStatus != null &&
                        z.TaskStatus.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                case "Imię pracownika":
                    List = new ObservableCollection<ZadaniaForAllView>(List.Where(z => z.EmployeesFirstName != null &&
                        z.EmployeesFirstName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                default:
                    break;
            }
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