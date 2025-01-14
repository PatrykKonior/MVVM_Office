using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class WszyscyPracownicyViewModel : WszystkieViewModel<Employees>
    {

        #region Constructor
        public WszyscyPracownicyViewModel()
            :base("Pracownicy")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Imię",
                "Nazwisko",
                "Stanowisko",
                "Telefon kontaktowy",
                "E-mail",
                "Data zatrudnienia",
                "Miesięczne wynagrodzenie"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Imię":
                    List = new ObservableCollection<Employees>(List.OrderBy(e => e.FirstName));
                    break;
                case "Nazwisko":
                    List = new ObservableCollection<Employees>(List.OrderBy(e => e.LastName));
                    break;
                case "Stanowisko":
                    List = new ObservableCollection<Employees>(List.OrderBy(e => e.Position));
                    break;
                case "Telefon kontaktowy":
                    List = new ObservableCollection<Employees>(List.OrderBy(e => e.PhoneNumber));
                    break;
                case "E-mail":
                    List = new ObservableCollection<Employees>(List.OrderBy(e => e.Email));
                    break;
                case "Data zatrudnienia":
                    List = new ObservableCollection<Employees>(List.OrderBy(e => e.HireDate));
                    break;
                case "Miesięczne wynagrodzenie":
                    List = new ObservableCollection<Employees>(List.OrderBy(e => e.Salary));
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
                "Imię",
                "Nazwisko",
                "Stanowisko",
                "Telefon kontaktowy",
                "E-mail",
                "Data zatrudnienia",
                "Miesięczne wynagrodzenie"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Imię":
                    List = new ObservableCollection<Employees>(List.Where(e => e.FirstName != null &&
                        e.FirstName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwisko":
                    List = new ObservableCollection<Employees>(List.Where(e => e.LastName != null &&
                        e.LastName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Stanowisko":
                    List = new ObservableCollection<Employees>(List.Where(e => e.Position != null &&
                        e.Position.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Telefon kontaktowy":
                    List = new ObservableCollection<Employees>(List.Where(e => e.PhoneNumber != null &&
                        e.PhoneNumber.StartsWith(FindTextBox)));
                    break;

                case "E-mail":
                    List = new ObservableCollection<Employees>(List.Where(e => e.Email != null &&
                        e.Email.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Data zatrudnienia":
                    List = new ObservableCollection<Employees>(List.Where(e =>
                    {
                        if (e.HireDate == null) return false;

                        if (DateTime.TryParse(FindTextBox, out var exactDate))
                        {
                            return e.HireDate == exactDate;
                        }
                        else if (int.TryParse(FindTextBox, out var year))
                        {
                            return e.HireDate.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay))
                        {
                            return e.HireDate.Value.Month == monthDay.Month && e.HireDate.Value.Day == monthDay.Day;
                        }
                        return false;
                    }));
                    break;

                case "Miesięczne wynagrodzenie":
                    List = new ObservableCollection<Employees>(List.Where(e => e.Salary != null &&
                        e.Salary.Value.ToString("F2").StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Employees>
            (
                designOfficeEntities.Employees.ToList()
            );
        }
        #endregion
    }


}