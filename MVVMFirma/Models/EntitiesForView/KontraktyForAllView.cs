using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class KontraktyForAllView
    {
        public int ContractID { get; set; }
        public string ProjectsName { get; set; }
        public string ProjectsType { get; set; }
        public DateTime? ContractDate { get; set; }
        public decimal? ContractValueNet { get; set; }
        public decimal? VATRate { get; set; }
        public decimal? ContractValueGross { get; set; }
        public DateTime? ClientSignatureDate { get; set; }
        public DateTime? CompanySignatureDate { get; set; }
    }
}
