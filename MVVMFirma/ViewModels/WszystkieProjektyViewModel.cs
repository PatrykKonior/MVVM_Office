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
