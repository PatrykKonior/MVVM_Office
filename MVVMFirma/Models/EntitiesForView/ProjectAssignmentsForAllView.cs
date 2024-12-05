using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class ProjectAssignmentsForAllView
    {
        public int ProjectAssignmentID { get; set; }
        public string ProjectsName { get; set; }
        public string EmployeesFirstName { get; set; }

        public string EmployeesLastName { get; set; }

        public string EmployeesPosition { get; set; }

        public string Role { get; set; }
        public DateTime? AssignmentDate { get; set; }
    }
}
