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

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Data sprzedaży",
                "Nazwa klienta",
                "Opis produktu",
                "Ilość",
                "Cena jednostkowa netto",
                "Kwota netto",
                "Stawka VAT",
                "Kwota VAT",
                "Kwota brutto"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Data sprzedaży":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.SalesSaleDate));
                    break;
                case "Nazwa klienta":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.ClientsCompanyName));
                    break;
                case "Opis produktu":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.ItemDescription));
                    break;
                case "Ilość":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.Quantity));
                    break;
                case "Cena jednostkowa netto":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.UnitPriceNet));
                    break;
                case "Kwota netto":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.NetAmount));
                    break;
                case "Stawka VAT":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.VATRate));
                    break;
                case "Kwota VAT":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.VATAmount));
                    break;
                case "Kwota brutto":
                    List = new ObservableCollection<SaleItemsForAllView>(List.OrderBy(s => s.GrossAmount));
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
                "Data sprzedaży",
                "Nazwa klienta",
                "Opis produktu",
                "Stawka VAT",
                "Kwota brutto"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Data sprzedaży":
                    List = new ObservableCollection<SaleItemsForAllView>(List.Where(s =>
                    {
                        if (s.SalesSaleDate == null) return false;

                        if (DateTime.TryParse(FindTextBox, out var exactDate))
                        {
                            return s.SalesSaleDate == exactDate;
                        }
                        else if (int.TryParse(FindTextBox, out var year))
                        {
                            return s.SalesSaleDate.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay))
                        {
                            return s.SalesSaleDate.Value.Month == monthDay.Month && s.SalesSaleDate.Value.Day == monthDay.Day;
                        }
                        return false;
                    }));
                    break;

                case "Nazwa klienta":
                    List = new ObservableCollection<SaleItemsForAllView>(List.Where(s => s.ClientsCompanyName != null &&
                        s.ClientsCompanyName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Opis produktu":
                    List = new ObservableCollection<SaleItemsForAllView>(List.Where(s => s.ItemDescription != null &&
                        s.ItemDescription.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Stawka VAT":
                    if (decimal.TryParse(FindTextBox, out var vatRate))
                    {
                        List = new ObservableCollection<SaleItemsForAllView>(List.Where(s => s.VATRate == vatRate));
                    }
                    break;

                case "Kwota brutto":
                    List = new ObservableCollection<SaleItemsForAllView>(List.Where(s =>
                        s.GrossAmount != null &&
                        s.GrossAmount.Value.ToString("F2").StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                default:
                    break;
            }
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
