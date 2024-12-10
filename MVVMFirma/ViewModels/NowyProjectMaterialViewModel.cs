using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowyProjectMaterialViewModel : JedenViewModel<ProjectMaterials>
    {
        #region Constructor
        public NowyProjectMaterialViewModel() : base("Nowy materiał w projekcie")
        {
            item = new ProjectMaterials();
            LoadData();
        }
        #endregion

        #region Properties

        public int? ProjectID
        {
            get => item.ProjectID;
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
                OnPropertyChanged(() => SelectedProject);
            }
        }

        public KeyAndValue SelectedProject
        {
            get => ProjectsItems.FirstOrDefault(p => p.Key == ProjectID);
            set
            {
                ProjectID = value?.Key;
                OnPropertyChanged(() => SelectedProject);
            }
        }

        // Wybór materiału
        public int? MaterialID
        {
            get => item.MaterialID;
            set
            {
                item.MaterialID = value;
                OnPropertyChanged(() => MaterialID);
                OnPropertyChanged(() => SelectedMaterial);
            }
        }

        public KeyAndValue SelectedMaterial
        {
            get => MaterialsItems.FirstOrDefault(m => m.Key == MaterialID);
            set
            {
                MaterialID = value?.Key;
                OnPropertyChanged(() => SelectedMaterial);
            }
        }

        public decimal? Quantity
        {
            get => item.Quantity;
            set
            {
                item.Quantity = value;
                CalculateVATAndTotal();
                OnPropertyChanged(() => Quantity);
            }
        }

        public decimal? UnitPrice
        {
            get => item.UnitPrice;
            set
            {
                item.UnitPrice = value;
                CalculateVATAndTotal();
                OnPropertyChanged(() => UnitPrice);
            }
        }

        public decimal? VATAmount
        {
            get => item.VATAmount;
            set
            {
                item.VATAmount = value;
                OnPropertyChanged(() => VATAmount);
            }
        }

        #endregion

        #region Collections
        public ObservableCollection<KeyAndValue> ProjectsItems { get; set; }
        public ObservableCollection<KeyAndValue> MaterialsItems { get; set; }
        #endregion

        #region Helpers
        private void LoadData()
        {
            // Pobieranie projektów
            var rawProjects = new ProjectsAdd(designOfficeEntities).GetProjectsKeyAndValueItems().ToList();
            ProjectsItems = new ObservableCollection<KeyAndValue>(rawProjects);

            // Pobieranie materiałów
            var rawMaterials = new MaterialsAdd(designOfficeEntities).GetMaterialsKeyAndValueItems().ToList();
            MaterialsItems = new ObservableCollection<KeyAndValue>(rawMaterials);
        }

        private void CalculateVATAndTotal()
        {
            decimal vatRate = 0.23m; // VAT domyślny 23%
            if (Quantity.HasValue && UnitPrice.HasValue)
            {
                VATAmount = Quantity * UnitPrice * vatRate;
            }
        }

        public override void Save()
        {
            designOfficeEntities.ProjectMaterials.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
