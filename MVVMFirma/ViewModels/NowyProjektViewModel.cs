using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowyProjektViewModel : JedenViewModel<Projects>
    {
        #region Constructor
        public NowyProjektViewModel() : base("Nowy Projekt")
        {
            item = new Projects();
            LoadData();
            ProjectStartDate = DateTime.Now; // Domyślna data rozpoczęcia projektu
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
            }
        }

        public string ProjectType
        {
            get => item.ProjectType;
            set
            {
                item.ProjectType = value;
                OnPropertyChanged(() => ProjectType);
            }
        }

        public DateTime? ProjectStartDate
        {
            get => item.ProjectStartDate;
            set
            {
                item.ProjectStartDate = value;
                OnPropertyChanged(() => ProjectStartDate);
            }
        }

        public DateTime? ProjectEndDate
        {
            get => item.ProjectEndDate;
            set
            {
                item.ProjectEndDate = value;
                OnPropertyChanged(() => ProjectEndDate);
            }
        }

        public decimal? ProjectBudget
        {
            get => item.ProjectBudget;
            set
            {
                item.ProjectBudget = value;
                OnPropertyChanged(() => ProjectBudget);
            }
        }

        public decimal? VATRate
        {
            get => item.VATRate;
            set
            {
                item.VATRate = value;
                OnPropertyChanged(() => VATRate);
            }
        }

        public string ProjectStatus
        {
            get => item.ProjectStatus;
            set
            {
                item.ProjectStatus = value;
                OnPropertyChanged(() => ProjectStatus);
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
            designOfficeEntities.Projects.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
