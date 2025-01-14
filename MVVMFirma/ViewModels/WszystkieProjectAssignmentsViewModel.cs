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
            return new List<string>
            {
                "Nazwa projektu",
                "Imię pracownika",
                "Nazwisko pracownika",
                "Stanowisko",
                "Rola",
                "Data przydziału"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.OrderBy(pa => pa.ProjectsName));
                    break;
                case "Imię pracownika":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.OrderBy(pa => pa.EmployeesFirstName));
                    break;
                case "Nazwisko pracownika":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.OrderBy(pa => pa.EmployeesLastName));
                    break;
                case "Stanowisko":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.OrderBy(pa => pa.EmployeesPosition));
                    break;
                case "Rola":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.OrderBy(pa => pa.Role));
                    break;
                case "Data przydziału":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.OrderBy(pa => pa.AssignmentDate));
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
                "Nazwa projektu",
                "Imię pracownika",
                "Nazwisko pracownika",
                "Stanowisko",
                "Rola",
                "Data przydziału"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.Where(pa => pa.ProjectsName != null &&
                        pa.ProjectsName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Imię pracownika":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.Where(pa => pa.EmployeesFirstName != null &&
                        pa.EmployeesFirstName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwisko pracownika":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.Where(pa => pa.EmployeesLastName != null &&
                        pa.EmployeesLastName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Stanowisko":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.Where(pa => pa.EmployeesPosition != null &&
                        pa.EmployeesPosition.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Rola":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.Where(pa => pa.Role != null &&
                        pa.Role.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Data przydziału":
                    List = new ObservableCollection<ProjectAssignmentsForAllView>(List.Where(pa =>
                    {
                        if (pa.AssignmentDate == null) return false;

                        if (DateTime.TryParse(FindTextBox, out var exactDate))
                        {
                            return pa.AssignmentDate == exactDate;
                        }
                        else if (int.TryParse(FindTextBox, out var year))
                        {
                            return pa.AssignmentDate.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay))
                        {
                            return pa.AssignmentDate.Value.Month == monthDay.Month && pa.AssignmentDate.Value.Day == monthDay.Day;
                        }
                        return false;
                    }));
                    break;

                default:
                    break;
            }
        }
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