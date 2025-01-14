using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;
using System.Windows.Documents;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class WszystkieProjektyViewModel:WszystkieViewModel<ProjectsForAllView>
    {
        #region Constructor
        public WszystkieProjektyViewModel()
            : base("Projekty")
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
                "Data rozpoczęcia",
                "Data zakończenia",
                "Budżet projektu",
                "Stawka VAT",
                "Status projektu",
                "Nazwa klienta",
                "NIP klienta",
                "REGON klienta"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ProjectName));
                    break;
                case "Typ projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ProjectType));
                    break;
                case "Data rozpoczęcia":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ProjectStartDate));
                    break;
                case "Data zakończenia":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ProjectEndDate));
                    break;
                case "Budżet projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ProjectBudget));
                    break;
                case "Stawka VAT":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.VATRate));
                    break;
                case "Status projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ProjectStatus));
                    break;
                case "Nazwa klienta":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ClientsCompanyName));
                    break;
                case "NIP klienta":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ClientsNIP));
                    break;
                case "REGON klienta":
                    List = new ObservableCollection<ProjectsForAllView>(List.OrderBy(p => p.ClientsRegon));
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
                "Data rozpoczęcia",
                "Data zakończenia",
                "Budżet projektu",
                "Status projektu",
                "Nazwa klienta",
                "NIP klienta",
                "REGON klienta"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p => p.ProjectName != null &&
                        p.ProjectName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Typ projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p => p.ProjectType != null &&
                        p.ProjectType.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Data rozpoczęcia":
                case "Data zakończenia":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p =>
                    {
                        var date = FindField == "Data rozpoczęcia" ? p.ProjectStartDate : p.ProjectEndDate;
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

                case "Budżet projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p =>
                        p.ProjectBudget != null &&
                        p.ProjectBudget.ToString().StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)));
                    break;

                case "Status projektu":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p => p.ProjectStatus != null &&
                        p.ProjectStatus.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "Nazwa klienta":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p => p.ClientsCompanyName != null &&
                        p.ClientsCompanyName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;

                case "NIP klienta":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p => p.ClientsNIP != null &&
                        p.ClientsNIP.StartsWith(FindTextBox)));
                    break;

                case "REGON klienta":
                    List = new ObservableCollection<ProjectsForAllView>(List.Where(p => p.ClientsRegon != null &&
                        p.ClientsRegon.StartsWith(FindTextBox)));
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ProjectsForAllView>
            (
                List = new ObservableCollection<ProjectsForAllView>
                (
                    from project in designOfficeEntities.Projects
                    select new ProjectsForAllView
                    {
                        ProjectID = project.ProjectID,
                        ClientsCompanyName = project.Clients != null ? project.Clients.CompanyName : "Brak klienta",
                        ClientsNIP = project.Clients != null ? project.Clients.NIP : "Brak NIPu",
                        ClientsRegon = project.Clients != null ? project.Clients.Regon : "Brak REGONu",
                        ProjectName = project.ProjectName,
                        ProjectType = project.ProjectType,
                        ProjectStartDate = project.ProjectStartDate,
                        ProjectEndDate = project.ProjectEndDate,
                        ProjectBudget = project.ProjectBudget,
                        VATRate = project.VATRate,
                        ProjectStatus = project.ProjectStatus,
                        EmployeesFirstName = project.Employees != null ? project.Employees.FirstName : "Brak menadżera",
                        EmployeesLastName = project.Employees != null ? project.Employees.LastName : " ",
                        EmployeesPhoneNumber = project.Employees != null ? project.Employees.PhoneNumber : " ",
                        EmployeesEmail = project.Employees != null ? project.Employees.Email : " "
                    }
                    )
            );
        }
        #endregion
    }
}
