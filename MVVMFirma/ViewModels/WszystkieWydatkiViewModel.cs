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
    public class WszystkieWydatkiViewModel : WszystkieViewModel<WydatkiForAllView>
    {

        #region Constructor
        public WszystkieWydatkiViewModel()
            :base("Wydatki")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<WydatkiForAllView>
            (
                List = new ObservableCollection<WydatkiForAllView>
                (
                    from expense in designOfficeEntities.Expenses
                    select new WydatkiForAllView
                    {
                        ExpenseID = expense.ExpenseID,
                        ProjectsName = expense.Projects != null ? expense.Projects.ProjectName : "Brak projektu",
                        ProjectsType = expense.Projects != null ? expense.Projects.ProjectType : "Brak typu projektu",
                        ExpenseDate = expense.ExpenseDate,
                        ExpenseDescription = expense.ExpenseDescription,
                        NetAmount = expense.NetAmount,
                        VATAmount = expense.VATAmount,
                        GrossAmount = expense.GrossAmount
                    }
                )
            );
        }
        #endregion
    }


}