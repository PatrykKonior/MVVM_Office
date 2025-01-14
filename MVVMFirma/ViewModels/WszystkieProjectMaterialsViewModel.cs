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
