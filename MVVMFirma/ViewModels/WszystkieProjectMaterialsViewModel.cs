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
    public class WszystkieProjectMaterialsViewModel:WszystkieViewModel<ProjectMaterialsForAllView>
    {
        #region Constructor
        public WszystkieProjectMaterialsViewModel()
            : base("Materiały w projektach")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Nazwa projektu",
                "Nazwa materiału",
                "Ilość",
                "Cena jednostkowa",
                "Kwota VAT"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.OrderBy(pm => pm.ProjectsName));
                    break;
                case "Nazwa materiału":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.OrderBy(pm => pm.MaterialsName));
                    break;
                case "Ilość":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.OrderBy(pm => pm.Quantity));
                    break;
                case "Cena jednostkowa":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.OrderBy(pm => pm.UnitPrice));
                    break;
                case "Kwota VAT":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.OrderBy(pm => pm.VATAmount));
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
                "Nazwa projektu",
                "Nazwa materiału",
                "Ilość",
                "Cena jednostkowa",
                "Kwota VAT"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.Where(pm => pm.ProjectsName != null &&
                        pm.ProjectsName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwa materiału":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.Where(pm => pm.MaterialsName != null &&
                        pm.MaterialsName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Ilość":
                    if (decimal.TryParse(FindTextBox, out var quantity))
                    {
                        List = new ObservableCollection<ProjectMaterialsForAllView>(List.Where(pm => pm.Quantity == quantity));
                    }
                    break;

                case "Cena jednostkowa":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.Where(pm =>
                        pm.UnitPrice != null &&
                        pm.UnitPrice.Value.ToString("F2").StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                case "Kwota VAT":
                    List = new ObservableCollection<ProjectMaterialsForAllView>(List.Where(pm =>
                        pm.VATAmount != null &&
                        pm.VATAmount.Value.ToString("F2").StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ProjectMaterialsForAllView>
            (
                List = new ObservableCollection<ProjectMaterialsForAllView>
                (
                    from pm in designOfficeEntities.ProjectMaterials
                    select new ProjectMaterialsForAllView
                    {
                        ProjectMaterialID = pm.ProjectMaterialID,
                        ProjectsName = pm.Projects != null ? pm.Projects.ProjectName : "Brak projektu",
                        MaterialsName = pm.Materials != null ? pm.Materials.MaterialName : "Brak materiału",
                        Quantity = pm.Quantity,
                        UnitPrice = pm.UnitPrice,
                        VATAmount = pm.VATAmount,
                    }
                    )
            );
        }
        #endregion
    }
}
