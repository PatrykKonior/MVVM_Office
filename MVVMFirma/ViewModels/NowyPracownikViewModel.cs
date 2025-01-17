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
    public class NowyPracownikViewModel : JedenViewModel<Employees>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowyPracownikViewModel()
        : base("Nowy pracownik")
        {
            item = new Employees();
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
                case nameof(FirstName):
                    errors.AddRange(StringValidator.ValidateRequired(FirstName, "Imię"));
                    errors.AddRange(StringValidator.ValidateMaxLength(FirstName, 100, "Imię"));
                    break;
                case nameof(LastName):
                    errors.AddRange(StringValidator.ValidateRequired(LastName, "Nazwisko"));
                    errors.AddRange(StringValidator.ValidateMaxLength(LastName, 100, "Nazwisko"));
                    break;
                case nameof(Position):
                    errors.AddRange(StringValidator.ValidateRequired(Position, "Stanowisko"));
                    errors.AddRange(StringValidator.ValidateMaxLength(Position, 50, "Stanowisko"));
                    break;
                case nameof(PhoneNumber):
                    errors.AddRange(StringValidator.ValidatePhoneNumber(PhoneNumber, "Numer Telefonu", 11));
                    break;
                case nameof(Email):
                    errors.AddRange(StringValidator.ValidateRequired(Email, "E-mail"));
                    errors.AddRange(StringValidator.ValidateEmail(Email, "E-mail"));
                    break;
                case nameof(HireDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(HireDate, "Data zatrudnienia"));
                    break;
                case nameof(Salary):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(Salary, "Wynagrodzenie"));
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
        public string FirstName
        {
            get
            {
                return item.FirstName;
            }
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FirstName);
                ValidateProperty(nameof(FirstName));
            }
        }
        public string LastName
        {
            get
            {
                return item.LastName;
            }
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
                ValidateProperty(nameof(LastName));
            }
        }

        public string Position
        {
            get
            {
                return item.Position;
            }
            set
            {
                item.Position = value;
                OnPropertyChanged(() => Position);
                ValidateProperty(nameof(Position));
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
        
        public DateTime? HireDate
        {
            get
            {
                return item.HireDate;
            }
            set
            {
                item.HireDate = value;
                OnPropertyChanged(() => HireDate);
                ValidateProperty(nameof(HireDate));
            }
        }
        
        public decimal? Salary
        {
            get
            {
                return item.Salary;
            }
            set
            {
                item.Salary = value;
                OnPropertyChanged(() => Salary);
                ValidateProperty(nameof(Salary));
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

            designOfficeEntities.Employees.Add(item); 
            designOfficeEntities.SaveChanges(); 
        }
        #endregion
    }
}
