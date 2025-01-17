using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;

namespace MVVMFirma.ViewModels
{
    public class NowyDzialViewModel : JedenViewModel<Departments>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor
        public NowyDzialViewModel() : base("Nowy Dział")
        {
            item = new Departments();
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
                case nameof(DepartmentName):
                    errors.AddRange(StringValidator.ValidateRequired(DepartmentName, "Nazwa Działu"));
                    errors.AddRange(StringValidator.ValidateMaxLength(DepartmentName, 255, "Nazwa Działu"));
                    break;
                case nameof(ManagerID):
                    if (!ManagerID.HasValue)
                        errors.Add("Menedżer jest wymagany.");
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
        public string DepartmentName
        {
            get => item.DepartmentName;
            set
            {
                item.DepartmentName = value;
                OnPropertyChanged(() => DepartmentName);
                ValidateProperty(nameof(DepartmentName));
            }
        }

        public int? ManagerID
        {
            get => item.ManagerID;
            set
            {
                item.ManagerID = value;
                OnPropertyChanged(() => ManagerID);
                OnPropertyChanged(() => SelectedManager);
                ValidateProperty(nameof(ManagerID));
            }
        }

        public KeyAndValue SelectedManager
        {
            get => ManagersItems.FirstOrDefault(m => m.Key == ManagerID);
            set
            {
                ManagerID = value?.Key;
                OnPropertyChanged(() => SelectedManager);
            }
        }

        public ObservableCollection<KeyAndValue> ManagersItems { get; set; }
        #endregion

        #region Helpers
        private void LoadData()
        {
            var rawManagers = new EmployeesAdd(designOfficeEntities).GetEmployeesKeyAndValueItems().ToList();
            ManagersItems = new ObservableCollection<KeyAndValue>(rawManagers);
        }

        public override void Save()
        {
            if (HasErrors)
            {
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.Departments.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
