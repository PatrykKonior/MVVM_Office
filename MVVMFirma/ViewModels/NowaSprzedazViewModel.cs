using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;

namespace MVVMFirma.ViewModels
{
    public class NowaSprzedazViewModel : JedenViewModel<Sales>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Modal

        public string SelectedClientFullName
        {
            get
            {
                var client = ClientsItems.FirstOrDefault(c => c.Key == ClientID);
                return client != null ? client.Value : "Nie wybrano klienta";
            }
        }

        public ICommand ShowAllClientsCommand { get; }
        
        #endregion

        #region Constructor
        public NowaSprzedazViewModel() : base("Nowa Sprzedaż")
        {
            item = new Sales();
            LoadData();
            SaleDate = DateTime.Now; // Domyślna data sprzedaży
            SaleStatusList = new ObservableCollection<string>
            {
                "Zakończona",
                "W trakcie",
                "Anulowana"
            };
            // Obsługa wyboru klienta
            ShowAllClientsCommand = new RelayCommand(ShowAllClients);
            Messenger.Default.Register<KeyAndValue>(this, "SelectedClient", GetSelectedClient);
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
                case nameof(SelectedClient):
                    errors.AddRange(StringValidator.ValidateRequired(ClientID?.ToString(), "Klient"));
                    break;
                case nameof(SaleDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(SaleDate, "Data Sprzedaży"));
                    break;
                case nameof(TotalNetAmount):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(TotalNetAmount, "Kwota Netto"));
                    break;
                case nameof(SaleStatus):
                    errors.AddRange(StringValidator.ValidateRequired(SaleStatus, "Status Sprzedaży"));
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
        
        private void ShowAllClients()
        {
            Messenger.Default.Send("KlienciAll");
        }

        private void GetSelectedClient(KeyAndValue selectedClient)
        {
            if (selectedClient != null)
            {
                ClientID = selectedClient.Key;
                OnPropertyChanged(() => SelectedClientFullName);
            }
        }
        
        public int? ClientID
        {
            get => item.ClientID;
            set
            {
                item.ClientID = value;
                OnPropertyChanged(() => ClientID);
                OnPropertyChanged(() => SelectedClient);
                ValidateProperty(nameof(SelectedClient));
            }
        }

        public KeyAndValue SelectedClient
        {
            get => ClientsItems.FirstOrDefault(c => c.Key == ClientID);
            set
            {
                ClientID = value?.Key;
                OnPropertyChanged(() => SelectedClient);
                ValidateProperty(nameof(SelectedClient));
            }
        }

        public DateTime? SaleDate
        {
            get => item.SaleDate;
            set
            {
                item.SaleDate = value;
                OnPropertyChanged(() => SaleDate);
                ValidateProperty(nameof(SaleDate));
            }
        }

        public string SaleStatus
        {
            get => item.SaleStatus;
            set
            {
                item.SaleStatus = value;
                OnPropertyChanged(() => SaleStatus);
                ValidateProperty(nameof(SaleStatus));
            }
        }

        public decimal? TotalNetAmount
        {
            get => item.TotalNetAmount;
            set
            {
                item.TotalNetAmount = value;
                CalculateAmounts();
                OnPropertyChanged(() => TotalNetAmount);
                ValidateProperty(nameof(TotalNetAmount));
            }
        }

        public decimal? TotalVATAmount
        {
            get => item.TotalVATAmount;
            set
            {
                item.TotalVATAmount = value;
                OnPropertyChanged(() => TotalVATAmount);
            }
        }

        public decimal? TotalGrossAmount
        {
            get => item.TotalGrossAmount;
            set
            {
                item.TotalGrossAmount = value;
                OnPropertyChanged(() => TotalGrossAmount);
            }
        }

        public ObservableCollection<string> SaleStatusList { get; set; }
        public ObservableCollection<KeyAndValue> ClientsItems { get; set; }
        #endregion

        #region Helpers
        private void LoadData()
        {
            var rawClients = new ClientsAdd(designOfficeEntities).GetClientsKeyAndValueItems().ToList();
            ClientsItems = new ObservableCollection<KeyAndValue>(rawClients);
        }

        private void CalculateAmounts()
        {
            // VAT 23% w obliczeniach
            decimal vatRate = 0.23m;

            if (TotalNetAmount.HasValue)
            {
                TotalVATAmount = TotalNetAmount * vatRate;
                TotalGrossAmount = TotalNetAmount + TotalVATAmount;
            }
        }

        public override void Save()
        {
            if (HasErrors)
            {
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.Sales.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
