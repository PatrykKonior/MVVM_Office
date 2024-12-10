using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class SaleItemsAdd : DatabaseClass
    {
        #region Constructor
        public SaleItemsAdd(DesignOfficeEntities db) : base(db) { }
        #endregion

        #region Functions
        public IQueryable<KeyAndValue> GetSalesKeyAndValueItems()
        {
            return db.Sales
                .Select(sale => new
                {
                    sale.SaleID,
                    sale.SaleDate,
                    CompanyName = sale.Clients.CompanyName ?? "Brak klienta"
                })
                .ToList() 
                .Select(sale => new KeyAndValue
                {
                    Key = sale.SaleID,
                    Value = $"{sale.SaleID} | {sale.SaleDate:yyyy-MM-dd} | {sale.CompanyName}"
                })
                .AsQueryable();
        }
        #endregion
    }
}
