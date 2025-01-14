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

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Data sprzedaży",
                "Kwota netto",
                "Kwota VAT",
                "Kwota brutto",
                "Status sprzedaży",
                "Nazwa klienta"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Data sprzedaży":
                    List = new ObservableCollection<SprzedazForAllView>(List.OrderBy(s => s.SaleDate));
                    break;
                case "Kwota netto":
                    List = new ObservableCollection<SprzedazForAllView>(List.OrderBy(s => s.TotalNetAmount));
                    break;
                case "Kwota VAT":
                    List = new ObservableCollection<SprzedazForAllView>(List.OrderBy(s => s.TotalVATAmount));
                    break;
                case "Kwota brutto":
                    List = new ObservableCollection<SprzedazForAllView>(List.OrderBy(s => s.TotalGrossAmount));
                    break;
                case "Status sprzedaży":
                    List = new ObservableCollection<SprzedazForAllView>(List.OrderBy(s => s.SaleStatus));
                    break;
                case "Nazwa klienta":
                    List = new ObservableCollection<SprzedazForAllView>(List.OrderBy(s => s.ClientsCompanyName));
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
                "NIP klienta",
                "Regon klienta",
                "Status sprzedaży"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Data sprzedaży":
                    List = new ObservableCollection<SprzedazForAllView>(List.Where(s =>
                    {
                        if (s.SaleDate == null) return false;

                        if (DateTime.TryParse(FindTextBox, out var exactDate))
                        {
                            return s.SaleDate == exactDate;
                        }
                        else if (int.TryParse(FindTextBox, out var year))
                        {
                            return s.SaleDate.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay))
                        {
                            return s.SaleDate.Value.Month == monthDay.Month && s.SaleDate.Value.Day == monthDay.Day;
                        }
                        return false;
                    }));
                    break;

                case "Nazwa klienta":
                    List = new ObservableCollection<SprzedazForAllView>(List.Where(s => s.ClientsCompanyName != null &&
                        s.ClientsCompanyName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "NIP klienta":
                    List = new ObservableCollection<SprzedazForAllView>(List.Where(s => s.ClientsNIP != null &&
                        s.ClientsNIP.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                case "Regon klienta":
                    List = new ObservableCollection<SprzedazForAllView>(List.Where(s => s.ClientsRegon != null &&
                        s.ClientsRegon.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                case "Status sprzedaży":
                    List = new ObservableCollection<SprzedazForAllView>(List.Where(s => s.SaleStatus != null &&
                        s.SaleStatus.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                default:
                    break;
            }
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