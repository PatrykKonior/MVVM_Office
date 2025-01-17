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
    public class NowyProjektViewModel : JedenViewModel<Projects>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Constructor
        public NowyProjektViewModel() : base("Nowy Projekt")
        {
            item = new Projects();
            LoadData();
            ProjectStartDate = DateTime.Now; // Domyślna data rozpoczęcia projektu
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
                case nameof(ClientID):
                    if (!ClientID.HasValue)
                        errors.Add("Klient jest wymagany.");
                    break;
                case nameof(ManagerID):
                    if (!ManagerID.HasValue)
                        errors.Add("Menedżer jest wymagany.");
                    break;
                case nameof(ProjectName):
                    errors.AddRange(StringValidator.ValidateRequired(ProjectName, "Nazwa projektu"));
                    errors.AddRange(StringValidator.ValidateMaxLength(ProjectName, 255, "Nazwa projektu"));
                    break;
                case nameof(ProjectType):
                    errors.AddRange(StringValidator.ValidateRequired(ProjectType, "Typ projektu"));
                    break;
                case nameof(ProjectStartDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(ProjectStartDate, "Data rozpoczęcia"));
                    break;
                case nameof(ProjectEndDate):
                    if (ProjectEndDate < ProjectStartDate)
                        errors.Add("Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.");
                    break;
                case nameof(ProjectBudget):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(ProjectBudget, "Budżet projektu"));
                    break;
                case nameof(VATRate):
                    if (!VATRate.HasValue)
                        errors.Add("Stawka VAT jest wymagana.");
                    else if (!(VATRate.Value == 0 || VATRate.Value == 5 || VATRate.Value == 8 || VATRate.Value == 23))
                        errors.Add("Stawka VAT musi wynosić 0%, 5%, 8% lub 23%.");
                    break;
                case nameof(ProjectStatus):
                    errors.AddRange(StringValidator.ValidateRequired(ProjectStatus, "Status projektu"));
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
        public int? ClientID
        {
            get => item.ClientID;
            set
            {
                item.ClientID = value;
                OnPropertyChanged(() => ClientID);
                OnPropertyChanged(() => SelectedClient);
                ValidateProperty(nameof(ClientID));
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

        public string ProjectName
        {
            get => item.ProjectName;
            set
            {
                item.ProjectName = value;
                OnPropertyChanged(() => ProjectName);
                ValidateProperty(nameof(ProjectName));
            }
        }

        public string ProjectType
        {
            get => item.ProjectType;
            set
            {
                item.ProjectType = value;
                OnPropertyChanged(() => ProjectType);
                ValidateProperty(nameof(ProjectType));
            }
        }

        public DateTime? ProjectStartDate
        {
            get => item.ProjectStartDate;
            set
            {
                item.ProjectStartDate = value;
                OnPropertyChanged(() => ProjectStartDate);
                ValidateProperty(nameof(ProjectStartDate));
            }
        }

        public DateTime? ProjectEndDate
        {
            get => item.ProjectEndDate;
            set
            {
                item.ProjectEndDate = value;
                OnPropertyChanged(() => ProjectEndDate);
                ValidateProperty(nameof(ProjectEndDate));
            }
        }

        public decimal? ProjectBudget
        {
            get => item.ProjectBudget;
            set
            {
                item.ProjectBudget = value;
                OnPropertyChanged(() => ProjectBudget);
                ValidateProperty(nameof(ProjectBudget));
            }
        }

        public decimal? VATRate
        {
            get => item.VATRate;
            set
            {
                item.VATRate = value;
                OnPropertyChanged(() => VATRate);
                ValidateProperty(nameof(VATRate));
            }
        }

        public string ProjectStatus
        {
            get => item.ProjectStatus;
            set
            {
                item.ProjectStatus = value;
                OnPropertyChanged(() => ProjectStatus);
                ValidateProperty(nameof(ProjectStatus));
            }
        }

        public ObservableCollection<KeyAndValue> ClientsItems { get; set; }
        public ObservableCollection<KeyAndValue> ManagersItems { get; set; }
        public ObservableCollection<string> ProjectStatusList { get; set; }
        #endregion

        #region Helpers
        private void LoadData()
        {
            var rawClients = new ClientsAdd(designOfficeEntities).GetClientsKeyAndValueItems().ToList();
            var rawManagers = new EmployeesAdd(designOfficeEntities).GetEmployeesKeyAndValueItems().ToList();

            ClientsItems = new ObservableCollection<KeyAndValue>(rawClients);
            ManagersItems = new ObservableCollection<KeyAndValue>(rawManagers);
            ProjectStatusList = new ObservableCollection<string>
            {
                "W trakcie",
                "Zakończony",
                "Anulowany"
            };
        }

        public override void Save()
        {
            if (HasErrors)
            {
                // Wyświetl komunikat w interfejsie użytkownika
                MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            designOfficeEntities.Projects.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
