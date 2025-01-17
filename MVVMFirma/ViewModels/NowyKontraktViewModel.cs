using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System.ComponentModel;

namespace MVVMFirma.ViewModels
{
    public class NowyKontraktViewModel : JedenViewModel<Contracts>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor

        public NowyKontraktViewModel() : base("Nowy Kontrakt")
        {
            item = new Contracts();
            LoadData();
            ContractDate = DateTime.Now; // Domyślna data kontraktu
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
                case nameof(ContractDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(ContractDate, "Data Kontraktu"));
                    break;
                case nameof(ContractValueNet):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(ContractValueNet, "Kwota Netto"));
                    break;
                case nameof(VATRate):
                    errors.AddRange(DecimalValidator.ValidateInSet(VATRate, new[] { 0m, 5m, 8m, 23m }, "VAT"));
                    break;
                case nameof(ClientSignatureDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(ClientSignatureDate, "Data podpisu klienta"));
                    break;
                case nameof(CompanySignatureDate):
                    if (ClientSignatureDate.HasValue && CompanySignatureDate < ClientSignatureDate)
                        errors.Add("Data podpisu firmy nie może być wcześniejsza niż data podpisu klienta.");
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

        public DateTime? ContractDate
        {
            get => item.ContractDate;
            set
            {
                item.ContractDate = value;
                OnPropertyChanged(() => ContractDate);
                ValidateProperty(nameof(ContractDate));
            }
        }

        public decimal? ContractValueNet
        {
            get => item.ContractValueNet ?? 0;
            set
            {
                item.ContractValueNet = value;
                OnPropertyChanged(() => ContractValueNet);
                OnPropertyChanged(() => ContractValueGross);
                ValidateProperty(nameof(ContractValueNet));
            }
        }

        public decimal? VATRate
        {
            get => item.VATRate ?? 0;
            set
            {
                item.VATRate = value;
                OnPropertyChanged(() => VATRate);
                OnPropertyChanged(() => ContractValueGross);
                ValidateProperty(nameof(VATRate));
            }
        }

        public decimal? ContractValueGross
        {
            get => ContractValueNet * (1 + VATRate / 100);
            set
            {
                item.ContractValueGross = value;
                OnPropertyChanged(() => ContractValueGross);
            }
        }

        public DateTime? ClientSignatureDate
        {
            get => item.ClientSignatureDate;
            set
            {
                item.ClientSignatureDate = value;
                OnPropertyChanged(() => ClientSignatureDate);
                ValidateProperty(nameof(CompanySignatureDate));
            }
        }

        public DateTime? CompanySignatureDate
        {
            get => item.CompanySignatureDate;
            set
            {
                item.CompanySignatureDate = value;
                OnPropertyChanged(() => CompanySignatureDate);
                ValidateProperty(nameof(CompanySignatureDate));
            }
        }

        public ObservableCollection<KeyAndValue> ProjectsItems { get; set; }

        #endregion

        #region Helpers

        private void LoadData()
        {
            // Pobieramy dane z bazy jako lista
            var rawProjects = new ContractsAdd(designOfficeEntities).GetProjectsKeyAndValueItems().ToList();

            // Formatowanie danych w pamięci
            ProjectsItems = new ObservableCollection<KeyAndValue>(
                rawProjects.Select(p => new KeyAndValue
                {
                    Key = p.Key,
                    Value = $"{p.Value}" 
                })
            );
        }

        public override void Save()
        {
            if (HasErrors)
            {
                // Display validation error
                System.Windows.MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // Przeliczanie wartości brutto przed zapisaniem
            item.ContractValueGross = ContractValueNet * (1 + VATRate / 100);

            // Zapis do bazy danych
            designOfficeEntities.Contracts.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
