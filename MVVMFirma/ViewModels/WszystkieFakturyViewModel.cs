using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class WszystkieFakturyViewModel:WorkspaceViewModel
    {
        public WszystkieFakturyViewModel()
        {
            base.DisplayName = "Wszystkie faktury";
        }
    }
}
