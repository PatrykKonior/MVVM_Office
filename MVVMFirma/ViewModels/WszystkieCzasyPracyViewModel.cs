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

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Imię pracownika",
                "Nazwisko pracownika",
                "Nazwa projektu",
                "Typ projektu",
                "Data rejestracji",
                "Przepracowane godziny",
                "Stawka godzinowa",
                "Kwota całkowita"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Imię pracownika":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.EmployeesFirstName));
                    break;
                case "Nazwisko pracownika":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.EmployeesLastName));
                    break;
                case "Nazwa projektu":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.ProjectsName));
                    break;
                case "Typ projektu":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.ProjectsType));
                    break;
                case "Data rejestracji":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.LogDate));
                    break;
                case "Przepracowane godziny":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.HoursWorked));
                    break;
                case "Stawka godzinowa":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.HourlyRate));
                    break;
                case "Kwota całkowita":
                    List = new ObservableCollection<CzasPracyForAllView>(List.OrderBy(c => c.TotalAmount));
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
                "Imię pracownika",
                "Nazwisko pracownika",
                "Nazwa projektu",
                "Typ projektu",
                "Data rejestracji",
                "Przepracowane godziny",
                "Stawka godzinowa",
                "Kwota całkowita"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Imię pracownika":
                    List = new ObservableCollection<CzasPracyForAllView>(List.Where(c => c.EmployeesFirstName != null &&
                        c.EmployeesFirstName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwisko pracownika":
                    List = new ObservableCollection<CzasPracyForAllView>(List.Where(c => c.EmployeesLastName != null &&
                        c.EmployeesLastName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwa projektu":
                    List = new ObservableCollection<CzasPracyForAllView>(List.Where(c => c.ProjectsName != null &&
                        c.ProjectsName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Typ projektu":
                    List = new ObservableCollection<CzasPracyForAllView>(List.Where(c => c.ProjectsType != null &&
                        c.ProjectsType.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Data rejestracji":
                    List = new ObservableCollection<CzasPracyForAllView>(List.Where(c =>
                    {
                        if (c.LogDate == null) return false;

                        if (DateTime.TryParse(FindTextBox, out var exactDate))
                        {
                            return c.LogDate == exactDate;
                        }
                        else if (int.TryParse(FindTextBox, out var year))
                        {
                            return c.LogDate.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay))
                        {
                            return c.LogDate.Value.Month == monthDay.Month && c.LogDate.Value.Day == monthDay.Day;
                        }
                        return false;
                    }));
                    break;

                case "Przepracowane godziny":
                case "Stawka godzinowa":
                case "Kwota całkowita":
                    Func<CzasPracyForAllView, decimal?> field = null;

                    if (FindField == "Przepracowane godziny")
                    {
                        field = c => c.HoursWorked;
                    }
                    else if (FindField == "Stawka godzinowa")
                    {
                        field = c => c.HourlyRate;
                    }
                    else if (FindField == "Kwota całkowita")
                    {
                        field = c => c.TotalAmount;
                    }

                    if (field != null)
                    {
                        List = new ObservableCollection<CzasPracyForAllView>(List.Where(c =>
                            field(c) != null &&
                            field(c).Value.ToString("F2").StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    }
                    break;

                default:
                    break;
            }
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
