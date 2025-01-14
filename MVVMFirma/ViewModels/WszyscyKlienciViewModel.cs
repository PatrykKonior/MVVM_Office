using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class WszyscyKlienciViewModel : WszystkieViewModel<Clients>
    {

        #region Constructor
        public WszyscyKlienciViewModel()
            :base("Klienci")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Nazwa firmy",
                "NIP",
                "REGON",
                "Telefon kontaktowy",
                "E-mail",
                "Osoba odpowiedzialna"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa firmy":
                    List = new ObservableCollection<Clients>(List.OrderBy(c => c.CompanyName));
                    break;
                case "NIP":
                    List = new ObservableCollection<Clients>(List.OrderBy(c => c.NIP));
                    break;
                case "REGON":
                    List = new ObservableCollection<Clients>(List.OrderBy(c => c.Regon));
                    break;
                case "Telefon kontaktowy":
                    List = new ObservableCollection<Clients>(List.OrderBy(c => c.PhoneNumber));
                    break;
                case "E-mail":
                    List = new ObservableCollection<Clients>(List.OrderBy(c => c.Email));
                    break;
                case "Osoba odpowiedzialna":
                    List = new ObservableCollection<Clients>(List.OrderBy(c => c.ContactPersonName));
                    break;
                default:
                    break;
            }
        }
        // tu decydujemy po czym filtrowac
        public override List<string> GetComboboxFindList()
        {
            return new List<string>
            {
                "Nazwa firmy",
                "NIP",
                "REGON",
                "Telefon kontaktowy",
                "E-mail",
                "Osoba odpowiedzialna"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa firmy":
                    List = new ObservableCollection<Clients>(List.Where(c => c.CompanyName != null &&
                        c.CompanyName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "NIP":
                    List = new ObservableCollection<Clients>(List.Where(c => c.NIP != null &&
                        c.NIP.StartsWith(FindTextBox)));
                    break;

                case "REGON":
                    List = new ObservableCollection<Clients>(List.Where(c => c.Regon != null &&
                        c.Regon.StartsWith(FindTextBox)));
                    break;

                case "Telefon kontaktowy":
                    List = new ObservableCollection<Clients>(List.Where(c => c.PhoneNumber != null &&
                        c.PhoneNumber.StartsWith(FindTextBox)));
                    break;

                case "E-mail":
                    List = new ObservableCollection<Clients>(List.Where(c => c.Email != null &&
                        c.Email.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Osoba odpowiedzialna":
                    List = new ObservableCollection<Clients>(List.Where(c => c.ContactPersonName != null &&
                        c.ContactPersonName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Clients>
            (
                designOfficeEntities.Clients.ToList()
            );
        }
        #endregion
    }


}