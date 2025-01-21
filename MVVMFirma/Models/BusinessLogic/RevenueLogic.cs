using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.Models.BusinessLogic
{
    public RevenueLogic(DesignOfficeEntities db) : base(db) { }

    public List<KeyAndValue> GetMonthlyRevenue()
    {
        // Pobiera dane o miesięcznych przychodach
        return db.Sales
            .GroupBy(s => s.SaleDate.Value.Month)
            .Select(g => new KeyAndValue
            {
                Key = g.Key,
                Value = g.Sum(s => s.TotalNetAmount).ToString("C")
            })
            .ToList();
    }
}