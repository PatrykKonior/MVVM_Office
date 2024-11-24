using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;

namespace MVVMFirma.ViewModels
{
    public class NowyKlientViewModel : JedenViewModel<Clients>
    {
        #region Constructor

        public NowyKlientViewModel()
        : base("Nowy klient")
        {
            item = new Clients();
        }

        #endregion

        #region Properties

        // dla każdego pola na interface tworzymy properties
        public string CompanyName
        {
            get
            {
                return item.CompanyName;
            }
            set
            {
                item.CompanyName = value;
                OnPropertyChanged(() => CompanyName);
            }
        }
        public string NIP
        {
            get
            {
                return item.NIP;
            }
            set
            {
                item.NIP = value;
                OnPropertyChanged(() => NIP);
            }
        }

        public string Regon
        {
            get
            {
                return item.Regon;
            }
            set
            {
                item.Regon = value;
                OnPropertyChanged(() => Regon);
            }
        }

        public string PhoneNumber
        {
            get
            {
                return item.PhoneNumber;
            }
            set
            {
                item.PhoneNumber = value;
                OnPropertyChanged(() => PhoneNumber);
            }
        }
        public string Email
        {
            get
            {
                return item.Email;
            }
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }
        
        public string ContactPersonName
        {
            get
            {
                return item.ContactPersonName;
            }
            set
            {
                item.ContactPersonName = value;
                OnPropertyChanged(() => ContactPersonName);
            }
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            designOfficeEntities.Clients.Add(item); 
            designOfficeEntities.SaveChanges(); 
        }
        #endregion
    }
}
