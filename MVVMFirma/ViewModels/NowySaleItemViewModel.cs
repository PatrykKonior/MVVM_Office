using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System.Windows;
using System.Collections.Generic;

namespace MVVMFirma.ViewModels
{
    public class NowySaleItemViewModel : JedenViewModel<SaleItems>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowySaleItemViewModel() : base("Nowy Element Sprzedaży")
        {
            item = new SaleItems();
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
            var errors = new List<string>();

            switch (propertyName)
            {
                case nameof(SaleID):
                    if (!SaleID.HasValue)
                        errors.Add("Sprzedaż jest wymagana.");
                    break;
                case nameof(Description):
                    errors.AddRange(StringValidator.ValidateRequired(Description, "Opis produktu"));
                    errors.AddRange(StringValidator.ValidateMaxLength(Description, 255, "Opis produktu"));
                    break;
                case nameof(Quantity):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(Quantity, "Ilość"));
                    break;
                case nameof(UnitPriceNet):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(UnitPriceNet, "Cena jednostkowa netto"));
                    break;
                case nameof(VATRate):
                    if (!VATRate.HasValue)
                        errors.Add("Stawka VAT jest wymagana.");
                    else if (!(VATRate.Value == 0 || VATRate.Value == 5 || VATRate.Value == 8 || VATRate.Value == 23))
                        errors.Add("Stawka VAT musi wynosić 0%, 5%, 8% lub 23%.");
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

        public int? SaleID
        {
            get => item.SaleID;
            set
            {
                item.SaleID = value;
                OnPropertyChanged(() => SaleID);
                OnPropertyChanged(() => SelectedSale);
                ValidateProperty(nameof(SaleID));
            }
        }

        public KeyAndValue SelectedSale
        {
            get => SalesItems.FirstOrDefault(s => s.Key == SaleID);
            set
            {
                SaleID = value?.Key;
                OnPropertyChanged(() => SelectedSale);
            }
        }

        public string Description
        {
            get => item.Description;
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
                ValidateProperty(nameof(Description));
            }
        }

        public decimal? Quantity
        {
            get => item.Quantity;
            set
            {
                item.Quantity = value;
                CalculateAmounts();
                OnPropertyChanged(() => Quantity);
                ValidateProperty(nameof(Quantity));
            }
        }

        public decimal? UnitPriceNet
        {
            get => item.UnitPriceNet;
            set
            {
                item.UnitPriceNet = value;
                CalculateAmounts();
                OnPropertyChanged(() => UnitPriceNet);
                ValidateProperty(nameof(UnitPriceNet));
            }
        }

        public decimal? VATRate
        {
            get => item.VATRate;
            set
            {
                item.VATRate = value;
                CalculateAmounts();
                OnPropertyChanged(() => VATRate);
                ValidateProperty(nameof(VATRate));
            }
        }

        public decimal? NetAmount
        {
            get => item.NetAmount;
            set
            {
                item.NetAmount = value;
                OnPropertyChanged(() => NetAmount);
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

        public decimal? GrossAmount
        {
            get => item.GrossAmount;
            set
            {
                item.GrossAmount = value;
                OnPropertyChanged(() => GrossAmount);
            }
        }

        public ObservableCollection<KeyAndValue> SalesItems { get; set; }

        #endregion

        #region Helpers

        private void LoadData()
        {
            var rawSales = new SaleItemsAdd(designOfficeEntities).GetSalesKeyAndValueItems().ToList();
            SalesItems = new ObservableCollection<KeyAndValue>(rawSales);
        }

        private void CalculateAmounts()
        {
            if (Quantity.HasValue && UnitPriceNet.HasValue && VATRate.HasValue)
            {
                NetAmount = Quantity.Value * UnitPriceNet.Value;
                VATAmount = NetAmount.Value * (VATRate.Value / 100);
                GrossAmount = NetAmount + VATAmount;
            }
        }

        public override void Save()
        {
            if(HasErrors)
            {
                // Wyświetl komunikat w interfejsie użytkownika
                MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            item.NetAmount = NetAmount;
            item.VATAmount = VATAmount;
            item.GrossAmount = GrossAmount;

            designOfficeEntities.SaleItems.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
