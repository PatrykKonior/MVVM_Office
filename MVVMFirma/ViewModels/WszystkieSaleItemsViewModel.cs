using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;
using System.Windows.Documents;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class WszystkieSaleItemsViewModel:WszystkieViewModel<SaleItemsForAllView>
    {
        #region Constructor
        public WszystkieSaleItemsViewModel()
            : base("Lista sprzedanych produktów")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<SaleItemsForAllView>
            (
                List = new ObservableCollection<SaleItemsForAllView>
                (
                    from saleItem in designOfficeEntities.SaleItems
                    select new SaleItemsForAllView
                    {
                        SaleItemID = saleItem.SaleItemID,
                        SalesSaleDate = saleItem.Sales.SaleDate,
                        ClientsCompanyName = saleItem.Sales.Clients.CompanyName ?? "Brak klienta",

                        ItemDescription = saleItem.Description,
                        Quantity = saleItem.Quantity,
                        UnitPriceNet = saleItem.UnitPriceNet,
                        NetAmount = saleItem.NetAmount,
                        VATRate = saleItem.VATRate,
                        VATAmount = saleItem.VATAmount,
                        GrossAmount = saleItem.GrossAmount
                    }
                    )
            );
        }
        #endregion
    }
}
