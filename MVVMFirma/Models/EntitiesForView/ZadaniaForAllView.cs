using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class ZadaniaForAllView
    {
        public int TaskID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string EmployeesFirstName { get; set; }
        public string EmployeesLastName { get; set; }

        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public decimal? EstimatedHours { get; set; }
        public string TaskStatus { get; set; }
    }
}
