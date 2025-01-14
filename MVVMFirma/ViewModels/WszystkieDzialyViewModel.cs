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