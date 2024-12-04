using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class WydatkiForAllView
    {
        public int ExpenseID { get; set; }
        public string ProjectsName { get; set; }
        public string ProjectsType { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string ExpenseDescription { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? GrossAmount { get; set; }
    }
}
