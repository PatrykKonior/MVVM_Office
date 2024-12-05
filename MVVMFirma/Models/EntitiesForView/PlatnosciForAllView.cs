using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class PlatnosciForAllView
    {
        public int PaymentID { get; set; }
        public int InvoiceNumber { get; set; }
        public string InvoiceStatus { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceTotalAmount { get; set; }
        public string ClientsCompanyName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
