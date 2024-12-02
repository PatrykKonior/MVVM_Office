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
    public class WszystkieFakturyViewModel:WszystkieViewModel<FakturaForAllView>
    {
        #region Constructor
        public WszystkieFakturyViewModel()
            : base("Faktury")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<FakturaForAllView>
            (
                List = new ObservableCollection<FakturaForAllView>
                (
                    from faktura in designOfficeEntities.Invoices
                    select new FakturaForAllView
                    {
                        InvoiceID = faktura.InvoiceID,
                        SalesSaleDate = faktura.Sales.SaleDate,
                        SalesTotalNetAmount = faktura.Sales.TotalNetAmount,
                        SalesTotalVATAmount = faktura.Sales.TotalVATAmount,
                        SalesTotalGrossAmount = faktura.Sales.TotalGrossAmount,
                        SalesSaleStatus = faktura.Sales.SaleStatus,
                        InvoiceDate = faktura.InvoiceDate,
                        PaymentDueDate = faktura.PaymentDueDate,
                        InvoiceStatus = faktura.InvoiceStatus,
                        TotalAmount = faktura.TotalAmount
                    }
                    )
            );
        }
        #endregion
    }
}
