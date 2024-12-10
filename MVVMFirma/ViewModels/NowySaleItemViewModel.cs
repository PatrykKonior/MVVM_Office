using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowySaleItemViewModel : JedenViewModel<SaleItems>
    {
        #region Constructor

        public NowySaleItemViewModel() : base("Nowy Element Sprzedaży")
        {
            item = new SaleItems();
            LoadData();
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

        public string Description
        {
            get => item.Description;
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public decimal? Quantity
        {
            get => item.Quantity;
            set
            {
                item.Quantity = value;
                CalculateAmounts();
                OnPropertyChanged(() => Quantity);
            }
        }

        public decimal? UnitPriceNet
        {
            get => item.UnitPriceNet;
            set
            {
                item.UnitPriceNet = value;
                CalculateAmounts();
                OnPropertyChanged(() => UnitPriceNet);
            }
        }

        public decimal? VATRate
        {
            get => item.VATRate;
            set
            {
                item.VATRate = value;
                CalculateAmounts();
                OnPropertyChanged(() => VATRate);
            }
        }

        public decimal? NetAmount
        {
            get => item.NetAmount;
            set
            {
                item.NetAmount = value;
                OnPropertyChanged(() => NetAmount);
            }
        }

        public decimal? VATAmount
        {
            get => item.VATAmount;
            set
            {
                item.VATAmount = value;
                OnPropertyChanged(() => VATAmount);
            }
        }

        public decimal? GrossAmount
        {
            get => item.GrossAmount;
            set
            {
                item.GrossAmount = value;
                OnPropertyChanged(() => GrossAmount);
            }
        }

        public ObservableCollection<KeyAndValue> SalesItems { get; set; }

        #endregion

        #region Helpers

        private void LoadData()
        {
            var rawSales = new SaleItemsAdd(designOfficeEntities).GetSalesKeyAndValueItems().ToList();
            SalesItems = new ObservableCollection<KeyAndValue>(rawSales);
        }

        private void CalculateAmounts()
        {
            if (Quantity.HasValue && UnitPriceNet.HasValue && VATRate.HasValue)
            {
                NetAmount = Quantity.Value * UnitPriceNet.Value;
                VATAmount = NetAmount.Value * (VATRate.Value / 100);
                GrossAmount = NetAmount + VATAmount;
            }
        }

        public override void Save()
        {
            item.NetAmount = NetAmount;
            item.VATAmount = VATAmount;
            item.GrossAmount = GrossAmount;

            designOfficeEntities.SaleItems.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
