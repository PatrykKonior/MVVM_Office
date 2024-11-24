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
    public class WszystkieDzialyViewModel : WszystkieViewModel<Departments>
    {

        #region Constructor
        public WszystkieDzialyViewModel()
            :base("Działy w biurze")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Departments>
            (
                designOfficeEntities.Departments.ToList()
            );
        }
        #endregion
    }


}