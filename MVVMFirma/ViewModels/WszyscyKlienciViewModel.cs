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
    public class WszyscyKlienciViewModel : WszystkieViewModel<Clients>
    {

        #region Constructor
        public WszyscyKlienciViewModel()
            :base("Klienci")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Clients>
            (
                designOfficeEntities.Clients.ToList()
            );
        }
        #endregion
    }


}