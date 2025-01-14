using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKontraktyViewModel : WszystkieViewModel<KontraktyForAllView>
    {

        #region Constructor
        public WszystkieKontraktyViewModel()
            :base("Kontrakty")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Nazwa projektu",
                "Typ projektu",
                "Data kontraktu",
                "Wartość netto",
                "Stawka VAT",
                "Wartość brutto",
                "Data podpisu klienta",
                "Data podpisu firmy"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.ProjectsName));
                    break;
                case "Typ projektu":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.ProjectsType));
                    break;
                case "Data kontraktu":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.ContractDate));
                    break;
                case "Wartość netto":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.ContractValueNet));
                    break;
                case "Stawka VAT":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.VATRate));
                    break;
                case "Wartość brutto":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.ContractValueGross));
                    break;
                case "Data podpisu klienta":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.ClientSignatureDate));
                    break;
                case "Data podpisu firmy":
                    List = new ObservableCollection<KontraktyForAllView>(List.OrderBy(c => c.CompanySignatureDate));
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
                "Nazwa projektu",
                "Typ projektu",
                "Data kontraktu",
                "Wartość netto",
                "Stawka VAT",
                "Wartość brutto",
                "Data podpisu klienta",
                "Data podpisu firmy"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<KontraktyForAllView>(List.Where(c => c.ProjectsName != null &&
                        c.ProjectsName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Typ projektu":
                    List = new ObservableCollection<KontraktyForAllView>(List.Where(c => c.ProjectsType != null &&
                        c.ProjectsType.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Data kontraktu":
                case "Data podpisu klienta":
                case "Data podpisu firmy":
                    List = new ObservableCollection<KontraktyForAllView>(List.Where(c =>
                    {
                        var date = FindField == "Data kontraktu" ? c.ContractDate :
                                   FindField == "Data podpisu klienta" ? c.ClientSignatureDate :
                                   c.CompanySignatureDate;
                        if (date == null) return false;

                        if (DateTime.TryParse(FindTextBox, out var exactDate))
                        {
                            return date == exactDate;
                        }
                        else if (int.TryParse(FindTextBox, out var year))
                        {
                            return date.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay))
                        {
                            return date.Value.Month == monthDay.Month && date.Value.Day == monthDay.Day;
                        }
                        return false;
                    }));
                    break;

                case "Wartość netto":
                case "Wartość brutto":
                case "Stawka VAT":
                    Func<KontraktyForAllView, decimal?> field = null;

                    if (FindField == "Wartość netto")
                    {
                        field = c => c.ContractValueNet;
                    }
                    else if (FindField == "Wartość brutto")
                    {
                        field = c => c.ContractValueGross;
                    }
                    else if (FindField == "Stawka VAT")
                    {
                        field = c => c.VATRate;
                    }

                    if (field != null)
                    {
                        List = new ObservableCollection<KontraktyForAllView>(List.Where(c =>
                            field(c) != null &&
                            field(c).Value.ToString("F2").StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<KontraktyForAllView>
            (
                List = new ObservableCollection<KontraktyForAllView>
                (
                    from contract in designOfficeEntities.Contracts
                    select new KontraktyForAllView
                    {
                        ContractID = contract.ContractID,
                        ProjectsName = contract.Projects != null ? contract.Projects.ProjectName : "Brak projektu",
                        ProjectsType = contract.Projects != null ? contract.Projects.ProjectType : "Brak typu projektu",
                        ContractDate = contract.ContractDate,
                        ContractValueNet = contract.ContractValueNet,
                        VATRate = contract.VATRate,
                        ContractValueGross = contract.ContractValueGross,
                        ClientSignatureDate = contract.ClientSignatureDate,
                        CompanySignatureDate = contract.CompanySignatureDate
                    }
                )
            );
        }
        #endregion
    }


}