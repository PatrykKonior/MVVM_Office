using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class SprzedazForAllView
    {
        public int SaleID { get; set; }
        public string ClientsCompanyName { get; set; }
        public string ClientsNIP { get; set; }
        public string ClientsRegon { get; set; }
        
        public DateTime? SaleDate { get; set; }
        public decimal? TotalNetAmount { get; set; }
        public decimal? TotalVATAmount { get; set; }
        public decimal? TotalGrossAmount { get; set; }
        public string SaleStatus { get; set; }
    }
}
