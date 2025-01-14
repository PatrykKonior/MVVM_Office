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
            return new List<string>
            {
                "Nazwa klienta",
                "NIP klienta",
                "REGON klienta",
                "Data sprzedaży",
                "Kwota netto sprzedaży",
                "Kwota VAT sprzedaży",
                "Kwota brutto sprzedaży",
                "Status sprzedaży",
                "Data faktury",
                "Termin płatności",
                "Status faktury",
                "Łączna kwota faktury"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa klienta":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.ClientsCompanyName));
                    break;
                case "NIP klienta":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.ClientsNIP));
                    break;
                case "REGON klienta":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.ClientsRegon));
                    break;
                case "Data sprzedaży":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.SalesSaleDate));
                    break;
                case "Kwota netto sprzedaży":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.SalesTotalNetAmount));
                    break;
                case "Kwota VAT sprzedaży":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.SalesTotalVATAmount));
                    break;
                case "Kwota brutto sprzedaży":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.SalesTotalGrossAmount));
                    break;
                case "Status sprzedaży":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.SalesSaleStatus));
                    break;
                case "Data faktury":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.InvoiceDate));
                    break;
                case "Termin płatności":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.PaymentDueDate));
                    break;
                case "Status faktury":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.InvoiceStatus));
                    break;
                case "Łączna kwota faktury":
                    List = new ObservableCollection<FakturaForAllView>(List.OrderBy(f => f.TotalAmount));
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
                "Nazwa klienta",
                "NIP klienta",
                "REGON klienta",
                "Data sprzedaży",
                "Kwota netto sprzedaży",
                "Kwota VAT sprzedaży",
                "Kwota brutto sprzedaży",
                "Status sprzedaży",
                "Data faktury",
                "Termin płatności",
                "Status faktury",
                "Łączna kwota faktury"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa klienta":
                    List = new ObservableCollection<FakturaForAllView>(List.Where(f => f.ClientsCompanyName != null &&
                        f.ClientsCompanyName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "NIP klienta":
                    List = new ObservableCollection<FakturaForAllView>(List.Where(f => f.ClientsNIP != null &&
                        f.ClientsNIP.StartsWith(FindTextBox)));
                    break;

                case "REGON klienta":
                    List = new ObservableCollection<FakturaForAllView>(List.Where(f => f.ClientsRegon != null &&
                        f.ClientsRegon.StartsWith(FindTextBox)));
                    break;

                case "Data sprzedaży":
                case "Data faktury":
                case "Termin płatności":
                    List = new ObservableCollection<FakturaForAllView>(List.Where(f =>
                    {
                        var date = FindField == "Data sprzedaży" ? f.SalesSaleDate :
                                   FindField == "Data faktury" ? f.InvoiceDate :
                                   f.PaymentDueDate;
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

                case "Kwota netto sprzedaży":
                case "Kwota VAT sprzedaży":
                case "Kwota brutto sprzedaży":
                case "Łączna kwota faktury":
                    Func<FakturaForAllView, decimal?> field = null;

                    if (FindField == "Kwota netto sprzedaży")
                    {
                        field = f => f.SalesTotalNetAmount;
                    }
                    else if (FindField == "Kwota VAT sprzedaży")
                    {
                        field = f => f.SalesTotalVATAmount;
                    }
                    else if (FindField == "Kwota brutto sprzedaży")
                    {
                        field = f => f.SalesTotalGrossAmount;
                    }
                    else if (FindField == "Łączna kwota faktury")
                    {
                        field = f => f.TotalAmount;
                    }

                    if (field != null)
                    {
                        List = new ObservableCollection<FakturaForAllView>(List.Where(f =>
                            field(f) != null &&
                            field(f).Value.ToString("F2").StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    }
                    break;

                case "Status sprzedaży":
                case "Status faktury":
                    List = new ObservableCollection<FakturaForAllView>(List.Where(f => f.InvoiceStatus != null &&
                        f.InvoiceStatus.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                default:
                    break;
            }
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
