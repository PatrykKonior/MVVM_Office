using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;

namespace MVVMFirma.ViewModels
{
    public class NowyPracownikViewModel : JedenViewModel<Employees>
    {
        #region Constructor

        public NowyPracownikViewModel()
        : base("Nowy pracownik")
        {
            item = new Employees();
        }

        #endregion

        #region Properties

        // dla każdego pola na interface tworzymy properties
        public string FirstName
        {
            get
            {
                return item.FirstName;
            }
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }
        public string LastName
        {
            get
            {
                return item.LastName;
            }
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public string Position
        {
            get
            {
                return item.Position;
            }
            set
            {
                item.Position = value;
                OnPropertyChanged(() => Position);
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
        
        public DateTime? HireDate
        {
            get
            {
                return item.HireDate;
            }
            set
            {
                item.HireDate = value;
                OnPropertyChanged(() => HireDate);
            }
        }
        
        public decimal? Salary
        {
            get
            {
                return item.Salary;
            }
            set
            {
                item.Salary = value;
                OnPropertyChanged(() => Salary);
            }
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            designOfficeEntities.Employees.Add(item); 
            designOfficeEntities.SaveChanges(); 
        }
        #endregion
    }
}
