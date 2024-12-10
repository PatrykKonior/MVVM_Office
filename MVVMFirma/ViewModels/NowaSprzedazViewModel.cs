using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowaSprzedazViewModel : JedenViewModel<Sales>
    {
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

        public DateTime? SaleDate
        {
            get => item.SaleDate;
            set
            {
                item.SaleDate = value;
                OnPropertyChanged(() => SaleDate);
            }
        }

        public string SaleStatus
        {
            get => item.SaleStatus;
            set
            {
                item.SaleStatus = value;
                OnPropertyChanged(() => SaleStatus);
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
            designOfficeEntities.Sales.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
