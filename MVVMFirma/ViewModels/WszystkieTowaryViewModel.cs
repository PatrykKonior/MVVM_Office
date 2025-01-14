using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTowaryViewModel : WszystkieViewModel<Materials>
    {

        #region Constructor
        public WszystkieTowaryViewModel()
            :base("Towary")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Nazwa",
                "Opis",
                "Cena",
                "Stawka VAT"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa":
                    List = new ObservableCollection<Materials>(List.OrderBy(m => m.MaterialName));
                    break;
                case "Opis":
                    List = new ObservableCollection<Materials>(List.OrderBy(m => m.MaterialDescription));
                    break;
                case "Cena":
                    List = new ObservableCollection<Materials>(List.OrderBy(m => m.UnitPrice));
                    break;
                case "Stawka VAT":
                    List = new ObservableCollection<Materials>(List.OrderBy(m => m.VATRate));
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
                "Nazwa",
                "Opis",
                "Cena",
                "Stawka VAT"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa":
                    List = new ObservableCollection<Materials>(List.Where(m => m.MaterialName != null &&
                        m.MaterialName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                case "Opis":
                    List = new ObservableCollection<Materials>(List.Where(m => m.MaterialDescription != null &&
                        m.MaterialDescription.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                case "Cena":
                    if (decimal.TryParse(FindTextBox, out var unitPrice))
                    {
                        List = new ObservableCollection<Materials>(List.Where(m => m.UnitPrice == unitPrice));
                    }
                    break;
                case "Stawka VAT":
                    if (decimal.TryParse(FindTextBox, out var vatRate))
                    {
                        List = new ObservableCollection<Materials>(List.Where(m => m.VATRate == vatRate));
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Materials>
            (
                designOfficeEntities.Materials.ToList()
            );
        }
        #endregion
    }


}