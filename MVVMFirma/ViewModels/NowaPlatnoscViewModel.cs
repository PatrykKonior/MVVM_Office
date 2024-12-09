using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowaPlatnoscViewModel : JedenViewModel<Payments>
    {
        #region Constructor

        public NowaPlatnoscViewModel() : base("Nowa Płatność")
        {
            item = new Payments();
            LoadData();
            PaymentDate = DateTime.Now; // Domyślna data płatności

            // Inicjalizacja metod płatności
            PaymentMethods = new ObservableCollection<string>
            {
                "Przelew bankowy",
                "Płatność gotówką",
                "Płatność kartą kredytową/debetową"
            };
        }

        #endregion

        #region Properties

        public int? InvoiceID
        {
            get => item.InvoiceID;
            set
            {
                item.InvoiceID = value;
                OnPropertyChanged(() => InvoiceID);
                OnPropertyChanged(() => SelectedInvoice);
            }
        }

        public KeyAndValue SelectedInvoice
        {
            get => InvoicesItems.FirstOrDefault(i => i.Key == InvoiceID);
            set
            {
                InvoiceID = value?.Key;
                OnPropertyChanged(() => SelectedInvoice);
            }
        }

        public DateTime? PaymentDate
        {
            get => item.PaymentDate;
            set
            {
                item.PaymentDate = value;
                OnPropertyChanged(() => PaymentDate);
            }
        }

        public decimal? PaymentAmount
        {
            get => item.PaymentAmount ?? 0;
            set
            {
                item.PaymentAmount = value;
                OnPropertyChanged(() => PaymentAmount);
            }
        }

        public string PaymentMethod
        {
            get => item.PaymentMethod;
            set
            {
                item.PaymentMethod = value;
                OnPropertyChanged(() => PaymentMethod);
            }
        }

        public ObservableCollection<KeyAndValue> InvoicesItems { get; set; }
        public ObservableCollection<string> PaymentMethods { get; set; }

        #endregion

        #region Helpers

        private void LoadData()
        {
            // Pobranie danych faktur
            var rawInvoices = new PaymentsAdd(designOfficeEntities).GetInvoicesKeyAndValueItems().ToList();
            InvoicesItems = new ObservableCollection<KeyAndValue>(rawInvoices);
        }

        public override void Save()
        {
            item.PaymentAmount = PaymentAmount;
            // Zapis danych płatności do bazy
            designOfficeEntities.Payments.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
