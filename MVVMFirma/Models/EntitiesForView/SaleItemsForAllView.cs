using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class SaleItemsForAllView
    {
        public int SaleItemID { get; set; }

        public DateTime? SalesSaleDate { get; set; }
        public string ClientsCompanyName { get; set; }

        public string ItemDescription { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPriceNet { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? VATRate { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? GrossAmount { get; set; }
    }
}
