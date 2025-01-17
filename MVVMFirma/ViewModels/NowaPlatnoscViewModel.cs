using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;

namespace MVVMFirma.ViewModels
{
    public class NowaPlatnoscViewModel : JedenViewModel<Payments>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowaPlatnoscViewModel() : base("Nowa Płatność")
        {
            item = new Payments();
            LoadData();
            PaymentDate = DateTime.Now; // Domyślna data płatności

            // Inicjalizacja metod płatności
            PaymentMethods = new ObservableCollection<string>
            {
                "Przelew bankowy",
                "Płatność gotówką",
                "Płatność kartą kredytową/debetową"
            };
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
                case nameof(SelectedInvoice):
                    errors.AddRange(StringValidator.ValidateRequired(InvoiceID?.ToString(), "Faktura"));
                    break;
                case nameof(PaymentDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(PaymentDate, "Data Płatności"));
                    break;
                case nameof(PaymentAmount):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(PaymentAmount, "Kwota"));
                    break;
                case nameof(PaymentMethod):
                    errors.AddRange(StringValidator.ValidateRequired(PaymentMethod, "Metoda Płatności"));
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

        public int? InvoiceID
        {
            get => item.InvoiceID;
            set
            {
                item.InvoiceID = value;
                OnPropertyChanged(() => InvoiceID);
                OnPropertyChanged(() => SelectedInvoice);
                ValidateProperty(nameof(SelectedInvoice));
            }
        }

        public KeyAndValue SelectedInvoice
        {
            get => InvoicesItems.FirstOrDefault(i => i.Key == InvoiceID);
            set
            {
                InvoiceID = value?.Key;
                OnPropertyChanged(() => SelectedInvoice);
                ValidateProperty(nameof(SelectedInvoice));
            }
        }

        public DateTime? PaymentDate
        {
            get => item.PaymentDate;
            set
            {
                item.PaymentDate = value;
                OnPropertyChanged(() => PaymentDate);
                ValidateProperty(nameof(PaymentDate));
            }
        }

        public decimal? PaymentAmount
        {
            get => item.PaymentAmount ?? 0;
            set
            {
                item.PaymentAmount = value;
                OnPropertyChanged(() => PaymentAmount);
                ValidateProperty(nameof(PaymentAmount));
            }
        }

        public string PaymentMethod
        {
            get => item.PaymentMethod;
            set
            {
                item.PaymentMethod = value;
                OnPropertyChanged(() => PaymentMethod);
                ValidateProperty(nameof(PaymentMethod));
            }
        }

        public ObservableCollection<KeyAndValue> InvoicesItems { get; set; }
        public ObservableCollection<string> PaymentMethods { get; set; }

        #endregion

        #region Helpers

        private void LoadData()
        {
            // Pobranie danych faktur
            var rawInvoices = new PaymentsAdd(designOfficeEntities).GetInvoicesKeyAndValueItems().ToList();
            InvoicesItems = new ObservableCollection<KeyAndValue>(rawInvoices);
        }

        public override void Save()
        {
            if (HasErrors)
            {
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            item.PaymentAmount = PaymentAmount;
            // Zapis danych płatności do bazy
            designOfficeEntities.Payments.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
