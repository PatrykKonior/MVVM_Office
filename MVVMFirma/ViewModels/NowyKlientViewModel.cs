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

namespace MVVMFirma.ViewModels
{
    public class NowyKlientViewModel : JedenViewModel<Clients>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowyKlientViewModel()
        : base("Nowy klient")
        {
            item = new Clients();
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
                case nameof(CompanyName):
                    errors.AddRange(StringValidator.ValidateRequired(CompanyName, "Nazwa Klienta"));
                    errors.AddRange(StringValidator.ValidateMaxLength(CompanyName, 255, "Nazwa Klienta"));
                    break;
                case nameof(NIP):
                    errors.AddRange(StringValidator.ValidateNIP(NIP, "NIP", 10));
                    break;
                case nameof(Regon):
                    errors.AddRange(StringValidator.ValidateRegon(Regon, "REGON", 9));
                    break;
                case nameof(PhoneNumber):
                    errors.AddRange(StringValidator.ValidatePhoneNumber(PhoneNumber, "Numer Telefonu", 11));
                    break;
                case nameof(Email):
                    errors.AddRange(StringValidator.ValidateRequired(Email, "E-mail"));
                    errors.AddRange(StringValidator.ValidateEmail(Email, "E-mail"));
                    break;
                case nameof(ContactPersonName):
                    errors.AddRange(StringValidator.ValidateRequired(ContactPersonName, "Osoba do kontaktu"));
                    errors.AddRange(StringValidator.ValidateMaxLength(ContactPersonName, 255, "Osoba do kontaktu"));
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
        public string CompanyName
        {
            get
            {
                return item.CompanyName;
            }
            set
            {
                item.CompanyName = value;
                OnPropertyChanged(() => CompanyName);
                ValidateProperty(nameof(CompanyName));
            }
        }
        public string NIP
        {
            get
            {
                return item.NIP;
            }
            set
            {
                item.NIP = value;
                OnPropertyChanged(() => NIP);
                ValidateProperty(nameof(NIP));
            }
        }

        public string Regon
        {
            get
            {
                return item.Regon;
            }
            set
            {
                item.Regon = value;
                OnPropertyChanged(() => Regon);
                ValidateProperty(nameof(Regon));
            }
        }

        public string PhoneNumber
        {
            get
            {
                return item.PhoneNumber;
            }
            set
            {
                item.PhoneNumber = value;
                OnPropertyChanged(() => PhoneNumber);
                ValidateProperty(nameof(PhoneNumber));
            }
        }
        public string Email
        {
            get
            {
                return item.Email;
            }
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
                ValidateProperty(nameof(Email));
            }
        }
        
        public string ContactPersonName
        {
            get
            {
                return item.ContactPersonName;
            }
            set
            {
                item.ContactPersonName = value;
                OnPropertyChanged(() => ContactPersonName);
                ValidateProperty(nameof(ContactPersonName));
            }
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            if (HasErrors)
            {
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.Clients.Add(item); 
            designOfficeEntities.SaveChanges(); 
        }
        #endregion
    }
}
