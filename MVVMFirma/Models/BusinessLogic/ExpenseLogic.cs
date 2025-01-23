using System;
using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.Models.BusinessLogic
{
    public class ExpenseLogic : DatabaseClass
    {
        public ExpenseLogic(DesignOfficeEntities db) : base(db) { }

        public List<KeyAndValue> GetExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            var rawData = db.Expenses
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .GroupBy(e => new { Year = e.ExpenseDate.Value.Year, Month = e.ExpenseDate.Value.Month })
                .Select(g => new
                {
                    Key = g.Key.Month,
                    TotalNetAmount = g.Sum(e => e.NetAmount ?? 0)
                })
                .ToList();

            if (!rawData.Any())
            {
                Console.WriteLine("Brak danych wydatków dla zakresu dat.");
                return new List<KeyAndValue>(); // Zwracamy pustą listę
            }

            return rawData.Select(g => new KeyAndValue
            {
                Key = g.Key,
                Value = g.TotalNetAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
            }).ToList();
        }

        public List<KeyAndValue> GetAllExpenses()
        {
            var rawData = db.Expenses
                .GroupBy(e => e.ExpenseDate.Value.Month)
                .Select(g => new
                {
                    Key = g.Key,
                    TotalNetAmount = g.Sum(e => e.NetAmount ?? 0) // Suma netto jako decimal
                })
                .ToList();
            
            if (!rawData.Any())
            {
                Console.WriteLine("Brak danych przychodów dla zakresu dat.");
            }

            return rawData.Select(g => new KeyAndValue
            {
                Key = g.Key,
                Value = g.TotalNetAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
            }).ToList();
        }
    }
}