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
    public class WszystkieDzialyViewModel : WszystkieViewModel<DzialyForAllView>
    {

        #region Constructor
        public WszystkieDzialyViewModel()
            :base("Działy w biurze")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Nazwa działu",
                "Imię menedżera",
                "Nazwisko menedżera",
                "Email menedżera",
                "Telefon menedżera"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa działu":
                    List = new ObservableCollection<DzialyForAllView>(List.OrderBy(d => d.DepartmentName));
                    break;
                case "Imię menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.OrderBy(d => d.EmployeeFirstName));
                    break;
                case "Nazwisko menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.OrderBy(d => d.EmployeeLastName));
                    break;
                case "Email menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.OrderBy(d => d.EmployeeEmail));
                    break;
                case "Telefon menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.OrderBy(d => d.EmployeePhoneNumber));
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
                "Nazwa działu",
                "Imię menedżera",
                "Nazwisko menedżera",
                "Email menedżera",
                "Telefon menedżera"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa działu":
                    List = new ObservableCollection<DzialyForAllView>(List.Where(d => d.DepartmentName != null &&
                        d.DepartmentName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Imię menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.Where(d => d.EmployeeFirstName != null &&
                        d.EmployeeFirstName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwisko menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.Where(d => d.EmployeeLastName != null &&
                        d.EmployeeLastName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Email menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.Where(d => d.EmployeeEmail != null &&
                        d.EmployeeEmail.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Telefon menedżera":
                    List = new ObservableCollection<DzialyForAllView>(List.Where(d => d.EmployeePhoneNumber != null &&
                        d.EmployeePhoneNumber.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<DzialyForAllView>
            (
                List = new ObservableCollection<DzialyForAllView>
                (
                    from department in designOfficeEntities.Departments
                    select new DzialyForAllView
                    {
                        DepartmentID = department.DepartmentID,
                        DepartmentName = department.DepartmentName,
                        EmployeeFirstName = department.Employees != null ? department.Employees.FirstName : "Brak menedżera",
                        EmployeeLastName = department.Employees != null ? department.Employees.LastName : " ",
                        EmployeeEmail = department.Employees != null ? department.Employees.Email : " ",
                        EmployeePhoneNumber = department.Employees != null ? department.Employees.PhoneNumber : " ",
                    }
                )
            );
        }
        #endregion
    }


}