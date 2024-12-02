using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    // ta klasa jest wzorowana na klasie Faktura, tylko zamiast kluczy obcych ma czytelne dla Klienta pola (klucze obce będą zastąpione czytelnymi danymi)
    public class FakturaForAllView
    {
        public int InvoiceID { get; set; }

        public DateTime? SalesSaleDate { get; set; }

        public decimal? SalesTotalNetAmount { get; set; }

        public decimal? SalesTotalVATAmount { get; set; }

        public decimal? SalesTotalGrossAmount { get; set; }

        public string SalesSaleStatus { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? PaymentDueDate { get; set; }

        public string InvoiceStatus { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
