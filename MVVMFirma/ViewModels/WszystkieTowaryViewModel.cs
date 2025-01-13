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
    public class WszystkieTowaryViewModel : WszystkieViewModel<Materials>
    {

        #region Constructor
        public WszystkieTowaryViewModel()
            :base("Towary")
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
            List = new ObservableCollection<Materials>
            (
                designOfficeEntities.Materials.ToList()
            );
        }
        #endregion
    }


}