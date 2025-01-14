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
    public class WszystkiePlatnosciViewModel:WszystkieViewModel<PlatnosciForAllView>
    {
        #region Constructor
        public WszystkiePlatnosciViewModel()
            : base("Lista płatności")
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
            List = new ObservableCollection<PlatnosciForAllView>
            (
                List = new ObservableCollection<PlatnosciForAllView>
                (
                    from payment in designOfficeEntities.Payments
                    select new PlatnosciForAllView
                    {
                        PaymentID = payment.PaymentID,
                        PaymentDate = payment.PaymentDate,
                        PaymentAmount = payment.PaymentAmount,
                        PaymentMethod = payment.PaymentMethod,
                        InvoiceNumber = payment.Invoices.InvoiceID,
                        InvoiceStatus = payment.Invoices.InvoiceStatus,
                        InvoiceDate = payment.Invoices.InvoiceDate,
                        InvoiceTotalAmount = payment.Invoices.TotalAmount,

                        ClientsCompanyName = payment.Invoices != null && payment.Invoices.Sales != null
                        ? payment.Invoices.Sales.Clients.CompanyName
                        : "Brak klienta",
                    }
                    )
            );
        }
        #endregion
    }
}
