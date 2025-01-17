using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Validators;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class NowyTowarViewModel : JedenViewModel<Materials>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowyTowarViewModel()
        : base("Towar")
        {
            item = new Materials();
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
                case nameof(MaterialName):
                    errors.AddRange(StringValidator.ValidateRequired(MaterialName, "Nazwa Materiału"));
                    errors.AddRange(StringValidator.ValidateMaxLength(MaterialName, 255, "Nazwa Materiału"));
                    break;
                case nameof(MaterialDescription):
                    errors.AddRange(StringValidator.ValidateRequired(MaterialDescription, "Opis Materiału"));
                    errors.AddRange(StringValidator.ValidateMaxLength(MaterialDescription, 500, "Opis Materiału"));
                    break;
                case nameof(UnitPrice):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(UnitPrice, "Cena Jednostkowa"));
                    break;
                case nameof(VATRate):
                    if (VATRate == null || !new[] { 0m, 5m, 8m, 23m }.Contains(VATRate.Value))
                    {
                        errors.Add("Stawka VAT musi być jedną z wartości: 0%, 5%, 8%, 23%.");
                    }
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

        // dla każdego pola na interface tworzymy properties
        public string MaterialName
        {
            get
            {
                return item.MaterialName;
            }
            set
            {
                item.MaterialName = value;
                OnPropertyChanged(() => MaterialName);
                ValidateProperty(nameof(MaterialName));
            }
        }
        public string MaterialDescription
        {
            get
            {
                return item.MaterialDescription;
            }
            set
            {
                item.MaterialDescription = value;
                OnPropertyChanged(() => MaterialDescription);
                ValidateProperty(nameof(MaterialDescription));
            }
        }

        public decimal? UnitPrice
        {
            get
            {
                return item.UnitPrice;
            }
            set
            {
                item.UnitPrice = value;
                OnPropertyChanged(() => UnitPrice);
                ValidateProperty(nameof(UnitPrice));
            }
        }

        public decimal? VATRate
        {
            get
            {
                return item.VATRate;
            }
            set
            {
                item.VATRate = value;
                OnPropertyChanged(() => VATRate);
                ValidateProperty(nameof(VATRate));
            }
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            if (HasErrors)
            {
                // Wyświetl komunikat w interfejsie użytkownika
                MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.Materials.Add(item); //dodaje towar do lokalnej kolekcji
            designOfficeEntities.SaveChanges(); //zapisuje zmiany do bazy danych
        }
        #endregion
    }
}
