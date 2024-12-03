using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class ProjectsForAllView
    {
        public int ProjectID { get; set; }

        public string ClientsCompanyName { get; set; }

        public string ClientsNIP { get; set; }

        public string ClientsRegon { get; set; }

        public string ProjectName { get; set; }

        public string ProjectType { get; set; }

        public DateTime? ProjectStartDate { get; set; }

        public DateTime? ProjectEndDate { get; set; }

        public decimal? ProjectBudget { get; set; }

        public decimal? VATRate { get; set; }

        public string ProjectStatus { get; set; }

        public string EmployeesFirstName { get; set; }

        public string EmployeesLastName { get; set; }

        public string EmployeesPhoneNumber { get; set; }

        public string EmployeesEmail { get; set; }
    }
}
