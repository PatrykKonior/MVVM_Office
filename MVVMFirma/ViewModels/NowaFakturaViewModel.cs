using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVMFirma.Validators;
using System.ComponentModel;

namespace MVVMFirma.ViewModels
{
    public class NowaFakturaViewModel:JedenViewModel<Invoices>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowaFakturaViewModel()
            : base("Nowa faktura")
        {
            item = new Invoices();
            LoadData();
            InvoiceDate = DateTime.Now; // Domyślnie ustawiona bieżąca data
            PaymentDueDate = DateTime.Now.AddDays(14); // Domyślnie termin płatności to 14 dni
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
                case nameof(SelectedSale):
                    errors.AddRange(StringValidator.ValidateRequired(SaleID?.ToString(), "Powiązana Sprzedaż"));
                    break;
                case nameof(SelectedClient):
                    errors.AddRange(StringValidator.ValidateRequired(ClientID?.ToString(), "Powiązany Klient"));
                    break;
                case nameof(InvoiceDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(InvoiceDate, "Data Faktury"));
                    break;
                case nameof(PaymentDueDate):
                    if (InvoiceDate.HasValue && PaymentDueDate < InvoiceDate)
                        errors.Add("Termin płatności nie może być wcześniejszy niż data faktury.");
                    break;
                case nameof(TotalAmount):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(TotalAmount, "Łączna Kwota"));
                    break;
                case nameof(InvoiceStatus):
                    errors.AddRange(StringValidator.ValidateRequired(InvoiceStatus, "Status Faktury"));
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
                ValidateProperty(nameof(SelectedSale));
            }
        }

        public KeyAndValue SelectedSale
        {
            get => SalesItems.FirstOrDefault(s => s.Key == SaleID);
            set
            {
                SaleID = value?.Key;
                OnPropertyChanged(() => SelectedSale);
                ValidateProperty(nameof(SelectedSale));
            }
        }

        public int? ClientID
        {
            get => item.Sales?.ClientID;
            set
            {
                if (item.Sales != null)
                {
                    item.Sales.ClientID = value;
                    OnPropertyChanged(() => ClientID);
                    ValidateProperty(nameof(ClientID));
                }
                OnPropertyChanged(() => SelectedClient);
            }
        }

        public KeyAndValue SelectedClient
        {
            get => ClientsItems.FirstOrDefault(c => c.Key == ClientID);
            set
            {
                ClientID = value?.Key;
                OnPropertyChanged(() => SelectedClient);
            }
        }

        public DateTime? InvoiceDate
        {
            get => item.InvoiceDate;
            set
            {
                item.InvoiceDate = value;
                OnPropertyChanged(() => InvoiceDate);
                ValidateProperty(nameof(InvoiceDate));
            }
        }

        public DateTime? PaymentDueDate
        {
            get => item.PaymentDueDate;
            set
            {
                item.PaymentDueDate = value;
                OnPropertyChanged(() => PaymentDueDate);
                ValidateProperty(nameof(PaymentDueDate));
            }
        }

        public string InvoiceStatus
        {
            get => item.InvoiceStatus;
            set
            {
                item.InvoiceStatus = value;
                OnPropertyChanged(() => InvoiceStatus);
                ValidateProperty(nameof(InvoiceStatus));
            }
        }

        public decimal? TotalAmount
        {
            get => item.TotalAmount;
            set
            {
                item.TotalAmount = value;
                OnPropertyChanged(() => TotalAmount);
                ValidateProperty(nameof(TotalAmount));
            }
        }

        public ObservableCollection<KeyAndValue> SalesItems { get; set; }
        public ObservableCollection<KeyAndValue> ClientsItems { get; set; }

        #endregion
        #region Helpers

        private void LoadData()
        {
            SalesItems = new ObservableCollection<KeyAndValue>(new SalesAdd(designOfficeEntities).GetSalesKeyAndValueItems());
            ClientsItems = new ObservableCollection<KeyAndValue>(new ClientsAdd(designOfficeEntities).GetClientsKeyAndValueItems());
        }

        public override void Save()
        {
            if (HasErrors)
            {
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (SaleID == null)
            {
                throw new InvalidOperationException("Sale must be selected.");
            }

            item.SaleID = SaleID;
            designOfficeEntities.Invoices.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion


    }
}
