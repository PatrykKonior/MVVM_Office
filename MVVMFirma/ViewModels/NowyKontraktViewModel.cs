using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowyKontraktViewModel : JedenViewModel<Contracts>
    {
        #region Constructor

        public NowyKontraktViewModel() : base("Nowy Kontrakt")
        {
            item = new Contracts();
            LoadData();
            ContractDate = DateTime.Now; // Domyślna data kontraktu
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
            }
        }

        public DateTime? CompanySignatureDate
        {
            get => item.CompanySignatureDate;
            set
            {
                item.CompanySignatureDate = value;
                OnPropertyChanged(() => CompanySignatureDate);
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
            // Przeliczanie wartości brutto przed zapisaniem
            item.ContractValueGross = ContractValueNet * (1 + VATRate / 100);

            // Zapis do bazy danych
            designOfficeEntities.Contracts.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
