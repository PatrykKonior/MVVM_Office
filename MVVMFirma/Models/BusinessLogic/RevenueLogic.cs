using System;
using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.Models.BusinessLogic
{
    public class RevenueLogic : DatabaseClass
    {
        public RevenueLogic(DesignOfficeEntities db) : base(db) { }

        public List<KeyAndValue> GetRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            var rawData = db.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .GroupBy(s => new { Year = s.SaleDate.Value.Year, Month = s.SaleDate.Value.Month })
                .Select(g => new
                {
                    Key = g.Key.Month,
                    TotalNetAmount = g.Sum(s => s.TotalNetAmount ?? 0)
                })
                .ToList();
            
            if (!rawData.Any())
            {
                Console.WriteLine("Brak danych przychodów dla zakresu dat.");
                return new List<KeyAndValue>(); // Zwracamy pustą listę
            }

            return rawData.Select(g => new KeyAndValue
            {
                Key = g.Key,
                Value = g.TotalNetAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
            }).ToList();
        }

        public List<KeyAndValue> GetAllRevenue()
        {
            var rawData = db.Sales
                .GroupBy(s => s.SaleDate.Value.Month)
                .Select(g => new
                {
                    Key = g.Key,
                    TotalNetAmount = g.Sum(s => s.TotalNetAmount ?? 0) // Suma netto jako decimal
                })
                .ToList();

            return rawData.Select(g => new KeyAndValue
            {
                Key = g.Key,
                Value = g.TotalNetAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
            }).ToList();
        }
    }
}