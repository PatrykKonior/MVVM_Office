using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;

namespace MVVMFirma.ViewModels
{
    public class NowyTowarViewModel : JedenViewModel<Materials>
    {
        #region Constructor

        public NowyTowarViewModel()
        : base("Towar")
        {
            item = new Materials();
        }

        #endregion

        #region Properties

        // dla każdego pola na interface tworzymy properties
        public string MaterialName
        {
            get
            {
                return item.MaterialName;
            }
            set
            {
                item.MaterialName = value;
                OnPropertyChanged(() => MaterialName);
            }
        }
        public string MaterialDescription
        {
            get
            {
                return item.MaterialDescription;
            }
            set
            {
                item.MaterialDescription = value;
                OnPropertyChanged(() => MaterialDescription);
            }
        }

        public decimal? UnitPrice
        {
            get
            {
                return item.UnitPrice;
            }
            set
            {
                item.UnitPrice = value;
                OnPropertyChanged(() => UnitPrice);
            }
        }

        public decimal? VATRate
        {
            get
            {
                return item.VATRate;
            }
            set
            {
                item.VATRate = value;
                OnPropertyChanged(() => VATRate);
            }
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            designOfficeEntities.Materials.Add(item); //dodaje towar do lokalnej kolekcji
            designOfficeEntities.SaveChanges(); //zapisuje zmiany do bazy danych
        }
        #endregion
    }
}
