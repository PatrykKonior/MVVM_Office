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
    public class WszystkieSprzedazeViewModel : WszystkieViewModel<SprzedazForAllView>
    {

        #region Constructor
        public WszystkieSprzedazeViewModel()
            :base("Wykaz sprzedaży")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<SprzedazForAllView>
            (
                List = new ObservableCollection<SprzedazForAllView>
                (
                    from sale in designOfficeEntities.Sales
                    select new SprzedazForAllView
                    {
                        SaleID = sale.SaleID,
                        ClientsCompanyName = sale.Clients != null ? sale.Clients.CompanyName : "Brak klienta",
                        ClientsNIP = sale.Clients.NIP,
                        ClientsRegon = sale.Clients.Regon,
                        SaleDate = sale.SaleDate,
                        TotalNetAmount = sale.TotalNetAmount,
                        TotalVATAmount = sale.TotalVATAmount,
                        TotalGrossAmount = sale.TotalGrossAmount,
                        SaleStatus = sale.SaleStatus
                    }
                )
            );
        }
        #endregion
    }


}