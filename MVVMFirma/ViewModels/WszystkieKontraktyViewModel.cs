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
    public class WszystkieKontraktyViewModel : WszystkieViewModel<KontraktyForAllView>
    {

        #region Constructor
        public WszystkieKontraktyViewModel()
            :base("Kontrakty")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<KontraktyForAllView>
            (
                List = new ObservableCollection<KontraktyForAllView>
                (
                    from contract in designOfficeEntities.Contracts
                    select new KontraktyForAllView
                    {
                        ContractID = contract.ContractID,
                        ProjectsName = contract.Projects != null ? contract.Projects.ProjectName : "Brak projektu",
                        ProjectsType = contract.Projects != null ? contract.Projects.ProjectType : "Brak typu projektu",
                        ContractDate = contract.ContractDate,
                        ContractValueNet = contract.ContractValueNet,
                        VATRate = contract.VATRate,
                        ContractValueGross = contract.ContractValueGross,
                        ClientSignatureDate = contract.ClientSignatureDate,
                        CompanySignatureDate = contract.CompanySignatureDate
                    }
                )
            );
        }
        #endregion
    }


}