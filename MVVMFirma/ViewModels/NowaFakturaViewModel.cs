using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NowaFakturaViewModel:JedenViewModel<Invoices>
    {
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
        #region Properties

        public int? SaleID
        {
            get => item.SaleID;
            set
            {
                item.SaleID = value;
                OnPropertyChanged(() => SaleID);
                OnPropertyChanged(() => SelectedSale);
            }
        }

        public KeyAndValue SelectedSale
        {
            get => SalesItems.FirstOrDefault(s => s.Key == SaleID);
            set
            {
                SaleID = value?.Key;
                OnPropertyChanged(() => SelectedSale);
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
            }
        }

        public DateTime? PaymentDueDate
        {
            get => item.PaymentDueDate;
            set
            {
                item.PaymentDueDate = value;
                OnPropertyChanged(() => PaymentDueDate);
            }
        }

        public string InvoiceStatus
        {
            get => item.InvoiceStatus;
            set
            {
                item.InvoiceStatus = value;
                OnPropertyChanged(() => InvoiceStatus);
            }
        }

        public decimal? TotalAmount
        {
            get => item.TotalAmount;
            set
            {
                item.TotalAmount = value;
                OnPropertyChanged(() => TotalAmount);
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
