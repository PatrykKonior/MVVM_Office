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
            return new List<string>
            {
                "Data płatności",
                "Kwota płatności",
                "Metoda płatności",
                "Numer faktury",
                "Status faktury",
                "Nazwa klienta",
                "Data faktury",
                "Kwota faktury"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Data płatności":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.PaymentDate));
                    break;
                case "Kwota płatności":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.PaymentAmount));
                    break;
                case "Metoda płatności":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.PaymentMethod));
                    break;
                case "Numer faktury":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.InvoiceNumber));
                    break;
                case "Status faktury":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.InvoiceStatus));
                    break;
                case "Nazwa klienta":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.ClientsCompanyName));
                    break;
                case "Data faktury":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.InvoiceDate));
                    break;
                case "Kwota faktury":
                    List = new ObservableCollection<PlatnosciForAllView>(List.OrderBy(p => p.InvoiceTotalAmount));
                    break;
                default:
                    break;
            }
        }
        // tu decydujemy po czym filtrowac
        public override List<string> GetComboboxFindList()
        {
            return new List<string>
            {
                "Data płatności",
                "Kwota płatności",
                "Metoda płatności",
                "Numer faktury",
                "Status faktury",
                "Nazwa klienta",
                "Data faktury",
                "Kwota faktury"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Data płatności":
                case "Data faktury":
                    List = new ObservableCollection<PlatnosciForAllView>(List.Where(p =>
                    {
                        var date = FindField == "Data płatności" ? p.PaymentDate : p.InvoiceDate;
                        if (date == null) return false;

                        if (DateTime.TryParse(FindTextBox, out var exactDate))
                        {
                            return date == exactDate;
                        }
                        else if (int.TryParse(FindTextBox, out var year))
                        {
                            return date.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay))
                        {
                            return date.Value.Month == monthDay.Month && date.Value.Day == monthDay.Day;
                        }
                        return false;
                    }));
                    break;

                case "Kwota płatności":
                case "Kwota faktury":
                    var amount = FindField == "Kwota płatności" ? List.Where(p => p.PaymentAmount != null) :
                        List.Where(p => p.InvoiceTotalAmount != null);

                    List = new ObservableCollection<PlatnosciForAllView>(amount.Where(p =>
                        (FindField == "Kwota płatności" ? p.PaymentAmount : p.InvoiceTotalAmount).Value.ToString("F2")
                        .StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                case "Metoda płatności":
                    List = new ObservableCollection<PlatnosciForAllView>(List.Where(p => p.PaymentMethod != null &&
                        p.PaymentMethod.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Numer faktury":
                    List = new ObservableCollection<PlatnosciForAllView>(List.Where(p => p.InvoiceNumber.ToString()
                        .StartsWith(FindTextBox)));
                    break;

                case "Status faktury":
                    List = new ObservableCollection<PlatnosciForAllView>(List.Where(p => p.InvoiceStatus != null &&
                        p.InvoiceStatus.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwa klienta":
                    List = new ObservableCollection<PlatnosciForAllView>(List.Where(p => p.ClientsCompanyName != null &&
                        p.ClientsCompanyName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                default:
                    break;
            }
        }
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
