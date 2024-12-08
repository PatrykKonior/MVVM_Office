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
        }
        #endregion
        #region Pole
       public int InvoiceID
        {
            get => item.InvoiceID;
            set
            {
                item.InvoiceID = value;
                OnPropertyChanged(() => InvoiceID);
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

        public int? SaleID
        {
            get => item.SaleID;
            set
            {
                item.SaleID = value;
                OnPropertyChanged(() => SaleID);
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
        #endregion
        #region Properties
        public IQueryable<KeyAndValue> SalesItems
        {
            get
            {
                return new SalesAdd(designOfficeEntities).GetSalesKeyAndValueItems();
            }
        }
        public IQueryable<KeyAndValue> ClientsItems
        {
            get
            {
                return new ClientsAdd(designOfficeEntities).GetClientsKeyAndValueItems();
            }
        }
        #endregion
        #region Helpers
        public override void Save()
        {
            designOfficeEntities.Invoices.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion


    }
}
