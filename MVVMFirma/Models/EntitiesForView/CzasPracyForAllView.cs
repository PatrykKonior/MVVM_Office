using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class CzasPracyForAllView
    {
        public int TimeLogID { get; set; }

        public string EmployeesFirstName { get; set; }
        public string EmployeesLastName { get; set; }
        public string EmployeesPosition { get; set; }
        public string ProjectsName { get; set; }
        public string ProjectsType { get; set; }
        public DateTime? LogDate { get; set; }

        public decimal? HoursWorked { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
