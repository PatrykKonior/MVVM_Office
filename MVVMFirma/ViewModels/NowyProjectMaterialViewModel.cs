using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System.Collections;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class NowyProjectMaterialViewModel : JedenViewModel<ProjectMaterials>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor
        public NowyProjectMaterialViewModel() : base("Nowy materiał w projekcie")
        {
            item = new ProjectMaterials();
            LoadData();
        }
        #endregion

        #region Validators
        public bool HasErrors => _validationErrors.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationErrors.ContainsKey(propertyName) ? _validationErrors[propertyName] : null;
        }

        private void ValidateProperty(string propertyName)
        {
            List<string> errors = new List<string>();

            switch (propertyName)
            {
                case nameof(ProjectID):
                    errors.AddRange(StringValidator.ValidateRequired(ProjectID?.ToString(), "Projekt"));
                    break;
                case nameof(MaterialID):
                    errors.AddRange(StringValidator.ValidateRequired(MaterialID?.ToString(), "Materiał"));
                    break;
                case nameof(Quantity):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(Quantity, "Ilość"));
                    break;
                case nameof(UnitPrice):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(UnitPrice, "Cena Jednostkowa"));
                    break;
            }

            if (errors.Any())
                _validationErrors[propertyName] = errors;
            else
                _validationErrors.Remove(propertyName);

            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(() => HasErrors);
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
                ValidateProperty(nameof(ProjectID));
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
                ValidateProperty(nameof(MaterialID));
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
                ValidateProperty(nameof(Quantity));
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
                ValidateProperty(nameof(UnitPrice));
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
            if (HasErrors)
            {
                // Wyświetl komunikat w interfejsie użytkownika
                MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.ProjectMaterials.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
