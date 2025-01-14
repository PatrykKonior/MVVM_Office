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
            List = new ObservableCollection<FakturaForAllView>
            (
                List = new ObservableCollection<FakturaForAllView>
                (
                    from faktura in designOfficeEntities.Invoices
                    select new FakturaForAllView
                    {
                        InvoiceID = faktura.InvoiceID,
                        ClientsCompanyName = faktura.Sales.Clients.CompanyName,
                        ClientsNIP = faktura.Sales.Clients.NIP,
                        ClientsRegon = faktura.Sales.Clients.Regon,
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
